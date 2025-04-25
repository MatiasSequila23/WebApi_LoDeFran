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

        // GET: api/Facturas
        [HttpGet]
        public async Task<ActionResult<IEnumerable<FacturaViewModel>>> GetFacturas()
        {
            var facturas = await _context.Facturas
                .Include(f => f.Pedido)
                .ToListAsync();

            var viewModels = _mapper.Map<List<FacturaViewModel>>(facturas);
            return Ok(viewModels);
        }

        // GET: api/Facturas/5
        [HttpGet("{id}")]
        public async Task<ActionResult<FacturaViewModel>> GetFactura(int id)
        {
            var factura = await _context.Facturas
                .Include(f => f.Pedido)
                .FirstOrDefaultAsync(f => f.Id == id);

            if (factura == null)
                return NotFound();

            return Ok(_mapper.Map<FacturaViewModel>(factura));
        }

        // POST: api/Facturas
        [HttpPost]
        public async Task<ActionResult<FacturaViewModel>> PostFactura(FacturaViewModel viewModel)
        {
            var factura = _mapper.Map<Factura>(viewModel);
            factura.FechaEmision = DateTime.UtcNow;

            _context.Facturas.Add(factura);
            await _context.SaveChangesAsync();

            var nuevoVM = _mapper.Map<FacturaViewModel>(factura);
            return CreatedAtAction(nameof(GetFactura), new { id = factura.Id }, nuevoVM);
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
