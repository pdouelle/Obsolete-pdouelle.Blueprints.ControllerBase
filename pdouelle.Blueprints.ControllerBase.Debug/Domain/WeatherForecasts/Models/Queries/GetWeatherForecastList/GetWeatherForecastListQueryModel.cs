using pdouelle.Blueprints.ControllerBase.Debug.Domain.WeatherForecasts.Entities;
using pdouelle.LinqExtensions.Attributes;
using pdouelle.QueryStringParameters;

namespace pdouelle.Blueprints.ControllerBase.Debug.Domain.WeatherForecasts.Models.Queries.GetWeatherForecastList
{
    public class GetWeatherForecastListQueryModel : QueryStringPaginationSort
    {
        [Include(Name = nameof(WeatherForecast.ChildEntity))]
        public bool IncludeChildEntities { get; set; }
    }
}