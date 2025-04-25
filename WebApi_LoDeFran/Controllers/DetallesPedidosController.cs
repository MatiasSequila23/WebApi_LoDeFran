using AutoMapper;
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
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDetallesPedido(int id)
        {
            var detallesPedido = await _context.DetallesPedidos.FindAsync(id);
            if (detallesPedido == null)
                return NotFound();

            _context.DetallesPedidos.Remove(detallesPedido);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
