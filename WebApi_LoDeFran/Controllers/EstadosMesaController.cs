using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApi_LoDeFran.Models;
using WebApi_LoDeFran.ViewModels;

namespace WebApi_LoDeFran.Controllers
{
    public class EstadosMesaController : Controller
    {
        private readonly LoDeFranContext _context;
        private readonly IMapper _mapper;

        public EstadosMesaController(LoDeFranContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<EstadoMesaViewModel>>> GetEstadosMesa()
        {
            var estados = await _context.EstadosMesas.ToListAsync();
            var estadosVM = _mapper.Map<List<EstadoMesaViewModel>>(estados);
            return Ok(estadosVM);
        }
    }
}
