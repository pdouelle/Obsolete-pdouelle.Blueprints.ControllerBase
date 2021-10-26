using System;
using pdouelle.Blueprints.ControllerBase.ModelValidations.Interfaces;

namespace pdouelle.Blueprints.ControllerBase.ModelValidations.Attributes
{
    public class UniqueAttribute : Attribute, IName, IResource
    {
        public Type Resource { get; set; }
        public string Name { get; set; }

        public UniqueAttribute(Type resource, string name = null)
        {
            Resource = resource;
            Name = name;
        }
    }
}