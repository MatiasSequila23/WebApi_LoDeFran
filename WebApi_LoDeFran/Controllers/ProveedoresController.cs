using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApi_LoDeFran.Models;
using WebApi_LoDeFran.ViewModels;

namespace WebApi_LoDeFran.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProveedoresController : ControllerBase
    {
        private readonly LoDeFranContext _context;
        private readonly IMapper _mapper;

        public ProveedoresController(LoDeFranContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/Proveedores
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProveedorViewModel>>> GetProveedores()
        {
            var proveedores = await _context.Proveedores.ToListAsync();
            var proveedoresVM = _mapper.Map<List<ProveedorViewModel>>(proveedores);
            return Ok(proveedoresVM);
        }

        // GET: api/Proveedores/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ProveedorViewModel>> GetProveedor(int id)
        {
            var proveedor = await _context.Proveedores.FirstOrDefaultAsync(p => p.Id == id);

            if (proveedor == null)
                return NotFound();

            var proveedorVM = _mapper.Map<ProveedorViewModel>(proveedor);
            return Ok(proveedorVM);
        }

        // POST: api/Proveedores
        [HttpPost]
        public async Task<ActionResult<ProveedorViewModel>> PostProveedor(ProveedorViewModel proveedorVM)
        {
            var proveedor = _mapper.Map<Proveedore>(proveedorVM);

            _context.Proveedores.Add(proveedor);
            await _context.SaveChangesAsync();

            var nuevoVM = _mapper.Map<ProveedorViewModel>(proveedor);
            return CreatedAtAction(nameof(GetProveedor), new { id = proveedor.Id }, nuevoVM);
        }

        // PUT: api/Proveedores/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProveedor(int id, ProveedorViewModel proveedorVM)
        {
            if (id != proveedorVM.Id)
                return BadRequest();

            var proveedor = await _context.Proveedores.FindAsync(id);
            if (proveedor == null)
                return NotFound();

            _mapper.Map(proveedorVM, proveedor);
            _context.Entry(proveedor).State = EntityState.Modified;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        // DELETE: api/Proveedores/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProveedor(int id)
        {
            var proveedor = await _context.Proveedores.FindAsync(id);
            if (proveedor == null)
                return NotFound();

            _context.Proveedores.Remove(proveedor);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
