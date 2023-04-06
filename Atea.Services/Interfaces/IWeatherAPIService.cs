using Atea.Common.Enums;
using Atea.Models.DTOs;
using Atea.Models.Responses;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Atea.Services.Interfaces
{
    public interface IWeatherAPIService
    {
        Task<PublicAPIWeatherResponse> GetForecastByCity(string city);
        Task<IEnumerable<WeatherAPIInfoDTO>> GetWeatherTrendByCity(CityEnum city, int pastHours);
        Task<List<WeatherAPIInfoDTO>> CreateWeatherApiInfos(List<PublicAPIWeatherResponse> responses);
    }
}