using System;
using pdouelle.Blueprints.ControllerBase.Debug.Domain.ChildEntities.Entities;
using pdouelle.Blueprints.ControllerBase.Debug.Domain.WeatherForecasts.Models.Queries.GetWeatherForecastList;
using pdouelle.Blueprints.ControllerBase.Debug.Domain.WeatherForecasts.Models.Queries.GetWeatherForecastSingle;
using pdouelle.Blueprints.MediatR.Attributes;
using pdouelle.Entity;

namespace pdouelle.Blueprints.ControllerBase.Debug.Domain.WeatherForecasts.Entities
{
    [
        ApiResource
        (
            QueryList = typeof(GetWeatherForecastListQueryModel),
            QuerySingle = typeof(GetWeatherForecastSingleQueryModel)
        )
    ]
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