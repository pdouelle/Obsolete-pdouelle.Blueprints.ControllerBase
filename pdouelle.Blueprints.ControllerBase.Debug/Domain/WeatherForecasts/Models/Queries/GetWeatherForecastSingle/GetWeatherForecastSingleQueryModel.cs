using System;
using Microsoft.AspNetCore.Mvc;
using pdouelle.Blueprints.ControllerBase.Debug.Domain.WeatherForecasts.Entities;
using pdouelle.Entity;
using pdouelle.LinqExtensions.Attributes;
using pdouelle.Sort;

namespace pdouelle.Blueprints.ControllerBase.Debug.Domain.WeatherForecasts.Models.Queries.GetWeatherForecastSingle
{
    public class GetWeatherForecastSingleQueryModel : IEntity
    {
        [Where(Name = nameof(WeatherForecast.Id))]
        [FromRoute]
        public Guid Id { get; set; }

        [Include(Name = nameof(WeatherForecast.ChildEntity))]
        public bool IncludeChildEntities { get; set; }
    }
}