using Consul.Demo.Helpers;
using Consul.Demo.Models;
using Microsoft.AspNetCore.Mvc;

namespace Consul.Demo.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private readonly ConsulClient _consulClient;
        private static readonly string[] Summaries = new[]
        {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };
        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, ConsulClient consulClient)
        {
            _logger = logger;
            _consulClient = consulClient;
        }

        [HttpGet(Name = "GetWeatherForecast")]
        public async Task<ActionResult> Get()
        {
            var consulDemoKey = await ConsulKeyValueProvider.GetValueAsync<ConsulDemoKey>(key: "ConsulDemoKey");

            if (consulDemoKey != null && consulDemoKey.IsEnabled)
            {
                return Ok(consulDemoKey);
            }

            return Ok("ConsulDemoKey is null");
        }

        [HttpPost("SetVal")]
        public async Task<ActionResult> Set(string value, string key)
        {
            if (!string.IsNullOrEmpty(key))
            {
                var setResult = await _consulClient.SetValueAsync(key, value);
            }
            return Ok();
        }
    }
}