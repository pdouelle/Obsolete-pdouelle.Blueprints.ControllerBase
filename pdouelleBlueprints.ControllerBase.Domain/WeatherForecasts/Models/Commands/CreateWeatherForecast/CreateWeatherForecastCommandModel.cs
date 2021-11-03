using System;
using System.ComponentModel.DataAnnotations;

namespace pdouelleBlueprints.ControllerBase.Domain.WeatherForecasts.Models.Commands.CreateWeatherForecast
{
    public class CreateWeatherForecastCommandModel
    {
        public Guid Id { get; set; }
        public DateTime Date { get; set; }
        public int TemperatureC { get; set; }
        public int TemperatureF { get; set; }
        [Required] public string Summary { get; set; }
    }
}