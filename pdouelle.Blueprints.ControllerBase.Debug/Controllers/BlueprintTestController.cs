using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using pdouelle.Blueprints.ControllerBase.Controllers;
using pdouelle.Blueprints.ControllerBase.ModelValidations;
using pdouelleBlueprints.ControllerBase.Domain.WeatherForecasts.Entities;
using pdouelleBlueprints.ControllerBase.Domain.WeatherForecasts.Models;
using pdouelleBlueprints.ControllerBase.Domain.WeatherForecasts.Models.Commands.CreateWeatherForecast;
using pdouelleBlueprints.ControllerBase.Domain.WeatherForecasts.Models.Commands.PatchWeatherForecast;
using pdouelleBlueprints.ControllerBase.Domain.WeatherForecasts.Models.Queries.GetWeatherForecastList;

namespace pdouelle.Blueprints.ControllerBase.Debug.Controllers
{
    [ApiController]
    [Route("api/blueprintTest")]
    [Produces("application/json")]
    public class BlueprintTestController : BlueprintController
    <
        WeatherForecast,
        WeatherForecastDto,
        GetWeatherForecastListQueryModel,
        CreateWeatherForecastCommandModel,
        PatchWeatherForecastCommandModel
    >
    {
        public BlueprintTestController
        (
            IMediator mediator,
            IMapper mapper,
            ILogger<BlueprintTestController> logger,
            IModelValidation model
        ) : base(mediator, mapper, logger, model)
        {
        }
    }
}