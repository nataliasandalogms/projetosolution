using Microsoft.AspNetCore.Mvc;
using System;

namespace ProjetoSolution.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class WeatherForecastController : ControllerBase
{
    private static readonly string[] Summaries = new[]
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    private readonly ILogger<WeatherForecastController> _logger;

    public WeatherForecastController(ILogger<WeatherForecastController> logger)
    {
        _logger = logger;
    }

    /// <summary>
    /// Obtém a previsão do tempo para os próximos dias
    /// </summary>
    /// <param name="days">Número de dias para prever (padrão: 5)</param>
    /// <returns>Array de previsões meteorológicas</returns>
    [HttpGet]
    public IEnumerable<WeatherForecast> Get(int days = 5)
    {
        _logger.LogInformation("Getting weather forecast for {Days} days", days);
        
        if (days <= 0 || days > 14)
        {
            throw new ArgumentException("Days must be between 1 and 14");
        }

        return Enumerable.Range(1, days).Select(index => new WeatherForecast
        {
            Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            TemperatureC = Random.Shared.Next(-20, 55),
            Summary = Summaries[Random.Shared.Next(Summaries.Length)]
        })
        .ToArray();
    }

    /// <summary>
    /// Obtém a previsão do tempo para um dia específico
    /// </summary>
    /// <param name="id">Número de dias no futuro (1 = amanhã)</param>
    /// <returns>Previsão para o dia específico</returns>
    [HttpGet("{id}")]
    public ActionResult<WeatherForecast> GetById(int id)
    {
        if (id <= 0 || id > 14)
        {
            return BadRequest("ID must be between 1 and 14");
        }

        _logger.LogInformation("Getting weather forecast for day {Day}", id);

        return new WeatherForecast
        {
            Date = DateOnly.FromDateTime(DateTime.Now.AddDays(id)),
            TemperatureC = Random.Shared.Next(-20, 55),
            Summary = Summaries[Random.Shared.Next(Summaries.Length)]
        };
    }

    /// <summary>
    /// Cria uma nova previsão do tempo (exemplo de POST)
    /// </summary>
    /// <param name="forecast">Dados da previsão</param>
    /// <returns>Previsão criada</returns>
    [HttpPost]
    public ActionResult<WeatherForecast> Post([FromBody] WeatherForecast forecast)
    {
        _logger.LogInformation("Creating new weather forecast for date {Date}", forecast.Date);
        
        // Simula salvamento no banco de dados
        forecast.TemperatureC += Random.Shared.Next(-5, 5); // Pequena variação
        
        return CreatedAtAction(nameof(GetById), new { id = 1 }, forecast);
    }
}

public class WeatherForecast
{
    public DateOnly Date { get; set; }
    public int TemperatureC { get; set; }
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
    public string? Summary { get; set; }
}