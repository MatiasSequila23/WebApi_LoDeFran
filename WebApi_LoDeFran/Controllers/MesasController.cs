using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApi_LoDeFran.Models;
using WebApi_LoDeFran.ViewModels;

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
            var mesas = await _context.Mesas.ToListAsync();
            var mesasVM = _mapper.Map<List<MesaViewModel>>(mesas);
            return Ok(mesasVM);
        }

        // GET: api/Mesas/5
        [HttpGet("{id}")]
        public async Task<ActionResult<MesaViewModel>> GetMesa(int id)
        {
            var mesa = await _context.Mesas.FirstOrDefaultAsync(m => m.Id == id);

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

            var nuevoVM = _mapper.Map<MesaViewModel>(mesa);
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
    }

}
