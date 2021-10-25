using System;
using pdouelle.Entity;
using pdouelleBlueprints.ControllerBase.Domain.ChildEntities.Models;

namespace pdouelleBlueprints.ControllerBase.Domain.WeatherForecasts.Models
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