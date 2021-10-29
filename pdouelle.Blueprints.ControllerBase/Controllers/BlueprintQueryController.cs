using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using pdouelle.Blueprints.ControllerBase.ModelValidations;
using pdouelle.Entity;
using pdouelle.LinqExtensions.Interfaces;
using pdouelle.Pagination;
using pdouelle.Sort;

namespace pdouelle.Blueprints.ControllerBase.Controllers
{
    public class BlueprintQueryController<TResource, TDto, TQueryList> : BlueprintControllerBase 
        where TResource : IEntity 
        where TQueryList : IPagination, ISort, IInclude
    {
        public BlueprintQueryController
        (
            IMediator mediator,
            IMapper mapper,
            ILogger<BlueprintQueryController<TResource, TDto, TQueryList>> logger,
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
    }
}