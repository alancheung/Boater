using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Boater.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class BoatingArea
    {
        /// <summary>
        /// Official title to display on the reading panel header.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// The URL to obtain the RSS feed of station readings.
        /// </summary>
        public string RssUrl { get; set; }

        /// <summary>
        /// The URL to obtain text updates to display in the left panel.
        /// </summary>
        public string TextUrl { get; set; }

        /// <summary>
        /// The maximum distance from the station to still allow results.
        /// </summary>
        public int MaxRange { get; set; }

        /// <summary>
        /// The data from each station
        /// </summary>
        /// <remarks>Property not constructed from JSON so set default now.</remarks>
        [JsonIgnore]
        public List<StationSource> StationData { get; private set; } = new List<StationSource>();

        /// <summary>
        /// The latest time of a reading in the <see cref="StationData"/> collection
        /// </summary>
        [JsonIgnore]
        public DateTimeOffset LastUpdateTime => StationData.Any() ? StationData.Max(d => d.UpdateTime) : DateTimeOffset.MinValue;
    }
}
