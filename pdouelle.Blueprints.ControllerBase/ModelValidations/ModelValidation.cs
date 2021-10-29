using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Ardalis.GuardClauses;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using pdouelle.Blueprints.ControllerBase.ModelValidations.Attributes;
using pdouelle.Blueprints.ControllerBase.ModelValidations.Interfaces;
using pdouelle.Blueprints.MediatR.Models.Queries.ExistsQuery;
using pdouelle.Errors;
using IName = pdouelle.Blueprints.ControllerBase.ModelValidations.Interfaces.IName;

namespace pdouelle.Blueprints.ControllerBase.ModelValidations
{
    public class ModelValidation : IModelValidation
    {
        private readonly IMediator _mediator;
        private readonly ILogger<ModelValidation> _logger;

        public ModelValidation(IMediator mediator, ILogger<ModelValidation> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        public async Task<ModelState> IsValid<TResource, TModel>(TModel model, CancellationToken cancellationToken)
        {
            Guard.Against.Null(model, nameof(model));

            PropertyInfo[] properties = typeof(TModel).GetProperties();

            foreach (PropertyInfo property in properties)
            {
                var propertyValue = property.GetValue(model, null);

                if (propertyValue is not null)
                {
                    Attribute attribute = property.GetCustomAttributes(typeof(ExistsAttribute)).SingleOrDefault();

                    if (attribute is ExistsAttribute existsAttribute)
                    {
                        var propertyNotExists = await IsExisting(property, existsAttribute, propertyValue, cancellationToken) != true;
                        

                        if (propertyNotExists)
                        {
                            _logger.LogInformation("{@Message}", new ResourceNotFound(existsAttribute.Resource, property.Name, propertyValue));
                            return new ModelState(new UnprocessableEntityResult());
                        }
                    }

                    attribute = property.GetCustomAttributes(typeof(UniqueAttribute)).SingleOrDefault();
                    if (attribute is UniqueAttribute uniqueAttribute)
                    {
                        var propertyExists = await IsExisting(property, uniqueAttribute, propertyValue, cancellationToken);

                        if (propertyExists)
                        {
                            _logger.LogInformation("{@Message}", new ResourceAlreadyExists(uniqueAttribute.Resource, property.Name, propertyValue));
                            return new ModelState(new ConflictResult());
                        }
                    }
                }
            }

            return new ModelState();
        }

        private async Task<bool> IsExisting<TAttribute>(MemberInfo property, TAttribute attribute, object value, CancellationToken cancellationToken)
            where TAttribute : class, IResource, IName
        {
            Guard.Against.Null(property, nameof(property));
            Guard.Against.Null(value, nameof(value));
            Guard.Against.Null(attribute, nameof(attribute));
            Guard.Against.Null(attribute.Resource, nameof(attribute.Resource));
            
            Type resourceType = attribute.Resource;
            
            Type existsType = typeof(ExistsQueryModel<>).MakeGenericType(resourceType);

            var propertyName = GetPropertyName(property, attribute);

            var keyValues = new List<KeyValuePair<string, object>> { new(propertyName, value) };
            
            var existQueryModel = Activator.CreateInstance(existsType, keyValues);

            return (bool) await _mediator.Send(existQueryModel, cancellationToken);
        }

        private static string GetPropertyName<TCustomAttribute>(MemberInfo propertyInfo, TCustomAttribute customAttribute)
            where TCustomAttribute : class, IName
        {
            Guard.Against.Null(propertyInfo, nameof(propertyInfo));
            Guard.Against.Null(customAttribute, nameof(customAttribute));

            string propertyName;

            if (string.IsNullOrEmpty(customAttribute?.Name))
                propertyName = propertyInfo.Name;
            else
                propertyName = customAttribute.Name;

            return propertyName;
        }
    }
}