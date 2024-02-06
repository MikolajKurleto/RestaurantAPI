using Microsoft.AspNetCore.Mvc;

namespace RestaurantAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private readonly ILogger<WeatherForecastController> _logger;
        private readonly IWeatherForecastService _service;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, IWeatherForecastService service)
        {
            _logger = logger;
            _service = service;
        }

        [HttpPost("generate")]
        public IActionResult Generate([FromQuery] int toCreate, [FromBody] TemperatureRequest request) 
        {
            if (toCreate < 0 || request.MaximumTemperature < request.MinimumTemperature)
            {
                return BadRequest();
            }

            var result = _service.Get(toCreate, request.MinimumTemperature, request.MaximumTemperature);
            return Ok(result);
        }
    }
}
