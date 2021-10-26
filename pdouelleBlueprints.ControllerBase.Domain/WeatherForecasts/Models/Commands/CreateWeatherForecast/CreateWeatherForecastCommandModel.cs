using System;
using pdouelle.Blueprints.ControllerBase.ModelValidations.Attributes;
using pdouelleBlueprints.ControllerBase.Domain.WeatherForecasts.Entities;

namespace pdouelleBlueprints.ControllerBase.Domain.WeatherForecasts.Models.Commands.CreateWeatherForecast
{
    public class CreateWeatherForecastCommandModel
    {
        public Guid Id { get; set; }
        public DateTime Date { get; set; }
        
        //[Exists(Resource = typeof(WeatherForecast))]
        public int TemperatureC { get; set; }
        
        [Unique(typeof(WeatherForecast))]
        public int TemperatureF { get; set; }
        
        public string Summary { get; set; }
    }
}