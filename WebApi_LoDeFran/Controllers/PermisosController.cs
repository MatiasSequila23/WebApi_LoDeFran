using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApi_LoDeFran.Models;
using WebApi_LoDeFran.ViewModels;

namespace WebApi_LoDeFran.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PermisosController : ControllerBase
    {
        private readonly LoDeFranContext _context;
        private readonly IMapper _mapper;

        public PermisosController(LoDeFranContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/Permisos
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PermisoViewModel>>> GetPermisos()
        {
            var permisos = await _context.Permisos.ToListAsync();
            var permisosVM = _mapper.Map<List<PermisoViewModel>>(permisos);
            return Ok(permisosVM);
        }

        // GET: api/Permisos/5
        [HttpGet("{id}")]
        public async Task<ActionResult<PermisoViewModel>> GetPermiso(int id)
        {
            var permiso = await _context.Permisos.FirstOrDefaultAsync(p => p.Id == id);

            if (permiso == null)
                return NotFound();

            var permisoVM = _mapper.Map<PermisoViewModel>(permiso);
            return Ok(permisoVM);
        }

        // POST: api/Permisos
        [HttpPost]
        public async Task<ActionResult<PermisoViewModel>> PostPermiso(PermisoViewModel permisoVM)
        {
            var permiso = _mapper.Map<Permiso>(permisoVM);
            _context.Permisos.Add(permiso);
            await _context.SaveChangesAsync();

            var nuevoVM = _mapper.Map<PermisoViewModel>(permiso);
            return CreatedAtAction(nameof(GetPermiso), new { id = permiso.Id }, nuevoVM);
        }

        // PUT: api/Permisos/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPermiso(int id, PermisoViewModel permisoVM)
        {
            if (id != permisoVM.Id)
                return BadRequest();

            var permiso = await _context.Permisos.FindAsync(id);
            if (permiso == null)
                return NotFound();

            _mapper.Map(permisoVM, permiso);
            _context.Entry(permiso).State = EntityState.Modified;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        // DELETE: api/Permisos/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePermiso(int id)
        {
            var permiso = await _context.Permisos.FindAsync(id);
            if (permiso == null)
                return NotFound();

            _context.Permisos.Remove(permiso);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }

}
