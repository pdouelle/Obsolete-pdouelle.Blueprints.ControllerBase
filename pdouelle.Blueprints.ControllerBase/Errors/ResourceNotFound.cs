using System;
using System.Reflection;

namespace pdouelle.Blueprints.ControllerBase.Errors
{
    public class ResourceNotFound : ErrorObject
    {
        public ResourceNotFound(MemberInfo type, string propertyName, string value)
        {
            Title = "The requested resource does not exist.";
            Detail = $"Resource of type '{type.Name}' with {propertyName} '{value}' does not exist.";
        }

        public ResourceNotFound(MemberInfo type, string propertyName, Guid value) : this(type, propertyName, value.ToString())
        {
        }
    }
}