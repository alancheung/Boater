using Boater.Models;
using System;
using System.Linq;

namespace Boater
{
    public partial class MainForm
    {
        private void UpdateUI()
        {
            MainPanel.Visible = State.IsMainPanel;
            ReadingsPanel.Visible = State.IsMainPanel;
            MapPanel.Visible = State.IsMapPanel;
        }

        private void SwapAndUpdatePanels()
        {
            State.SwapRightPanel();
            UpdateUI();
        }


        private async void SetActiveArea(BoatingArea area)
        {
            if (area != null)
            {
                State.ActiveArea = area;

                // NOAA.GetLatestData(State.ActiveArea);
                await OWM.UpdateWeather(State.ActiveArea);
                AreaChanged(State.ActiveArea);
            }
            else
            {
                Console.WriteLine($"New area selection was null! Did not set a new area!");
            }
        }

        private void AreaChanged(BoatingArea area)
        {
            StationLabel.Text = area.Title;

            double? temp = area.StationData.FirstOrDefault(d => d.AirTemperature.HasValue)?.AirTemperature;
            if (temp.HasValue)
            {
                TemperatureLabel.Text = $"Temperature\n{temp}";
            }
            else
            {
                TemperatureLabel.Text = "No data...";
            }

            double? windSpeed = area.StationData.FirstOrDefault(d => d.WindSpeed.HasValue)?.WindSpeed;
            if (windSpeed.HasValue)
            {
                WindLabel.Text = $"Wind\n{windSpeed}";
            }
            else
            {
                WindLabel.Text = "No data...";
            }

            double? waveHeight = area.StationData.FirstOrDefault(d => d.SignificantWaveHeight.HasValue)?.SignificantWaveHeight;
            if (waveHeight.HasValue)
            {
                WaveLabel.Text = $"Wave Height\n{waveHeight}";
            }
            else
            {
                WaveLabel.Text = "No data...";
            }
        }
    }
}
