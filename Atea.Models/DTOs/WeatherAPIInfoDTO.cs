using System;

namespace Atea.Models.DTOs
{
    public class WeatherAPIInfoDTO
    {
        public int Id { get; set; }
        public int CityId { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string CityState { get; set; }
        public double Temperature { get; set; }
        public double WindSpeed { get; set; }
        public DateTime UtcTimestamp { get; set; }
    }
}