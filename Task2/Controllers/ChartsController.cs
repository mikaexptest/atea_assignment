using Atea.Common.Enums;
using Atea.Common.Extensions;
using Atea.Models.DTOs;
using Atea.Models.Responses;
using Atea.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Task2.Controllers
{
    public class ChartsController : Controller
    {
        private readonly IWeatherAPIService _weatherAPIService;

        public ChartsController(IWeatherAPIService weatherAPIService)
        {
            _weatherAPIService = weatherAPIService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> GetNewWeatherData()
        {
            var apiResponseList = new List<PublicAPIWeatherResponse>();

            foreach (var cityEnum in Enum.GetNames(typeof(CityEnum)))
            {
                apiResponseList.Add(await _weatherAPIService.GetForecastByCity(cityEnum));
            }

            var result = await _weatherAPIService.CreateWeatherApiInfos(apiResponseList);

            return new JsonResult(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetWeatherTrendByCity(WeatherAPIInfoDTO request)
        {
            if (request == null)
                return null;

            var result = await _weatherAPIService.GetWeatherTrendByCity(request.City.Parse<CityEnum>(), Atea.Common.Constants.AteaConstants.WEATHER_TREND_PAST_HOURS);

            return new JsonResult(result);
        }
    }
}