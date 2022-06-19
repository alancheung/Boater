using System;
using System.Linq;

namespace Boater.Models
{
    /// <summary>
    /// A representation of the UI state and any objects required for the UI.
    /// </summary>
    public class ViewModel
    {
        /// <summary>
        /// Is the main panel (with the readings) being displayed?
        /// </summary>
        public bool IsMainPanel = true;

        /// <summary>
        /// Is the map panel being displayed?
        /// </summary>
        public bool IsMapPanel => !IsMainPanel;

        /// <summary>
        /// Switch the right panel between readings view and map view.
        /// </summary>
        public void SwapRightPanel()
        {
            IsMainPanel = !IsMainPanel;
        }
        public BoatingArea ActiveArea { get; set; }

        /// <summary>
        /// The number of days from today to show the forecast.
        /// </summary>
        public int ForecastDaysOut = 0;

        public DateTimeOffset LastForecastChangeTime = DateTimeOffset.MinValue;
    }
}
