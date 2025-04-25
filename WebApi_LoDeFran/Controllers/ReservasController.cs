using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApi_LoDeFran.Models;
using WebApi_LoDeFran.ViewModels;

namespace WebApi_LoDeFran.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReservasController : ControllerBase
    {
        private readonly LoDeFranContext _context;
        private readonly IMapper _mapper;

        public ReservasController(LoDeFranContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/Reservas
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ReservaViewModel>>> GetReservas()
        {
            var reservas = await _context.Reservas
                .Include(r => r.Cliente)
                .Include(r => r.Mesa)
                .ToListAsync();

            var reservasVM = _mapper.Map<List<ReservaViewModel>>(reservas);
            return Ok(reservasVM);
        }

        // GET: api/Reservas/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ReservaViewModel>> GetReserva(int id)
        {
            var reserva = await _context.Reservas
                .Include(r => r.Cliente)
                .Include(r => r.Mesa)
                .FirstOrDefaultAsync(r => r.Id == id);

            if (reserva == null)
                return NotFound();

            var reservaVM = _mapper.Map<ReservaViewModel>(reserva);
            return Ok(reservaVM);
        }

        // POST: api/Reservas
        [HttpPost]
        public async Task<ActionResult<ReservaViewModel>> PostReserva(ReservaViewModel reservaVM)
        {
            var reserva = _mapper.Map<Reserva>(reservaVM);
            _context.Reservas.Add(reserva);
            await _context.SaveChangesAsync();

            var nuevoVM = _mapper.Map<ReservaViewModel>(reserva);
            return CreatedAtAction(nameof(GetReserva), new { id = reserva.Id }, nuevoVM);
        }

        // PUT: api/Reservas/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutReserva(int id, ReservaViewModel reservaVM)
        {
            if (id != reservaVM.Id)
                return BadRequest();

            var reserva = await _context.Reservas.FindAsync(id);
            if (reserva == null)
                return NotFound();

            _mapper.Map(reservaVM, reserva);
            _context.Entry(reserva).State = EntityState.Modified;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        // DELETE: api/Reservas/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteReserva(int id)
        {
            var reserva = await _context.Reservas.FindAsync(id);
            if (reserva == null)
                return NotFound();

            _context.Reservas.Remove(reserva);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }

}
