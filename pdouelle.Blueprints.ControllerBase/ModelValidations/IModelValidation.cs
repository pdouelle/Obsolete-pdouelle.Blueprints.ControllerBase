using System.Threading;
using System.Threading.Tasks;

namespace pdouelle.Blueprints.ControllerBase.ModelValidations
{
    public interface IModelValidation
    {
        Task<ModelState> IsValid<TResource, TModel, TQuery>(TModel model, CancellationToken cancellationToken)
            where TQuery : new();
    }
}