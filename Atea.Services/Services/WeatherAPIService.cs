using Atea.Common.Enums;
using Atea.Common.Extensions;
using Atea.Data;
using Atea.Data.Entities;
using Atea.Models.DTOs;
using Atea.Models.Responses;
using Atea.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Atea.Services.Services
{
    public class WeatherAPIService : IWeatherAPIService
    {
        private readonly HttpClient _httpClient;
        protected AteaDbContext _context;

        public WeatherAPIService(HttpClient httpClient, AteaDbContext context)
        {
            _httpClient = httpClient;
            _context = context;
        }

        public async Task<PublicAPIWeatherResponse> GetForecastByCity(string city)
        {
            var publicWeatherAPIUrl = Common.Constants.AteaConstants.PUBLIC_WEATHER_API_URL;
            publicWeatherAPIUrl = publicWeatherAPIUrl.Replace("{city}", city).Replace("{appId}", Common.Constants.AteaConstants.OPEN_WEATHER_APPID);

            HttpRequestMessage newRequest = new HttpRequestMessage(HttpMethod.Get, publicWeatherAPIUrl);

            //Read Server Response
            HttpResponseMessage response = await _httpClient.SendAsync(newRequest);

            if (response.IsSuccessStatusCode)
            {
                var responseBody = await response.Content.ReadAsStringAsync();
                var responseData = JsonConvert.DeserializeObject<PublicAPIWeatherResponse>(responseBody);
                return responseData;
            }

            return null;
        }

        public async Task<IEnumerable<WeatherAPIInfoDTO>> GetWeatherTrendByCity(CityEnum city, int pastHours)
        {
            var cityTempDetails = await _context.WeatherAPIInfos.Where(x => x.CityId == city && x.UtcTimestamp >= DateTime.Now.AddHours(pastHours)).ToListAsync();
            if (cityTempDetails == null)
                return null;

            var result = cityTempDetails.Select(x => WeatherApiInfoDTOMap(x)).AsEnumerable();

            return result;
        }

        public async Task<List<WeatherAPIInfoDTO>> CreateWeatherApiInfos(List<PublicAPIWeatherResponse> responses)
        {
            if (responses == null || !responses.Any())
                return null;

            var resultList = (from resp in responses select WeatherApiInfoMap(resp)).ToList();

            _context.WeatherAPIInfos.AddRange(resultList);
            await _context.SaveChangesAsync();

            var result = (from res in resultList select WeatherApiInfoDTOMap(res)).ToList();

            return result;
        }

        public WeatherAPIInfo WeatherApiInfoMap(PublicAPIWeatherResponse response)
        {
            if (response == null)
                return null;

            var result = new WeatherAPIInfo
            {
                CityId = response.Name.Parse<CityEnum>(),
                State = response?.Sys.Country ?? string.Empty,
                Temperature = response?.Main.Temp ?? 0,
                WindSpeed = response?.Wind.Speed ?? 0,
                UtcTimestamp = DateTime.Now
            };

            return result;
        }

        public WeatherAPIInfoDTO WeatherApiInfoDTOMap(WeatherAPIInfo weatherApiInfo)
        {
            return new WeatherAPIInfoDTO
            {
                Id = weatherApiInfo.Id,
                CityId = ((int)weatherApiInfo.CityId),
                City = weatherApiInfo.CityId.ToString(),
                State = weatherApiInfo.State,
                CityState = $"{weatherApiInfo.CityId} / {weatherApiInfo.State}",
                Temperature = weatherApiInfo.Temperature,
                WindSpeed = weatherApiInfo.WindSpeed,
                UtcTimestamp = weatherApiInfo.UtcTimestamp
            };
        }
    }
}