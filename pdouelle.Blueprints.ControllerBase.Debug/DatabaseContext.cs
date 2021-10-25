using Microsoft.EntityFrameworkCore;
using pdouelleBlueprints.ControllerBase.Domain.ChildEntities.Entities;
using pdouelleBlueprints.ControllerBase.Domain.WeatherForecasts.Entities;

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