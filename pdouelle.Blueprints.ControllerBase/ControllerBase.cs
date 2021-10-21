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
using pdouelle.Blueprints.ControllerBase.Errors;
using pdouelle.Blueprints.MediatR.Models.Commands.Create;
using pdouelle.Blueprints.MediatR.Models.Commands.Delete;
using pdouelle.Blueprints.MediatR.Models.Commands.Save;
using pdouelle.Blueprints.MediatR.Models.Commands.Update;
using pdouelle.Blueprints.MediatR.Models.Queries.IdQuery;
using pdouelle.Blueprints.MediatR.Models.Queries.ListQuery;
using pdouelle.Blueprints.MediatR.Models.Queries.SingleQuery;
using pdouelle.Entity;
using pdouelle.Pagination;
using pdouelle.Sort;

namespace pdouelle.Blueprints.ControllerBase
{
    public class ControllerBase : Microsoft.AspNetCore.Mvc.ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;

        public ControllerBase(IMediator mediator, IMapper mapper, ILogger logger)
        {
            Guard.Against.Null(mediator, nameof(mediator));
            Guard.Against.Null(mapper, nameof(mapper));
            Guard.Against.Null(logger, nameof(logger));

            _mediator = mediator;
            _mapper = mapper;
            _logger = logger;
        }

        /// <summary>
        /// Get all
        /// </summary>
        /// <returns></returns>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [NonAction]
        protected virtual async Task<IActionResult> GetAsync<TEntity, TDto, TQueryList>([FromQuery] TQueryList request, CancellationToken cancellationToken)
            where TEntity : IEntity
            where TQueryList : IPagination, ISort
        {
            PagedList<TEntity> entities = await _mediator.Send(new ListQueryModel<TEntity, TQueryList>(request), cancellationToken);
            
            var metadata = new
            {
                entities.TotalCount,
                entities.PageSize,
                entities.CurrentPage,
                entities.TotalPages,
                entities.HasNext,
                entities.HasPrevious
            };

            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(metadata));

            var entitiesDto = _mapper.Map<IEnumerable<TDto>>(entities);

            return Ok(entitiesDto);
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
        protected virtual async Task<IActionResult> GetByIdAsync<TEntity, TDto>(Guid id, CancellationToken cancellationToken)
        {
            TEntity entity = await _mediator.Send(new IdQueryModel<TEntity>(id), cancellationToken);

            if (entity is null)
            {
                _logger.LogInformation("{@Message}", new EntityNotFound(id, typeof(TEntity)));
                return NotFound();
            }

            var entityDto = _mapper.Map<TDto>(entity);

            return Ok(entityDto);
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
        protected virtual async Task<IActionResult> GetSingleAsync<TEntity, TDto, TQuerySingle>([FromBody] TQuerySingle request, CancellationToken cancellationToken)
            where TQuerySingle : IEntity
        {
            TEntity entity = await _mediator.Send(new SingleQueryModel<TEntity, TQuerySingle>(request), cancellationToken);

            if (entity is null)
            {
                _logger.LogInformation("{@Message}", new EntityNotFound(request.Id, typeof(TEntity)));
                return NotFound();
            }

            var entityDto = _mapper.Map<TDto>(entity);

            return Ok(entityDto);
        }

        /// <summary>
        /// Create
        /// </summary>
        /// <returns></returns>
        [ProducesResponseType(StatusCodes.Status201Created)]
        [NonAction]
        protected virtual async Task<IActionResult> PostAsync<TEntity, TDto, TCreate>([FromBody] TCreate model, CancellationToken cancellationToken)
            where TDto : IEntity
        {
            var request = _mapper.Map<TEntity>(model);

            TEntity entity = await _mediator.Send(new CreateCommandModel<TEntity>(request), cancellationToken);

            await _mediator.Send(new SaveCommandModel<TEntity>(), cancellationToken);

            var entityDto = _mapper.Map<TDto>(entity);

            return Created($"{HttpContext.Request.Path}/{entityDto.Id}", entityDto);
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
        protected virtual async Task<IActionResult> PutAsync<TEntity, TDto, TUpdate>(Guid id, [FromBody] TUpdate model, CancellationToken cancellationToken)
        {
            TEntity entity = await _mediator.Send(new IdQueryModel<TEntity>(id), cancellationToken);

            if (entity is null)
            {
                _logger.LogInformation("{@Message}", new EntityNotFound(id, typeof(TEntity)));
                return NotFound();
            }

            _mapper.Map(model, entity);

            await _mediator.Send(new UpdateCommandModel<TEntity>(entity), cancellationToken);

            await _mediator.Send(new SaveCommandModel<TEntity>(), cancellationToken);

            var entityDto = _mapper.Map<TDto>(entity);

            return Ok(entityDto);
        }

        /// <summary>
        /// Patch
        /// </summary>
        /// <param name="id"></param>
        /// <param name="patch"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [NonAction]
        protected virtual async Task<IActionResult> PatchAsync<TEntity, TDto, TPatch>(Guid id, [FromBody] JsonPatchDocument<TPatch> patch, CancellationToken cancellationToken)
            where TPatch : class, new()
        {
            TEntity entity = await _mediator.Send(new IdQueryModel<TEntity>(id), cancellationToken);

            if (entity is null)
            {
                _logger.LogInformation("{@Message}", new EntityNotFound(id, typeof(TEntity)));
                return NotFound();
            }

            var entityCopy = _mapper.Map<TPatch>(entity);
            patch.ApplyTo(entityCopy);
            _mapper.Map(entityCopy, entity);

            await _mediator.Send(new UpdateCommandModel<TEntity>(entity), cancellationToken);

            await _mediator.Send(new SaveCommandModel<TEntity>(), cancellationToken);

            var entityDto = _mapper.Map<TDto>(entity);

            return Ok(entityDto);
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
        protected virtual async Task<IActionResult> DeleteAsync<TEntity>(Guid id, CancellationToken cancellationToken)
        {
            TEntity entity = await _mediator.Send(new IdQueryModel<TEntity>(id), cancellationToken);

            if (entity is null)
            {
                _logger.LogInformation("{@Message}", new EntityNotFound(id, typeof(TEntity)));
                return NotFound();
            }

            await _mediator.Send(new DeleteCommandModel<TEntity>(entity), cancellationToken);

            await _mediator.Send(new SaveCommandModel<TEntity>(), cancellationToken);

            return NoContent();
        }
    }
}