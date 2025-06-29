using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApi_LoDeFran.Models;
using WebApi_LoDeFran.ViewModels;

namespace WebApi_LoDeFran.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MotivosMovimientoController : Controller
    {
        private readonly LoDeFranContext _context;
        private readonly IMapper _mapper;

        public MotivosMovimientoController(LoDeFranContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/MotivosMovimiento
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MotivoMovimientoViewModel>>> GetMotivos()
        {
            var motivos = await _context.MotivosMovimientos
                .Include(m => m.TipoMovimiento)
                .ToListAsync();

            return Ok(_mapper.Map<List<MotivoMovimientoViewModel>>(motivos));
        }

        // GET: api/MotivosMovimiento/egreso
        [HttpGet("tipo/{tipo}")]
        public async Task<ActionResult<IEnumerable<MotivoMovimientoViewModel>>> GetPorTipo(int tipo)
        {
            var motivos = await _context.MotivosMovimientos
                .Where(m => m.TipoMovimiento.Id == tipo)
                .Include(m => m.TipoMovimiento)
                .ToListAsync();

            return Ok(_mapper.Map<List<MotivoMovimientoViewModel>>(motivos));
        }
    }
}
