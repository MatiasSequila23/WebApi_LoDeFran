using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApi_LoDeFran.Models;
using WebApi_LoDeFran.ViewModels;

namespace WebApi_LoDeFran.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RolesController : ControllerBase
    {
        private readonly LoDeFranContext _context;
        private readonly IMapper _mapper;

        public RolesController(LoDeFranContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/Roles
        [HttpGet]
        public async Task<ActionResult<IEnumerable<RolViewModel>>> GetRoles()
        {
            var roles = await _context.Roles
            .Include(r => r.Permisos)  // Incluir permisos asociados
                .ToListAsync();

            var rolesVM = _mapper.Map<List<RolViewModel>>(roles);
            return Ok(rolesVM);
        }

        // GET: api/Roles/5
        [HttpGet("{id}")]
        public async Task<ActionResult<RolViewModel>> GetRole(int id)
        {
            var role = await _context.Roles
                .Include(r => r.Permisos)  // Incluir permisos asociados
            .FirstOrDefaultAsync(r => r.Id == id);

            if (role == null)
                return NotFound();

            var roleVM = _mapper.Map<RolViewModel>(role);
            return Ok(roleVM);
        }

        // POST: api/Roles
        [HttpPost]
        public async Task<ActionResult<RolViewModel>> PostRole(RolViewModel roleVM)
        {
            var role = _mapper.Map<Role>(roleVM);
            _context.Roles.Add(role);
            await _context.SaveChangesAsync();

            var nuevoVM = _mapper.Map<RolViewModel>(role);
            return CreatedAtAction(nameof(GetRole), new { id = role.Id }, nuevoVM);
        }

        // PUT: api/Roles/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutRole(int id, RolViewModel roleVM)
        {
            if (id != roleVM.Id)
                return BadRequest();

            var role = await _context.Roles.FindAsync(id);
            if (role == null)
                return NotFound();

            _mapper.Map(roleVM, role);
            _context.Entry(role).State = EntityState.Modified;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        // DELETE: api/Roles/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRole(int id)
        {
            var role = await _context.Roles.FindAsync(id);
            if (role == null)
                return NotFound();

            _context.Roles.Remove(role);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }

}
