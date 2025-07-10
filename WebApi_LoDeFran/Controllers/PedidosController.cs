using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApi_LoDeFran.Models;
using WebApi_LoDeFran.ViewModels;

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
                .Include(p => p.DetallesPedidos)
                .ThenInclude(dp => dp.Producto)
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
            var pedido = _mapper.Map<Pedido>(pedidoVM);
            pedido.FechaPedido = DateTime.UtcNow;

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
                EstadoId =(int)EstadoPedido.Abierto,
                DetallesPedidos = new List<DetallesPedido>()
            };
            mesa.IdEstado = 2; // 2 = Ocupada

            _context.Pedidos.Add(pedido);
            _context.Mesas.Update(mesa);
            await _context.SaveChangesAsync();

            var pedidoVM = _mapper.Map<PedidoViewModel>(pedido);
            return CreatedAtAction(nameof(GetPedido), new { id = pedido.Id }, pedidoVM);
        }
        [HttpPut("{id}/estado")]
        public async Task<IActionResult> CambiarEstado(int id, [FromBody] EstadoPedido nuevoEstado)
        {
            var pedido = await _context.Pedidos.FindAsync(id);
            if (pedido == null)
                return NotFound();

            pedido.EstadoId = (int)nuevoEstado;
            await _context.SaveChangesAsync();

            return NoContent();
        }
        [HttpPost("{id}/agregar-producto")]
        public async Task<IActionResult> AgregarProducto(int id, [FromBody] ProductoPedidoDto dto)
        {
            var pedido = await _context.Pedidos
                .Include(p => p.DetallesPedidos)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (pedido == null)
                return NotFound($"No se encontró el pedido con ID {id}");

            var producto = await _context.Productos.FindAsync(dto.productoId);
            if (producto == null)
                return NotFound($"No se encontró el producto con ID {dto.productoId}");

            // Verificar si el producto ya está en el pedido
            var detalleExistente = pedido.DetallesPedidos
                .FirstOrDefault(d => d.ProductoId == dto.productoId);

            if (detalleExistente != null)
            {
                detalleExistente.Cantidad += 1;
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

            await _context.SaveChangesAsync();
            return NoContent();
        }
        [HttpGet("por-mesa/{idMesa}")]
        public async Task<ActionResult<PedidoViewModel>> ObtenerPedidoPorMesa(int idMesa)
        {
            var pedido = await _context.Pedidos
                //.Include(p => p.Estado)
                .Include(p => p.DetallesPedidos)
                .ThenInclude(dp => dp.Producto)
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
            var pedidos = await _context.Pedidos
                .Where(p => p.EstadoId == (int)EstadoPedido.Abierto || p.EstadoId == (int)EstadoPedido.EnPreparacion)
                .Include(p => p.Mesa)
                .Include(p => p.TipoPedido)
                .Include(p => p.DetallesPedidos)
                    .ThenInclude(dp => dp.Producto)
                .ToListAsync();

            var pedidosVM = _mapper.Map<List<PedidoViewModel>>(pedidos);
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
            pedido.EstadoId = (int)EstadoPedido.Abierto;

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
                EstadoId = (int)EstadoPedido.Abierto,
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
                EstadoId = (int)EstadoPedido.Abierto, // Asegurate de tener un enum o valor fijo
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
            if (tipoNombre.Contains("delivery"))
            {
                var cliente = await _context.Clientes.FindAsync(dto.ClienteId);
                if (cliente == null)
                    return BadRequest("El cliente no existe para delivery.");
            }

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
                ClienteId = dto.ClienteId,
                TipoPedidoId = dto.TipoPedidoId,
                FechaPedido = DateTime.Now,
                EstadoId = (int)EstadoPedido.Abierto,
                MesaId = null // Pedido sin mesa
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

    }
    public enum EstadoPedido
    {
        Abierto = 1,             // Pedido creado y se están cargando productos.
        EnPreparacion = 2,       // Cocina está preparando.
        ListoParaEntregar = 3,   // Cocina termina, listo para el mozo.
        Entregado = 4,           // El mozo entrega a la mesa.
        A_Cobrar = 5,            // El cliente pide la cuenta.
        Cobrado = 6,             // Cajero cobra el pedido.
        Cerrado = 7,             // Se liberó la mesa.
        Cancelado = 8
    }
    public class ProductoPedidoDto
    {
        public int productoId { get; set; }
        public int cantidad { get; set; }
    }
    public class CrearPedidoRequest
    {
        public int ClienteId { get; set; }
        public int TipoPedidoId { get; set; }
    }

}
