using BlazorWebAssemblyOidc.Shared.Interfaces;
using BlazorWebAssemblyOidc.Shared.Models;
using System.Collections.Generic;

namespace BlazorWebAssemblyOidc.Shared.Services
{
    public class WeatherForecastService : IWeatherForecastService
    {
        private readonly IWeatherForecastRepository _weatherForecastRepository;

        public WeatherForecastService(IWeatherForecastRepository weatherForecastRepository)
        {
            _weatherForecastRepository = weatherForecastRepository;
        }

        public IEnumerable<WeatherForecast> Get()
        {
            return _weatherForecastRepository.Get();
        }
    }
}