using System;
using System.Collections;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Ardalis.GuardClauses;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using pdouelle.Blueprints.ControllerBase.Errors;
using pdouelle.Blueprints.ControllerBase.ModelValidations.Attributes;
using pdouelle.Blueprints.ControllerBase.ModelValidations.Interfaces;
using pdouelle.Blueprints.MediatR.Models.Queries.ListQuery;
using pdouelle.Blueprints.MediatR.Models.Queries.SingleQuery;
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

        public async Task<ModelState> IsValid<TResource, TModel, TQuery>(TModel model, CancellationToken cancellationToken)
            where TQuery : new()
        {
            Guard.Against.Null(model, nameof(model));

            PropertyInfo[] properties = model.GetType().GetProperties();

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
                            _logger.LogInformation("{@Message}", new ResourceNotFound(typeof(TResource), property.Name, propertyValue));
                            return new ModelState(new UnprocessableEntityResult());
                        }
                    }

                    attribute = property.GetCustomAttributes(typeof(UniqueAttribute)).SingleOrDefault();
                    if (attribute is UniqueAttribute uniqueAttribute)
                    {
                        var propertyExists = await IsExisting<TResource, TQuery, UniqueAttribute>(property, uniqueAttribute, propertyValue, cancellationToken);

                        if (propertyExists)
                        {
                            _logger.LogInformation("{@Message}", new ResourceAlreadyExists(typeof(TResource), property.Name, propertyValue));
                            return new ModelState(new ConflictResult());
                        }
                    }
                }
            }

            return new ModelState();
        }

        private const bool IsResourceExisting = true;
        private const bool IsResourceNotExisting = false;

        private async Task<bool> IsExisting<TAttribute>(MemberInfo property, TAttribute attribute, object value, CancellationToken cancellationToken)
            where TAttribute : class, IExist, IName
        {
            Guard.Against.Null(property, nameof(property));
            Guard.Against.Null(value, nameof(value));
            Guard.Against.Null(attribute, nameof(attribute));

            Type queryModelType = attribute.QueryType;

            PropertyInfo propertyInfo = GetProperty(queryModelType, property, attribute);

            var queryModel = Activator.CreateInstance(queryModelType);

            SetValueOnModel(queryModel, propertyInfo, value);

            Type resourceType = attribute.Resource;

            Type listQueryType = typeof(ListQueryModel<,>).MakeGenericType(resourceType, queryModelType);

            var listQueryModel = Activator.CreateInstance(listQueryType, queryModel);

            var entities = (IList)await _mediator.Send(listQueryModel, cancellationToken);

            return entities?.Count > 0 ? IsResourceExisting : IsResourceNotExisting;
        }

        private async Task<bool> IsExisting<TResource, TQuery, TAttribute>(MemberInfo property, TAttribute attribute, object value, CancellationToken cancellationToken)
            where TQuery : new()
            where TAttribute : class, IName
        {
            Guard.Against.Null(property, nameof(property));
            Guard.Against.Null(value, nameof(value));
            Guard.Against.Null(attribute, nameof(attribute));

            PropertyInfo propertyInfo = GetProperty(typeof(TQuery), property, attribute);

            var queryModel = new TQuery();

            SetValueOnModel(queryModel, propertyInfo, value);

            TResource resource = await _mediator.Send(new SingleQueryModel<TResource, TQuery>(queryModel), cancellationToken);

            return resource is not null ? IsResourceExisting : IsResourceNotExisting;
        }

        private static string GetPropertyName<TCustomAttribute>(MemberInfo propertyInfo, TCustomAttribute customAttribute)
            where TCustomAttribute : class, IName
        {
            Guard.Against.Null(propertyInfo, nameof(propertyInfo));
            Guard.Against.Null(customAttribute, nameof(customAttribute));

            string propertyName;

            if (string.IsNullOrEmpty(customAttribute?.Name))
            {
                propertyName = propertyInfo.Name;
            }
            else
            {
                propertyName = customAttribute.Name;
            }

            return propertyName;
        }

        private static void SetValueOnModel<TModel>(TModel model, PropertyInfo propertyInfo, object value)
        {
            Type t = Nullable.GetUnderlyingType(propertyInfo.PropertyType) ?? propertyInfo.PropertyType;
            var safeValue = Convert.ChangeType(value, t);

            propertyInfo.SetValue(model, safeValue);
        }

        private static PropertyInfo GetProperty<TAttribute>(Type model, MemberInfo property, TAttribute attribute)
            where TAttribute : class, IName
        {
            var propertyName = GetPropertyName(property, attribute);

            PropertyInfo propertyInfo = model.GetProperty(propertyName);

            Guard.Against.Null(propertyInfo, nameof(propertyInfo), new ResourceNotFound(model, nameof(propertyName), propertyName).ToString());

            return propertyInfo;
        }
    }
}