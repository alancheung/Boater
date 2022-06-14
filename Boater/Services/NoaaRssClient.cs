using Boater.Models;
using System;
using System.Configuration;
using System.ServiceModel.Syndication;
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
        public bool GetLatestData(BoatingArea area)
        {
            // Read out to only hit dependent property once.
            DateTimeOffset lastUpdateTime = area.LastUpdateTime;
            if (lastUpdateTime > DateTimeOffset.Now - MaxMinsBeforeUpdateRequired)
            {
                // Note this only represents data available from NOAA. We might have attempted to retrieve data within the limit but the data from NOAA was stale.
                Console.WriteLine($"No update required. Last update for area {area.Title} was {lastUpdateTime}");
                return false;
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
                return false;
            }

            try
            {
                // Small amount of total data so probably faster to clear than re-init
                area.StationData.Clear();
                foreach (SyndicationItem item in feed.Items)
                {
                    StationSource source = new StationSource(item.Title.Text);
                    source.Update(item.Summary.Text);

                    if (source.DistanceFromOrigin > area.MaxRange)
                    {
                        break;
                    }
                    area.StationData.Add(source);
                }

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"EXCEPTION: {area.Title} encountered an error parsing RSS data at {DateTimeOffset.Now}.{Environment.NewLine}" +
                    $"{ex}");
                return false;
            }
        }
    }
}
