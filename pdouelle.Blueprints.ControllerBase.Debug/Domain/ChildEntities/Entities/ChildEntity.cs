using System;
using pdouelle.Blueprints.ControllerBase.Debug.Domain.ChildEntities.Models.Queries.GetChildEntityList;
using pdouelle.Blueprints.ControllerBase.Debug.Domain.WeatherForecasts.Entities;
using pdouelle.Blueprints.MediatR.Attributes;
using pdouelle.Entity;

namespace pdouelle.Blueprints.ControllerBase.Debug.Domain.ChildEntities.Entities
{
    [ApiResource(QueryList = typeof(GetChildEntityListQueryModel))]
    public class ChildEntity : IEntity
    {
        public Guid Id { get; set; }
        public string Name { get; set; }

        public Guid WeatherForecastId { get; set; }
        public WeatherForecast WeatherForecast { get; set; }
    }
}