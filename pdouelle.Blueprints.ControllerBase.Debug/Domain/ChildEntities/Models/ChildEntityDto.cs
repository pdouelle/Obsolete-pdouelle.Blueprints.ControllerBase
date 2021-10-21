using System;
using pdouelle.Entity;

namespace pdouelle.Blueprints.ControllerBase.Debug.Domain.ChildEntities.Models
{
    public class ChildEntityDto : IEntity
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
    }
}