using Microsoft.AspNetCore.Components;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.AspNetCore.Authorization;
using BlazorWebAssemblyOidc.Shared.ApiClients;
using System.Collections.Generic;

namespace BlazorWebAssemblyOidc.Client.Pages
{
    [Authorize]
    public class WeatherForecastPageBase : ComponentBase
    {
        protected ICollection<WeatherForecast> forecasts;
        protected string error;

        [Inject]
        public NavigationManager Navigation { get; set; }

        [Inject]
        public IAccessTokenProvider AuthenticationService { get; set; }

        protected override async Task OnInitializedAsync()
        {
            var tokenResult = await AuthenticationService.RequestAccessToken();

            if (tokenResult.TryGetToken(out var token))
            {

                using (var httpClient = new HttpClient())
                {
                    try
                    {
                        httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {token.Value}");
                        var weatherForecastApiClient = new WeatherForecastApiClient(Navigation.BaseUri, httpClient);
                        forecasts = await weatherForecastApiClient.WeatherForecastAsync();
                    }
                    catch (HttpRequestException exception)
                    {
                        error = exception.Message;
                    }
                }
            }
            else
            {
                Navigation.NavigateTo(tokenResult.RedirectUrl);
            }
        }
    }
}
