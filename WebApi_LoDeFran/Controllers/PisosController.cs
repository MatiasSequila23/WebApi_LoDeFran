using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApi_LoDeFran.Models;
using WebApi_LoDeFran.ViewModels;

namespace WebApi_LoDeFran.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PisosController : ControllerBase
    {
        private readonly LoDeFranContext _context;
        private readonly IMapper _mapper;

        public PisosController(LoDeFranContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PisoViewModel>>> GetPisos()
        {
            var pisos = await _context.Pisos.ToListAsync();
            var pisosVM = _mapper.Map<List<PisoViewModel>>(pisos);
            return Ok(pisosVM);
        }
        // GET: api/pisos/5
        [HttpGet("{id}")]
        public async Task<ActionResult<PisoViewModel>> GetPiso(int id)
        {
            var piso = await _context.Pisos.FindAsync(id);
            if (piso == null)
                return NotFound();

            return Ok(_mapper.Map<PisoViewModel>(piso));
        }

        // POST: api/pisos
        [HttpPost]
        public async Task<ActionResult<PisoViewModel>> CreatePiso(PisoViewModel pisoVM)
        {
            var piso = _mapper.Map<Piso>(pisoVM);
            _context.Pisos.Add(piso);
            await _context.SaveChangesAsync();

            pisoVM.IdPiso = piso.IdPiso;

            return CreatedAtAction(nameof(GetPiso), new { id = piso.IdPiso }, pisoVM);
        }

        // PUT: api/pisos/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePiso(int id, PisoViewModel pisoVM)
        {
            if (id != pisoVM.IdPiso)
                return BadRequest("ID de piso incorrecto.");

            var piso = await _context.Pisos.FindAsync(id);
            if (piso == null)
                return NotFound();

            _mapper.Map(pisoVM, piso);
            _context.Entry(piso).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/pisos/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePiso(int id)
        {
            var piso = await _context.Pisos.FindAsync(id);
            if (piso == null)
                return NotFound();

            _context.Pisos.Remove(piso);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpGet("{id}/mesas")]
        public async Task<ActionResult<IEnumerable<MesaViewModel>>> GetMesasPorPiso(int id)
        {
            var pisoExiste = await _context.Pisos.AnyAsync(p => p.IdPiso == id);
            if (!pisoExiste)
            {
                return NotFound($"No existe un piso con ID {id}");
            }

            var mesas = await _context.Mesas
                .Where(m => m.IdPiso == id)
                .Include(m => m.IdEstadoNavigation)
                .Include(m => m.IdPisoNavigation)
                .ToListAsync();

            var mesasVM = _mapper.Map<List<MesaViewModel>>(mesas);

            return Ok(mesasVM);
        }
    }
}
