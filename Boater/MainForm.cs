using Boater.Models;
using Boater.Services;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using WeatherNet.Model;

namespace Boater
{
    public partial class MainForm : Form
    {
        private static readonly string DateTimeOffsetFormat = $"MM/dd/yyyy{Environment.NewLine}HH:mm:ss";

        private readonly ViewModel State;

        private readonly OpenWeatherMapClient OWM;

        private readonly NoaaRssClient NOAA;

        private readonly IReadOnlyCollection<BoatingArea> BoatingAreas;

        private readonly string FlatIconPath;

        private readonly string OpenWeatherContentPath;

        private readonly TimeSpan MaxMinsBeforeUpdateRequired;

        private readonly int PeriodicUpdateTimeMs;

        public MainForm()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="initialModel">The initial state of the UI to display.</param>
        /// <param name="weatherSource">A helper preconstructed to retrieve data from OpenWeatherMap</param>
        /// <param name="noaaSource">A helper preconstructed to retrieve data from NOAA</param>
        /// <param name="boatingAreas">A list of selectable <see cref="BoatingArea"/></param>
        /// <param name="flaticonPath">The path to the icons used for the UI</param>
        /// <param name="openweatherPath">The path to the icons used for the openweather weather icons</param>
        /// <param name="maxMinsBeforeUpdateRequired">The maximum amount of time before an update is required</param>
        /// <param name="periodicUpdateTime">Time between periodic updates</param>
        /// <param name="initialAreaTitle">The initial selection from <paramref name="boatingAreas"/></param>
        /// <remarks>The this() constructor runs first then this constructor.</remarks>
        public MainForm(ViewModel initialModel, 
            OpenWeatherMapClient weatherSource, 
            NoaaRssClient noaaSource, 
            IReadOnlyCollection<BoatingArea> boatingAreas, 
            string flaticonPath, 
            string openweatherPath,
            TimeSpan maxMinsBeforeUpdateRequired, 
            int periodicUpdateTime,
            string initialAreaTitle = null) : this()
        {
            State = initialModel;
            OWM = weatherSource;
            NOAA = noaaSource;
            BoatingAreas = boatingAreas;
            UpdateUI();

            FlatIconPath = flaticonPath;
            OpenWeatherContentPath = openweatherPath;

            MaxMinsBeforeUpdateRequired = maxMinsBeforeUpdateRequired;
            PeriodicUpdateTimeMs = periodicUpdateTime;
            PeriodicUpdateTimer.Interval = periodicUpdateTime;
            PeriodicUpdateTimer.Start();
        }

        /// <summary>
        /// Only update the datetime shown to user
        /// </summary>
        /// <param name="sender">The UI timer</param>
        /// <param name="e">Empty argument</param>
        private void DateTimeTimer_Tick(object sender, EventArgs e)
        {
            TimeLabel.Text = DateTimeOffset.Now.ToString(DateTimeOffsetFormat);
        }

        /// <summary>
        /// Background timer to check if any information needs to be retrieved.
        /// </summary>
        /// <param name="sender">The UI timer</param>
        /// <param name="e">Empty argument</param>
        private async void PeriodicUpdateTimer_Tick(object sender, EventArgs e)
        {
            if (State.ActiveArea != null && State.IsMainPanel)
            {
                // Update all data if ready to update
                DateTimeOffset oldestUpdate = State.ActiveArea.OldestUpdate;
                TimeSpan timeSinceLastUpdate = DateTimeOffset.Now - oldestUpdate;

                if (timeSinceLastUpdate > MaxMinsBeforeUpdateRequired)
                {
                    await SetActiveArea(State.ActiveArea);
                }
                // Setup text if not already showing an error
                else if (OtherLabel.ForeColor != Color.Red)
                {
                    SetLastUpdateText(oldestUpdate);
                }

                if ((DateTimeOffset.Now - State.LastForecastChangeTime).TotalMilliseconds > PeriodicUpdateTimeMs)
                {
                    // OpenWeatherMap only returns a maximum of 5 days
                    State.ForecastDaysOut = (State.ForecastDaysOut + 1) % 5;
                    State.LastForecastChangeTime = DateTimeOffset.Now;

                    UpdateForecastData(State.ActiveArea.ForecastResult, State.ForecastDaysOut);
                }
            }
        }

        private void ChooseButton_Click(object sender, EventArgs e)
        {
            SwapAndUpdatePanels();
        }

        private async void AreaButton_Click(object sender, EventArgs e)
        {
            if (sender is Button)
            {
                Button senderButton = (Button)sender;

                BoatingArea newArea = null;
                if (senderButton.Name == Area1Button.Name)
                {
                    newArea = BoatingAreas.ElementAtOrDefault(0);
                }
                else if (senderButton.Name == Area2Button.Name)
                {
                    newArea = BoatingAreas.ElementAtOrDefault(1);
                }
                else if (senderButton.Name == Area3Button.Name)
                {
                    newArea = BoatingAreas.ElementAtOrDefault(2);
                }

                SwapAndUpdatePanels();
                await SetActiveArea(newArea);
            }
            else
            {
                Console.WriteLine($"{nameof(AreaButton_Click)} called with not a button! Type was {sender.GetType()}");
            }
        }
    }
}
