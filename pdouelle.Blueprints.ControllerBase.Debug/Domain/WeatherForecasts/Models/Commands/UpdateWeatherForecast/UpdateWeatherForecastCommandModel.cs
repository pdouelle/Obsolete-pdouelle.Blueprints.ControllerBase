using System;
using pdouelle.Blueprints.ControllerBase.ModelValidations.Attributes;

namespace pdouelle.Blueprints.ControllerBase.Debug.Domain.WeatherForecasts.Models.Commands.UpdateWeatherForecast
{
    public class UpdateWeatherForecastCommandModel
    {
        public DateTime Date { get; set; }
        public int TemperatureC { get; set; }
        [Exists]
        public int TemperatureF { get; set; }
        public string Summary { get; set; }
    }
}