using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using WebApi_LoDeFran.Models;
using WebApi_LoDeFran.ViewModels;
using Microsoft.EntityFrameworkCore;


namespace WebApi_LoDeFran.Controllers
{
    public class CajaController : ControllerBase
    {
        private readonly LoDeFranContext _context;
        private readonly IMapper _mapper;

        public CajaController(LoDeFranContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CajaViewModel>>> GetCajas()
        {
            var cajas = await _context.Cajas.Include(c => c.Usuario).Include(c => c.MovimientoCajas).ToListAsync();
            return Ok(_mapper.Map<List<CajaViewModel>>(cajas));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CajaViewModel>> GetCaja(int id)
        {
            var caja = await _context.Cajas.Include(c => c.Usuario).Include(c => c.MovimientoCajas).FirstOrDefaultAsync(c => c.Id == id);
            if (caja == null) return NotFound();
            return Ok(_mapper.Map<CajaViewModel>(caja));
        }

        [HttpPost]
        public async Task<ActionResult<CajaViewModel>> PostCaja(CajaViewModel cajaVM)
        {
            var caja = _mapper.Map<Caja>(cajaVM);
            _context.Cajas.Add(caja);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetCaja), new { id = caja.Id }, _mapper.Map<CajaViewModel>(caja));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutCaja(int id, CajaViewModel cajaVM)
        {
            if (id != cajaVM.Id) return BadRequest();
            var caja = await _context.Cajas.FindAsync(id);
            if (caja == null) return NotFound();
            _mapper.Map(cajaVM, caja);
            _context.Entry(caja).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCaja(int id)
        {
            var caja = await _context.Cajas.FindAsync(id);
            if (caja == null) return NotFound();
            _context.Cajas.Remove(caja);
            await _context.SaveChangesAsync();
            return NoContent();
        }
        [HttpPost("abrir")]
        public async Task<ActionResult<CajaViewModel>> AbrirCaja(AbrirCajaViewModel datos)
        {
            // Validar que el usuario no tenga una caja abierta
            var cajaAbierta = await _context.Cajas
                .FirstOrDefaultAsync(c => c.UsuarioId == datos.UsuarioId && c.Estado == "abierta");

            if (cajaAbierta != null)
            {
                return BadRequest("Ya hay una caja abierta para este usuario.");
            }

            var nuevaCaja = new Caja
            {
                UsuarioId = datos.UsuarioId,
                FechaApertura = DateTime.Now,
                MontoInicial = datos.MontoInicial,
                Estado = "abierta"
            };

            _context.Cajas.Add(nuevaCaja);
            await _context.SaveChangesAsync();

            var result = _mapper.Map<CajaViewModel>(nuevaCaja);
            return CreatedAtAction(nameof(GetCaja), new { id = nuevaCaja.Id }, result);
        }
        // PATCH: api/Cajas/5/cerrar
        [HttpPatch("{id}/cerrar")]
        public async Task<IActionResult> CerrarCaja(int id)
        {
            var caja = await _context.Cajas.Include(c => c.MovimientoCajas).FirstOrDefaultAsync(c => c.Id == id);
            if (caja == null || caja.Estado != "abierta") return NotFound();

            caja.FechaCierre = DateTime.Now;
            caja.MontoFinal = caja.MovimientoCajas.Sum(m => m.Monto) + caja.MontoInicial;
            caja.Estado = "cerrada";

            await _context.SaveChangesAsync();

            return NoContent();
        }
        // GET: api/Cajas/abierta/usuario/5
        [HttpGet("abierta/usuario/{usuarioId}")]
        public async Task<ActionResult<CajaViewModel>> GetCajaAbierta(int usuarioId)
        {
            var caja = await _context.Cajas
                .Include(c => c.Usuario)
                .Include(c => c.MovimientoCajas)
                .FirstOrDefaultAsync(c => c.UsuarioId == usuarioId && c.Estado == "abierta");

            if (caja == null) return Ok(null);

            return Ok(_mapper.Map<CajaViewModel>(caja));
        }
    }
    public class AbrirCajaViewModel
    {
        public int UsuarioId { get; set; }
        public decimal MontoInicial { get; set; }
    }


}
