using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApi_LoDeFran.Models;
using WebApi_LoDeFran.ViewModels;

namespace WebApi_LoDeFran.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EstadosInsumosController : Controller
    {
        private readonly LoDeFranContext _context;
        private readonly IMapper _mapper;

        public EstadosInsumosController(LoDeFranContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/EstadosProducto
        [HttpGet]
        public async Task<ActionResult<IEnumerable<EstadoProductoViewModel>>> GetEstadosInsumos()
        {
            var estados = await _context.EstadosInsumos.ToListAsync();
            var vm = _mapper.Map<List<EstadoInsumoViewModel>>(estados);
            return Ok(vm);
        }

        // GET: api/EstadosProducto/5
        [HttpGet("{id}")]
        public async Task<ActionResult<EstadoInsumoViewModel>> GetEstadoInsumo(int id)
        {
            var estado = await _context.EstadosInsumos.FindAsync(id);
            if (estado == null) return NotFound();

            return Ok(_mapper.Map<EstadoInsumoViewModel>(estado));
        }

        // POST: api/EstadosProducto
        [HttpPost]
        public async Task<ActionResult<EstadoInsumoViewModel>> PostEstadoInsumo(EstadoInsumoViewModel viewModel)
        {
            var estado = _mapper.Map<EstadosInsumo>(viewModel);
            _context.EstadosInsumos.Add(estado);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetEstadoInsumo), new { id = estado.Id }, _mapper.Map<EstadoInsumoViewModel>(estado));
        }

        // PUT: api/EstadosProducto/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEstadoInsumo(int id, EstadoInsumoViewModel viewModel)
        {
            if (id != viewModel.Id) return BadRequest();

            var estado = await _context.EstadosInsumos.FindAsync(id);
            if (estado == null) return NotFound();

            _mapper.Map(viewModel, estado);
            _context.Entry(estado).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/EstadosProducto/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEstadoInsumo(int id)
        {
            var estado = await _context.EstadosInsumos.FindAsync(id);
            if (estado == null) return NotFound();

            _context.EstadosInsumos.Remove(estado);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
