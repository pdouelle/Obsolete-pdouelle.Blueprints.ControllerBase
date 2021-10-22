using Microsoft.Extensions.DependencyInjection;
using pdouelle.Blueprints.ControllerBase.ModelValidations;

namespace pdouelle.Blueprints.ControllerBase
{
    public static class ControllerBaseServiceExtensions
    {
        public static IServiceCollection AddModelValidation(this IServiceCollection services)
        {
            services.AddTransient<IModelValidation, ModelValidation>();
            
            return services;
        }
    }
}