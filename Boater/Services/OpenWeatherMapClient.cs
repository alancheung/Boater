using Boater.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Threading.Tasks;
using WeatherNet.Clients;
using WeatherNet.Model;

namespace Boater.Services
{
    public class OpenWeatherMapClient
    {
        /// <summary>
        /// Path the assets folder for OpenWeatherMap.
        /// </summary>
        public static string OpenWeatherMapContentFolderPath;

        /// <summary>
        /// The maximum amount of time before data should be updated.
        /// </summary>
        private readonly TimeSpan MaxMinsBeforeUpdateRequired;

        /// <summary>
        /// Should we get the real weather from the source?
        /// </summary>
        public bool UseRealWeather { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="apiKey">API key for OpenWeather</param>
        /// <param name="useRealWeather">Use the real weather.</param>
        public OpenWeatherMapClient(string apiKey, string contentPath, bool useRealWeather = false)
        {
            WeatherNet.ClientSettings.SetApiKey(apiKey);
            OpenWeatherMapContentFolderPath = contentPath;
            MaxMinsBeforeUpdateRequired = TimeSpan.FromMinutes(int.Parse(ConfigurationManager.AppSettings[nameof(MaxMinsBeforeUpdateRequired)]));
            UseRealWeather = useRealWeather;
        }

        public static string GetImage(WeatherResult result)
        {
            string icon = Path.Combine(OpenWeatherMapContentFolderPath, result.Icon);
            icon = Path.ChangeExtension(icon, "png");
            return icon;
        }

        /// <summary>
        /// Get the current
        /// </summary>
        /// <returns>Tuple representing if the request was successful and the data in the request.</returns>
        public async Task<bool> UpdateWeather(BoatingArea area)
        {
            if (area.LastWeatherUpdateTime > DateTimeOffset.Now - MaxMinsBeforeUpdateRequired)
            {
                // Note this only represents data available from NOAA. We might have attempted to retrieve data within the limit but the data from NOAA was stale.
                Console.WriteLine($"No weather update required. Last update for area {area.Title} was {area.LastWeatherUpdateTime}");
                return false;
            }

            // Get the data from the source
            Task<Tuple<bool, CurrentWeatherResult>> task = Task.Run(() => GetWeather(area.Latitude, area.Longitude));
            await task;

            // Parse it into the area object.
            Tuple<bool, CurrentWeatherResult> result = task.Result;
            bool success = result.Item1;

            // Only update if successful so that the cached data is always viewable at least.
            if (success)
            {
                area.WeatherResult = result.Item2;
                area.LastWeatherUpdateTime = DateTimeOffset.Now;
            }

            return success;
        }

        private Tuple<bool, CurrentWeatherResult> GetWeather(double latitude, double longitude)
        {
            try
            {
                SingleResult<CurrentWeatherResult> result;
                if (UseRealWeather)
                {
                    result = CurrentWeather.GetByCoordinates(lat: latitude, lon: longitude, "en", "imperial");
                }
                else
                {
                    string debugWeatherFilePath = Path.Combine(OpenWeatherMapContentFolderPath, "fakeWeather.json");
                    string fakeWeatherJson = File.ReadAllText(debugWeatherFilePath);
                    result = JsonConvert.DeserializeObject<SingleResult<CurrentWeatherResult>>(fakeWeatherJson);
                    Console.WriteLine("Faked weather results!");
                }

                if (result.Success)
                {
                    result.Item.Date = TimeZone.CurrentTimeZone.ToLocalTime(result.Item.Date);
                }
                else
                {
                    Console.WriteLine($"Weather update failed with message: {result.Message}");
                }

                return Tuple.Create(result.Success, result.Item);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception handled during weather update: {ex.Message}");
                return Tuple.Create(false, (CurrentWeatherResult)null);
            }
        }

        /// <summary>
        /// Update the forecasted weather held in memory and broadcast the updates.
        /// </summary>
        /// <returns>Was the update successful</returns>
        public async Task<bool> UpdateForecast(BoatingArea area)
        {
            if (area.LastForecastResult > DateTimeOffset.Now - MaxMinsBeforeUpdateRequired)
            {
                // Note this only represents data available from NOAA. We might have attempted to retrieve data within the limit but the data from NOAA was stale.
                Console.WriteLine($"No forecast update required. Last update for area {area.Title} was {area.LastWeatherUpdateTime}");
                return false;
            }

            // Get the data from the source
            Task<Tuple<bool, List<FiveDaysForecastResult>>> task = Task.Run(() => GetForecast(area.Latitude, area.Longitude));
            await task;

            // Parse it into the area object.
            Tuple<bool, List<FiveDaysForecastResult>> result = task.Result;
            bool success = result.Item1;

            // Only update if successful so that the cached data is always viewable at least.
            if (success)
            {
                area.ForecastResult = result.Item2;
                area.LastForecastResult = DateTimeOffset.Now;
            }

            return success;
        }

        private Tuple<bool, List<FiveDaysForecastResult>> GetForecast(double latitude, double longitude)
        {
            try
            {
                Result<FiveDaysForecastResult> result;
                if (UseRealWeather)
                {
                    result = FiveDaysForecast.GetByCoordinates(lat: latitude, lon: longitude, "en", "imperial");
                }
                else
                {
                    string debugWeatherFilePath = Path.Combine(OpenWeatherMapContentFolderPath, "fakeWeather.json");
                    string fakeWeatherJson = File.ReadAllText(debugWeatherFilePath);
                    result = JsonConvert.DeserializeObject<Result<FiveDaysForecastResult>>(fakeWeatherJson);
                    Console.WriteLine("Faked weather results!");
                }

                if (result.Success)
                {
                    result.Items.ForEach(r => r.Date = TimeZone.CurrentTimeZone.ToLocalTime(r.Date));
                    return Tuple.Create(true, result.Items);
                }
                else
                {
                    Console.WriteLine($"Weather update failed with message: {result.Message}");
                    return Tuple.Create(false, result.Items);
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception handled during forecast update: {ex.Message}");
                return Tuple.Create(false, (List<FiveDaysForecastResult>)null);
            }
        }
    }
}
