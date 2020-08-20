using BankModel;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace BankClientApp
{
    class WeatherLoader
    {
        public async Task<List<WeatherForecast>> LoadWeatherForecasts()
        {
            List<WeatherForecast> weatherForecasts = new List<WeatherForecast>();

            using (HttpClientHandler handler = new HttpClientHandler())
            {
                // This will allow unsigned SSL certificates
                handler.ServerCertificateCustomValidationCallback =
                    (httpRequestMessage, cert, cetChain, policyErrors) =>
                    {
                        return true;
                    };

                using (HttpClient httpClient = new HttpClient(handler))
                {
                    using (var response = await httpClient.GetAsync("https://localhost:5001/WeatherForecast"))
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();
                        weatherForecasts = JsonSerializer.Deserialize<List<WeatherForecast>>(apiResponse);
                    }
                }
            }

            return weatherForecasts;
        }

    }
}
