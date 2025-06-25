using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApi_LoDeFran.Models;
using WebApi_LoDeFran.ViewModels;

namespace WebApi_LoDeFran.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MovimientoCajaController : Controller
    {
        private readonly LoDeFranContext _context;
        private readonly IMapper _mapper;

        public MovimientoCajaController(LoDeFranContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MovimientoCajaViewModel>>> GetMovimientos()
        {
            var movimientos = await _context.MovimientoCajas.Include(m => m.MetodoPago).ToListAsync();
            return Ok(_mapper.Map<List<MovimientoCajaViewModel>>(movimientos));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<MovimientoCajaViewModel>> GetMovimiento(int id)
        {
            var movimiento = await _context.MovimientoCajas.Include(m => m.MetodoPago).FirstOrDefaultAsync(m => m.Id == id);
            if (movimiento == null) return NotFound();
            return Ok(_mapper.Map<MovimientoCajaViewModel>(movimiento));
        }

        [HttpPost]
        public async Task<ActionResult<MovimientoCajaViewModel>> PostMovimiento(MovimientoCajaViewModel movimientoVM)
        {
            var movimiento = _mapper.Map<MovimientoCaja>(movimientoVM);
            _context.MovimientoCajas.Add(movimiento);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetMovimiento), new { id = movimiento.Id }, _mapper.Map<MovimientoCajaViewModel>(movimiento));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutMovimiento(int id, MovimientoCajaViewModel movimientoVM)
        {
            if (id != movimientoVM.Id) return BadRequest();
            var movimiento = await _context.MovimientoCajas.FindAsync(id);
            if (movimiento == null) return NotFound();
            _mapper.Map(movimientoVM, movimiento);
            _context.Entry(movimiento).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMovimiento(int id)
        {
            var movimiento = await _context.MovimientoCajas.FindAsync(id);
            if (movimiento == null) return NotFound();
            _context.MovimientoCajas.Remove(movimiento);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
