using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApi_LoDeFran.Models;
using WebApi_LoDeFran.ViewModels;

namespace WebApi_LoDeFran.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PromocionesController : ControllerBase
    {
        private readonly LoDeFranContext _context;
        private readonly IMapper _mapper;

        public PromocionesController(LoDeFranContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PromocionViewModel>>> GetAll()
        {
            var promociones = await _context.Promociones
                .Include(p => p.Aplicacion)
                .Include(p => p.Estado)
                .Include(p => p.TipoPromocion)
                .Include(p => p.TipoDescuento)
                .Include(p => p.PromocionDia)
                    .ThenInclude(dp => dp.Dia)
                .ToListAsync();

            var viewModels = _mapper.Map<List<PromocionViewModel>>(promociones);
            return Ok(viewModels);
        }


        // GET: api/Promociones/5
        [HttpGet("{id}")]
        public async Task<ActionResult<PromocionViewModel>> Get(int id)
        {
            var promocion = await _context.Promociones
                .Include(p => p.Aplicacion)
                .Include(p => p.Estado)
                .Include(p => p.TipoPromocion)
                .Include(p => p.TipoDescuento)
                .Include(p => p.PromocionDia)
                    .ThenInclude(dp => dp.Dia)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (promocion == null)
                return NotFound();

            return Ok(_mapper.Map<PromocionViewModel>(promocion));
        }

        // POST: api/Promociones
        [HttpPost]
		public async Task<ActionResult<PromocionViewModel>> Post(PromocionViewModel model)
		{
			var entity = _mapper.Map<Promocione>(model);
			_context.Promociones.Add(entity);
			await _context.SaveChangesAsync();

			model.Id = entity.Id;
			return CreatedAtAction(nameof(Get), new { id = model.Id }, model);
		}

		// PUT: api/Promociones/5
		[HttpPut("{id}")]
		public async Task<IActionResult> Put(int id, PromocionViewModel model)
		{
			if (id != model.Id)
				return BadRequest("ID mismatch");

			var entity = await _context.Promociones.FindAsync(id);
			if (entity == null)
				return NotFound();

			_mapper.Map(model, entity);
			await _context.SaveChangesAsync();

			return NoContent();
		}

		// DELETE: api/Promociones/5
		[HttpDelete("{id}")]
		public async Task<IActionResult> Delete(int id)
		{
			var entity = await _context.Promociones.FindAsync(id);
			if (entity == null)
				return NotFound();

			_context.Promociones.Remove(entity);
			await _context.SaveChangesAsync();

			return NoContent();
		}
        [HttpGet("form-data")]
        public async Task<ActionResult<PromocionFormDataViewModel>> GetFormData()
        {
            var result = new PromocionFormDataViewModel
            {
                TiposDescuento = await _context.TipoDescuentos
                    .Select(t => new TipoDescuentoViewModel { Id = t.Id, Nombre = t.Nombre })
                    .ToListAsync(),

                Aplicaciones = await _context.PromocionesAplicaciones
                    .Select(a => new PromocionAplicacionViewModel { Id = a.Id, Nombre = a.Nombre })
                    .ToListAsync(),

                Dias = await _context.Dias
                    .Select(d => new DiaViewModel { Id = d.Id, Nombre = d.Nombre, Codigo = d.Codigo })
                    .ToListAsync(),

                //Estados = await _context.EstadosPromociones
                //    .Select(e => new EstadoPromocionViewModel { Id = e.Id, Nombre = e.Nombre })
                //    .ToListAsync(),

                TiposPromocion = await _context.TipoPromocions
                    .Select(tp => new TipoPromocionViewModel { Id = tp.Id, Nombre = tp.Nombre })
                    .ToListAsync()
            };

            return Ok(result);
        }
        [HttpGet("promociones_disponibles_para_pedido/{pedidoId}")]
        public async Task<IActionResult> ObtenerPromocionesAplicables(int pedidoId)
        {
            var pedido = await _context.Pedidos
                .Include(p => p.Cliente)
                .Include(p => p.DetallesPedidos)
                .FirstOrDefaultAsync(p => p.Id == pedidoId);

            if (pedido == null)
                return NotFound("Pedido no encontrado.");

            var promocionesActivas = await _context.Promociones
                .Where(p =>
                    p.FechaInicio <= DateTime.Now &&
                    p.FechaFin >= DateTime.Now &&
                    p.EstadoId == 1)
                .ToListAsync();

            var promocionesAplicables = new List<Promocione>();

            foreach (var promo in promocionesActivas)
            {
                switch (promo.TipoPromocionId)
                {
                    case 1: // Happy Hour
                        var hora = pedido.FechaPedido?.TimeOfDay;
                        if (hora >= TimeSpan.FromHours(17) && hora <= TimeSpan.FromHours(19))
                            promocionesAplicables.Add(promo);
                        break;

                    case 2: // Lunes a viernes
                        var dia = pedido.FechaPedido?.DayOfWeek;
                        if (pedido.Total >= promo.MontoMinimo && dia >= DayOfWeek.Monday && dia <= DayOfWeek.Friday)
                            promocionesAplicables.Add(promo);
                        break;

                    case 3: // Cliente VIP
                        if (pedido.Cliente?.EsVip == true)
                            promocionesAplicables.Add(promo);
                        break;

                    case 4: // Más de 5 pedidos este mes
                        var pedidosEsteMes = await _context.Pedidos
                            .Where(p =>
                                p.ClienteId == pedido.ClienteId &&
                                p.FechaPedido.HasValue &&
                                p.FechaPedido.Value.Month == DateTime.Now.Month &&
                                p.FechaPedido.Value.Year == DateTime.Now.Year)
                            .CountAsync();
                        if (pedidosEsteMes >= 5)
                            promocionesAplicables.Add(promo);
                        break;

                    case 5: // Cumpleaños
                        if (pedido.Cliente?.FechaNacimiento.HasValue == true && pedido.Cliente?.FechaNacimiento == DateOnly.FromDateTime(DateTime.Today))
                            promocionesAplicables.Add(promo);
                        break;

                    case 6: // Código Promo (este podrías validarlo en otro lado si se ingresa un código)
                        promocionesAplicables.Add(promo); // siempre disponible por fecha
                        break;
                }
            }

            var promosMapeadas = _mapper.Map<List<PromocionViewModel>>(promocionesAplicables);
            return Ok(promosMapeadas);
        }
        [HttpPut("{id}/asignar_promocion")]
        public async Task<IActionResult> AsignarPromocion(int id, [FromBody] AsignarPromocionDto dto)
        {
            var pedido = await _context.Pedidos
                .Include(p => p.Mesa)
                .Include(p => p.Promocion)
                .Include(p => p.DetallesPedidos)
                    .ThenInclude(dp => dp.Producto)
                .Include(p => p.PedidoCombos)
                    .ThenInclude(pc => pc.Combo)
                .Include(p => p.PedidoCombos)
                    .ThenInclude(pc => pc.PedidoComboItems)
                        .ThenInclude(pci => pci.Producto)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (pedido == null)
                return NotFound();

            // Calcular el total sin descuento
            pedido.TotalSinDescuento = pedido.DetallesPedidos.Sum(d => d.Cantidad * d.PrecioUnitario);
            if (pedido.PedidoCombos.Any())
                pedido.TotalSinDescuento += pedido.PedidoCombos.Sum(c => c.Cantidad * c.Combo.Precio);

            // Si no se seleccionó ninguna promoción, quitamos la promo y el descuento
            if (dto.PromocionId == 0)
            {
                pedido.PromocionId = null;
                pedido.MontoDescuento = 0;
                pedido.Total = pedido.TotalSinDescuento;
            }
            else
            {
                pedido.Promocion = await _context.Promociones.FirstOrDefaultAsync(p => p.Id == dto.PromocionId);
                pedido.PromocionId = dto.PromocionId;
                // Aplicar lógica de promoción
                (pedido.MontoDescuento, pedido.Total) = CalcularDescuentoYTotal(pedido);
            }

            await _context.SaveChangesAsync();

            return NoContent();
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
        public class AsignarPromocionDto
        {
            public int PromocionId { get; set; }
        }

    }

}
