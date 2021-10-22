using System;
using Microsoft.AspNetCore.Mvc;
using pdouelle.Blueprints.ControllerBase.Debug.Domain.WeatherForecasts.Entities;
using pdouelle.Entity;
using pdouelle.LinqExtensions.Attributes;

namespace pdouelle.Blueprints.ControllerBase.Debug.Domain.WeatherForecasts.Models.Queries.GetWeatherForecastSingle
{
    public class GetWeatherForecastSingleQueryModel : IEntity
    {
        [FromRoute]
        [Where]
        public Guid Id { get; set; }
        
        [Where]
        public int TemperatureC { get; set; }
        
        [Where]
        public int TemperatureF { get; set; }
        
        [Include(Name = nameof(WeatherForecast.ChildEntity))]
        public bool IncludeChildEntities { get; set; }
    }
}