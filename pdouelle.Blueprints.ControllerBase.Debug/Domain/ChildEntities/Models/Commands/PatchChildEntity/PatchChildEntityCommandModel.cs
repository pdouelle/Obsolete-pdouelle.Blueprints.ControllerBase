using System;

namespace pdouelle.Blueprints.ControllerBase.Debug.Domain.ChildEntities.Models.Commands.PatchChildEntity
{
    public class PatchChildEntityCommandModel
    {
        public string Name { get; set; }
        
        public Guid WeatherForecastId { get; set; }
    }
}