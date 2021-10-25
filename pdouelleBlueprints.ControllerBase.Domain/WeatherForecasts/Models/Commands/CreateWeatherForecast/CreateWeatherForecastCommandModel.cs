using System;
using pdouelle.Blueprints.ControllerBase.ModelValidations.Attributes;
using pdouelleBlueprints.ControllerBase.Domain.WeatherForecasts.Entities;
using pdouelleBlueprints.ControllerBase.Domain.WeatherForecasts.Models.Queries.GetWeatherForecastList;

namespace pdouelleBlueprints.ControllerBase.Domain.WeatherForecasts.Models.Commands.CreateWeatherForecast
{
    public class CreateWeatherForecastCommandModel
    {
        public Guid Id { get; set; }
        public DateTime Date { get; set; }
        
        [Exists(Resource = typeof(WeatherForecast), QueryType = typeof(GetWeatherForecastListQueryModel))]
        public int TemperatureC { get; set; }
        
        [Unique]
        public int TemperatureF { get; set; }
        
        public string Summary { get; set; }
    }
}