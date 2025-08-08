using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApi_LoDeFran.Models;
using WebApi_LoDeFran.ViewModels;

namespace WebApi_LoDeFran.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PromocionAplicacionesController : Controller
    {
        private readonly LoDeFranContext _context;
        private readonly IMapper _mapper;

        public PromocionAplicacionesController(LoDeFranContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PromocionAplicacionViewModel>>> Get()
        {
            var tipos = await _context.PromocionesAplicaciones.ToListAsync();
            var tiposVM = _mapper.Map<List<PromocionAplicacionViewModel>>(tipos);
            return Ok(tiposVM);
        }
    }
}
