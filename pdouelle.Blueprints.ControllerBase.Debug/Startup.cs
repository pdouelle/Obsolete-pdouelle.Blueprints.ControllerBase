using System;
using System.IO;
using System.Linq;
using System.Reflection;
using Autofac;
using AutoFixture;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using pdouelle.Blueprints.ControllerBase.Debug.Domain.ChildEntities.Entities;
using pdouelle.Blueprints.ControllerBase.Debug.Domain.WeatherForecasts.Entities;
using pdouelle.Blueprints.MediatR;

namespace pdouelle.Blueprints.ControllerBase.Debug
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddApplicationInsightsTelemetry();
            
            services.AddControllers().AddNewtonsoftJson();

            services.AddMediatR(typeof(Startup).Assembly);
            services.AddBlueprintMediatR(typeof(Startup).Assembly);
            services.AddAutoMapper(typeof(Startup).Assembly);
            services.AddModelValidation();
            
            services.AddDbContext<DatabaseContext>(options => options.UseInMemoryDatabase("Database"));
            
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "pdouelle.Blueprints.ControllerBase.Debug", Version = "v1" });
                
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, DatabaseContext context)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "pdouelle.Blueprints.ControllerBase.Debug v1"));
            }

            app.UseHttpsRedirection();
            
            SeedDatabase(context);

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
        
        public void ConfigureContainer(ContainerBuilder builder)
        {
            builder.ConfigureContainer(typeof(DatabaseContext));
        }
        
        private static void SeedDatabase(DatabaseContext context)
        {
            context.Database.EnsureCreated();
            
            if (!context.WeatherForecasts.Any())
            {
                var fixture = new Fixture();

                var weatherForecast = new WeatherForecast
                {
                    Date = fixture.Create<DateTime>(),
                    Summary = fixture.Create<string>(),
                    TemperatureC = fixture.Create<int>(),
                    TemperatureF = fixture.Create<int>(),
                };
                
                context.WeatherForecasts.Add(weatherForecast);
                
                context.SaveChanges();

                var childEntity = new ChildEntity
                {
                    WeatherForecastId = weatherForecast.Id,
                    Name = fixture.Create<string>()
                };
                
                context.ChildEntities.Add(childEntity);
                
                context.SaveChanges();
            }
        }
    }
}