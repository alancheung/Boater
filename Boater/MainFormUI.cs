using Boater.Common;
using Boater.Models;
using System;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Boater
{
    public partial class MainForm
    {
        private const string TemperatureFormat = "{0}\u00B0 F";
        private const string WindFormat = "{0} knots\n{1}\u00B0";
        private const string WaveFormat = "{0} feet\n{1} Dominant Period";
        private const string ForecastFormat = "{0}\n{1}";

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

        private async Task SetActiveArea(BoatingArea area)
        {
            if (area != null)
            {
                SetLoading(area.Title);

                State.ActiveArea = area;
                await Task.WhenAll(NOAA.UpdateLatestStationData(State.ActiveArea), OWM.UpdateWeather(State.ActiveArea));
                AreaChanged(State.ActiveArea);
            }
            else
            {
                Console.WriteLine($"New area selection was null! Did not set a new area!");
            }
        }

        private string NoDataString(string format, int numArgs)
        {
            switch (numArgs)
            {
                case 0:
                    return format;
                case 1:
                    return string.Format(format, "--");
                case 2:
                    return string.Format(format, "--", "--");
                case 3:
                    return string.Format(format, "--", "--", "--");
                case 4:
                    return string.Format(format, "--", "--", "--", "--");
                case 5:
                    return string.Format(format, "--", "--", "--", "--", "--");
                case 6:
                    return string.Format(format, "--", "--", "--", "--", "--", "--");
                case 7:
                    return string.Format(format, "--", "--", "--", "--", "--", "--", "--");
                default:
                    return format;
            }
        }

        private void AreaChanged(BoatingArea area)
        {
            StationLabel.Text = area.Title;

            double? temp = area.StationData.FirstOrDefault(d => d.AirTemperature.HasValue)?.AirTemperature;
            if (temp.HasValue)
            {
                TemperatureLabel.Text = string.Format(TemperatureFormat, temp);
            }
            else
            {
                TemperatureLabel.Text = NoDataString(TemperatureFormat, 1);
            }

            double? waterTemp = area.StationData.FirstOrDefault(d => d.WaterTemperature.HasValue)?.WaterTemperature;
            if (waterTemp.HasValue)
            {
                TemperatureAdditionalLabel.Text = string.Format(TemperatureFormat, waterTemp);
            }
            else
            {
                TemperatureAdditionalLabel.Text = NoDataString(TemperatureFormat, 1);
            }

            double? windSpeed = area.StationData.FirstOrDefault(d => d.WindSpeed.HasValue)?.WindSpeed;
            int? windDirection = area.StationData.FirstOrDefault(d => d.WindAngle.HasValue)?.WindAngle;
            if (windSpeed.HasValue)
            {
                WindLabel.Text = string.Format(WindFormat, windSpeed.ToString() ?? "--", windDirection.ToString() ?? "--");
                WindImage.Image = WinFormExtensions.RotateImage(Image.FromFile("C:\\Users\\Alan\\Desktop\\CodeLibrary\\VisualStudio\\Boater\\Boater\\Content\\Flaticon\\wind-scaled-north.png"), windDirection.Value);
            }
            else
            {
                WindLabel.Text = NoDataString(WindFormat, 2);
            }

            double? waveHeight = area.StationData.FirstOrDefault(d => d.SignificantWaveHeight.HasValue)?.SignificantWaveHeight;
            double? wavePeriod = area.StationData.FirstOrDefault(d => d.DominantWavePeriod.HasValue)?.DominantWavePeriod;
            if (waveHeight.HasValue)
            {
                WaveLabel.Text = string.Format(WaveFormat, waveHeight.ToString() ?? "--", wavePeriod.ToString() ?? "--");
            }
            else
            {
                WaveLabel.Text = NoDataString(WaveFormat, 2);
            }
        }

        private void SetLoading(string title)
        {
            StationLabel.Text = $"Loading {title}...";
            TemperatureLabel.Text = NoDataString(TemperatureFormat, 1);
            TemperatureAdditionalLabel.Text = NoDataString(TemperatureFormat, 1);
            WindLabel.Text = NoDataString(WindFormat, 2);
            WaveLabel.Text = NoDataString(WaveFormat, 2);
            ForecastLabel.Text = NoDataString(ForecastFormat, 2);
        }
    }
}
