using Microsoft.AspNetCore.Mvc;
using RabbitMQ.Client;

namespace API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private readonly IModel rabbitMQ;
        private readonly IConfiguration configuration;

        private static readonly string[] Summaries = new[]
        {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, IModel rabbitMQ, IConfiguration configuration)
        {
            _logger = logger;
            this.rabbitMQ = rabbitMQ;
            this.configuration = configuration;
        }

        [HttpGet(Name = "GetWeatherForecast")]
        public IEnumerable<WeatherForecast> Get()
        {
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }

        [HttpPost()]
        public IEnumerable<WeatherForecast> Post(string mensagem)
        {


            string objectInJson = System.Text.Json.JsonSerializer.Serialize(mensagem);
            byte[] objectInByteArray = System.Text.Encoding.UTF8.GetBytes(objectInJson);

            this.rabbitMQ.BasicPublish(
                exchange: configuration.GetSection("DevWeek:RabbitMQ:DownloadPipeline:Exchange").Get<string>(),
                routingKey: configuration.GetSection("DevWeek:RabbitMQ:DownloadPipeline:RouteKey").Get<string>(),
                basicProperties: null,
                body: objectInByteArray
                );

            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }
    }
}