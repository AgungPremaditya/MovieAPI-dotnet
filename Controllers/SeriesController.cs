using Microsoft.AspNetCore.Mvc;
using MovieAPI_dotnet.Services;

namespace MovieAPI_dotnet.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SeriesController : Controller
    {
        private readonly IExternalApiService _externalApiService;
        public SeriesController(IExternalApiService externalApiService)
        {
            _externalApiService = externalApiService;
        }

        [HttpGet]
        public async Task<IActionResult> GetSeries()
        {
            var series = await _externalApiService.GetHttpAsync();

            return Ok(series);
        }
    }
}
