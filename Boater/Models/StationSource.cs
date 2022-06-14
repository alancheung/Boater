using Boater.Common;
using Boater.Common.Attributes;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;

namespace Boater.Models
{
    /// <summary>
    /// Details about a specific NOAA station. All properties are expected to be nullable.
    /// </summary>
    public class StationSource
    {
        private static List<Tuple<PropertyInfo, Regex>> _settableProperties = new List<Tuple<PropertyInfo, Regex>>();
        public static List<Tuple<PropertyInfo, Regex>> SettableProperties
        {
            get
            {
                if (!_settableProperties.Any())
                {
                    Type thisType = typeof(StationSource);
                    foreach (PropertyInfo prop in thisType.GetProperties().Where(p => p.IsDefined(typeof(RegexSearchAttribute))))
                    {
                        RegexSearchAttribute attribute = (RegexSearchAttribute)prop.GetCustomAttribute(typeof(RegexSearchAttribute), inherit: false);
                        StationSource._settableProperties.Add(new Tuple<PropertyInfo, Regex>(prop, new Regex(attribute.Regex)));
                    }
                }

                return _settableProperties;
            }
        }

        [RegexSearch(@"<strong>(.*)E[SD]?T</strong><br />")]
        public DateTimeOffset Time { get; set; }

        /// <summary>
        /// The coordinates of the source location
        /// </summary>
        [RegexSearch(@"<strong>Location:</strong> (\d*.\d*N \d*.\d*W) or \d* nautical miles \w* of search location of .*<br />")]
        public string Location { get; set; }

        /// <summary>
        /// The distance in nautical miles from the search source.
        /// </summary>
        [RegexSearch(@"<strong>Location:</strong> .* or (\d*) nautical miles \w* of search location of .*<br />")]
        public int? DistanceFromSource { get; set; }

        [RegexSearch(@"<strong>Wind Direction:</strong> (..?) \(\d*&#176;\)<br />")]
        public string WindDirection { get; set; }

        [RegexSearch(@"<strong>Wind Direction:</strong> ..? \((\d*)&#176;\)<br />")]
        public int? WindAngle { get; set; }

        [RegexSearch(@"<strong>Wind Speed:</strong> (\d*\.?\d*) knots<br />")]
        public double? WindSpeed { get; set; }

        [RegexSearch(@"<strong>Wind Gust:</strong> (\d*\.?\d*) knots<br />")]
        public double? WindGust { get; set; }

        [RegexSearch(@"<strong>Significant Wave Height:</strong> (\d*\.?\d*) ft<br />")]
        public double? SignificantWaveHeight { get; set; }

        [RegexSearch(@"<strong>Dominant Wave Period:</strong> (\d*\.?\d*) sec<br />")]
        public double? DominantWavePeriod { get; set; }

        [RegexSearch(@"<strong>Atmospheric Pressure:</strong> (\d*\.?\d*) in \(.*mb\)<br />")]
        public double? AtmosphericPressure { get; set; }

        [RegexSearch(@"<strong>Air Temperature:</strong> (\d*\.?\d*)&#176;F \(.*&#176;C\)<br />")]
        public double? AirTemperature { get; set; }

        [RegexSearch(@"<strong>Water Temperature:</strong> (\d*\.?\d*)&#176;F \(.*&#176;C\)<br />")]
        public double? WaterTemperature { get; set; }

        /// <summary>
        /// The long title of the NOAA station (ex. 'Station 44063 - Annapolis, MD')
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// The raw RSS feed data.
        /// </summary>
        private string Raw { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="station"><see cref="Title"/></param>
        public StationSource(string station)
        {
            Title = station;
        }

        /// <summary>
        /// Update this object's updatable fields (those denoted by the <see cref="RegexSearchAttribute"/>). If the property is not found within the input <paramref name="raw"/> then it is set to null.
        /// </summary>
        /// <param name="raw"></param>
        public void Update(string raw)
        {
            Raw = raw;
            foreach (Tuple<PropertyInfo, Regex> property in SettableProperties)
            {
                PropertyInfo info = property.Item1;
                Type propertyType = info.PropertyType;

                // Regex matching, if no match need to reset the property.
                GroupCollection matches = property.Item2.Match(raw).Groups;
                if (matches.Count != RegexSearchAttribute.MAX_MATCHES)
                {
                    Debug.WriteLine($"Could not match property {info.Name} on station {Title}!");
                    info.SetValue(this, null);
                    continue;
                }
                string str = matches[1].Value;

                // Hard-casting down here because I'm too dumb to make it truly dynamic.
                // We could also use an 'object val' here since everything should be nullable but dynamic works through this.
                dynamic val;
                if (propertyType.IsAssignableFrom(typeof(DateTimeOffset)))
                {
                    val = DateTimeOffset.Parse(str);
                }
                else if (propertyType.IsAssignableFrom(typeof(int?)))
                {
                    val = int.Parse(str);
                }
                else if (propertyType.IsAssignableFrom(typeof(double?)))
                {
                    val = double.Parse(str);
                }
                else
                {
                    val = Convert.ChangeType(str, property.Item1.PropertyType);
                }
                info.SetValue(this, val);
            }
        }

    }
}
