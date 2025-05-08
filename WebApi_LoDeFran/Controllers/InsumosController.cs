using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApi_LoDeFran.Models;
using WebApi_LoDeFran.ViewModels;

namespace WebApi_LoDeFran.Controllers;

[ApiController]
[Route("api/[controller]")]
public class InsumosController : ControllerBase
{
    private readonly LoDeFranContext _context;
    private readonly IMapper _mapper;

    public InsumosController(LoDeFranContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    // GET: api/Insumos
    [HttpGet]
    public async Task<ActionResult<IEnumerable<InsumoViewModel>>> GetInsumos()
    {
        var insumos = await _context.Insumos
            .Include(i => i.Proveedor)
            .Include(p => p.Estado)
            .ToListAsync();

        var viewModels = _mapper.Map<List<InsumoViewModel>>(insumos);
        return Ok(viewModels);
    }

    // GET: api/Insumos/5
    [HttpGet("{id}")]
    public async Task<ActionResult<InsumoViewModel>> GetInsumo(int id)
    {
        var insumo = await _context.Insumos
            .Include(i => i.Proveedor)
            .FirstOrDefaultAsync(i => i.Id == id);

        if (insumo == null)
            return NotFound();

        return _mapper.Map<InsumoViewModel>(insumo);
    }

    // POST: api/Insumos
    [HttpPost]
    public async Task<ActionResult<InsumoViewModel>> PostInsumo(InsumoViewModel insumoVM)
    {
        var insumo = _mapper.Map<Insumo>(insumoVM);
        insumo.FechaUltimoIngreso = DateTime.UtcNow;

        _context.Insumos.Add(insumo);
        await _context.SaveChangesAsync();

        var resultVM = _mapper.Map<InsumoViewModel>(insumo);
        return CreatedAtAction(nameof(GetInsumo), new { id = insumo.Id }, resultVM);
    }

    // PUT: api/Insumos/5
    [HttpPut("{id}")]
    public async Task<IActionResult> PutInsumo(int id, InsumoViewModel insumoVM)
    {
        if (id != insumoVM.Id)
            return BadRequest();

        var insumo = await _context.Insumos.FindAsync(id);
        if (insumo == null)
            return NotFound();

        _mapper.Map(insumoVM, insumo);
        _context.Entry(insumo).State = EntityState.Modified;
        await _context.SaveChangesAsync();

        return NoContent();
    }

    // DELETE: api/Insumos/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteInsumo(int id)
    {
        var insumo = await _context.Insumos.FindAsync(id);
        if (insumo == null)
            return NotFound();

        _context.Insumos.Remove(insumo);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}
