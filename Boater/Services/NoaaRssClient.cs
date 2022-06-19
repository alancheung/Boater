using Boater.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.ServiceModel.Syndication;
using System.Threading.Tasks;
using System.Xml;

namespace Boater.Services
{
    public class NoaaRssClient
    {
        private readonly TimeSpan MaxMinsBeforeUpdateRequired;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="maxMinsBeforeUpdateRequired">The maximum amount of time before an update is required</param>
        public NoaaRssClient(TimeSpan maxMinsBeforeUpdateRequired)
        {
            MaxMinsBeforeUpdateRequired = maxMinsBeforeUpdateRequired;
        }

        /// <summary>
        /// Get the latest NOAA data for <paramref name="area"/> within the <paramref name="area"/>'s <see cref="BoatingArea.MaxRange"/>.
        /// </summary>
        /// <param name="area">The <see cref="BoatingArea"/> to update</param>
        /// <returns>True if <see cref="BoatingArea.StationData"/> collection updated, false otherwise</returns>
        public async Task<bool> UpdateLatestStationData(BoatingArea area)
        {
            // Get the data from the source
            Task<Tuple<bool, List<StationSource>>> task = Task.Run(() => GetLatestStationData(area));
            await task;

            // Parse it into the area object.
            Tuple<bool, List<StationSource>> result = task.Result;
            bool success = result.Item1;

            // Only update if successful so that the cached data is always viewable at least.
            if (success && result.Item2 != null && result.Item2.Any())
            {
                area.StationData.Clear();
                area.StationData.AddRange(result.Item2);
                area.LastStationUpdateTime = DateTimeOffset.Now;
            }

            return success;
        }

        private Tuple<bool, List<StationSource>> GetLatestStationData(BoatingArea area)
        {
            if (area.LastStationUpdateTime > DateTimeOffset.Now - MaxMinsBeforeUpdateRequired)
            {
                Console.WriteLine($"No station update required. Last update for area {area.Title} was {area.LastStationUpdateTime}");
                return Tuple.Create(true, (List<StationSource>)null);
            }

            SyndicationFeed feed = null;
            try
            {
                using (XmlReader reader = XmlReader.Create(area.RssUrl))
                {
                    feed = SyndicationFeed.Load(reader);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"EXCEPTION: {area.Title} encountered an error reading XML data from NOAA at {DateTimeOffset.Now}.{Environment.NewLine}" +
                    $"{ex}");
                return Tuple.Create(false, (List<StationSource>)null);
            }

            try
            {
                List<StationSource> stations = new List<StationSource>();
                foreach (SyndicationItem item in feed.Items)
                {
                    StationSource source = new StationSource(item.Title.Text);
                    source.Update(item.Summary.Text);

                    if (source.DistanceFromOrigin > area.MaxRange)
                    {
                        break;
                    }
                    stations.Add(source);
                }

                return Tuple.Create(true, stations);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"EXCEPTION: {area.Title} encountered an error parsing RSS data at {DateTimeOffset.Now}.{Environment.NewLine}" +
                    $"{ex}");
                return Tuple.Create(false, (List<StationSource>)null);
            }
        }

        /// <summary>
        /// Get the latest NOAA text update for <paramref name="area"/> using the <see cref="BoatingArea.TextUrl"/>.
        /// </summary>
        /// <param name="area">The <see cref="BoatingArea"/> to update</param>
        /// <returns>True if <see cref="BoatingArea.TextUrl"/> collection updated, false otherwise</returns>
        public async Task<bool> UpdateLatestTextData(BoatingArea area)
        {
            // Get the data from the source
            Task<Tuple<bool, string>> task = GetLatestTextData(area);
            await task;

            // Parse it into the area object.
            Tuple<bool, string> result = task.Result;
            bool success = result.Item1;

            // Only update if successful so that the cached data is always viewable at least.
            if (success && !string.IsNullOrWhiteSpace(result.Item2))
            {
                area.TextUpdate = result.Item2;
                area.LastTextUpdateTime = DateTimeOffset.Now;
            }

            return success;
        }

        private async Task<Tuple<bool, string>> GetLatestTextData(BoatingArea area)
        {
            if (area.LastTextUpdateTime > DateTimeOffset.Now - MaxMinsBeforeUpdateRequired)
            {
                Console.WriteLine($"No text update required. Last update for area {area.Title} was {area.LastTextUpdateTime}");
                return Tuple.Create(true, (string)null);
            }

            try
            {
                using (HttpClient client = new HttpClient())
                {
                    HttpResponseMessage response = await client.GetAsync(area.TextUrl);
                    if (response.IsSuccessStatusCode)
                    {
                        string text = await response.Content.ReadAsStringAsync();
                        if (!string.IsNullOrWhiteSpace(text))
                        {
                            text = text.Replace("\n", Environment.NewLine);
                            return Tuple.Create(true, text);
                        }
                    }

                    return Tuple.Create(false, (string)null);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"EXCEPTION: {area.Title} encountered an error reading text data from NOAA at {DateTimeOffset.Now}.{Environment.NewLine}" +
                    $"{ex}");
                return Tuple.Create(false, (string)null);
            }
        }
    }
}
