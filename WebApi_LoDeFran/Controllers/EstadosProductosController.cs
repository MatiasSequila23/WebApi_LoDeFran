using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApi_LoDeFran.Models;
using WebApi_LoDeFran.ViewModels;

namespace WebApi_LoDeFran.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EstadosProductosController : ControllerBase
    {
        private readonly LoDeFranContext _context;
        private readonly IMapper _mapper;

        public EstadosProductosController(LoDeFranContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/EstadosProducto
        [HttpGet]
        public async Task<ActionResult<IEnumerable<EstadoProductoViewModel>>> GetEstadosProducto()
        {
            var estados = await _context.EstadosProductos.ToListAsync();
            var vm = _mapper.Map<List<EstadoProductoViewModel>>(estados);
            return Ok(vm);
        }

        // GET: api/EstadosProducto/5
        [HttpGet("{id}")]
        public async Task<ActionResult<EstadoProductoViewModel>> GetEstadoProducto(int id)
        {
            var estado = await _context.EstadosProductos.FindAsync(id);
            if (estado == null) return NotFound();

            return Ok(_mapper.Map<EstadoProductoViewModel>(estado));
        }

        // POST: api/EstadosProducto
        [HttpPost]
        public async Task<ActionResult<EstadoProductoViewModel>> PostEstadoProducto(EstadoProductoViewModel viewModel)
        {
            var estado = _mapper.Map<EstadosProducto>(viewModel);
            _context.EstadosProductos.Add(estado);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetEstadoProducto), new { id = estado.Id }, _mapper.Map<EstadoProductoViewModel>(estado));
        }

        // PUT: api/EstadosProducto/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEstadoProducto(int id, EstadoProductoViewModel viewModel)
        {
            if (id != viewModel.Id) return BadRequest();

            var estado = await _context.EstadosProductos.FindAsync(id);
            if (estado == null) return NotFound();

            _mapper.Map(viewModel, estado);
            _context.Entry(estado).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/EstadosProducto/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEstadoProducto(int id)
        {
            var estado = await _context.EstadosProductos.FindAsync(id);
            if (estado == null) return NotFound();

            _context.EstadosProductos.Remove(estado);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }

}
