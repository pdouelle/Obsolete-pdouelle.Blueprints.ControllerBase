using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using pdouelle.Blueprints.MediatR.Models.Commands.Create;
using pdouelle.Blueprints.MediatR.Models.Commands.Delete;
using pdouelle.Blueprints.MediatR.Models.Commands.Patch;
using pdouelle.Blueprints.MediatR.Models.Commands.Save;
using pdouelle.Blueprints.MediatR.Models.Commands.Update;
using pdouelle.Blueprints.MediatR.Models.Queries.IdQuery;
using pdouelle.Blueprints.MediatR.Models.Queries.ListQuery;
using pdouelle.Entity;
using pdouelle.Pagination;
using pdouelle.Sort;

namespace pdouelle.Blueprints.ControllerBase
{
    public class ControllerBase<TEntity, TDto, TQueryById> : Microsoft.AspNetCore.Mvc.ControllerBase 
        where TEntity : IEntity
        where TDto : IEntity
        where TQueryById : IEntity, new() 
    {
        protected readonly IMediator Mediator;
        protected readonly IMapper Mapper;
        protected readonly ILogger Logger;

        public ControllerBase(IMediator mediator, IMapper mapper, ILogger logger)
        {
            Mediator = mediator;
            Mapper = mapper;
            Logger = logger;
        }

        /// <summary>
        /// Get all
        /// </summary>
        /// <returns></returns>
        [ProducesResponseType(StatusCodes.Status200OK)]
        public virtual async Task<IActionResult> GetList<TQueryList>([FromQuery] TQueryList request, CancellationToken cancellationToken) where TQueryList : IPagination, ISort
        {
            PagedList<TEntity> response = await Mediator.Send(new ListQueryModel<TEntity, TQueryList>
            {
                Request = request
            }, cancellationToken);
            
            var metadata = new
            {
                response.TotalCount,
                response.PageSize,
                response.CurrentPage,
                response.TotalPages,
                response.HasNext,
                response.HasPrevious
            };
            
            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(metadata));

            var mappedResponse = Mapper.Map<IEnumerable<TDto>>(response);

            return Ok(mappedResponse);
        }

        /// <summary>
        /// Get by id
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public virtual async Task<IActionResult> GetById([FromQuery] TQueryById request, CancellationToken cancellationToken)
        {
            TEntity response = await Mediator.Send(new IdQueryModel<TEntity, TQueryById>
            {
                Request = request
            }, cancellationToken);

            if (response is null)
            {
                Logger.LogInformation("NOT FOUND | request: {@Request}", request);
                return NotFound();
            }

            var mappedResponse = Mapper.Map<TDto>(response);

            return Ok(mappedResponse);
        }
        
        /// <summary>
        /// Create
        /// </summary>
        /// <returns></returns>
        [ProducesResponseType(StatusCodes.Status201Created)]
        public virtual async Task<IActionResult> Create<TCreate>([FromBody] TCreate request, CancellationToken cancellationToken)
        {
            TEntity entity = await Mediator.Send(new CreateCommandModel<TEntity, TCreate>
            {
                Request = request
            }, cancellationToken);

            await Mediator.Send(new SaveCommandModel<TEntity>(), cancellationToken);

            var mappedResponse = Mapper.Map<TDto>(entity);

            return CreatedAtAction(nameof(GetById), new {id = mappedResponse.Id}, mappedResponse);
        }

        /// <summary>
        /// Update
        /// </summary>
        /// <param name="id"></param>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public virtual async Task<IActionResult> Update<TUpdate>(Guid id, [FromBody] TUpdate request, CancellationToken cancellationToken) 
            where TUpdate : IEntity
        {
            request.Id = id;
            
            TEntity entity = await Mediator.Send(new IdQueryModel<TEntity, TQueryById>
            {
                Request = new TQueryById {Id = request.Id}
            }, cancellationToken);

            if (entity is null)
            {
                Logger.LogInformation("NOT FOUND | request: {@Request}", request);
                return NotFound();
            }

            await Mediator.Send(new UpdateCommandModel<TEntity, TUpdate>
            {
                Entity = entity,
                Request = request
            }, cancellationToken);

            await Mediator.Send(new SaveCommandModel<TEntity>(), cancellationToken);

            var mappedResponse = Mapper.Map<TDto>(entity);

            return Ok(mappedResponse);
        }

        /// <summary>
        /// Patch
        /// </summary>
        /// <param name="id"></param>
        /// <param name="patchDocument"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public virtual async Task<IActionResult> Patch<TPatch>(Guid id, [FromBody] JsonPatchDocument<TPatch> patchDocument, CancellationToken cancellationToken)
            where TPatch : class, new()
        {
            var request = new TPatch();
            patchDocument.ApplyTo(request);
            
            TEntity entity = await Mediator.Send(new IdQueryModel<TEntity, TQueryById>
            {
                Request = new TQueryById {Id = id}
            }, cancellationToken);

            if (entity is null)
            {
                Logger.LogInformation("NOT FOUND | request: {@Request}", request);
                return NotFound();
            }

            TEntity response = await Mediator.Send(new PatchCommandModel<TEntity, TPatch>
            {
                Entity = entity,
                Request = request
            }, cancellationToken);

            await Mediator.Send(new SaveCommandModel<TEntity>(), cancellationToken);

            var mappedResponse = Mapper.Map<TDto>(response);

            return Ok(mappedResponse);
        }

        /// <summary>
        /// Delete
        /// </summary>
        /// <param name="id"></param>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public virtual async Task<IActionResult> Delete<TDelete>(Guid id, [FromQuery] TDelete request, CancellationToken cancellationToken)
            where TDelete : IEntity
        {
            request.Id = id;
            
            TEntity entity = await Mediator.Send(new IdQueryModel<TEntity, TQueryById>
            {
                Request = new TQueryById {Id = request.Id}
            }, cancellationToken);

            if (entity is null)
            {
                Logger.LogInformation("NOT FOUND | request: {@Request}", request);
                return NotFound();
            }

            await Mediator.Send(new DeleteCommandModel<TEntity, TDelete>
            {
                Entity = entity,
                Request = request
            }, cancellationToken);

            await Mediator.Send(new SaveCommandModel<TEntity>(), cancellationToken);

            return NoContent();
        }
    }
}