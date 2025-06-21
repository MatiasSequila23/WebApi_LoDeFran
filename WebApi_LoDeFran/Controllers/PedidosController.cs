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

}
