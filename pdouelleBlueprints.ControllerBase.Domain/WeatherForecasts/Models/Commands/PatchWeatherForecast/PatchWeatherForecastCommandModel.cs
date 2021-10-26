using System;
using pdouelle.Blueprints.ControllerBase.ModelValidations.Attributes;
using pdouelleBlueprints.ControllerBase.Domain.WeatherForecasts.Entities;

namespace pdouelleBlueprints.ControllerBase.Domain.WeatherForecasts.Models.Commands.PatchWeatherForecast
{
    public class PatchWeatherForecastCommandModel
    {
        public Guid Id { get; set; }
        public DateTime Date { get; set; }
        [Exists(typeof(WeatherForecast))]
        public int TemperatureC { get; set; }
        public int TemperatureF { get; set; }
        public string Summary { get; set; }
    }
}