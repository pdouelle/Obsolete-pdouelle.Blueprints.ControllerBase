using AutoMapper;
using pdouelle.Blueprints.ControllerBase.Debug.Domain.WeatherForecasts.Entities;
using pdouelle.Blueprints.ControllerBase.Debug.Domain.WeatherForecasts.Models;
using pdouelle.Blueprints.ControllerBase.Debug.Domain.WeatherForecasts.Models.Commands.CreateWeatherForecast;
using pdouelle.Blueprints.ControllerBase.Debug.Domain.WeatherForecasts.Models.Commands.PatchWeatherForecast;

namespace pdouelle.Blueprints.ControllerBase.Debug.Domain.WeatherForecasts.Profiles
{
    public class WeatherForecastProfile : Profile
    {
        public WeatherForecastProfile()
        {
            CreateMap<WeatherForecast, WeatherForecastDto>();
            CreateMap<CreateWeatherForecastCommandModel, WeatherForecast>();
            CreateMap<WeatherForecast, PatchWeatherForecastCommandModel>().ReverseMap();
        }
    }
}