using System;
using pdouelle.Blueprints.MediatR.Attributes;
using pdouelle.Entity;
using pdouelleBlueprints.ControllerBase.Domain.ChildEntities.Models.Queries.GetChildEntityList;
using pdouelleBlueprints.ControllerBase.Domain.WeatherForecasts.Entities;

namespace pdouelleBlueprints.ControllerBase.Domain.ChildEntities.Entities
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