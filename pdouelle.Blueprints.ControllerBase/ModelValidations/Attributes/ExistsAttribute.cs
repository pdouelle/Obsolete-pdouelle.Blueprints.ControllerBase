using System;
using pdouelle.Blueprints.ControllerBase.ModelValidations.Interfaces;

namespace pdouelle.Blueprints.ControllerBase.ModelValidations.Attributes
{
    public class ExistsAttribute : Attribute, IExist, IName
    {
        public Type Resource { get; set; }
        public Type QueryType { get; set; }
        public string Name { get; set; }
    }
}