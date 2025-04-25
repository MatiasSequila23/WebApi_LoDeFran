using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApi_LoDeFran.Models;
using WebApi_LoDeFran.ViewModels;

namespace WebApi_LoDeFran.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StocksController : ControllerBase
    {
        private readonly LoDeFranContext _context;
        private readonly IMapper _mapper;

        public StocksController(LoDeFranContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/Stock
        [HttpGet]
        public async Task<ActionResult<IEnumerable<StockViewModel>>> GetStocks()
        {
            var stocks = await _context.Stocks
                .Include(s => s.Producto)  // Incluir producto relacionado
                .ToListAsync();

            var stockVM = _mapper.Map<List<StockViewModel>>(stocks);
            return Ok(stockVM);
        }

        // GET: api/Stock/5
        [HttpGet("{id}")]
        public async Task<ActionResult<StockViewModel>> GetStock(int id)
        {
            var stock = await _context.Stocks
                .Include(s => s.Producto)  // Incluir producto relacionado
                .FirstOrDefaultAsync(s => s.Id == id);

            if (stock == null)
                return NotFound();

            var stockVM = _mapper.Map<StockViewModel>(stock);
            return Ok(stockVM);
        }

        // POST: api/Stock
        [HttpPost]
        public async Task<ActionResult<StockViewModel>> PostStock(StockViewModel stockVM)
        {
            var stock = _mapper.Map<Stock>(stockVM);
            _context.Stocks.Add(stock);
            await _context.SaveChangesAsync();

            var nuevoVM = _mapper.Map<StockViewModel>(stock);
            return CreatedAtAction(nameof(GetStock), new { id = stock.Id }, nuevoVM);
        }

        // PUT: api/Stock/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutStock(int id, StockViewModel stockVM)
        {
            if (id != stockVM.Id)
                return BadRequest();

            var stock = await _context.Stocks.FindAsync(id);
            if (stock == null)
                return NotFound();

            _mapper.Map(stockVM, stock);
            _context.Entry(stock).State = EntityState.Modified;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        // DELETE: api/Stock/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStock(int id)
        {
            var stock = await _context.Stocks.FindAsync(id);
            if (stock == null)
                return NotFound();

            _context.Stocks.Remove(stock);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }

}
