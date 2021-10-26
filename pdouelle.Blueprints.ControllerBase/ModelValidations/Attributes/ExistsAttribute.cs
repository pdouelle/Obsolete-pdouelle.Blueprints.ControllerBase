using System;
using pdouelle.Blueprints.ControllerBase.ModelValidations.Interfaces;

namespace pdouelle.Blueprints.ControllerBase.ModelValidations.Attributes
{
    public class ExistsAttribute : Attribute, IResource, IName
    {
        public Type Resource { get; set; }
        public string Name { get; set; }

        public ExistsAttribute(Type resource, string name = null)
        {
            Resource = resource;
            Name = name;
        }
    }
}