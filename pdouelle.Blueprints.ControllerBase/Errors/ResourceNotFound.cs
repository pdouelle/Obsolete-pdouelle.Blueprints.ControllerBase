using System;
using System.Reflection;
using Newtonsoft.Json;

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
        
        public ResourceNotFound(MemberInfo type, string propertyName, object value) : this(type, propertyName, value.ToString())
        {
        }
        
        public ResourceNotFound(MemberInfo type, object value) : this(type, "multiple properties", JsonConvert.SerializeObject(value))
        {
        }
    }
}