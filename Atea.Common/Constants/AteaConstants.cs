namespace Atea.Common.Constants
{
    public static class AteaConstants
    {
        //Task 1
        public static readonly string PUBLIC_API_URL = "https://api.publicapis.org/random?auth=null";

        //Task 2
        public static readonly string PUBLIC_WEATHER_API_URL = "http://api.openweathermap.org/data/2.5/weather?q={city}&units=metric&APPID={appId}";
        public static readonly int WEATHER_TREND_PAST_HOURS = -2;
    }
}