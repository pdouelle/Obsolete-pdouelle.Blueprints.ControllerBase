using System;
using System.Reflection;

namespace pdouelle.Blueprints.ControllerBase.Errors
{
    public class EntityNotFound : ErrorObject
    {
        public EntityNotFound(Guid id, MemberInfo entityType)
        {
            Title = "The requested resource does not exist.";
            Detail = $"Resource of type '{entityType.Name}' with ID '{id}' does not exist.";
        }
    }
}