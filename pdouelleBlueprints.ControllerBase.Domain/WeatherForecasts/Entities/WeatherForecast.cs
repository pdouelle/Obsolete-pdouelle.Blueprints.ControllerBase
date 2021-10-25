using System;
using pdouelle.Blueprints.MediatR.Attributes;
using pdouelle.Entity;
using pdouelleBlueprints.ControllerBase.Domain.ChildEntities.Entities;
using pdouelleBlueprints.ControllerBase.Domain.WeatherForecasts.Models.Queries.GetWeatherForecastList;
using pdouelleBlueprints.ControllerBase.Domain.WeatherForecasts.Models.Queries.GetWeatherForecastSingle;

namespace pdouelleBlueprints.ControllerBase.Domain.WeatherForecasts.Entities
{
    [ApiResource(QueryList = typeof(GetWeatherForecastListQueryModel), QuerySingle = typeof(GetWeatherForecastSingleQueryModel))]
    public class WeatherForecast : IEntity
    {
        public Guid Id { get; set; }
        public DateTime Date { get; set; }
        public int TemperatureC { get; set; }
        public int TemperatureF { get; set; }
        public string Summary { get; set; }
        
        public virtual ChildEntity ChildEntity { get; set; }
    }
}