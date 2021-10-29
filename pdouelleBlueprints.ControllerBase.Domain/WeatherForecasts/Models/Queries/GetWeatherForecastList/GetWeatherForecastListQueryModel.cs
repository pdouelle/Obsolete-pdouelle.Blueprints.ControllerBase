using System.Collections.Generic;
using pdouelle.LinqExtensions.Attributes;
using pdouelle.LinqExtensions.Interfaces;
using pdouelle.QueryStringParameters;

namespace pdouelleBlueprints.ControllerBase.Domain.WeatherForecasts.Models.Queries.GetWeatherForecastList
{
    public class GetWeatherForecastListQueryModel : QueryStringPaginationSort, IInclude
    {
        [Where]
        public int? TemperatureC { get; set; }

        public List<string> Include { get; set; }
    }
}