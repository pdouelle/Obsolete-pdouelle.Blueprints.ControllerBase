using System;
using pdouelle.Blueprints.ControllerBase.ModelValidations.Attributes;

namespace pdouelleBlueprints.ControllerBase.Domain.WeatherForecasts.Models.Commands.PatchWeatherForecast
{
    public class PatchWeatherForecastCommandModel
    {
        public Guid Id { get; set; }
        public DateTime Date { get; set; }
        [Exists]
        public int TemperatureC { get; set; }
        public int TemperatureF { get; set; }
        public string Summary { get; set; }
    }
}