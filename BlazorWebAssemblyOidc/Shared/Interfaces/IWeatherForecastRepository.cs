using BlazorWebAssemblyOidc.Shared.ApiClients;
using System.Collections.Generic;

namespace BlazorWebAssemblyOidc.Shared.Interfaces
{
    public interface IWeatherForecastRepository
    {
        IEnumerable<WeatherForecast> Get();
    }
}