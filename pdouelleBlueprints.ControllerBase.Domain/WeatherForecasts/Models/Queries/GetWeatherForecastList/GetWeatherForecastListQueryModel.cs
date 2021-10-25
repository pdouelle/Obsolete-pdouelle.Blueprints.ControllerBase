using pdouelle.LinqExtensions.Attributes;
using pdouelle.QueryStringParameters;
using pdouelleBlueprints.ControllerBase.Domain.WeatherForecasts.Entities;

namespace pdouelleBlueprints.ControllerBase.Domain.WeatherForecasts.Models.Queries.GetWeatherForecastList
{
    public class GetWeatherForecastListQueryModel : QueryStringPaginationSort
    {
        [Where]
        public int? TemperatureC { get; set; }
        
        [Include(Name = nameof(WeatherForecast.ChildEntity))]
        public bool IncludeChildEntities { get; set; }
    }
}