using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using WebApi_LoDeFran.Models;
using WebApi_LoDeFran.ViewModels;
using Microsoft.EntityFrameworkCore;


namespace WebApi_LoDeFran.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MetodoPagoController : Controller
    {
        private readonly LoDeFranContext _context;
        private readonly IMapper _mapper;

        public MetodoPagoController(LoDeFranContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MetodoPagoViewModel>>> GetMetodos()
        {
            var metodos = await _context.MetodoPagos.ToListAsync();
            return Ok(_mapper.Map<List<MetodoPagoViewModel>>(metodos));
        }

        [HttpPost]
        public async Task<ActionResult<MetodoPagoViewModel>> PostMetodo(MetodoPagoViewModel metodoVM)
        {
            var metodo = _mapper.Map<MetodoPago>(metodoVM);
            _context.MetodoPagos.Add(metodo);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetMetodos), new { id = metodo.Id }, _mapper.Map<MetodoPagoViewModel>(metodo));
        }
    }
}
