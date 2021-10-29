using System;
using Microsoft.AspNetCore.Mvc;
using pdouelle.LinqExtensions.Attributes;
using pdouelleBlueprints.ControllerBase.Domain.WeatherForecasts.Entities;

namespace pdouelleBlueprints.ControllerBase.Domain.WeatherForecasts.Models.Queries.GetWeatherForecastSingle
{
    public class GetWeatherForecastSingleQueryModel
    {
        [FromRoute]
        [Where]
        public Guid? Id { get; set; }
        
        [Where]
        public int? TemperatureC { get; set; }
        
        [Where]
        public int? TemperatureF { get; set; }

        [Where]
        public string Summary { get; set; }
    }
}