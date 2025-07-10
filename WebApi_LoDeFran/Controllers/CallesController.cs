using AutoMapper;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApi_LoDeFran.Models;
using WebApi_LoDeFran.ViewModels;

namespace WebApi_LoDeFran.Controllers
{
    [Route("api/calles")]
    [ApiController]
    public class CallesController : ControllerBase
    {
        private readonly LoDeFranContext _context;
        private readonly IMapper _mapper;

        public CallesController(LoDeFranContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpPost("importar")]
        public async Task<IActionResult> ImportarDesdeExcel()
        {
            string rutaArchivo = @"C:\Users\USUARIO\Downloads\callejero.xlsx";

            if (!System.IO.File.Exists(rutaArchivo))
                return NotFound("No se encontró el archivo en la ruta especificada.");

            var callesBuscadas = new string[]
            {
        "Jerónimo Salguero", "Raúl Scalabrini Ortiz", "Julián Álvarez", "Coronel Díaz", "Mario Bravo", "Laprida",
        "Soler", "Santa Fe", "Honduras", "Nicaragua", "El Salvador", "Guatemala", "Paraguay", "Cabrera", "Gorriti",
        "Armenia", "Thames", "Uriarte", "Malabia", "Bonpland", "Fitz Roy", "Dorrego", "Gurruchaga", "Costa Rica",
        "Jufré", "Yatay", "Muñecas", "Avenida Corrientes", "Avenida Rivadavia", "Avenida Medrano", "Avenida Díaz Vélez",
        "Avenida Córdoba", "Avenida Estado de Israel", "Avenida La Plata", "Gascón", "Rawson", "Billinghurst",
        "Humahuaca", "Sánchez de Bustamante", "Castro Barros", "Bulnes", "Gallo", "Lambaré", "Loria", "Río de Janeiro",
        "Guardia Vieja", "Pueyrredón", "Paso", "Viamonte", "Bartolomé Mitre", "Uriburu", "Junín", "Rivadavia",
        "Jean Jaurès", "Tucumán", "Lavalle", "Ayacucho", "Perón", "Sarmiento", "San Luis", "Anchorena",
        "Avenida Independencia", "Avenida Belgrano", "José María Moreno", "Doblas", "Av. Directorio", "Curapaligüe",
        "Avenida Alberdi", "Avenida Pedro Goyena", "Hortiguera", "Neuquén", "Valle", "Varela", "Riglos", "Rojas",
        "Campichuelo", "Bogotá", "Franklin", "Donato Álvarez", "Av. Gaona", "Ambrosetti", "Av. Acoyte", "Morelos",
        "Aranguren", "Av. Scalabrini Ortiz", "Av. Corrientes (Norte)", "Acevedo", "Aguirre", "Camargo", "Lavalleja",
        "Padilla", "Murillo", "Serrano", "Av. Juan B. Justo", "Luis Viale", "Av. Warnes", "Av. San Martín",
        "Tres Arroyos", "Apolinario Figueroa", "Ramírez de Velasco", "Av. Honorio Pueyrredón"
            };

            try
            {
                using var doc = SpreadsheetDocument.Open(rutaArchivo, false);
                var sheet = doc.WorkbookPart.Workbook.Sheets.GetFirstChild<Sheet>();
                var wsPart = (WorksheetPart)doc.WorkbookPart.GetPartById(sheet.Id);
                var rows = wsPart.Worksheet.Descendants<Row>().ToList();

                int importadas = 0;

                foreach (var row in rows.Skip(1))
                {
                    var values = row.Elements<Cell>().Select(c => GetValue(doc, c)).ToList();
                    if (values.Count < 3) continue;

                    var nomMapa = values.ElementAtOrDefault(8)?.Trim();

                    if (string.IsNullOrWhiteSpace(nomMapa))
                        continue;

                    if (!callesBuscadas.Any(c => nomMapa.Contains(c, StringComparison.OrdinalIgnoreCase)))
                        continue;

                    var calle = new Calle
                    {
                        Id = int.TryParse(values[0], out var id) ? id : 0,
                        Codigo = TryParse(values.ElementAtOrDefault(1)),
                        NomOficial = values.ElementAtOrDefault(2),
                        AltIzqIni = TryParse(values.ElementAtOrDefault(3)),
                        AltIzqFin = TryParse(values.ElementAtOrDefault(4)),
                        AltDerIni = TryParse(values.ElementAtOrDefault(5)),
                        AltDerFin = TryParse(values.ElementAtOrDefault(6)),
                        NomAnter = values.ElementAtOrDefault(7),
                        NomMapa = nomMapa,
                        TipoC = values.ElementAtOrDefault(9),
                        Bicisenda = values.ElementAtOrDefault(13),
                        RedJerarq = values.ElementAtOrDefault(14),
                        TipoFfcc = values.ElementAtOrDefault(15),
                        Comuna = TryParse(values.ElementAtOrDefault(16)),
                        ComPar = TryParse(values.ElementAtOrDefault(17)),
                        ComImpar = TryParse(values.ElementAtOrDefault(18)),
                        Barrio = values.ElementAtOrDefault(19),
                        BarrioPar = values.ElementAtOrDefault(20),
                        BarrioImp = values.ElementAtOrDefault(21),
                        Geometry = values.ElementAtOrDefault(22)
                    };

                    _context.Calles.Add(calle);
                    importadas++;
                }

                await _context.SaveChangesAsync();
                return Ok($"Importación exitosa. Total importadas: {importadas}");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error: {ex.Message}");
            }
        }

