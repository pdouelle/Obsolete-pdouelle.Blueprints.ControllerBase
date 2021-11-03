using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using pdouelle.LinqExtensions.Attributes;
using pdouelle.LinqExtensions.Interfaces;

namespace pdouelleBlueprints.ControllerBase.Domain.WeatherForecasts.Models.Queries.GetWeatherForecastSingle
{
    public class GetWeatherForecastSingleQueryModel : IInclude
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

        public List<string> Include { get; set; }
    }
}