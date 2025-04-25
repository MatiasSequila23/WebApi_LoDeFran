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
                //.Include(p => p.Estado)
                .Include(p => p.DetallesPedidos)
                .ThenInclude(dp => dp.Producto)  // Incluir los productos dentro del detalle
                .ToListAsync();

            var pedidosVM = _mapper.Map<List<PedidoViewModel>>(pedidos);
            return Ok(pedidosVM);
        }

        // GET: api/Pedidos/5
        [HttpGet("{id}")]
        public async Task<ActionResult<PedidoViewModel>> GetPedido(int id)
        {
            var pedido = await _context.Pedidos
                //.Include(p => p.Estado)
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
    }


}
