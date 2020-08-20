using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using BankWeb.Models;
using System.Net.Http;
using System.Text.Json.Serialization;
using System.Text.Json;

namespace BankWeb.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public async Task<IActionResult> Test()
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

            return View(weatherForecasts);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
