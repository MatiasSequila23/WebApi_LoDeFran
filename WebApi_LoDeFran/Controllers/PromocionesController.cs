using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApi_LoDeFran.Models;
using WebApi_LoDeFran.ViewModels;

namespace WebApi_LoDeFran.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PromocionesController : ControllerBase
    {
        private readonly LoDeFranContext _context;
        private readonly IMapper _mapper;

        public PromocionesController(LoDeFranContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/Promociones
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PromocionViewModel>>> GetPromociones()
        {
            var promociones = await _context.Promociones.ToListAsync();
            var promocionesVM = _mapper.Map<List<PromocionViewModel>>(promociones);
            return Ok(promocionesVM);
        }

        // GET: api/Promociones/5
        [HttpGet("{id}")]
        public async Task<ActionResult<PromocionViewModel>> GetPromocion(int id)
        {
            var promocion = await _context.Promociones.FirstOrDefaultAsync(p => p.Id == id);

            if (promocion == null)
                return NotFound();

            var promocionVM = _mapper.Map<PromocionViewModel>(promocion);
            return Ok(promocionVM);
        }

        // POST: api/Promociones
        [HttpPost]
        public async Task<ActionResult<PromocionViewModel>> PostPromocion(PromocionViewModel promocionVM)
        {
            var promocion = _mapper.Map<Promocione>(promocionVM);
            _context.Promociones.Add(promocion);
            await _context.SaveChangesAsync();

            var nuevoVM = _mapper.Map<PromocionViewModel>(promocion);
            return CreatedAtAction(nameof(GetPromocion), new { id = promocion.Id }, nuevoVM);
        }

        // PUT: api/Promociones/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPromocion(int id, PromocionViewModel promocionVM)
        {
            if (id != promocionVM.Id)
                return BadRequest();

            var promocion = await _context.Promociones.FindAsync(id);
            if (promocion == null)
                return NotFound();

            _mapper.Map(promocionVM, promocion);
            _context.Entry(promocion).State = EntityState.Modified;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        // DELETE: api/Promociones/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePromocion(int id)
        {
            var promocion = await _context.Promociones.FindAsync(id);
            if (promocion == null)
                return NotFound();

            _context.Promociones.Remove(promocion);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }

}
