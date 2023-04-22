using Microsoft.AspNetCore.Mvc;

namespace HowTo.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IEnumerable<WeatherForecast> Get([FromQuery] int zipCode)
        {
            if(zipCode == 0)
                return Enumerable.Empty<WeatherForecast>();

            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
                {
                    Date = DateTime.Now.AddDays(index),
                    TemperatureC = Random.Shared.Next(-20, 55),
                    PrecipitationChance = Random.Shared.Next(0, 101),
                    WindSpeed = Random.Shared.Next(0, 40),
                    WindDirection = Random.Shared.Next(0, 8) switch
                    {
                        0 => "N",
                        1 => "NW",
                        2 => "W",
                        3 => "SW",
                        4 => "S",
                        5 => "SE",
                        6 => "E",
                        7 => "NE"
                    }
                }).ToArray();
        }

        private readonly ILogger<WeatherForecastController> _logger;
    }
}