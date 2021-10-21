using System;

namespace pdouelle.Blueprints.ControllerBase.Debug.Domain.WeatherForecasts.Models.Commands.UpdateWeatherForecast
{
    public class UpdateWeatherForecastCommandModel
    {
        public DateTime Date { get; set; }
        public int TemperatureC { get; set; }
        public int TemperatureF { get; set; }
        public string Summary { get; set; }
    }
}