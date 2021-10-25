using AutoMapper;
using pdouelleBlueprints.ControllerBase.Domain.WeatherForecasts.Entities;
using pdouelleBlueprints.ControllerBase.Domain.WeatherForecasts.Models;
using pdouelleBlueprints.ControllerBase.Domain.WeatherForecasts.Models.Commands.CreateWeatherForecast;
using pdouelleBlueprints.ControllerBase.Domain.WeatherForecasts.Models.Commands.PatchWeatherForecast;
using pdouelleBlueprints.ControllerBase.Domain.WeatherForecasts.Models.Commands.UpdateWeatherForecast;

namespace pdouelleBlueprints.ControllerBase.Domain.WeatherForecasts.Profiles
{
    public class WeatherForecastProfile : Profile
    {
        public WeatherForecastProfile()
        {
            CreateMap<WeatherForecast, WeatherForecastDto>();
            CreateMap<CreateWeatherForecastCommandModel, WeatherForecast>();
            CreateMap<UpdateWeatherForecastCommandModel, WeatherForecast>();
            CreateMap<WeatherForecast, PatchWeatherForecastCommandModel>().ReverseMap();
        }
    }
}