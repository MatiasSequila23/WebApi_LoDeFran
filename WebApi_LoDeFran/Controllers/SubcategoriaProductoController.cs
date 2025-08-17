using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApi_LoDeFran.Models;
using WebApi_LoDeFran.ViewModels;

namespace WebApi_LoDeFran.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SubcategoriaProductoController : Controller
    {
        private readonly LoDeFranContext _context;
        private readonly IMapper _mapper;

        public SubcategoriaProductoController(LoDeFranContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/CategoriasProducto
        [HttpGet]
        public async Task<ActionResult<IEnumerable<SubcategoriaProductoViewModel>>> GetSubCategorias()
        {
            var categorias = await _context.SubcategoriasProductos.ToListAsync();
            var viewModels = _mapper.Map<List<SubcategoriaProductoViewModel>>(categorias);
            return Ok(viewModels);
        }


        [HttpGet("menu")]
        public async Task<ActionResult<List<CategoriaProductoViewModel>>> GetMenu()
        {
            var menuVM = await _context.CategoriasProductos
                .Include(c => c.SubcategoriasProductos)       // EF carga subcategorías
                    .ThenInclude(sc => sc.Productos)         // EF carga productos
                .ProjectTo<CategoriaProductoViewModel>(_mapper.ConfigurationProvider)
                .ToListAsync();

            return Ok(menuVM);
        }
    }
}
