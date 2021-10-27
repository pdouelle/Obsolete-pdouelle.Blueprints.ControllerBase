using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using pdouelle.Blueprints.ControllerBase.ModelValidations;
using pdouelle.Entity;
using pdouelle.Pagination;
using pdouelle.Sort;

namespace pdouelle.Blueprints.ControllerBase.Controllers
{
    public class BlueprintController<TResource, TDto, TQueryList, TCreate, TPatch> : BlueprintControllerBase
        where TResource : IEntity
        where TDto : IEntity
        where TQueryList : IPagination, ISort
        where TPatch : class, new()
    {
        public BlueprintController
        (
            IMediator mediator,
            IMapper mapper,
            ILogger<BlueprintController<TResource, TDto, TQueryList, TCreate, TPatch>> logger,
            IModelValidation model
        ) : base(mediator, mapper, logger, model)
        {
        }

        /// <summary>
        /// Get all
        /// </summary>
        /// <returns></returns>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpGet]
        public async Task<IActionResult> GetAsync([FromQuery] TQueryList request, CancellationToken cancellationToken) =>
            await base.GetAsync<TResource, TDto, TQueryList>(request, cancellationToken);

        /// <summary>
        /// Get by id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdAsync(Guid id, CancellationToken cancellationToken) =>
            await base.GetByIdAsync<TResource, TDto>(id, cancellationToken);

        /// <summary>
        /// Create
        /// </summary>
        /// <returns></returns>
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        [HttpPost]
        public async Task<IActionResult> PostAsync([FromBody] TCreate model, CancellationToken cancellationToken) =>
            await base.PostAsync<TResource, TDto, TCreate>(model, cancellationToken);

        /// <summary>
        /// Patch
        /// </summary>
        /// <param name="id"></param>
        /// <param name="model"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        [HttpPatch("{id}")]
        public async Task<IActionResult> PatchAsync(Guid id, [FromBody] JsonPatchDocument<TPatch> model, CancellationToken cancellationToken) =>
            await base.PatchAsync<TResource, TDto, TPatch>(id, model, cancellationToken);

        /// <summary>
        /// Delete
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(Guid id, CancellationToken cancellationToken) =>
            await base.DeleteAsync<TResource>(id, cancellationToken);
    }
}