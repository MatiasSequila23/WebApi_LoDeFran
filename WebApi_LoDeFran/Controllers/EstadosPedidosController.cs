using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApi_LoDeFran.Models;
using WebApi_LoDeFran.ViewModels;

namespace WebApi_LoDeFran.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EstadosPedidosController : ControllerBase
    {
        private readonly LoDeFranContext _context;
        private readonly IMapper _mapper;

        public EstadosPedidosController(LoDeFranContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/EstadosPedido
        [HttpGet]
        public async Task<ActionResult<IEnumerable<EstadoPedidoViewModel>>> GetEstadosPedido()
        {
            var estados = await _context.EstadosPedidos.ToListAsync();
            var vm = _mapper.Map<List<EstadoPedidoViewModel>>(estados);
            return Ok(vm);
        }

        // GET: api/EstadosPedido/5
        [HttpGet("{id}")]
        public async Task<ActionResult<EstadoPedidoViewModel>> GetEstadoPedido(int id)
        {
            var estado = await _context.EstadosPedidos.FindAsync(id);
            if (estado == null) return NotFound();

            return Ok(_mapper.Map<EstadoPedidoViewModel>(estado));
        }

        // POST: api/EstadosPedido
        [HttpPost]
        public async Task<ActionResult<EstadoPedidoViewModel>> PostEstadoPedido(EstadoPedidoViewModel viewModel)
        {
            var estado = _mapper.Map<EstadosPedido>(viewModel);
            _context.EstadosPedidos.Add(estado);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetEstadoPedido), new { id = estado.Id }, _mapper.Map<EstadoPedidoViewModel>(estado));
        }

        // PUT: api/EstadosPedido/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEstadoPedido(int id, EstadoPedidoViewModel viewModel)
        {
            if (id != viewModel.Id) return BadRequest();

            var estado = await _context.EstadosPedidos.FindAsync(id);
            if (estado == null) return NotFound();

            _mapper.Map(viewModel, estado);
            _context.Entry(estado).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/EstadosPedido/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEstadoPedido(int id)
        {
            var estado = await _context.EstadosPedidos.FindAsync(id);
            if (estado == null) return NotFound();

            _context.EstadosPedidos.Remove(estado);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }

}
