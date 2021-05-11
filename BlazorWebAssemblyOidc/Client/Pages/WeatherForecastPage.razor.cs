using Microsoft.AspNetCore.Components;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.AspNetCore.Authorization;
using System.Collections.Generic;
using System.Net.Http.Json;
using BlazorWebAssemblyOidc.Shared.Models;

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
                        httpClient.BaseAddress = new Uri(Navigation.BaseUri);
                        httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {token.Value}");
                        forecasts = await httpClient.GetFromJsonAsync<WeatherForecast[]>("api/WeatherForecast");
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
