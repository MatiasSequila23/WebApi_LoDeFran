using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApi_LoDeFran.Models;
using WebApi_LoDeFran.ViewModels;

namespace WebApi_LoDeFran.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MovimientosCajaController : ControllerBase
    {
        private readonly LoDeFranContext _context;
        private readonly IMapper _mapper;

        public MovimientosCajaController(LoDeFranContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/MovimientosCaja/caja/5
        [HttpGet("caja/{cajaId}")]
        public async Task<ActionResult<IEnumerable<MovimientoCajaViewModel>>> GetMovimientosPorCaja(int cajaId)
        {
            var movimientos = await _context.MovimientoCajas
                .Include(m => m.MetodoPago)
                .Include(m => m.MotivoMovimiento)
                    .ThenInclude(mm => mm.TipoMovimiento)
                .Where(m => m.CajaId == cajaId)
                .OrderByDescending(m => m.FechaMovimiento)
                .ToListAsync();

            var movimientosVM = _mapper.Map<List<MovimientoCajaViewModel>>(movimientos);
            return Ok(movimientosVM);
        }

        // GET: api/MovimientosCaja/5
        [HttpGet("{id}")]
        public async Task<ActionResult<MovimientoCajaViewModel>> GetMovimiento(int id)
        {
            var movimiento = await _context.MovimientoCajas
                .Include(m => m.MetodoPago)
                .Include(m => m.MotivoMovimiento)
                    .ThenInclude(mm => mm.TipoMovimiento)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (movimiento == null)
                return NotFound();

            return Ok(_mapper.Map<MovimientoCajaViewModel>(movimiento));
        }

        // POST: api/MovimientosCaja
        [HttpPost]
        public async Task<ActionResult<MovimientoCajaViewModel>> PostMovimiento(MovimientoCajaViewModel movimientoVM)
        {
            var movimiento = _mapper.Map<MovimientoCaja>(movimientoVM);
            _context.MovimientoCajas.Add(movimiento);
            await _context.SaveChangesAsync();

            // Recargar con includes
            await _context.Entry(movimiento).Reference(m => m.MotivoMovimiento).LoadAsync();
            await _context.Entry(movimiento.MotivoMovimiento!).Reference(m => m.TipoMovimiento).LoadAsync();
            await _context.Entry(movimiento).Reference(m => m.MetodoPago).LoadAsync();

            var nuevoVM = _mapper.Map<MovimientoCajaViewModel>(movimiento);
            return CreatedAtAction(nameof(GetMovimiento), new { id = movimiento.Id }, nuevoVM);
        }

        // PUT: api/MovimientosCaja/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMovimiento(int id, MovimientoCajaViewModel movimientoVM)
        {
            if (id != movimientoVM.Id)
                return BadRequest();

            var movimiento = await _context.MovimientoCajas.FindAsync(id);
            if (movimiento == null)
                return NotFound();

            _mapper.Map(movimientoVM, movimiento);
            _context.Entry(movimiento).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/MovimientosCaja/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMovimiento(int id)
        {
            var movimiento = await _context.MovimientoCajas.FindAsync(id);
            if (movimiento == null)
                return NotFound();

            _context.MovimientoCajas.Remove(movimiento);
            await _context.SaveChangesAsync();

            return NoContent();
        }
        [HttpGet]
        public async Task<ActionResult<List<MovimientoCajaViewModel>>> GetMovimientos()
        {
            var movimientos = await _context.MovimientoCajas
                .Include(m => m.MotivoMovimiento)
                    .ThenInclude(mm => mm.TipoMovimiento)
                .Include(m => m.MetodoPago)
                .Include(m => m.Caja)
                    .ThenInclude(c => c.Usuario)
                .ToListAsync();

            var resultado = _mapper.Map<List<MovimientoCajaViewModel>>(movimientos);
            return Ok(resultado);
        }
        [HttpGet("filtrar")]
        public async Task<ActionResult<List<MovimientoCajaViewModel>>> FiltrarMovimientos(
            [FromQuery] int? cajaId,
            [FromQuery] int? metodoPagoId,
            [FromQuery] int? motivoId,
            [FromQuery] DateTime? desde,
            [FromQuery] DateTime? hasta)
        {
            var query = _context.MovimientoCajas
                .Include(m => m.Caja).ThenInclude(c => c.Usuario)
                .Include(m => m.MetodoPago)
                .Include(m => m.MotivoMovimiento).ThenInclude(m => m.TipoMovimiento)
                .AsQueryable();

            if (cajaId.HasValue) query = query.Where(m => m.CajaId == cajaId.Value);
            if (metodoPagoId.HasValue) query = query.Where(m => m.MetodoPagoId == metodoPagoId.Value);
            if (motivoId.HasValue) query = query.Where(m => m.MotivoMovimientoId == motivoId.Value);
            if (desde.HasValue) query = query.Where(m => m.FechaMovimiento >= desde.Value);
            if (hasta.HasValue) query = query.Where(m => m.FechaMovimiento <= hasta.Value);

            var resultado = await query.ToListAsync();
            return Ok(_mapper.Map<List<MovimientoCajaViewModel>>(resultado));
        }

    }
}
