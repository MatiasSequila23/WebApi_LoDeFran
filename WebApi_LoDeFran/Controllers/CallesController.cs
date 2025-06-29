using Microsoft.AspNetCore.Mvc;

namespace WebApi_LoDeFran.Controllers
{
    [Route("api/calles")]
    [ApiController]
    public class CallesController : ControllerBase
    {
        private readonly HttpClient _httpClient;

        public CallesController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        [HttpGet("{texto}")]
        public async Task<IActionResult> GetCalles(string texto)
        {
            var url = $"https://servicios.usig.buenosaires.gob.ar/Buscar/Buscar?texto={texto}&maxResultados=10";

            try
            {
                var response = await _httpClient.GetAsync(url);
                var content = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                    return StatusCode((int)response.StatusCode, content);

                return Content(content, "application/json");
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }
    }

}
