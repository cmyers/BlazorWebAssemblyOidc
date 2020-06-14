using BlazorWebAssemblyOidc.Shared.Models;
using Microsoft.AspNetCore.Components;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.AspNetCore.Authorization;
using System.Net.Http.Json;

namespace BlazorWebAssemblyOidc.Client.Pages
{
    [Authorize]
    public class WeatherForecastPageBase : ComponentBase
    {
        protected WeatherForecast[] forecasts;
        protected string error;

        [Inject]
        public NavigationManager Navigation { get; set; }

        [Inject]
        public IAccessTokenProvider AuthenticationService { get; set; }

        protected override async Task OnInitializedAsync()
        {
            var httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri(Navigation.BaseUri);

            var tokenResult = await AuthenticationService.RequestAccessToken();

            if (tokenResult.TryGetToken(out var token))
            {
                try
                {
                    httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {token.Value}");
                    forecasts = await httpClient.GetFromJsonAsync<WeatherForecast[]>("api/WeatherForecast");
                }
                catch (HttpRequestException exception)
                {
                    error = exception.Message;
                }
            }
            else
            {
                Navigation.NavigateTo(tokenResult.RedirectUrl);
            }
        }
    }
}
