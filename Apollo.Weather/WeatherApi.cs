using Apollo.Weather.Models;
using ApolloBot.Core;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Apollo.Weather
{
    public class WeatherApi : IApi
    {
        private const string URL_CITY_SEARCH = "http://api.openweathermap.org/data/2.5/weather?q={0}&units=metric&APPID={1}";
        private const string URL_WEEK_CITY_SEARCH = "http://api.openweathermap.org/data/2.5/forecast?q={0}&units=metric&APPID={1}";

        private string _key;

        private HttpClient _client;

        public WeatherApi()
        {
            _client = new HttpClient();
        }

        public async Task<string> GetCurrentDay(string location)
        {
            if(string.IsNullOrEmpty(location))
            {
                location = "Lyon";
            }

            var request = new HttpRequestMessage(HttpMethod.Get, string.Format(URL_CITY_SEARCH, location, _key));

            var response = await _client.SendAsync(request);
            var responseStr = await response.Content.ReadAsStringAsync();

            return ConvertWeatherDayToString(JsonConvert.DeserializeObject<ResultDayData> (responseStr));
        }

        public async Task<string> GetCurrentWeek(string location)
        {
            if (string.IsNullOrEmpty(location))
            {
                location = "Lyon";
            }

            var request = new HttpRequestMessage(HttpMethod.Get, string.Format(URL_WEEK_CITY_SEARCH, location, _key));

            var response = await _client.SendAsync(request);
            var responseStr = await response.Content.ReadAsStringAsync();

            return ConvertWeatherWeekToString(JsonConvert.DeserializeObject<ResultWeekData>(responseStr));
        }

        private string ConvertWeatherDayToString(ResultDayData dayData)
        {
            var html = $"";

            var date = UnixTimeStampToDateTime(dayData.DateRaw);

            if (date.Hour >= 8 && date.Hour <= 21)
            {
                html += $"\t{ConvertSkyToEmoji(dayData.Weather[0].Main)}";
                html += $"\t{dayData.Main.Temp.ToString("00.00") + "°C"}";
                html += $"\t{dayData.Main.Humidity.ToString("000") + "%"}";
                html += $"\t{dayData.Wind.Speed.ToString("00.00") + "m/s"}";
                html += $"\n";
            }

            html += $"";

            return html;
        }

        private string ConvertWeatherWeekToString(ResultWeekData weekData)
        {
            var html = $"";
            DateTime? currentDay = null;

            foreach (var day in weekData.List)
            {
                var date = UnixTimeStampToDateTime(day.DateRaw);

                if(currentDay != date.Date)
                {
                    html += $"*{date.ToString("dddd")}*:\n";
                    currentDay = date.Date;
                }

                if (date.Hour >= 8 && date.Hour <= 21)
                {
                    html += $"\t{date.ToString("HH:mm")}:";
                    html += $"\t{ConvertSkyToEmoji(day.Weather[0].Main)}";
                    html += $"\t{day.Main.Temp.ToString("00.00") + "°C"}";
                    html += $"\t{day.Main.Humidity.ToString("000") + "%"}";
                    html += $"\t{day.Wind.Speed.ToString("00.00") + "m/s"}";
                    html += $"\n";
                }
            }

            html += $"";

            return html;
        }

        private string ConvertSkyToEmoji(string sky)
        {
            switch(sky)
            {
                case "Clear":
                    return ":sunny:";
                case "Rain":
                    return ":rain_cloud:";
                case "Clouds":
                    return ":sun_behind_cloud:";
                case "Snow":
                    return ":snow_cloud:";
                case "Thunderstorm":
                    return ":thunder_cloud_and_rain:";
            }

            return sky;
        }
        
        public static DateTime UnixTimeStampToDateTime(double unixTimeStamp)
        {
            // Unix timestamp is seconds past epoch
            System.DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
            dtDateTime = dtDateTime.AddSeconds(unixTimeStamp).ToLocalTime();
            return dtDateTime;
        }

        public void Init(IConfigurationRoot configuration)
        {
            _key = configuration.GetValue<string>("Weather:Key");
        }

        public IEnumerable<BotAction> GetActions()
        {
            var listActions = new List<BotAction>();

            var weekAction = new BotAction()
            {
                CommandLine = Constants.CMD_WEATHER_WEEK,
                Description = Constants.DESC_WEATHER_WEEK,
                Category = Constants.CAT_WEATHER,
                Execute = async (parameters, log, currentReader, user, time) =>
                {
                    var week = await GetCurrentWeek(parameters.Length >= 1 ? parameters[0] : null);
                    if (week == null)
                    {
                        return $"Une erreur est survenue";
                    }

                    return week;
                }
            };
            listActions.Add(weekAction);

            var dayAction = new BotAction()
            {
                CommandLine = Constants.CMD_WEATHER_DAY,
                Description = Constants.DESC_WEATHER_DAY,
                Category = Constants.CAT_WEATHER,
                Execute = async (parameters, log, currentReader, user, time) =>
                {
                    var day = await GetCurrentDay(parameters.Length >= 1 ? parameters[0] : null);
                    if (day == null)
                    {
                        return $"Une erreur est survenue";
                    }

                    return day;
                }
            };
            listActions.Add(dayAction);

            return listActions;
        }

        public IEnumerable<BotReader> GetReaders()
        {
            return null;
        }
    }
}
