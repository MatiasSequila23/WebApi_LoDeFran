using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApi_LoDeFran.Models;
using WebApi_LoDeFran.ViewModels;

namespace WebApi_LoDeFran.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoriasProductosController : Controller
    {
        private readonly LoDeFranContext _context;
        private readonly IMapper _mapper;

        public CategoriasProductosController(LoDeFranContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/CategoriasProducto
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CategoriaProductoViewModel>>> GetCategorias()
        {
            var categorias = await _context.CategoriasProductos.ToListAsync();
            var viewModels = _mapper.Map<List<CategoriaProductoViewModel>>(categorias);
            return Ok(viewModels);
        }

        // GET: api/CategoriasProducto/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CategoriaProductoViewModel>> GetCategoria(int id)
        {
            var categoria = await _context.CategoriasProductos.FindAsync(id);
            if (categoria == null) return NotFound();

            var viewModel = _mapper.Map<CategoriaProductoViewModel>(categoria);
            return Ok(viewModel);
        }

        // POST: api/CategoriasProducto
        [HttpPost]
        public async Task<IActionResult> PostCategoria(CategoriaProductoViewModel viewModel)
        {
            var categoria = _mapper.Map<CategoriasProducto>(viewModel);
            _context.CategoriasProductos.Add(categoria);
            await _context.SaveChangesAsync();
            return Ok();
        }

        // PUT: api/CategoriasProducto/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCategoria(int id, CategoriaProductoViewModel viewModel)
        {
            if (id != viewModel.Id)
                return BadRequest();

            var categoria = await _context.CategoriasProductos.FindAsync(id);
            if (categoria == null)
                return NotFound();

            _mapper.Map(viewModel, categoria);
            _context.Entry(categoria).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        // DELETE: api/CategoriasProducto/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategoria(int id)
        {
            var categoria = await _context.CategoriasProductos.FindAsync(id);
            if (categoria == null)
                return NotFound();

            _context.CategoriasProductos.Remove(categoria);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
