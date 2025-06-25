using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApi_LoDeFran.Models;
using WebApi_LoDeFran.ViewModels;

namespace WebApi_LoDeFran.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FacturasController : ControllerBase
    {
        private readonly LoDeFranContext _context;
        private readonly IMapper _mapper;

        public FacturasController(LoDeFranContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<FacturaViewModel>>> GetFacturas()
        {
            var facturas = await _context.Facturas.Include(f => f.Cliente).Include(f => f.MetodoPago).ToListAsync();
            return Ok(_mapper.Map<List<FacturaViewModel>>(facturas));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<FacturaViewModel>> GetFactura(int id)
        {
            var factura = await _context.Facturas.Include(f => f.Cliente).Include(f => f.MetodoPago).FirstOrDefaultAsync(f => f.Id == id);
            if (factura == null) return NotFound();
            return Ok(_mapper.Map<FacturaViewModel>(factura));
        }

        [HttpPost]
        public async Task<ActionResult<FacturaViewModel>> PostFactura(FacturaViewModel facturaVM)
        {
            var factura = _mapper.Map<Factura>(facturaVM);
            _context.Facturas.Add(factura);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetFactura), new { id = factura.Id }, _mapper.Map<FacturaViewModel>(factura));
        }
        // PUT: api/Facturas/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutFactura(int id, FacturaViewModel viewModel)
        {
            if (id != viewModel.Id)
                return BadRequest();

            var factura = await _context.Facturas.FindAsync(id);
            if (factura == null)
                return NotFound();

            _mapper.Map(viewModel, factura);
            _context.Entry(factura).State = EntityState.Modified;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        // DELETE: api/Facturas/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFactura(int id)
        {
            var factura = await _context.Facturas.FindAsync(id);
            if (factura == null)
                return NotFound();

            _context.Facturas.Remove(factura);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }

}
