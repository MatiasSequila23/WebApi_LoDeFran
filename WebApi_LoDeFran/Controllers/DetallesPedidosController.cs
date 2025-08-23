

using AutoMapper;
using DocumentFormat.OpenXml.Office2010.Excel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApi_LoDeFran.Models;
using WebApi_LoDeFran.ViewModels;

namespace WebApi_LoDeFran.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DetallesPedidosController : Controller
    {
        private readonly LoDeFranContext _context;
        private readonly IMapper _mapper;

        public DetallesPedidosController(LoDeFranContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/DetallesPedidos
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DetallePedidoViewModel>>> GetDetallesPedidos()
        {
            var detallesPedidos = await _context.DetallesPedidos
            .Include(dp => dp.Producto)
            .Include(dp => dp.Pedido)
            .Include(dp => dp.EstadoCocina)
                .ToListAsync();

            var detallesPedidosVM = _mapper.Map<List<DetallePedidoViewModel>>(detallesPedidos);
            return Ok(detallesPedidosVM);
        }

        // GET: api/DetallesPedidos/5
        [HttpGet("{id}")]
        public async Task<ActionResult<DetallePedidoViewModel>> GetDetallesPedido(int id)
        {
            var detallesPedido = await _context.DetallesPedidos
                .Include(dp => dp.Producto)
                .Include(dp => dp.Pedido)
                .Include(dp => dp.EstadoCocina)
                .FirstOrDefaultAsync(dp => dp.Id == id);

            if (detallesPedido == null)
                return NotFound();

            var detallesPedidoVM = _mapper.Map<DetallePedidoViewModel>(detallesPedido);
            return Ok(detallesPedidoVM);
        }

        // POST: api/DetallesPedidos
        [HttpPost]
        public async Task<ActionResult<DetallePedidoViewModel>> PostDetallesPedido(DetallePedidoViewModel detallesPedidoVM)
        {
            var detallesPedido = _mapper.Map<DetallesPedido>(detallesPedidoVM);

            _context.DetallesPedidos.Add(detallesPedido);
            await _context.SaveChangesAsync();

            var nuevoVM = _mapper.Map<DetallePedidoViewModel>(detallesPedido);
            return CreatedAtAction(nameof(GetDetallesPedido), new { id = detallesPedido.Id }, nuevoVM);
        }

        // PUT: api/DetallesPedidos/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDetallesPedido(int id, DetallePedidoViewModel detallesPedidoVM)
        {
            if (id != detallesPedidoVM.Id)
                return BadRequest();

            var detallesPedido = await _context.DetallesPedidos.FindAsync(id);
            if (detallesPedido == null)
                return NotFound();

            _mapper.Map(detallesPedidoVM, detallesPedido);
            _context.Entry(detallesPedido).State = EntityState.Modified;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        // DELETE: api/DetallesPedidos/5
        [HttpDelete("pedidos/{idPedido}/detalle/{idDetalle}")]
        public async Task<IActionResult> DeleteDetalle(int idPedido, int idDetalle)
        {
            // Traer el pedido junto con los detalles y combos
            var pedido = await _context.Pedidos
              .Include(p => p.DetallesPedidos)
              .Include(p => p.Promocion)
              .Include(p => p.PedidoCombos)
                  .ThenInclude(pc => pc.Combo) // <-- Importante
              .Include(p => p.PedidoCombos)
                  .ThenInclude(pc => pc.PedidoComboItems)
              .Include(p => p.Promocion)
              .FirstOrDefaultAsync(p => p.Id == idPedido);

            if (pedido == null)
                return NotFound();

            var detalle = pedido.DetallesPedidos.FirstOrDefault(d => d.Id == idDetalle);
            if (detalle == null)
                return NotFound();

            // Eliminar el detalle primero
            _context.DetallesPedidos.Remove(detalle);
            await _context.SaveChangesAsync();

            // Recalcular totales luego de la eliminación
            (pedido.MontoDescuento, pedido.Total) = CalcularDescuentoYTotal(pedido);
            pedido.TotalSinDescuento = pedido.MontoDescuento + pedido.Total;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpPost("{id}/agregar-producto")]
        public async Task<IActionResult> AgregarProducto(int id, [FromBody] ProductoPedidoDto dto)
        {
            var pedido = await _context.Pedidos
                .Include(p => p.DetallesPedidos)
                .Include(p => p.Promocion)
                .Include(p => p.PedidoCombos)
                    .ThenInclude(pc => pc.Combo) // <-- Importante
                .Include(p => p.PedidoCombos)
                    .ThenInclude(pc => pc.PedidoComboItems)
                .Include(p => p.Promocion)
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
                    PrecioUnitario = producto.Precio,
                    EstadoCocinaId = 1 // Estado inicial (por ejemplo, "Pendiente" o "En cocina")
                });
            }

            // Recalcular total sin descuento
           // pedido.TotalSinDescuento = pedido.DetallesPedidos.Sum(d => d.Cantidad * d.PrecioUnitario);

            // Aplicar lógica para calcular descuento y total según promoción
            (pedido.MontoDescuento, pedido.Total) = CalcularDescuentoYTotal(pedido);
            pedido.TotalSinDescuento = pedido.MontoDescuento + pedido.Total;
            await _context.SaveChangesAsync();
            return NoContent();
        }
        [HttpPut("comentario/{id}")]
        public async Task<IActionResult> ActualizarComentario(int id, [FromBody] string comentario)
        {
            var detalle = await _context.DetallesPedidos.FindAsync(id);

            if (detalle == null)
                return NotFound();

            detalle.Comentario = comentario;
            _context.Entry(detalle).Property(d => d.Comentario).IsModified = true;

            await _context.SaveChangesAsync();

            return NoContent();
        }
        [HttpPut("detalle/{idDetalle}/estado-cocina")]
        public async Task<IActionResult> CambiarEstadoCocina(int idDetalle, [FromBody] int nuevoEstadoId)
        {
            var detalle = await _context.DetallesPedidos.FindAsync(idDetalle);
            if (detalle == null) return NotFound();

            detalle.EstadoCocinaId = nuevoEstadoId;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpPut("detalle/{idDetalle}/cantidad")]
        public async Task<IActionResult> ModificarCantidad(int idDetalle, [FromBody] int nuevaCantidad)
        {
            var detalle = await _context.DetallesPedidos.FindAsync(idDetalle);
            if (detalle == null) return NotFound();

            detalle.Cantidad = nuevaCantidad;
            await _context.SaveChangesAsync();

            // Obtener pedido con detalles y promocion para recalcular totales
            var pedido = await _context.Pedidos
                .Include(p => p.DetallesPedidos)
                .Include(p => p.Promocion)
                .Include(p => p.PedidoCombos)
                    .ThenInclude(pc => pc.Combo) // <-- Importante
                .Include(p => p.PedidoCombos)
                    .ThenInclude(pc => pc.PedidoComboItems)
            .Include(p => p.Promocion)
                .FirstOrDefaultAsync(p => p.Id == detalle.PedidoId);

            if (pedido == null) return NotFound();

            (pedido.MontoDescuento, pedido.Total) = CalcularDescuentoYTotal(pedido);
            pedido.TotalSinDescuento = pedido.MontoDescuento + pedido.Total;

            await _context.SaveChangesAsync();

            return NoContent();
        }
        [HttpPut("detalle/{idDetalle}/cantidad-confirmada")]
        public async Task<IActionResult> ModificarCantidadConfirmada(int idDetalle, [FromBody] int nuevaCantidadConfirm)
        {
            var detalle = await _context.DetallesPedidos.FindAsync(idDetalle);
            if (detalle == null) return NotFound();

            detalle.CantidadConfirmada = nuevaCantidadConfirm;
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
    }
}
