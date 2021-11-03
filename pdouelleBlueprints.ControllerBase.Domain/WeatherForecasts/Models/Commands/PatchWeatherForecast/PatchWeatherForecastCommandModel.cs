using System;
using System.ComponentModel.DataAnnotations;

namespace pdouelleBlueprints.ControllerBase.Domain.WeatherForecasts.Models.Commands.PatchWeatherForecast
{
    public class PatchWeatherForecastCommandModel
    {
        public DateTime Date { get; set; }
        public int TemperatureC { get; set; }
        public int TemperatureF { get; set; }
        [Required] public string Summary { get; set; }
        [Required] public string Coucou { get; set; }
    }
}