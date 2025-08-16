using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApi_LoDeFran.Models;
using WebApi_LoDeFran.ViewModels;

namespace WebApi_LoDeFran.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CombosController : Controller
    {
        private readonly LoDeFranContext _context;
        private readonly IMapper _mapper;

        public CombosController(LoDeFranContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // Listar todos los combos
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var combos = await _context.Combos
            .Select(c => new Combo
            {
                Id = c.Id,
                Nombre = c.Nombre,
                CombosItems = c.CombosItems.Select(ci => new CombosItem
                {
                    Cantidad = ci.Cantidad,
                    Producto = new Producto
                    {
                        Id = ci.Producto.Id,
                        Nombre = ci.Producto.Nombre,
                        Stock = ci.Producto.Stock,
                        InsumosProductos = ci.Producto.InsumosProductos.Select(ip => new InsumosProducto
                        {
                            Cantidad = ip.Cantidad,
                            Insumo = new Insumo
                            {
                                CantidadDisponible = ip.Insumo.CantidadDisponible
                            }
                        }).ToList()
                    }
                }).ToList()
            }).ToListAsync();


            var comboViewModels = new List<ComboViewModel>();

            foreach (var combo in combos)
            {
                var comboVM = _mapper.Map<ComboViewModel>(combo);

                int? stockCombo = null;

                foreach (var item in combo.CombosItems)
                {
                    var producto = item.Producto;
                    int cantidadEnCombo = item.Cantidad;

                    if (producto.InsumosProductos != null && producto.InsumosProductos.Any())
                    {
                        decimal? stockProducto = null;

                        foreach (var ip in producto.InsumosProductos)
                        {
                            if (ip.Insumo == null || ip.Insumo.CantidadDisponible == null || ip.Cantidad == 0)
                                continue;

                            decimal disponible = ip.Insumo.CantidadDisponible;
                            decimal necesarioPorUnidad = (decimal)ip.Cantidad;
                            decimal posible = Math.Floor(disponible / necesarioPorUnidad);

                            stockProducto = stockProducto == null ? posible : Math.Min(stockProducto.Value, posible);
                        }

                        // Tener en cuenta la cantidad del producto requerida por el combo
                        int stockParaCombo = stockProducto == null ? 0 : (int)(stockProducto.Value / cantidadEnCombo);

                        stockCombo = stockCombo == null ? stockParaCombo : Math.Min(stockCombo.Value, stockParaCombo);
                    }
                    else
                    {
                        // Si no tiene insumos, usamos directamente su stock
                        int stockDisponible = producto.Stock;
                        int stockParaCombo = stockDisponible / cantidadEnCombo;

                        stockCombo = stockCombo == null ? stockParaCombo : Math.Min(stockCombo.Value, stockParaCombo);
                    }
                }

                comboVM.Stock = stockCombo ?? 0;
                comboViewModels.Add(comboVM);
            }

            return Ok(comboViewModels);
        }

        // Obtener un combo por id
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var combo = await _context.Combos
                .Include(c => c.CombosItems)
                .ThenInclude(ci => ci.Producto)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (combo == null)
                return NotFound();

            return Ok(_mapper.Map<ComboViewModel>(combo));
        }

        // Crear combo con items
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ComboViewModel vm)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var combo = _mapper.Map<Combo>(vm);
            combo.FechaCreacion = DateTime.Now;

            _context.Combos.Add(combo);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new { id = combo.Id }, _mapper.Map<ComboViewModel>(combo));
        }

        // Editar combo con items
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] ComboViewModel vm)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var existingCombo = await _context.Combos
                .Include(c => c.CombosItems)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (existingCombo == null)
                return NotFound();

            // Actualizar propiedades básicas
            existingCombo.Nombre = vm.Nombre;
            existingCombo.Descripcion = vm.Descripcion;
            existingCombo.Precio = vm.Precio;
            existingCombo.EstadoId = vm.EstadoId;

            // Eliminar items antiguos
            _context.CombosItems.RemoveRange(existingCombo.CombosItems);

            // Agregar nuevos items
            existingCombo.CombosItems = vm.Items.Select(i => new CombosItem
            {
                ProductoId = i.ProductoId,
                Cantidad = i.Cantidad,
                CategoriaGrupo = i.CategoriaGrupo,
                EsOpcional = i.EsOpcional
            }).ToList();

            await _context.SaveChangesAsync();

            return NoContent();
        }

        // Dar de baja (no eliminar físicamente)
        [HttpDelete("{id}")]
        public async Task<IActionResult> DarDeBaja(int id)
        {
            var existingCombo = await _context.Combos.FindAsync(id);

            if (existingCombo == null)
                return NotFound();

            existingCombo.EstadoId = 2; // Inactivo
            await _context.SaveChangesAsync();

            return NoContent();
        }
        [HttpPost("crear-retornar-id")]
        public async Task<IActionResult> CreateAndReturnId([FromBody] ComboViewModel vm)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var combo = _mapper.Map<Combo>(vm);
            combo.FechaCreacion = DateTime.Now;

            _context.Combos.Add(combo);
            await _context.SaveChangesAsync();

            return Ok(new { combo.Id });
        }
        [HttpPatch("{id}/estado/{nuevoEstadoId}")]
        public async Task<IActionResult> CambiarEstado(int id, int nuevoEstadoId)
        {
            var combo = await _context.Combos.FindAsync(id);

            if (combo == null)
                return NotFound();

            combo.EstadoId = nuevoEstadoId;
            await _context.SaveChangesAsync();

            return Ok(new { combo.Id, combo.EstadoId });
        }
        [HttpPut("comentario/{id}")]
        public async Task<IActionResult> ActualizarComentario(int id, [FromBody] string comentario)
        {
            var detalle = await _context.PedidoCombos.FindAsync(id);

            if (detalle == null)
                return NotFound();

            detalle.Comentario = comentario;
            _context.Entry(detalle).Property(d => d.Comentario).IsModified = true;

            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
