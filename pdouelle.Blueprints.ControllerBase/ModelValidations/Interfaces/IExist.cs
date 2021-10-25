using System;

namespace pdouelle.Blueprints.ControllerBase.ModelValidations.Interfaces
{
    public interface IExist
    {
        public Type Resource { get; set; }
        public Type QueryType { get; set; }
    }
}