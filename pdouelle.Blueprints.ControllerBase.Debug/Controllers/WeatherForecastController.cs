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
using pdouelle.Blueprints.ControllerBase.Debug.Domain.ChildEntities.Entities;
using pdouelle.Blueprints.ControllerBase.Debug.Domain.ChildEntities.Models;
using pdouelle.Blueprints.ControllerBase.Debug.Domain.ChildEntities.Models.Commands.CreateChildEntity;
using pdouelle.Blueprints.ControllerBase.Debug.Domain.ChildEntities.Models.Commands.PatchChildEntity;
using pdouelle.Blueprints.ControllerBase.Debug.Domain.WeatherForecasts.Entities;
using pdouelle.Blueprints.ControllerBase.Debug.Domain.WeatherForecasts.Models;
using pdouelle.Blueprints.ControllerBase.Debug.Domain.WeatherForecasts.Models.Commands.CreateWeatherForecast;
using pdouelle.Blueprints.ControllerBase.Debug.Domain.WeatherForecasts.Models.Commands.PatchWeatherForecast;
using pdouelle.Blueprints.ControllerBase.Debug.Domain.WeatherForecasts.Models.Commands.UpdateWeatherForecast;
using pdouelle.Blueprints.ControllerBase.Debug.Domain.WeatherForecasts.Models.Queries.GetWeatherForecastList;
using pdouelle.Blueprints.ControllerBase.Debug.Domain.WeatherForecasts.Models.Queries.GetWeatherForecastSingle;

namespace pdouelle.Blueprints.ControllerBase.Debug.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        public WeatherForecastController(IMediator mediator, IMapper mapper, ILogger<WeatherForecastController> logger) : base(mediator, mapper, logger)
        {
            Guard.Against.Null(mediator, nameof(mediator));
            Guard.Against.Null(mapper, nameof(mapper));
            Guard.Against.Null(logger, nameof(logger));
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK,Type = typeof(List<WeatherForecastDto>))]
        public async Task<IActionResult> GetAsync([FromQuery] GetWeatherForecastListQueryModel request, CancellationToken cancellationToken)
        {
            return await base.GetAsync<WeatherForecast, WeatherForecastDto, GetWeatherForecastListQueryModel>(request, cancellationToken);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetSingleAsync([FromQuery] GetWeatherForecastSingleQueryModel request, CancellationToken cancellationToken)
        {
            return await base.GetSingleAsync<WeatherForecast, WeatherForecastDto, GetWeatherForecastSingleQueryModel>(request, cancellationToken);
        }

        [HttpPost]
        public async Task<IActionResult> PostAsync([FromBody] CreateWeatherForecastCommandModel model, CancellationToken cancellationToken)
        {
            return await base.PostAsync<WeatherForecast, WeatherForecastDto, CreateWeatherForecastCommandModel>(model, cancellationToken);
        }
        
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAsync(Guid id, [FromBody] UpdateWeatherForecastCommandModel model, CancellationToken cancellationToken)
        {
            return await base.PutAsync<WeatherForecast, WeatherForecastDto, UpdateWeatherForecastCommandModel>(id, model, cancellationToken);
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> PatchAsync(Guid id, [FromBody] JsonPatchDocument<PatchWeatherForecastCommandModel> patch, CancellationToken cancellationToken)
        {
            return await base.PatchAsync<WeatherForecast, WeatherForecastDto, PatchWeatherForecastCommandModel>(id, patch, cancellationToken);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(Guid id, CancellationToken cancellationToken)
        {
            return await base.DeleteAsync<WeatherForecast>(id, cancellationToken);
        }
        
        [HttpPost("{id}/childEntities")]
        public async Task<IActionResult> PostRelationshipAsync(Guid id, [FromBody] CreateChildEntityCommandModel model, CancellationToken cancellationToken)
        {
            model.WeatherForecastId = id;
            
            return await base.PostAsync<ChildEntity, ChildEntityDto, CreateChildEntityCommandModel>(model, cancellationToken);
        }

        [HttpPatch("{parentId}/childEntities/{id}")]
        public async Task<IActionResult> PatchRelationshipAsync(Guid parentId, Guid id, JsonPatchDocument<PatchChildEntityCommandModel> patch, CancellationToken cancellationToken)
        {
            return await base.PatchAsync<ChildEntity, ChildEntityDto, PatchChildEntityCommandModel>(id, patch, cancellationToken);
        }

        [HttpDelete("{parentId}/childEntities/{id}")]
        public async Task<IActionResult> DeleteRelationshipAsync(Guid parentId, Guid id, CancellationToken cancellationToken)
        {
            return await base.DeleteAsync<ChildEntity>(id, cancellationToken);
        }
    }
}