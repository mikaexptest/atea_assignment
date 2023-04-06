using Atea.Common.Enums;
using System;

namespace Atea.Data.Entities
{
    public class WeatherAPIInfo
    {
        public int Id { get; set; }
        public CityEnum CityId { get; set; }
        public string State { get; set; }
        public double Temperature { get; set; }
        public double WindSpeed { get; set; }
        public DateTime UtcTimestamp { get; set; }
    }
}