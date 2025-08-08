using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApi_LoDeFran.Models;
using WebApi_LoDeFran.ViewModels;

namespace WebApi_LoDeFran.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TipoDescuentoController : Controller
    {
        private readonly LoDeFranContext _context;
        private readonly IMapper _mapper;

        public TipoDescuentoController(LoDeFranContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TipoDescuentoViewModel>>> Get()
        {
            var tipos = await _context.TipoDescuentos.ToListAsync();
            var tiposVM = _mapper.Map<List<TipoDescuentoViewModel>>(tipos);
            return Ok(tiposVM);
        }
    }
}
