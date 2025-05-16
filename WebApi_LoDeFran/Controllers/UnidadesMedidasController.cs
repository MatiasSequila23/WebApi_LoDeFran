using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApi_LoDeFran.Models;
using WebApi_LoDeFran.ViewModels;

namespace WebApi_LoDeFran.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UnidadesMedidasController : Controller
    {
        private readonly LoDeFranContext _context;
        private readonly IMapper _mapper;

        public UnidadesMedidasController(LoDeFranContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/UnidadMedida
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UnidadMedidaViewModel>>> GetUnidadMedidas()
        {
            var unidades = await _context.UnidadMedida.ToListAsync();
            return Ok(_mapper.Map<List<UnidadMedidaViewModel>>(unidades));
        }

        // GET: api/UnidadMedida/5
        [HttpGet("{id}")]
        public async Task<ActionResult<UnidadMedidaViewModel>> GetUnidadMedida(int id)
        {
            var unidad = await _context.UnidadMedida.FindAsync(id);
            if (unidad == null) return NotFound();

            return Ok(_mapper.Map<UnidadMedidaViewModel>(unidad));
        }

        // POST: api/UnidadMedida
        [HttpPost]
        public async Task<ActionResult<UnidadMedidaViewModel>> PostUnidadMedida(UnidadMedidaViewModel viewModel)
        {
            var unidad = _mapper.Map<UnidadMedidum>(viewModel);
            _context.UnidadMedida.Add(unidad);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetUnidadMedida), new { id = unidad.Id }, _mapper.Map<UnidadMedidaViewModel>(unidad));
        }

        // PUT: api/UnidadMedida/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUnidadMedida(int id, UnidadMedidaViewModel viewModel)
        {
            if (id != viewModel.Id) return BadRequest();

            var unidad = await _context.UnidadMedida.FindAsync(id);
            if (unidad == null) return NotFound();

            _mapper.Map(viewModel, unidad);
            _context.Entry(unidad).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/UnidadMedida/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUnidadMedida(int id)
        {
            var unidad = await _context.UnidadMedida.FindAsync(id);
            if (unidad == null) return NotFound();

            _context.UnidadMedida.Remove(unidad);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
