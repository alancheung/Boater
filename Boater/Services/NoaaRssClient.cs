using Boater.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.ServiceModel.Syndication;
using System.Threading.Tasks;
using System.Xml;

namespace Boater.Services
{
    public class NoaaRssClient
    {
        private readonly TimeSpan MaxMinsBeforeUpdateRequired;

        public NoaaRssClient()
        {
            MaxMinsBeforeUpdateRequired = TimeSpan.FromMinutes(int.Parse(ConfigurationManager.AppSettings[nameof(MaxMinsBeforeUpdateRequired)]));
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
            if (success)
            {
                area.StationData.Clear();
                area.StationData.AddRange(result.Item2);
            }

            return success;

        }

        private Tuple<bool, List<StationSource>> GetLatestStationData(BoatingArea area)
        {
            // Read out to only hit dependent property once.
            DateTimeOffset lastUpdateTime = area.LastUpdateTime;
            if (lastUpdateTime > DateTimeOffset.Now - MaxMinsBeforeUpdateRequired)
            {
                // Note this only represents data available from NOAA. We might have attempted to retrieve data within the limit but the data from NOAA was stale.
                Console.WriteLine($"No update required. Last update for area {area.Title} was {lastUpdateTime}");
                return Tuple.Create(false, (List<StationSource>)null);
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
    }
}
