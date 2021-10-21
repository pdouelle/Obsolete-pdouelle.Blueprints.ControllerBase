using System;
using pdouelle.Blueprints.ControllerBase.Debug.Domain.ChildEntities.Models;
using pdouelle.Entity;

namespace pdouelle.Blueprints.ControllerBase.Debug.Domain.WeatherForecasts.Models
{
    public class WeatherForecastDto : IEntity
    {
        public Guid Id { get; set; }
        public DateTime Date { get; set; }
        public int TemperatureC { get; set; }
        public int TemperatureF { get; set; }
        public string Summary { get; set; }

        public ChildEntityDto ChildEntity { get; set; }
    }
}