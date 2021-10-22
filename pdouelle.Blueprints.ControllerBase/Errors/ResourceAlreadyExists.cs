using System.Reflection;

namespace pdouelle.Blueprints.ControllerBase.Errors
{
    public class ResourceAlreadyExists : ErrorObject
    {
        public ResourceAlreadyExists(MemberInfo type, string propertyName, string value)
        {
            Title = $"Another resource with the specified {propertyName} already exists.";
            Detail = $"Another resource of type '{type.Name}' with {propertyName} '{value}' already exists.";
        }
    }
}