using Boater.Models;
using System;
using System.Collections.Generic;
using System.ServiceModel.Syndication;
using System.Xml;

namespace Boater.Services
{
    public class NoaaRssFeed
    {
        public NoaaRssFeed()
        {

        }

        public void GetLatestData(BoatingArea area)
        {
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
            }
            catch (Exception ex)
            {
                Console.WriteLine($"EXCEPTION: {area.Title} encountered an error parsing RSS data at {DateTimeOffset.Now}.{Environment.NewLine}" +
                    $"{ex}");
            }
        }
    }
}
