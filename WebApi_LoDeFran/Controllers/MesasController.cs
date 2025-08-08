using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApi_LoDeFran.Models;
using WebApi_LoDeFran.ViewModels;
using WebApi_LoDeFran.Utlis.ClassAux;
using WebApi_LoDeFran.Utlis;

namespace WebApi_LoDeFran.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MesasController : ControllerBase
    {
        private readonly LoDeFranContext _context;
        private readonly IMapper _mapper;

        public MesasController(LoDeFranContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/Mesas
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MesaViewModel>>> GetMesas()
        {
            var mesas = await _context.Mesas
                .Include(m => m.IdEstadoNavigation)
                .Include(m => m.IdPisoNavigation)
                .ToListAsync();

            var mesasVM = _mapper.Map<List<MesaViewModel>>(mesas);
            return Ok(mesasVM);
        }

        // GET: api/Mesas/5
        [HttpGet("{id}")]
        public async Task<ActionResult<MesaViewModel>> GetMesa(int id)
        {
            var mesa = await _context.Mesas
                .Include(m => m.IdEstadoNavigation)
                .Include(m => m.IdPisoNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (mesa == null)
                return NotFound();

            var mesaVM = _mapper.Map<MesaViewModel>(mesa);
            return Ok(mesaVM);
        }

        // POST: api/Mesas
        [HttpPost]
        public async Task<ActionResult<MesaViewModel>> PostMesa(MesaViewModel mesaVM)
        {
            var mesa = _mapper.Map<Mesa>(mesaVM);
            _context.Mesas.Add(mesa);
            await _context.SaveChangesAsync();

            // Volver a incluir navegación para devolver el objeto completo
            var nuevaMesa = await _context.Mesas
                .Include(m => m.IdEstadoNavigation)
                .Include(m => m.IdPisoNavigation)
                .FirstOrDefaultAsync(m => m.Id == mesa.Id);

            var nuevoVM = _mapper.Map<MesaViewModel>(nuevaMesa);
            return CreatedAtAction(nameof(GetMesa), new { id = mesa.Id }, nuevoVM);
        }

        // PUT: api/Mesas/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMesa(int id, MesaViewModel mesaVM)
        {
            if (id != mesaVM.Id)
                return BadRequest();

            var mesa = await _context.Mesas.FindAsync(id);
            if (mesa == null)
                return NotFound();

            _mapper.Map(mesaVM, mesa);
            _context.Entry(mesa).State = EntityState.Modified;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        // DELETE: api/Mesas/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMesa(int id)
        {
            var mesa = await _context.Mesas.FindAsync(id);
            if (mesa == null)
                return NotFound();

            _context.Mesas.Remove(mesa);
            await _context.SaveChangesAsync();

            return NoContent();
        }
        // GET: api/Mesas/disponibles
        [HttpGet("disponibles")]
        public async Task<ActionResult<IEnumerable<MesaViewModel>>> GetMesasDisponibles()
        {
            var mesasDisponibles = await _context.Mesas
                .Include(m => m.IdEstadoNavigation)
                .Include(m => m.IdPisoNavigation)
                .Where(m => m.IdEstadoNavigation.Nombre == "Disponible") // Ajustá si usás ID
                .ToListAsync();

            var mesasVM = _mapper.Map<List<MesaViewModel>>(mesasDisponibles);
            return Ok(mesasVM);
        }
        // PUT: api/Mesas/{id}/estado
        [HttpPut("{id}/estado")]
        public async Task<IActionResult> CambiarEstadoMesa(int id, [FromBody] Enums.EstadoMesa nuevoEstado)
        {
            var mesa = await _context.Mesas.FindAsync(id);
            if (mesa == null)
                return NotFound($"No se encontró la mesa con ID {id}");

            mesa.IdEstado = (int)nuevoEstado;

            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
    
}
