using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApi_LoDeFran.Models;
using WebApi_LoDeFran.Utlis.Dto;
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
            var facturas = await _context.Facturas
                .Include(f => f.Cliente)
                .Include(f => f.MetodoPago)
                .Include(f => f.Estado)
                .Include(f => f.Usuario)
                .Include(f => f.Caja)
                .Include(f => f.Pedido)
                .ToListAsync();
            return Ok(_mapper.Map<List<FacturaViewModel>>(facturas));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<FacturaViewModel>> GetFactura(int id)
        {
            var factura = await _context.Facturas
                .Include(f => f.Cliente)
                .Include(f => f.MetodoPago)
                .Include(f => f.Estado)
                .Include(f => f.Usuario)
                .Include(f => f.Caja)
                .Include(f => f.Pedido)
                .FirstOrDefaultAsync(f => f.Id == id);

            if (factura == null)
                return NotFound();

            return Ok(_mapper.Map<FacturaViewModel>(factura));
        }


        [HttpPost]
        public async Task<ActionResult<FacturaViewModel>> PostFactura(FacturaDto dto)
        {
            var factura = _mapper.Map<Factura>(dto);

            // Datos que se generan en el backend
            factura.FechaEmision = DateTime.Now;
            factura.EstadoId = 1; // Por ejemplo: "Emitida"
            factura.NumeroFactura = GenerarNumeroFactura();

            _context.Facturas.Add(factura);
            await _context.SaveChangesAsync();

            var facturaVM = _mapper.Map<FacturaViewModel>(factura);
            return CreatedAtAction(nameof(GetFactura), new { id = factura.Id }, facturaVM);
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

        private string GenerarNumeroFactura()
        {
            // Ejemplo básico: "FAC-00000123"
            int ultimoId = _context.Facturas.OrderByDescending(f => f.Id).Select(f => f.Id).FirstOrDefault();
            return $"FAC-{(ultimoId + 1).ToString("D8")}";
        }
        [HttpGet("filtrar")]
        public async Task<ActionResult<IEnumerable<FacturaViewModel>>> FiltrarFacturas(string? numeroFactura, int? estadoId, DateTime? fechaDesde, DateTime? fechaHasta)
        {
            var query = _context.Facturas
                .Include(f => f.Cliente)
                .Include(f => f.MetodoPago)
                .Include(f => f.Estado)
                .Include(f => f.Usuario)
                .Include(f => f.Caja)
                .Include(f => f.Pedido)
                .AsQueryable();

            if (!string.IsNullOrEmpty(numeroFactura))
                query = query.Where(f => f.NumeroFactura.Contains(numeroFactura));

            if (estadoId.HasValue)
                query = query.Where(f => f.EstadoId == estadoId.Value);

            if (fechaDesde.HasValue)
                query = query.Where(f => f.FechaEmision >= fechaDesde.Value);

            if (fechaHasta.HasValue)
                query = query.Where(f => f.FechaEmision <= fechaHasta.Value);

            var facturas = await query.ToListAsync();
            return Ok(_mapper.Map<List<FacturaViewModel>>(facturas));
        }

    }

}
