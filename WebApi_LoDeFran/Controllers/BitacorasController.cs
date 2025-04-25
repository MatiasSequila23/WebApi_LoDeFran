using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using WebApi_LoDeFran.Models;
using WebApi_LoDeFran.ViewModels;
using Microsoft.EntityFrameworkCore;


namespace WebApi_LoDeFran.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BitacoraController : ControllerBase
    {
        private readonly LoDeFranContext _context;
        private readonly IMapper _mapper;

        public BitacoraController(LoDeFranContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/Bitacora
        [HttpGet]
        public async Task<ActionResult<IEnumerable<BitacoraViewModel>>> GetBitacora()
        {
            var registros = await _context.Bitacoras
                .Include(b => b.UsuarioId)
                .OrderByDescending(b => b.Fecha)
                .ToListAsync();

            var viewModels = _mapper.Map<List<BitacoraViewModel>>(registros);
            return Ok(viewModels);
        }

        // GET: api/Bitacora/usuario/5
        [HttpGet("usuario/{usuarioId}")]
        public async Task<ActionResult<IEnumerable<BitacoraViewModel>>> GetBitacoraPorUsuario(int usuarioId)
        {
            var registros = await _context.Bitacoras
                .Include(b => b.Usuario)
                .Where(b => b.UsuarioId == usuarioId)
                .OrderByDescending(b => b.Fecha)
                .ToListAsync();

            var viewModels = _mapper.Map<List<BitacoraViewModel>>(registros);
            return Ok(viewModels);
        }

        // POST: api/Bitacora
        [HttpPost]
        public async Task<IActionResult> PostBitacora(BitacoraViewModel bitacoraVM)
        {
            var bitacora = _mapper.Map<Bitacora>(bitacoraVM);
            bitacora.Fecha = DateTime.UtcNow;

            _context.Bitacoras.Add(bitacora);
            await _context.SaveChangesAsync();

            return Ok();
        }
        // GET: api/Bitacora/rango?desde=2025-04-01&hasta=2025-04-16
        [HttpGet("rango")]
        public async Task<ActionResult<IEnumerable<BitacoraViewModel>>> GetBitacoraPorRangoFecha(
            [FromQuery] DateTime desde,
            [FromQuery] DateTime hasta)
        {
            var registros = await _context.Bitacoras
                .Include(b => b.Usuario)
                .Where(b => b.Fecha >= desde && b.Fecha <= hasta)
                .OrderByDescending(b => b.Fecha)
                .ToListAsync();

            var viewModels = _mapper.Map<List<BitacoraViewModel>>(registros);
            return Ok(viewModels);
        }

    }

}
