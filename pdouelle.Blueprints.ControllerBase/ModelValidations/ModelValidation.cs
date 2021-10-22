using System;
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

        public async Task<ModelState> IsValid<TEntity, TModel, TQuery>(TModel model, CancellationToken cancellationToken)
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
                        var propertyNotExists = await IsExists<TEntity, TQuery, ExistsAttribute>(property, existsAttribute, propertyValue, cancellationToken) != true;

                        if (propertyNotExists)
                        {
                            _logger.LogInformation("{@Message}", new ResourceNotFound(typeof(TEntity), property.Name, propertyValue));
                            return new ModelState(new UnprocessableEntityResult());
                        }
                    }
                    
                    attribute = property.GetCustomAttributes(typeof(UniqueAttribute)).SingleOrDefault();
                    if (attribute is UniqueAttribute uniqueAttribute)
                    {
                        var propertyExists = await IsExists<TEntity, TQuery, UniqueAttribute>(property, uniqueAttribute, propertyValue, cancellationToken);

                        if (propertyExists)
                        {
                            _logger.LogInformation("{@Message}", new ResourceAlreadyExists(typeof(TEntity), property.Name, propertyValue));
                            return new ModelState(new ConflictResult());
                        }
                    }
                }
            }

            return new ModelState();
        }

        private async Task<bool> IsExists<TEntity, TQuery, TAttribute>(MemberInfo property, TAttribute attribute, object value, CancellationToken cancellationToken)
            where TQuery : new() 
            where TAttribute : class, IName
        {
            Guard.Against.Null(property, nameof(property));
            Guard.Against.Null(value, nameof(value));
            
            var propertyName = GetPropertyName(property, attribute);
            
            PropertyInfo propertyQuery = typeof(TQuery).GetProperty(propertyName);

            Guard.Against.Null(propertyQuery, nameof(propertyQuery), new ResourceNotFound(typeof(TQuery), nameof(propertyName), propertyName).ToString());

            var queryModel = new TQuery();

            propertyQuery.SetValue(queryModel, Convert.ChangeType(value, propertyQuery.PropertyType));

            TEntity entity = await _mediator.Send(new SingleQueryModel<TEntity, TQuery>(queryModel), cancellationToken);

            if (entity is null)
                return false;

            return true;
        }
        
        private string GetPropertyName<TCustomAttribute>(MemberInfo propertyInfo, TCustomAttribute customAttribute)
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
    }
}