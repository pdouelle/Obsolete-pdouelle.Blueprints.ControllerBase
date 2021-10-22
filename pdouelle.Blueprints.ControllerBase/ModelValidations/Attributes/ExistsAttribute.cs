using System;
using pdouelle.Blueprints.ControllerBase.ModelValidations.Interfaces;

namespace pdouelle.Blueprints.ControllerBase.ModelValidations.Attributes
{
    public class ExistsAttribute : Attribute, IName
    {
        public string Name { get; set; }
    }
}