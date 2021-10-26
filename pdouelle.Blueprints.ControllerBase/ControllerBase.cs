using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Ardalis.GuardClauses;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using pdouelle.Blueprints.ControllerBase.ModelValidations;
using pdouelle.Blueprints.MediatR.Models.Commands.Create;
using pdouelle.Blueprints.MediatR.Models.Commands.Delete;
using pdouelle.Blueprints.MediatR.Models.Commands.Save;
using pdouelle.Blueprints.MediatR.Models.Commands.Update;
using pdouelle.Blueprints.MediatR.Models.Queries.IdQuery;
using pdouelle.Blueprints.MediatR.Models.Queries.ListQuery;
using pdouelle.Blueprints.MediatR.Models.Queries.SingleQuery;
using pdouelle.Entity;
using pdouelle.Errors;
using pdouelle.Pagination;
using pdouelle.Sort;

namespace pdouelle.Blueprints.ControllerBase
{
    public class ControllerBase : Microsoft.AspNetCore.Mvc.ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        private readonly ILogger<ControllerBase> _logger;
        private readonly IModelValidation _model;

        public ControllerBase(IMediator mediator, IMapper mapper, ILogger<ControllerBase> logger, IModelValidation model)
        {
            Guard.Against.Null(mediator, nameof(mediator));
            Guard.Against.Null(mapper, nameof(mapper));
            Guard.Against.Null(logger, nameof(logger));
            Guard.Against.Null(model, nameof(model));

            _mediator = mediator;
            _mapper = mapper;
            _logger = logger;
            _model = model;
        }

        /// <summary>
        /// Get all
        /// </summary>
        /// <returns></returns>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [NonAction]
        protected virtual async Task<IActionResult> GetAsync<TResource, TDto, TQueryList>([FromQuery] TQueryList request, CancellationToken cancellationToken)
            where TResource : IEntity
            where TQueryList : IPagination, ISort
        {
            Guard.Against.Null(request, nameof(request));
            
            PagedList<TResource> resources = await _mediator.Send(new ListQueryModel<TResource, TQueryList>(request), cancellationToken);
            
            var metadata = new
            {
                resources.TotalCount,
                resources.PageSize,
                resources.CurrentPage,
                resources.TotalPages,
                resources.HasNext,
                resources.HasPrevious
            };

            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(metadata));

            var dto = _mapper.Map<IEnumerable<TDto>>(resources);

