using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApi_LoDeFran.Models;
using WebApi_LoDeFran.ViewModels;
using WebApi_LoDeFran.Utlis.ClassAux;
using WebApi_LoDeFran.Utlis;
using DocumentFormat.OpenXml.Drawing.Charts;

namespace WebApi_LoDeFran.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PedidosController : ControllerBase
    {
        private readonly LoDeFranContext _context;
        private readonly IMapper _mapper;

        public PedidosController(LoDeFranContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/Pedidos
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PedidoViewModel>>> GetPedidos()
        {
            var pedidos = await _context.Pedidos
                .Include(p => p.Mesa)
                .Include(p => p.DetallesPedidos)
                .Include(p => p.Promocion)
                .Include(p => p.DetallesPedidos)
                    .ThenInclude(dp => dp.Producto)
                .ToListAsync();

            var pedidosVM = _mapper.Map<List<PedidoViewModel>>(pedidos);
            return Ok(pedidosVM);
        }


        // GET: api/Pedidos/5
        [HttpGet("{id}")]
        public async Task<ActionResult<PedidoViewModel>> GetPedido(int id)
        {
            var pedido = await _context.Pedidos
                .Include(p => p.Mesa)
                .Include(p => p.Promocion)
                .Include(p => p.DetallesPedidos)
                    .ThenInclude(dp => dp.Producto)
                        .ThenInclude(dpi => dpi.CategoriaProducto)
                .Include(p => p.DetallesPedidos) // ⬅ Asegura que se incluya EstadoCocina
                    .ThenInclude(dp => dp.EstadoCocina)
                .Include(p => p.PedidoCombos)
                    .ThenInclude(pc => pc.Combo)
                .Include(p => p.PedidoCombos)
                    .ThenInclude(pc => pc.PedidoComboItems)
                        .ThenInclude(pci => pci.Producto)
                .Include(p => p.Usuario)
                .Include(p => p.Cliente)
                .Include(p => p.TipoPedido)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (pedido == null)
                return NotFound();

            var pedidoVM = _mapper.Map<PedidoViewModel>(pedido);
            return Ok(pedidoVM);
        }



        // POST: api/Pedidos
        [HttpPost]
        public async Task<ActionResult<PedidoViewModel>> PostPedido(PedidoViewModel pedidoVM)
        {
            //try
            //{
                pedidoVM.CategoriaId = 1;

                var pedido = _mapper.Map<Pedido>(pedidoVM);
                pedido.FechaPedido = DateTime.UtcNow;

                if (pedidoVM.PromocionId == 0)
                    pedidoVM.PromocionId = null;


                _context.Pedidos.Add(pedido);
                await _context.SaveChangesAsync();

                var nuevoVM = _mapper.Map<PedidoViewModel>(pedido);
                return CreatedAtAction(nameof(GetPedido), new { id = pedido.Id }, nuevoVM);
            //}
            //catch (Exception ex)
            //{
            //    // Puedes registrar el error si usás un logger, por ejemplo:
            //    // _logger.LogError(ex, "Error al crear el pedido");

            //    return StatusCode(500, new
            //    {
            //        mensaje = "Ocurrió un error al crear el pedido.",
            //        error = ex.Message,
            //        innerException = ex.InnerException?.Message
            //    });
            //}
        }
        [HttpPost("NuevoPedido")]
        public async Task<ActionResult<PedidoViewModel>> NuevoPedidos([FromBody] PedidoViewModel pedidoVM)
        {
            pedidoVM.CategoriaId = 1;

            var pedido = _mapper.Map<Pedido>(pedidoVM);
            pedido.FechaPedido = DateTime.UtcNow;

            if (pedidoVM.PromocionId == 0)
                pedidoVM.PromocionId = null;


            _context.Pedidos.Add(pedido);
            await _context.SaveChangesAsync();

            var nuevoVM = _mapper.Map<PedidoViewModel>(pedido);
            return CreatedAtAction(nameof(GetPedido), new { id = pedido.Id }, nuevoVM);
           
        }

        // PUT: api/Pedidos/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPedido(int id, PedidoViewModel pedidoVM)
        {
            if (id != pedidoVM.Id)
                return BadRequest();

            var pedido = await _context.Pedidos.FindAsync(id);
            if (pedido == null)
                return NotFound();

            _mapper.Map(pedidoVM, pedido);
            _context.Entry(pedido).State = EntityState.Modified;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        // DELETE: api/Pedidos/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePedido(int id)
        {
            var pedido = await _context.Pedidos.FindAsync(id);
            if (pedido == null)
                return NotFound();

            _context.Pedidos.Remove(pedido);
            await _context.SaveChangesAsync();

            return NoContent();
        }
        [HttpPost("iniciar")]
        public async Task<ActionResult<PedidoViewModel>> IniciarPedido([FromBody] int idMesa)
        {
            var mesa = await _context.Mesas.FindAsync(idMesa);
            if (mesa == null)
                return NotFound($"No se encontró la mesa con ID {idMesa}");
            if (mesa.IdEstado != 1) 
                return BadRequest("La mesa no está disponible");

            var pedido = new Pedido
            {
                MesaId = idMesa,
                FechaPedido = DateTime.UtcNow,
                EstadoId =(int)Enums.EstadoPedido.Abierto,
                DetallesPedidos = new List<DetallesPedido>()
            };
            mesa.IdEstado = 2; // 2 = Ocupada

            _context.Pedidos.Add(pedido);
            _context.Mesas.Update(mesa);
            await _context.SaveChangesAsync();

            var pedidoVM = _mapper.Map<PedidoViewModel>(pedido);
            return CreatedAtAction(nameof(GetPedido), new { id = pedido.Id }, pedidoVM);
        }
        


        [HttpPost("{id}/agregar-producto")]
        public async Task<IActionResult> AgregarProducto(int id, [FromBody] ProductoPedidoDto dto)
        {
            var pedido = await _context.Pedidos
                .Include(p => p.DetallesPedidos)
                .Include(p => p.Promocion) // Si tienes la promoción vinculada
                .FirstOrDefaultAsync(p => p.Id == id);

            if (pedido == null)
                return NotFound($"No se encontró el pedido con ID {id}");

            var producto = await _context.Productos.FindAsync(dto.productoId);
            if (producto == null)
                return NotFound($"No se encontró el producto con ID {dto.productoId}");

            var detalleExistente = pedido.DetallesPedidos
                .FirstOrDefault(d => d.ProductoId == dto.productoId);

            if (detalleExistente != null)
            {
                detalleExistente.Cantidad += dto.cantidad; // usar dto.cantidad, no solo +1
            }
            else
            {
                pedido.DetallesPedidos.Add(new DetallesPedido
                {
                    ProductoId = dto.productoId,
                    Cantidad = dto.cantidad,
                    PrecioUnitario = producto.Precio
                });
            }

            // Recalcular total sin descuento
            pedido.TotalSinDescuento = pedido.DetallesPedidos.Sum(d => d.Cantidad * d.PrecioUnitario);

            // Aplicar lógica para calcular descuento y total según promoción
            (pedido.MontoDescuento, pedido.Total) = CalcularDescuentoYTotal(pedido);

            await _context.SaveChangesAsync();
            return NoContent();
        }



        [HttpPost("{pedidoId}/agregar-combo")]
        public async Task<IActionResult> AgregarComboAlPedido(int pedidoId, [FromBody] AgregarComboRequest request)
        {
            if (request == null)
                return BadRequest("Datos del combo no enviados.");

            if (request.Cantidad <= 0)
                return BadRequest("La cantidad debe ser mayor a cero.");

            var pedido = await _context.Pedidos
                .Include(p => p.PedidoCombos)
                    .ThenInclude(pc => pc.PedidoComboItems)
                .Include(p => p.Promocion)
                .FirstOrDefaultAsync(p => p.Id == pedidoId);

            if (pedido == null)
                return NotFound($"No se encontró el pedido con ID {pedidoId}");

            var comboExiste = await _context.Combos.AnyAsync(c => c.Id == request.ComboId);
            if (!comboExiste)
                return NotFound($"No se encontró el combo con ID {request.ComboId}");

            var comboItems = await _context.CombosItems
                .Where(ci => ci.ComboId == request.ComboId)
                .ToListAsync();

            if (!comboItems.Any())
                return BadRequest($"El combo con ID {request.ComboId} no tiene productos asociados.");

            // Verificar si ya existe el combo en el pedido
            var pedidoComboExistente = pedido.PedidoCombos
                .FirstOrDefault(pc => pc.ComboId == request.ComboId);

            if (pedidoComboExistente != null)
            {
                // Si ya existe, sumar la cantidad
                pedidoComboExistente.Cantidad += request.Cantidad;

                // Actualizar también los PedidoComboItems
                foreach (var item in comboItems)
                {
                    var pedidoComboItemExistente = pedidoComboExistente.PedidoComboItems
                        .FirstOrDefault(pci => pci.ProductoId == item.ProductoId);

                    if (pedidoComboItemExistente != null)
                    {
                        pedidoComboItemExistente.Cantidad += item.Cantidad * request.Cantidad;
                    }
                    else
                    {
                        _context.PedidoComboItems.Add(new PedidoComboItem
                        {
                            PedidoComboId = pedidoComboExistente.Id,
                            ProductoId = item.ProductoId,
                            Cantidad = item.Cantidad * request.Cantidad
                        });
                    }
                }
            }
            else
            {
                // Si no existe, crear un nuevo PedidoCombo
                var nuevoPedidoCombo = new PedidoCombo
                {
                    PedidoId = pedidoId,
                    ComboId = request.ComboId,
                    Cantidad = request.Cantidad,
                };

                _context.PedidoCombos.Add(nuevoPedidoCombo);
                await _context.SaveChangesAsync(); // Guardar para obtener ID

                foreach (var item in comboItems)
                {
                    _context.PedidoComboItems.Add(new PedidoComboItem
                    {
                        PedidoComboId = nuevoPedidoCombo.Id,
                        ProductoId = item.ProductoId,
                        Cantidad = item.Cantidad * request.Cantidad
                    });
                }
            }

            await _context.SaveChangesAsync();

            // Recalcular totales y descuentos
            pedido = await _context.Pedidos
                .Include(p => p.DetallesPedidos)
                .Include(p => p.PedidoCombos)
                    .ThenInclude(pc => pc.Combo)
                .Include(p => p.PedidoCombos)
                    .ThenInclude(pc => pc.PedidoComboItems)
                .Include(p => p.Promocion)
                .FirstOrDefaultAsync(p => p.Id == pedidoId);

            var (montoDescuento, totalConDescuento) = CalcularDescuentoYTotal(pedido);

            pedido.MontoDescuento = montoDescuento;
            pedido.Total = totalConDescuento;
            pedido.TotalSinDescuento = pedido.MontoDescuento + pedido.Total;

            await _context.SaveChangesAsync();

            return Ok("Combo agregado correctamente al pedido.");
        }


        [HttpPut("{pedidoId}/combo/{comboId}")]
        public async Task<IActionResult> ActualizarCombo(int pedidoId, int comboId, [FromBody] ActualizarComboRequest request)
        {
            if (pedidoId != request.PedidoId || comboId != request.ComboId)
                return BadRequest("IDs no coinciden.");

            var combo = await _context.PedidoCombos
                .FirstOrDefaultAsync(c => c.Id == comboId && c.PedidoId == pedidoId);

            if (combo == null) return NotFound();

            // Actualizar combo
            combo.Cantidad = request.Cantidad;
            combo.Comentario = request.Comentario;
            await _context.SaveChangesAsync();

            // Recalcular totales del pedido
            var pedido = await _context.Pedidos
                .Include(p => p.DetallesPedidos)
                .Include(p => p.PedidoCombos).ThenInclude(pc => pc.Combo)
                .Include(p => p.PedidoCombos).ThenInclude(pc => pc.PedidoComboItems)
                .Include(p => p.Promocion)
                .FirstOrDefaultAsync(p => p.Id == pedidoId);

            if (pedido != null)
            {
                var (montoDescuento, totalConDescuento) = CalcularDescuentoYTotal(pedido);
                pedido.MontoDescuento = montoDescuento;
                pedido.Total = totalConDescuento;
                pedido.TotalSinDescuento = pedido.MontoDescuento + pedido.Total;
                await _context.SaveChangesAsync();
            }

            return NoContent();
        }



        [HttpGet("por-mesa/{idMesa}")]
        public async Task<ActionResult<PedidoViewModel>> ObtenerPedidoPorMesa(int idMesa)
        {
            var pedido = await _context.Pedidos
                .Include(p => p.Mesa)
                .Include(p => p.TipoPedido)
                .Include(p => p.DetallesPedidos)
                    .ThenInclude(dp => dp.Producto)
                .Include(p => p.PedidoCombos)
                    .ThenInclude(pc => pc.Combo)
                .Include(p => p.Usuario)   // para traer info del mozo
                .Include(p => p.Cliente)   // para traer info del cliente
                .FirstOrDefaultAsync(p => p.MesaId == idMesa && p.EstadoId != 7);

            if (pedido == null)
                return NotFound();

            var pedidoVM = _mapper.Map<PedidoViewModel>(pedido);
            return Ok(pedidoVM);
        }

        [HttpPut("detalle/{idDetalle}/cantidad")]
        public async Task<IActionResult> ModificarCantidad(int idDetalle, [FromBody] int nuevaCantidad)
        {
            var detalle = await _context.DetallesPedidos.FindAsync(idDetalle);
            if (detalle == null) return NotFound();

            detalle.Cantidad = nuevaCantidad;
            await _context.SaveChangesAsync();

            return NoContent();
        }
		[HttpGet("cocina")]
		public async Task<ActionResult<IEnumerable<PedidoViewModel>>> GetPedidosParaCocina()
		{
			var estadoEnPreparacion = (int)Enums.EstadoPedido.EnPreparacion;
			var estadoDetalleEnPreparacion = (int)Enums.EstadoDetallePedidoCocina.EnPreparacion;

			// Traer pedidos base
			var pedidosBase = await _context.Pedidos
				.Where(p => p.EstadoId == estadoEnPreparacion)
				.Include(p => p.Mesa)
				.Include(p => p.TipoPedido)
				.Include(p => p.Promocion)
				.Include(p => p.PedidoCombos)
					.ThenInclude(pc => pc.Combo)
				.Include(p => p.PedidoCombos)
					.ThenInclude(pc => pc.PedidoComboItems)
						.ThenInclude(pci => pci.Producto)
				.Include(p => p.PedidoCombos)
					.ThenInclude(pc => pc.Combo)
						.ThenInclude(c => c.CombosItems)
							.ThenInclude(cp => cp.Producto)
				.ToListAsync();

			foreach (var pedido in pedidosBase)
			{
				pedido.DetallesPedidos = await _context.DetallesPedidos
					.Where(dp => dp.PedidoId == pedido.Id && dp.EstadoCocinaId == estadoDetalleEnPreparacion)
					.Include(dp => dp.Producto)
					.Include(dp => dp.EstadoCocina)
					.ToListAsync();
			}

			// Filtrar los pedidos que tienen al menos un detalle en preparación
			var pedidosConDetalles = pedidosBase
				.Where(p => p.DetallesPedidos != null && p.DetallesPedidos.Any())
				.ToList();

			var pedidosVM = _mapper.Map<List<PedidoViewModel>>(pedidosConDetalles);
			return Ok(pedidosVM);
		}





		// GET: api/Pedidos?estado=2
		[HttpGet("por-estado/{idEstado}")]
        public async Task<ActionResult<IEnumerable<PedidoViewModel>>> GetPedidosPorEstado(int idEstado)
        {
            var pedidos = await _context.Pedidos
                .Include(p => p.Mesa)
                .Include(p => p.DetallesPedidos)
                    .ThenInclude(dp => dp.Producto)
                .Where(p => p.EstadoId == idEstado)
                .ToListAsync();


            var pedidosVM = _mapper.Map<List<PedidoViewModel>>(pedidos);
            return Ok(pedidosVM);
        }
        [HttpGet("tipos")]
        public async Task<ActionResult<IEnumerable<TipoPedidoViewModel>>> GetTiposPedido()
        {
            var tipos = await _context.TiposPedidos.ToListAsync();

            var tiposVM = _mapper.Map<List<TipoPedidoViewModel>>(tipos);
            return Ok(tiposVM);
        }
        [HttpPost("sin-mesa")]
        public async Task<ActionResult<int>> CrearPedidoSinMesa([FromBody] PedidoViewModel pedidoVM)
        {
            if (pedidoVM.TipoPedidoId == null)
                return BadRequest("El tipo de pedido es obligatorio.");

            // Si viene ClienteId, verificar si existe
            if (pedidoVM.ClienteId != null)
            {
                var cliente = await _context.Clientes.FindAsync(pedidoVM.ClienteId);
                if (cliente == null)
                    return BadRequest("El cliente indicado no existe.");
            }

            var pedido = _mapper.Map<Pedido>(pedidoVM);
            pedido.FechaPedido = DateTime.UtcNow;
            pedido.MesaId = 0; // sin mesa
            pedido.EstadoId = (int)Enums.EstadoPedido.Abierto;

            _context.Pedidos.Add(pedido);
            await _context.SaveChangesAsync();

            return Ok(pedido.Id);
        }

        [HttpPost("crear-con-id")]
        public async Task<ActionResult<int>> CrearPedido([FromBody] PedidoViewModel pedido)
        {
            if (pedido == null || pedido.TipoPedidoId == null)
                return BadRequest("Datos incompletos.");

            var entidad = _mapper.Map<Pedido>(pedido);
            entidad.FechaPedido = DateTime.UtcNow;

            _context.Pedidos.Add(entidad);
            await _context.SaveChangesAsync();

            return Ok(entidad.Id);
        }

        [HttpPost("iniciar_pedido_out")]
        public async Task<ActionResult<PedidoViewModel>> IniciarPedidoOut([FromBody] int idMesa)
        {
            var mesa = await _context.Mesas.FindAsync(idMesa);
            if (mesa == null)
                return NotFound($"No se encontró la mesa con ID {idMesa}");
            if (mesa.IdEstado != 1)
                return BadRequest("La mesa no está disponible");

            var pedido = new Pedido
            {
                MesaId = idMesa,
                FechaPedido = DateTime.UtcNow,
                EstadoId = (int)Enums.EstadoPedido.Abierto,
                DetallesPedidos = new List<DetallesPedido>()
            };
            mesa.IdEstado = 2; // 2 = Ocupada

            _context.Pedidos.Add(pedido);
            _context.Mesas.Update(mesa);
            await _context.SaveChangesAsync();

            var pedidoVM = _mapper.Map<PedidoViewModel>(pedido);
            return CreatedAtAction(nameof(GetPedido), new { id = pedido.Id }, pedidoVM);
        }
        [HttpPost("crear-pedido")]
        public async Task<ActionResult<PedidoViewModel>> CrearPedidoConClienteYTipo([FromBody] CrearPedidoRequest request)
        {
            // Validar tipo de pedido
            var tipoPedido = await _context.TiposPedidos.FindAsync(request.TipoPedidoId);
            if (tipoPedido == null)
                return BadRequest("Tipo de pedido inválido.");

            var tipoNombre = tipoPedido.Nombre?.ToLower();

            // DELIVERY
            if (tipoNombre.Contains("delivery"))
            {
                var cliente = await _context.Clientes.FindAsync(request.ClienteId);
                if (cliente == null)
                    return BadRequest("El cliente no existe para delivery.");
            }

            // MOSTRADOR
            if (tipoNombre.Contains("mostrador"))
            {
                if (request.ClienteId == 0)
                {
                    // Buscar o crear un cliente genérico
                    var clienteGenerico = await _context.Clientes
                        .FirstOrDefaultAsync(c => c.Nombre == "Genérico" && c.Telefono == "0");

                    if (clienteGenerico == null)
                    {
                        clienteGenerico = new Cliente
                        {
                            Nombre = "Genérico",
                            Apellido = "Mostrador",
                            Telefono = "0",
                            FechaCreacion = DateTime.Now
                        };
                        _context.Clientes.Add(clienteGenerico);
                        await _context.SaveChangesAsync();
                    }

                    request.ClienteId = clienteGenerico.Id;
                }
                else
                {
                    var cliente = await _context.Clientes.FindAsync(request.ClienteId);
                    if (cliente == null)
                        return BadRequest("El cliente no existe.");
                }
            }

            // Crear el pedido
            var pedido = new Pedido
            {
                ClienteId = request.ClienteId,
                TipoPedidoId = request.TipoPedidoId,
                FechaPedido = DateTime.Now,
                EstadoId = (int)Enums.EstadoPedido.Abierto, // Asegurate de tener un enum o valor fijo
                MesaId = null // Sin mesa
            };

            _context.Pedidos.Add(pedido);
            await _context.SaveChangesAsync();

            var viewModel = _mapper.Map<PedidoViewModel>(pedido);
            return Ok(viewModel);
        }

        [HttpPost("crear_pedido_por_cliente")]
        public async Task<IActionResult> CrearPedidoPorCliente([FromBody] CrearPedidoRequest dto)
        {
            // Validar tipo de pedido
            var tipoPedido = await _context.TiposPedidos.FindAsync(dto.TipoPedidoId);
            if (tipoPedido == null)
                return BadRequest("Tipo de pedido inválido.");

            var tipoNombre = tipoPedido.Nombre?.ToLower();

            // DELIVERY
            if (tipoNombre.Contains("delivery") && dto.ClienteId.HasValue)
            {
                var cliente = await _context.Clientes.FindAsync(dto.ClienteId.Value);
                if (cliente == null)
                    return BadRequest("El cliente no existe para delivery.");
            }

            // Si ClienteId es null, se crea el pedido sin cliente asignado


            // MOSTRADOR
            if (tipoNombre.Contains("mostrador"))
            {
                if (dto.ClienteId == 0)
                {
                    var clienteGenerico = await _context.Clientes
                        .FirstOrDefaultAsync(c => c.Nombre == "Genérico" && c.Telefono == "0");

                    if (clienteGenerico == null)
                    {
                        clienteGenerico = new Cliente
                        {
                            Nombre = "Genérico",
                            Apellido = "Mostrador",
                            Telefono = "0",
                            FechaCreacion = DateTime.Now
                        };
                        _context.Clientes.Add(clienteGenerico);
                        await _context.SaveChangesAsync();
                    }

                    dto.ClienteId = clienteGenerico.Id;
                }
                else
                {
                    var cliente = await _context.Clientes.FindAsync(dto.ClienteId);
                    if (cliente == null)
                        return BadRequest("El cliente no existe.");
                }
            }

            // Crear el pedido
            var pedido = new Pedido
            {
                ClienteId = dto.ClienteId, // puede ser null
                TipoPedidoId = dto.TipoPedidoId,
                FechaPedido = DateTime.Now,
                EstadoId = (int)Enums.EstadoPedido.Abierto,
                MesaId = null
            };


            _context.Pedidos.Add(pedido);
            await _context.SaveChangesAsync();

            var viewModel = _mapper.Map<PedidoViewModel>(pedido);
            return Ok(viewModel);
        }
        // GET: api/Pedidos/no-salon
        [HttpGet("no-salon")]
        public async Task<ActionResult<IEnumerable<PedidoViewModel>>> GetPedidosFueraDeSalon()
        {
            // IDs de estado que se quieren excluir (cerrado/cancelado)
            var estadosExcluir = new[] { 7, 8 }; // <- por ejemplo: 3 = cerrado, 4 = cancelado
            var tipoSalonId = 1; // <- suponiendo que 1 = salón

            var pedidos = await _context.Pedidos
                .Where(p => !estadosExcluir.Contains(p.EstadoId) && p.TipoPedidoId != tipoSalonId)
                .Include(p => p.Mesa)
                .Include(p => p.Promocion)
                .Include(p => p.DetallesPedidos)
                    .ThenInclude(dp => dp.Producto)
                .Include(p => p.Cliente)
                .Include(p => p.TipoPedido) // ✅ Necesario para mapear TipoPedidoNombre
                .ToListAsync();


            var pedidosVM = _mapper.Map<List<PedidoViewModel>>(pedidos);

            // Enriquecer con ClienteNombre
            for (int i = 0; i < pedidos.Count; i++)
            {
                var cliente = pedidos[i].Cliente;
                if (cliente != null)
                    pedidosVM[i].ClienteNombre = $"{cliente.Nombre} {cliente.Apellido}";
            }

            return Ok(pedidosVM);
        }
        [HttpPost("crear_pedido_generico")]
        public async Task<IActionResult> CrearPedidoGenerico([FromBody] CrearPedidoRequest dto)
        {
            // Validar tipo de pedido
            var tipoPedido = await _context.TiposPedidos.FindAsync(dto.TipoPedidoId);
            if (tipoPedido == null)
                return BadRequest("Tipo de pedido inválido.");

            var tipoNombre = tipoPedido.Nombre?.ToLower();

            // Validación y asignación de cliente
            if (dto.TipoPedidoId ==3)
            {
                // Cliente obligatorio
                if (dto.ClienteId == null)
                    return BadRequest("El cliente es obligatorio para delivery.");

                var cliente = await _context.Clientes.FindAsync(dto.ClienteId);
                if (cliente == null)
                    return BadRequest("El cliente no existe.");
            }
            else if (dto.TipoPedidoId == 2)
            {
                // Cliente opcional
                if (dto.ClienteId == null || dto.ClienteId == 0)
                {
                    var clienteGenerico = await _context.Clientes
                        .FirstOrDefaultAsync(c => c.Nombre == "Genérico" && c.Telefono == "0");

                    if (clienteGenerico == null)
                    {
                        clienteGenerico = new Cliente
                        {
                            Nombre = "Genérico",
                            Apellido = "Mostrador",
                            Telefono = "0",
                            FechaCreacion = DateTime.Now
                        };
                        _context.Clientes.Add(clienteGenerico);
                        await _context.SaveChangesAsync();
                    }

                    dto.ClienteId = clienteGenerico.Id;
                }
                else
                {
                    var cliente = await _context.Clientes.FindAsync(dto.ClienteId);
                    if (cliente == null)
                        return BadRequest("El cliente no existe.");
                }
            }
            else if (dto.TipoPedidoId == 1)
            {
                // Cliente opcional
                if (dto.MesaId == null)
                    return BadRequest("La mesa es obligatoria para pedidos de mesa.");

                var mesa = await _context.Mesas.FindAsync(dto.MesaId);
                if (mesa == null)
                    return BadRequest("La mesa no existe.");

                // Marcar mesa como ocupada
                mesa.IdEstado = (int)Enums.EstadoMesa.Ocupada;
                _context.Mesas.Update(mesa);
            }

            var pedido = new Pedido
            {
                ClienteId = dto.ClienteId,
                TipoPedidoId = dto.TipoPedidoId,
                FechaPedido = DateTime.Now,
                EstadoId = (int)Enums.EstadoPedido.Abierto,
                MesaId = dto.MesaId,
                UsuarioId = dto.UsuarioId ?? 0, // opcional
                //PromocionId = dto.PromocionId,
                //MontoDescuento = dto.MontoDescuento,
                //TotalSinDescuento = dto.TotalSinDescuento,
                //Notas = dto.Notas
            };

            _context.Pedidos.Add(pedido);
            await _context.SaveChangesAsync();

            var viewModel = _mapper.Map<PedidoViewModel>(pedido);
            return Ok(viewModel);
        }

        private (decimal montoDescuento, decimal totalConDescuento) CalcularDescuentoYTotal(Pedido pedido)
        {
            // Total por detalles del pedido
            decimal totalDetalles = pedido.DetallesPedidos.Sum(d => d.Cantidad * d.PrecioUnitario);

            // Total por combos del pedido
            decimal totalCombos = pedido.PedidoCombos
                .Where(pc => pc.Combo != null && pc.Combo.Precio > 0)
                .Sum(pc => pc.Cantidad * pc.Combo.Precio);

            decimal totalSinDescuento = totalDetalles + totalCombos;
            decimal montoDescuento = 0;

            if (pedido.Promocion == null || pedido.Promocion.EstadoId != 1) // Solo promociones activas
                return (0, totalSinDescuento);

            var promo = pedido.Promocion;

            switch (promo.AplicacionId)
            {
                case 1: // Total de la compra
                    montoDescuento = AplicarDescuento(promo, totalSinDescuento);
                    break;

                //case 2: // Producto específico
                //    montoDescuento = CalcularDescuentoProductoEspecifico(pedido, promo);
                //    break;

                //case 3: // Categoría de producto
                //    montoDescuento = CalcularDescuentoCategoria(pedido, promo);
                //    break;

                case 4: // Primer compra
                    if (EsPrimerCompra(pedido.ClienteId))
                    {
                        montoDescuento = AplicarDescuento(promo, totalSinDescuento);
                    }
                    break;

                default:
                    montoDescuento = 0;
                    break;
            }

            decimal totalConDescuento = totalSinDescuento - montoDescuento;

            // Evitar valores negativos
            if (totalConDescuento < 0) totalConDescuento = 0;

            return (montoDescuento, totalConDescuento);
        }
        private decimal AplicarDescuento(Promocione promo, decimal baseAmount)
        {
            decimal valorDescuento = promo.ValorDescuento ?? 0m; // asignar 0 si es null
            switch (promo.TipoDescuentoId)
            {
                case 1: // Porcentaje
                    return baseAmount * (valorDescuento / 100m);
                case 2: // Monto fijo
                    return valorDescuento;
                case 3: // Envío gratis - no aplica monto descuento
                    return 0;
                case 4: // Regalo - no aplica monto descuento
                    return 0;
                default:
                    return 0;
            }
        }


        //private decimal CalcularDescuentoProductoEspecifico(Pedido pedido, Promocione promo)
        //{
        //    // Aquí necesitas la lógica para productos específicos:
        //    // Ejemplo: descuento solo si el producto X está en el pedido
        //    // Supongamos promo tiene una lista o un Id de producto asociado (deberías extender tu modelo)

        //    decimal descuento = 0;

        //    // Ejemplo básico: buscamos un producto con ID promo.ProductoId (deberías agregar esta propiedad)
        //    var productoDetalle = pedido.DetallesPedidos.FirstOrDefault(d => d.ProductoId == promo.ProductoId);
        //    if (productoDetalle != null)
        //    {
        //        decimal baseAmount = productoDetalle.Cantidad * productoDetalle.PrecioUnitario;
        //        descuento = AplicarDescuento(promo, baseAmount);
        //    }
        //    return descuento;
        //}

        //private decimal CalcularDescuentoCategoria(Pedido pedido, Promocione promo)
        //{
        //    // Similar al anterior, pero filtramos por categoría
        //    decimal descuento = 0;

        //    // Aquí deberías tener promo.CategoriaId o similar para filtrar
        //    var detallesCategoria = pedido.DetallesPedidos
        //        .Where(d => d.Producto.CategoriaProductoId == promo.CategoriaId);

        //    decimal baseAmount = detallesCategoria.Sum(d => d.Cantidad * d.PrecioUnitario);
        //    descuento = AplicarDescuento(promo, baseAmount);

        //    return descuento;
        //}

        private bool EsPrimerCompra(int? clienteId)
        {
            if (!clienteId.HasValue) return false;

            // Lógica para validar si es la primera compra del cliente
            return !_context.Pedidos.Any(p => p.ClienteId == clienteId.Value);
        }

        [HttpPut("{id}/estado")]
        public async Task<IActionResult> CambiarEstado(int id, [FromBody] Enums.EstadoPedido nuevoEstado)
        {
            var pedido = await _context.Pedidos
                .Include(p => p.DetallesPedidos)
                .ThenInclude(d => d.Producto)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (pedido == null)
                return NotFound();

            if (pedido.EstadoId != (int)nuevoEstado)
            {
                if (nuevoEstado == Enums.EstadoPedido.EnPreparacion)
                {
                    await DescontarStockPorPedido(pedido);
                }
                else if (nuevoEstado == Enums.EstadoPedido.Cancelado)
                {
                    await RevertirStockPorPedido(pedido);
                }

                pedido.EstadoId = (int)nuevoEstado;
                await _context.SaveChangesAsync();
            }

            return NoContent();
        }
        private async Task DescontarStockPorPedido(Pedido pedido)
        {
            foreach (var detalle in pedido.DetallesPedidos)
            {
                var producto = await _context.Productos
                    .Include(p => p.InsumosProductos)
                    .FirstOrDefaultAsync(p => p.Id == detalle.ProductoId);

                if (producto == null) continue;

                // Si tiene insumos: descontar insumos
                if (producto.InsumosProductos != null && producto.InsumosProductos.Any())
                {
                    foreach (var insumoProducto in producto.InsumosProductos)
                    {
                        var insumo = await _context.Insumos.FindAsync(insumoProducto.InsumoId);
                        if (insumo != null)
                        {
                            var cantidadTotal = insumoProducto.Cantidad * detalle.Cantidad;

                            if (cantidadTotal == null)
                                continue; // o loguear error, o lanzar excepción según el caso

                            insumo.CantidadDisponible -= cantidadTotal.Value;

                            if (insumo.CantidadDisponible < 0)
                                insumo.CantidadDisponible = 0;
                        }
                    }
                }
                else
                {
                    // Si no tiene insumos: descontar stock del producto
                    producto.Stock -= detalle.Cantidad;
                    if (producto.Stock < 0)
                        producto.Stock = 0;
                }
            }
        }


        private async Task RevertirStockPorPedido(Pedido pedido)
        {
            foreach (var detalle in pedido.DetallesPedidos)
            {
                var producto = await _context.Productos
                    .Include(p => p.InsumosProductos)
                    .FirstOrDefaultAsync(p => p.Id == detalle.ProductoId);

                if (producto == null) continue;

                if (producto.InsumosProductos != null && producto.InsumosProductos.Any())
                {
                    foreach (var insumoProducto in producto.InsumosProductos)
                    {
                        var insumo = await _context.Insumos.FindAsync(insumoProducto.InsumoId);
                        if (insumo != null)
                        {
                            var cantidadTotal = insumoProducto.Cantidad * detalle.Cantidad;

                            if (cantidadTotal == null)
                                return; // O loguear un error, o lanzar una excepción

                            insumo.CantidadDisponible += cantidadTotal.Value;
                        }
                    }
                }
                else
                {
                    producto.Stock += detalle.Cantidad;
                }
            }
        }

        [HttpPost("asignar_cliente")]
        public async Task<IActionResult> AsignarCliente([FromBody] AsignarClienteRequest dto)
        {
            var pedido = await _context.Pedidos.FindAsync(dto.PedidoId);
            if (pedido == null) return NotFound();

            var cliente = await _context.Clientes.FindAsync(dto.ClienteId);
            if (cliente == null) return BadRequest("Cliente no encontrado");

            pedido.ClienteId = cliente.Id;
            await _context.SaveChangesAsync();

            return Ok();
        }


    }


    public class ProductoPedidoDto
    {
        public int productoId { get; set; }
        public int cantidad { get; set; }
    }
  

    public class AgregarComboRequest
    {
        public int ComboId { get; set; }
        public int Cantidad { get; set; }
    }
    public class CrearPedidoRequestMesa
    {
        public int? ClienteId { get; set; }
        public int? TipoPedidoId { get; set; }
        public int? MesaId { get; set; }
    }
    public class AsignarClienteRequest
    {
        public int? PedidoId { get; set; }
        public int? ClienteId { get; set; }
    }
}
