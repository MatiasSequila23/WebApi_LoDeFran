using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApi_LoDeFran.Models;
using WebApi_LoDeFran.ViewModels;

namespace WebApi_LoDeFran.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DiasController : Controller
    {
        private readonly LoDeFranContext _context;
        private readonly IMapper _mapper;

        public DiasController(LoDeFranContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DiaViewModel>>> GetMetodos()
        {
            var metodos = await _context.Dias.ToListAsync();
            return Ok(_mapper.Map<List<DiaViewModel>>(metodos));
        }

        [HttpPost]
        public async Task<ActionResult<DiaViewModel>> PostMetodo(DiaViewModel metodoVM)
        {
            var metodo = _mapper.Map<Dia>(metodoVM);
            _context.Dias.Add(metodo);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetMetodos), new { id = metodo.Id }, _mapper.Map<DiaViewModel>(metodo));
        }
    }
}