            return Ok(dto);
        }

        /// <summary>
        /// Get by id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [NonAction]
        protected virtual async Task<IActionResult> GetByIdAsync<TResource, TDto>(Guid id, CancellationToken cancellationToken)
        {
            TResource resource = await _mediator.Send(new IdQueryModel<TResource>(id), cancellationToken);

            if (resource is null)
            {
                _logger.LogInformation("{@Message}", new ResourceNotFound(typeof(TResource), nameof(id), id));
                return NotFound();
            }

            var dto = _mapper.Map<TDto>(resource);

            return Ok(dto);
        }

        /// <summary>
        /// Get single by id
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [NonAction]
        protected virtual async Task<IActionResult> GetSingleAsync<TResource, TDto, TQuerySingle>([FromQuery] TQuerySingle request, CancellationToken cancellationToken)
        {
            Guard.Against.Null(request, nameof(request));
            
            TResource resource = await _mediator.Send(new SingleQueryModel<TResource, TQuerySingle>(request), cancellationToken);

            if (resource is null)
            {
                _logger.LogInformation("{@Message}", new ResourceNotFound(typeof(TResource), request));
                return NotFound();
            }

            var dto = _mapper.Map<TDto>(resource);

            return Ok(dto);
        }

        /// <summary>
        /// Create
        /// </summary>
        /// <returns></returns>
        [ProducesResponseType(StatusCodes.Status201Created)]
        [NonAction]
        protected virtual async Task<IActionResult> PostAsync<TResource, TDto, TCreate>([FromBody] TCreate model, CancellationToken cancellationToken)
            where TDto : IEntity 
        {
            Guard.Against.Null(model, nameof(model));
            
            ModelState modelState = await _model.IsValid<TResource, TCreate>(model, cancellationToken);

            if (modelState.HasError()) 
                return modelState.Error;
            
            var request = _mapper.Map<TResource>(model);

            TResource resource = await _mediator.Send(new CreateCommandModel<TResource>(request), cancellationToken);

            await _mediator.Send(new SaveCommandModel<TResource>(), cancellationToken);

            var dto = _mapper.Map<TDto>(resource);

            return Created($"{HttpContext.Request.Path}/{dto.Id}", dto);
        }

        /// <summary>
        /// Update
        /// </summary>
        /// <param name="id"></param>
        /// <param name="model"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [NonAction]
        protected virtual async Task<IActionResult> PutAsync<TResource, TDto, TUpdate>(Guid id, [FromBody] TUpdate model, CancellationToken cancellationToken) 
        {
            Guard.Against.Null(model, nameof(model));

            ModelState modelState = await _model.IsValid<TResource, TUpdate>(model, cancellationToken);

            if (modelState.HasError()) 
                return modelState.Error;

            TResource resource = await _mediator.Send(new IdQueryModel<TResource>(id), cancellationToken);

            if (resource is null)
            {
                _logger.LogInformation("{@Message}", new ResourceNotFound(typeof(TResource), nameof(id), id));
                return NotFound();
            }

            _mapper.Map(model, resource);

            await _mediator.Send(new UpdateCommandModel<TResource>(resource), cancellationToken);

            await _mediator.Send(new SaveCommandModel<TResource>(), cancellationToken);

            var dto = _mapper.Map<TDto>(resource);

            return Ok(dto);
        }

        /// <summary>
        /// Patch
        /// </summary>
        /// <param name="id"></param>
        /// <param name="model"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [NonAction]
        protected virtual async Task<IActionResult> PatchAsync<TResource, TDto, TPatch>(Guid id, [FromBody] JsonPatchDocument<TPatch> model, CancellationToken cancellationToken)
            where TPatch : class, new() 
        {
            Guard.Against.Null(model, nameof(model));

            var  modelToValidate = new TPatch();
            model.ApplyTo(modelToValidate);
            ModelState modelState = await _model.IsValid<TResource, TPatch>(modelToValidate, cancellationToken);
            
            if (modelState.HasError()) 
                return modelState.Error;
            
            TResource resource = await _mediator.Send(new IdQueryModel<TResource>(id), cancellationToken);

            if (resource is null)
            {
                _logger.LogInformation("{@Message}", new ResourceNotFound(typeof(TResource), nameof(id), id));
                return NotFound();
            }

            var resourceCopy = _mapper.Map<TPatch>(resource);
            model.ApplyTo(resourceCopy);
            _mapper.Map(resourceCopy, resource);

            await _mediator.Send(new UpdateCommandModel<TResource>(resource), cancellationToken);

            await _mediator.Send(new SaveCommandModel<TResource>(), cancellationToken);

            var dto = _mapper.Map<TDto>(resource);

            return Ok(dto);
        }

        /// <summary>
        /// Delete
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [NonAction]
        protected virtual async Task<IActionResult> DeleteAsync<TResource>(Guid id, CancellationToken cancellationToken)
        {
            TResource resource = await _mediator.Send(new IdQueryModel<TResource>(id), cancellationToken);

            if (resource is null)
            {
                _logger.LogInformation("{@Message}", new ResourceNotFound(typeof(TResource), nameof(id), id));
                return NotFound();
            }

            await _mediator.Send(new DeleteCommandModel<TResource>(resource), cancellationToken);

            await _mediator.Send(new SaveCommandModel<TResource>(), cancellationToken);

            return NoContent();
        }
    }
}