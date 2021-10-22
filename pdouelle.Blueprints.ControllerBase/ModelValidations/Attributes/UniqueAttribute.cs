using System;
using pdouelle.Blueprints.ControllerBase.ModelValidations.Interfaces;

namespace pdouelle.Blueprints.ControllerBase.ModelValidations.Attributes
{
    public class UniqueAttribute : Attribute, IName
    {
        public string Name { get; set; }
    }
}