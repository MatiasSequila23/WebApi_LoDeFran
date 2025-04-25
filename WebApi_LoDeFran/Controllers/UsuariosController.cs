using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApi_LoDeFran.ViewModels;
using WebApi_LoDeFran.Models;

namespace WebApi_LoDeFran.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsuariosController : ControllerBase

    {
        private readonly LoDeFranContext _context;
        private readonly IMapper _mapper;

        public UsuariosController(LoDeFranContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/Usuarios
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UsuarioViewModel>>> GetUsuarios()
        {
            var usuarios = await _context.Usuarios
                .Include(u => u.Rols)
                .Include(u => u.Permisos)
                .ToListAsync();

            var usuariosVM = _mapper.Map<List<UsuarioViewModel>>(usuarios);
            return Ok(usuariosVM);
        }

        // GET: api/Usuarios/5
        [HttpGet("{id}")]
        public async Task<ActionResult<UsuarioViewModel>> GetUsuario(int id)
        {
            var usuario = await _context.Usuarios
                .Include(u => u.Rols)
                .Include(u => u.Permisos)
                .FirstOrDefaultAsync(u => u.Id == id);

            if (usuario == null)
                return NotFound();

            var usuarioVM = _mapper.Map<UsuarioViewModel>(usuario);
            return Ok(usuarioVM);
        }

        // POST: api/Usuarios
        [HttpPost]
        public async Task<ActionResult<UsuarioViewModel>> PostUsuario(UsuarioViewModel usuarioVM)
        {
            var usuario = _mapper.Map<Usuario>(usuarioVM);
            usuario.FechaCreacion = DateTime.UtcNow;

            _context.Usuarios.Add(usuario);
            await _context.SaveChangesAsync();

            var nuevoVM = _mapper.Map<UsuarioViewModel>(usuario);
            return CreatedAtAction(nameof(GetUsuario), new { id = usuario.Id }, nuevoVM);
        }

        // PUT: api/Usuarios/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUsuario(int id, UsuarioViewModel usuarioVM)
        {
            if (id != usuarioVM.Id)
                return BadRequest();

            var usuario = await _context.Usuarios
                .Include(u => u.Rols)
                .Include(u => u.Permisos)
                .FirstOrDefaultAsync(u => u.Id == id);

            if (usuario == null)
                return NotFound();

            // Actualiza propiedades básicas
            _mapper.Map(usuarioVM, usuario);
            _context.Entry(usuario).State = EntityState.Modified;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        // DELETE: api/Usuarios/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUsuario(int id)
        {
            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario == null)
                return NotFound();

            _context.Usuarios.Remove(usuario);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