        private int? TryParse(string? text) =>
            int.TryParse(text, out var value) ? value : null;

        private string GetValue(SpreadsheetDocument doc, Cell cell)
        {
            var val = cell.InnerText;
            if (cell.DataType?.Value == CellValues.SharedString)
            {
                var strTable = doc.WorkbookPart.SharedStringTablePart.SharedStringTable;
                return strTable.ElementAt(int.Parse(val)).InnerText;
            }
            return val;
        }

        // CRUD en el mismo CallesController
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CalleViewModel>>> GetCalles()
        {
            var calles = await _context.Calles.ToListAsync();
            return Ok(_mapper.Map<IEnumerable<CalleViewModel>>(calles));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CalleViewModel>> GetCalle(int id)
        {
            var calle = await _context.Calles.FindAsync(id);
            if (calle == null)
                return NotFound();

            return Ok(_mapper.Map<CalleViewModel>(calle));
        }

        [HttpPost]
        public async Task<ActionResult<CalleViewModel>> CreateCalle(CalleViewModel model)
        {
            var calle = _mapper.Map<Calle>(model);
            _context.Calles.Add(calle);
            await _context.SaveChangesAsync();

            model.Id = calle.Id;
            return CreatedAtAction(nameof(GetCalle), new { id = model.Id }, model);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCalle(int id, CalleViewModel model)
        {
            if (id != model.Id)
                return BadRequest();

            var calle = await _context.Calles.FindAsync(id);
            if (calle == null)
                return NotFound();

            _mapper.Map(model, calle);
            _context.Entry(calle).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCalle(int id)
        {
            var calle = await _context.Calles.FindAsync(id);
            if (calle == null)
                return NotFound();

            _context.Calles.Remove(calle);
            await _context.SaveChangesAsync();
            return NoContent();
        }
        [HttpGet("buscar")]
        public async Task<IActionResult> BuscarCalles([FromQuery] string nombre)
        {
            if (string.IsNullOrWhiteSpace(nombre))
                return BadRequest("Debe ingresar un nombre de calle.");

            var coincidencias = await _context.Calles
                .Where(c => c.NomOficial != null && c.NomOficial.Contains(nombre, StringComparison.OrdinalIgnoreCase) ||
                            c.NomMapa != null && c.NomMapa.Contains(nombre, StringComparison.OrdinalIgnoreCase))
                .ToListAsync();

            return Ok(_mapper.Map<List<CalleViewModel>>(coincidencias));
        }
        [HttpGet("buscar-en-excel")]
        public IActionResult BuscarEnExcel([FromQuery] string nombre)
        {
            if (string.IsNullOrWhiteSpace(nombre))
                return BadRequest("Debe ingresar un nombre de calle.");

            string rutaArchivo = Path.Combine(Directory.GetCurrentDirectory(), "files", "callejero.xlsx");


            if (!System.IO.File.Exists(rutaArchivo))
                return NotFound("No se encontró el archivo en la ruta especificada.");

            try
            {
                using var doc = SpreadsheetDocument.Open(rutaArchivo, false);
                var sheet = doc.WorkbookPart.Workbook.Sheets.GetFirstChild<Sheet>();
                var wsPart = (WorksheetPart)doc.WorkbookPart.GetPartById(sheet.Id);
                var rows = wsPart.Worksheet.Descendants<Row>().ToList();

                var resultados = new List<CalleViewModel>();

                foreach (var row in rows.Skip(1))
                {
                    var values = row.Elements<Cell>().Select(c => GetValue(doc, c)).ToList();
                    if (values.Count < 9) continue;

                    var nomOficial = values.ElementAtOrDefault(2)?.Trim();
                    var nomMapa = values.ElementAtOrDefault(8)?.Trim();

                    if ((nomOficial != null && nomOficial.Contains(nombre, StringComparison.OrdinalIgnoreCase)) ||
                        (nomMapa != null && nomMapa.Contains(nombre, StringComparison.OrdinalIgnoreCase)))
                    {
                        resultados.Add(new CalleViewModel
                        {
                            Id = int.TryParse(values[0], out var id) ? id : 0,
                            Codigo = TryParse(values.ElementAtOrDefault(1)),
                            NomOficial = nomOficial,
                            AltIzqIni = TryParse(values.ElementAtOrDefault(3)),
                            AltIzqFin = TryParse(values.ElementAtOrDefault(4)),
                            AltDerIni = TryParse(values.ElementAtOrDefault(5)),
                            AltDerFin = TryParse(values.ElementAtOrDefault(6)),
                            NomAnter = values.ElementAtOrDefault(7),
                            NomMapa = nomMapa,
                            TipoC = values.ElementAtOrDefault(9),
                            Bicisenda = values.ElementAtOrDefault(13),
                            RedJerarq = values.ElementAtOrDefault(14),
                            TipoFfcc = values.ElementAtOrDefault(15),
                            Comuna = TryParse(values.ElementAtOrDefault(16)),
                            ComPar = TryParse(values.ElementAtOrDefault(17)),
                            ComImpar = TryParse(values.ElementAtOrDefault(18)),
                            Barrio = values.ElementAtOrDefault(19),
                            BarrioPar = values.ElementAtOrDefault(20),
                            BarrioImp = values.ElementAtOrDefault(21),
                            Geometry = values.ElementAtOrDefault(22)
                        });
                    }
                }

                return Ok(resultados);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error: {ex.Message}");
            }
        }
        [HttpGet("autocomplete")]
        public async Task<IActionResult> AutocompletarCalles([FromQuery] string texto)
        {
            if (string.IsNullOrWhiteSpace(texto) || texto.Length < 3)
                return Ok(new List<object>());

            // Normalizar fuera del query para evitar problemas de traducción EF Core
            texto = texto.Trim();

            // Traer todos los NomMapa que contengan el texto (insensible a mayúsculas), sin repetir
            var coincidencias = await _context.Calles
                .Where(c => c.NomMapa != null && EF.Functions.Like(c.NomMapa, $"%{texto}%"))
                .Select(c => c.NomMapa) // solo el nombre
                .Distinct()
                .OrderBy(n => n)
                .Take(20)
                .ToListAsync();

            var resultados = coincidencias.Select(n => new
            {
                id = n,               // usamos el nombre como ID para ahora
                descripcion = n       // para mostrar
            });

            return Ok(resultados);
        }



    }

}
