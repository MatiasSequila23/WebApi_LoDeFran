using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApi_LoDeFran.Models;
using WebApi_LoDeFran.ViewModels;

namespace WebApi_LoDeFran.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductosController : ControllerBase
{
    private readonly LoDeFranContext _context;
    private readonly IMapper _mapper;

    public ProductosController(LoDeFranContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    // GET: api/Productos
    [HttpGet]
    public async Task<ActionResult<IEnumerable<ProductoViewModel>>> GetProductos()
    {
        var productos = await _context.Productos
            .Include(p => p.CategoriaProducto) // Corregido
            .Include(p => p.Estado)
            .ToListAsync();

        var productosVM = _mapper.Map<List<ProductoViewModel>>(productos);
        return Ok(productosVM);
    }

    // GET: api/Productos/5 
    [HttpGet("{id}")]
    public async Task<ActionResult<ProductoViewModel>> GetProducto(int id)
    {
        var producto = await _context.Productos
            .Include(p => p.CategoriaProducto) // Corregido
            .Include(p => p.Estado)
            .FirstOrDefaultAsync(p => p.Id == id);

        if (producto == null)
            return NotFound();

        var productoVM = _mapper.Map<ProductoViewModel>(producto);
        return Ok(productoVM);
    }

    // POST: api/Productos
    [HttpPost]
    public async Task<ActionResult<ProductoViewModel>> PostProducto(ProductoViewModel productoVM)
    {
        var producto = _mapper.Map<Producto>(productoVM);
        producto.FechaCreacion = DateTime.UtcNow;

        _context.Productos.Add(producto);
        await _context.SaveChangesAsync();

        var nuevoVM = _mapper.Map<ProductoViewModel>(producto);
        return CreatedAtAction(nameof(GetProducto), new { id = producto.Id }, nuevoVM);
    }

    // PUT: api/Productos/5
    [HttpPut("{id}")]
    public async Task<IActionResult> PutProducto(int id, ProductoViewModel productoVM)
    {
        if (id != productoVM.Id)
            return BadRequest();

        var producto = await _context.Productos.FindAsync(id);
        if (producto == null)
            return NotFound();

        _mapper.Map(productoVM, producto);
        _context.Entry(producto).State = EntityState.Modified;

        await _context.SaveChangesAsync();
        return NoContent();
    }

    // DELETE: api/Productos/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteProducto(int id)
    {
        var producto = await _context.Productos.FindAsync(id);
        if (producto == null)
            return NotFound();

        _context.Productos.Remove(producto);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    [HttpPost("{id}/insumos")]
    public async Task<IActionResult> AsociarInsumo(int id, InsumoProductoViewModel insumo)
    {
        var producto = await _context.Productos.Include(p => p.InsumosProductos)
                                               .FirstOrDefaultAsync(p => p.Id == id);

        if (producto == null)
            return NotFound();

        producto.InsumosProductos.Add(new InsumosProducto
        {
            ProductoId = id,
            InsumoId = insumo.InsumoId,
            Cantidad = insumo.Cantidad ?? 0
        });

        await _context.SaveChangesAsync();
        return NoContent();
    }
    [HttpGet("{id}/insumos")]
    public async Task<IActionResult> GetInsumosAsignados(int id)
    {
        var insumos = await _context.InsumosProductos
            .Include(ip => ip.Insumo) // importante para que funcione el mapeo de Insumo.Nombre
            .Where(ip => ip.ProductoId == id)
            .ToListAsync();

        if (insumos == null || !insumos.Any())
            return NotFound();

        var insumosVM = _mapper.Map<List<InsumoProductoViewModel>>(insumos);
        return Ok(insumosVM);
    }
    [HttpPut("{productoId}/insumos/{insumoId}")]
    public async Task<IActionResult> ActualizarInsumoProducto(int productoId, int insumoId, [FromBody] InsumoProductoViewModel insumo)
    {
        if (productoId != insumo.ProductoId || insumoId != insumo.InsumoId)
            return BadRequest("Los IDs no coinciden.");

        var insumoProducto = await _context.InsumosProductos
            .FirstOrDefaultAsync(ip => ip.ProductoId == productoId && ip.InsumoId == insumoId);

        if (insumoProducto == null)
            return NotFound("Insumo asignado no encontrado.");

        if (insumo.Cantidad.HasValue)
            insumoProducto.Cantidad = insumo.Cantidad.Value;

        await _context.SaveChangesAsync();
        return NoContent();
    }
    [HttpDelete("{productoId}/insumos/{insumoId}")]
    public async Task<IActionResult> EliminarInsumoProducto(int productoId, int insumoId)
    {
        var insumoProducto = await _context.InsumosProductos
            .FirstOrDefaultAsync(ip => ip.ProductoId == productoId && ip.InsumoId == insumoId);

        if (insumoProducto == null)
            return NotFound("Insumo asignado no encontrado.");

        _context.InsumosProductos.Remove(insumoProducto);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}
