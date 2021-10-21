using System;

namespace pdouelle.Blueprints.ControllerBase.Debug.Domain.ChildEntities.Models.Commands.CreateChildEntity
{
    public class CreateChildEntityCommandModel
    {
        public string Name { get; set; }

        [System.Text.Json.Serialization.JsonIgnore]
        public Guid WeatherForecastId { get; set; }
    }
}