using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApi_LoDeFran.Models;
using WebApi_LoDeFran.ViewModels;

namespace WebApi_LoDeFran.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ClientesController : ControllerBase
    {
        private readonly LoDeFranContext _context;
        private readonly IMapper _mapper;

        public ClientesController(LoDeFranContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/Clientes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ClienteViewModel>>> GetClientes()
        {
            var clientes = await _context.Clientes.ToListAsync();
            var viewModels = _mapper.Map<List<ClienteViewModel>>(clientes);
            return Ok(viewModels);
        }

        // GET: api/Clientes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ClienteViewModel>> GetCliente(int id)
        {
            var cliente = await _context.Clientes.FindAsync(id);
            if (cliente == null) return NotFound();

            var viewModel = _mapper.Map<ClienteViewModel>(cliente);
            return Ok(viewModel);
        }

        // POST: api/Clientes
        [HttpPost]
        public async Task<IActionResult> PostCliente(ClienteViewModel clienteVm)
        {
            var existente = await _context.Clientes
                .FirstOrDefaultAsync(c => c.Telefono == clienteVm.Telefono);

            if (existente != null)
            {
                return Ok(_mapper.Map<ClienteViewModel>(existente));
            }

            var cliente = _mapper.Map<Cliente>(clienteVm);
            cliente.FechaCreacion = DateTime.Now;

            // Buscar tramo de calle por nombre + altura
            if (!string.IsNullOrWhiteSpace(clienteVm.Calle) && int.TryParse(clienteVm.Altura, out int altura))
            {
                var tramo = await _context.Calles
                    .Where(c => c.NomMapa == clienteVm.Calle &&
                                ((c.AltIzqIni <= altura && c.AltIzqFin >= altura) ||
                                 (c.AltDerIni <= altura && c.AltDerFin >= altura)))
                    .OrderBy(c => Math.Abs((((c.AltIzqIni ?? 0) + (c.AltIzqFin ?? 0) + (c.AltDerIni ?? 0) + (c.AltDerFin ?? 0)) / 4m ) - (decimal)altura))
                    .FirstOrDefaultAsync();

                if (tramo != null)
                {
                    cliente.CalleId = tramo.Id;
                }
            }

            _context.Clientes.Add(cliente);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetCliente), new { id = cliente.Id }, _mapper.Map<ClienteViewModel>(cliente));
        }



        // PUT: api/Clientes/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCliente(int id, ClienteViewModel viewModel)
        {
            if (id != viewModel.Id) return BadRequest();

            var cliente = await _context.Clientes.FindAsync(id);
            if (cliente == null) return NotFound();

            _mapper.Map(viewModel, cliente);
            _context.Entry(cliente).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/Clientes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCliente(int id)
        {
            var cliente = await _context.Clientes.FindAsync(id);
            if (cliente == null) return NotFound();

            _context.Clientes.Remove(cliente);
            await _context.SaveChangesAsync();

            return NoContent();
        }
        [HttpGet("buscar")]
        public async Task<ActionResult<List<ClienteViewModel>>> BuscarClientes([FromQuery] string? busqueda)
        {
            if (string.IsNullOrWhiteSpace(busqueda))
                return BadRequest("La cadena de búsqueda no puede estar vacía.");

            var clientes = await _context.Clientes
                .Where(c =>
                    c.Nombre.Contains(busqueda) ||
                    c.Telefono.Contains(busqueda) ||
                    (c.Email != null && c.Email.Contains(busqueda)) ||
                    (c.Calle != null && c.Calle.Contains(busqueda)) ||
                    (c.Altura != null && c.Altura.Contains(busqueda))
                )
                .ToListAsync();

            var clientesVM = _mapper.Map<List<ClienteViewModel>>(clientes);
            return Ok(clientesVM);
        }


    }

}
