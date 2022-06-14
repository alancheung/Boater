using Boater.Common.Attributes;
using Boater.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.ServiceModel.Syndication;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace Boater.Services
{
    public class NoaaRssFeed
    {
        private static readonly string NOAA_URL = "https://www.ndbc.noaa.gov/data/latest_obs/{0}.rss";

        private Timer Timer;

        public Dictionary<string, StationSource> Stations { get; set; }

        /// <summary>
        /// Event fired when the forecasted weather has been updated.
        /// </summary>
        public EventHandler OnStationUpdate;

#if DEBUG
        internal Dictionary<string, string> DEBUG_DATA { get; set; } = new Dictionary<string, string>()
        {
            { "bltm2", @"
        <strong>June 13, 2022 7:48 pm EST</strong><br />
        <strong>Location:</strong> 39.267N 76.579W or 1 nautical miles SSW of search location of 39.28N 76.57W.<br />
        <strong>Wind Direction:</strong> WSW (250&#176;)<br />
        <strong>Wind Speed:</strong> 2 knots<br />
        <strong>Wind Gust:</strong> 3 knots<br />
        <strong>Atmospheric Pressure:</strong> 29.88 in (1012.0 mb)<br />
        <strong>Air Temperature:</strong> 84&#176;F (28.9&#176;C)<br />
        <strong>Water Temperature:</strong> 76&#176;F (24.3&#176;C)<br />
      " },
            { "fsnm2", @"
        <strong>May 30, 2022 2:30 pm EDT</strong><br />
        <strong>Location:</strong> 39.219N 76.525W<br />
        <strong>Wind Direction:</strong> E (100&#176;)<br />
        <strong>Wind Speed:</strong> 8.0 knots<br />
        <strong>Wind Gust:</strong> 9.9 knots<br />
        <strong>Atmospheric Pressure:</strong> 30.08 in (1018.7 mb)<br />
        <strong>Air Temperature:</strong> 82.2&#176;F (27.9&#176;C)<br />
      " },
            { "fskm2", @"
        <strong>May 30, 2022 2:30 pm EDT</strong><br />
        <strong>Location:</strong> 39.219N 76.528W<br />
        <strong>Wind Direction:</strong> SE (130&#176;)<br />
        <strong>Wind Speed:</strong> 8.0 knots<br />
        <strong>Wind Gust:</strong> 8.9 knots<br />
        <strong>Atmospheric Pressure:</strong> 30.08 in (1018.6 mb)<br />
        <strong>Air Temperature:</strong> 79.7&#176;F (26.5&#176;C)<br />
      " },
            { "44063", @"
        <strong>May 30, 2022 2:36 pm EDT</strong><br />
        <strong>Location:</strong> 38.963N 76.448W<br />
        <strong>Wind Direction:</strong> S (170&#176;)<br />
        <strong>Wind Speed:</strong> 7.8 knots<br />
        <strong>Wind Gust:</strong> 9.7 knots<br />
        <strong>Significant Wave Height:</strong> 1.0 ft<br />
        <strong>Dominant Wave Period:</strong> 2 sec<br />
        <strong>Atmospheric Pressure:</strong> 30.11 in (1019.5 mb)<br />
        <strong>Air Temperature:</strong> 74.5&#176;F (23.6&#176;C)<br />
        <strong>Water Temperature:</strong> 73.8&#176;F (23.2&#176;C)<br />
      " },
        };
#endif

        public NoaaRssFeed(List<Tuple<string, string>> registeredStations, int timerInterval = (30 * 60 * 1000))
        {
            Stations = new Dictionary<string, StationSource>();
            foreach (Tuple<string, string> stationNameDescription in registeredStations)
            {
                Stations.Add(stationNameDescription.Item1, new StationSource(stationNameDescription.Item2));
            }

            Type thisType = typeof(StationSource);
            foreach (PropertyInfo prop in thisType.GetProperties().Where(p => p.IsDefined(typeof(RegexSearchAttribute))))
            {
                RegexSearchAttribute attribute = (RegexSearchAttribute)prop.GetCustomAttribute(typeof(RegexSearchAttribute), inherit: false);
                StationSource.SettableProperties.Add(new Tuple<PropertyInfo, Regex>(prop, new Regex(attribute.Regex)));
            }

            Timer = new Timer();
            Timer.Interval = timerInterval;
            Timer.Tick += Timer_Tick;
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
#if DEBUG
            foreach (KeyValuePair<string, StationSource> station in Stations)
            {
                station.Value.Update(DEBUG_DATA[station.Key]);
            }
#else
            GetLatestData();
#endif
        }

        private void GetLatestData()
        {
            try
            {
                Dictionary<string, Task<SyndicationFeed>> requests = new Dictionary<string, Task<SyndicationFeed>>(Stations.Count);
                foreach (string stationKey in Stations.Keys)
                {
                    requests[stationKey] = Task.Run(() =>
                    {
                        SyndicationFeed feed = null;
                        try
                        {
                            using (var reader = XmlReader.Create(string.Format(NOAA_URL, stationKey)))
                            {
                                feed = SyndicationFeed.Load(reader);
                            }

                            return feed;
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Encountered exception retrieving station {stationKey} details. {ex}");
                            return null;
                        }
                    });
                }

                Task.WaitAll(requests.Values.ToArray());

                foreach (KeyValuePair<string, Task<SyndicationFeed>> rssData in requests)
                {
                    if (rssData.Value.Result is null)
                    {
                        Console.WriteLine($"Station {rssData.Key} returned no data!");
                        continue;
                    }

                    SyndicationFeed feed = rssData.Value.Result;
                    string rawText = feed.Items.First().Summary.Text;
                    Stations[rssData.Key].Update(rawText);
                }

                OnStationUpdate?.Invoke(this, EventArgs.Empty);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Could not trigger NOAA update at {DateTimeOffset.Now}. {ex}");
            }
        }

        public void Start(bool immediate = true)
        {
            Timer.Start();
            if (immediate)
            {
                Timer_Tick(this, EventArgs.Empty);
            }
        }

        public void Stop()
        {
            Timer.Stop();
        }
    }
}
