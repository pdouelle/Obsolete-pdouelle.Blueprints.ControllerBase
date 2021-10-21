using Microsoft.EntityFrameworkCore;
using pdouelle.Blueprints.ControllerBase.Debug.Domain.ChildEntities.Entities;
using pdouelle.Blueprints.ControllerBase.Debug.Domain.WeatherForecasts.Entities;

namespace pdouelle.Blueprints.ControllerBase.Debug
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
        {
        }

        public DbSet<WeatherForecast> WeatherForecasts { get; set; }
        public DbSet<ChildEntity> ChildEntities { get; set; }
    }
}