using Boater.Common;
using Boater.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using WeatherNet.Model;

namespace Boater
{
    public partial class MainForm
    {
        private const string TemperatureFormat = "{0}\u00B0 F";
        private const string WindFormat = "{0} knots";
        private const string WindDirectionFormat = "{0}\u00B0";
        private const string WaveFormat = "{0} feet\n{1} Dominant Period";
        private const string ForecastFormat = "{0}\n{1}\n{2}\u00B0 F\n{3}\u00B0 F";

        private const string WindImageName = "wind-scaled-north.png";

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

        private void SetLoading(string title)
        {
            StationLabel.Text = $"Loading {title}...";
            TemperatureLabel.Text = NoDataString(TemperatureFormat);
            TemperatureAdditionalLabel.Text = NoDataString(TemperatureFormat);
            WindLabel.Text = NoDataString(WindFormat);
            WindAddtionalLabel.Text = NoDataString(WindDirectionFormat);
            WaveLabel.Text = NoDataString(WaveFormat);
            ForecastLabel.Text = NoDataString(ForecastFormat);
        }

        private async Task SetActiveArea(BoatingArea area, int timeoutMs = 30000)
        {
            try
            {
                if (area != null)
                {
                    SetLoading(area.Title);
                    State.ActiveArea = area;

                    Task<bool> noaaTask = NOAA.UpdateLatestStationData(State.ActiveArea);
                    Task<bool> weatherTask = OWM.UpdateWeather(State.ActiveArea);
                    Task<bool> forecastTask = OWM.UpdateForecast(State.ActiveArea);

                    Task updateTasks = Task.WhenAll(noaaTask, weatherTask, forecastTask);
                    Task<Task> running = Task.WhenAny(updateTasks, Task.Delay(timeoutMs));
                    
                    await running;
                    
                    if (running.Result != updateTasks)
                    {
                        // If all updates failed then call the task failed, otherwise update what we can
                        if (!noaaTask.IsCompleted && !weatherTask.IsCompleted && !forecastTask.IsCompleted)
                        {
                            throw new TimeoutException($"Updating tasks did not finish within {timeoutMs / 1000} seconds. Showing stale data...");
                        }
                    }

                    // Trickery here to only attempt to get the result if the task is completed. Otherwise waiting for the result causes it to hang.
                    AreaChanged(State.ActiveArea, 
                        noaaUpdateSuccess: noaaTask.IsCompleted && noaaTask.Result, 
                        weatherSuccess: weatherTask.IsCompleted && weatherTask.Result, 
                        forecastSuccess: forecastTask.IsCompleted && forecastTask.Result);
                }
                else
                {
                    Console.WriteLine($"New area selection was null! Did not set a new area!");
                }
            }
            catch (Exception ex)
            {
                StationLabel.Text = $"{area.Title} (STALE)";
                OtherLabel.Text = $"{DateTimeOffset.Now.ToString("HH:mm:ss")} Update FAILED! Exception: {ex.Message}";
                OtherLabel.ForeColor = Color.Red;
            }
        }

        private string NoDataString(string format)
        {
            // string.Format requires at least the number of elements with no upper limit
            return string.Format(format, "--", "--", "--", "--", "--", "--", "--", "--");
        }

        private void AreaChanged(BoatingArea area, bool noaaUpdateSuccess, bool weatherSuccess, bool forecastSuccess)
        {
            StationLabel.Text = area.Title;
            
            if (noaaUpdateSuccess)
            {
                UpdateNoaaData(area.StationData);
            }

            if (weatherSuccess)
            {
                UpdateOpenWeatherData(area.WeatherResult);
            }

            if (forecastSuccess)
            {
                UpdateForecastData(area.ForecastResult);
            }
        }

        private void UpdateNoaaData(List<StationSource> stationData)
        {
            double? waterTemp = stationData.FirstOrDefault(d => d.WaterTemperature.HasValue)?.WaterTemperature;
            if (waterTemp.HasValue)
            {
                TemperatureAdditionalLabel.Text = string.Format(TemperatureFormat, waterTemp);
            }
            else
            {
                TemperatureAdditionalLabel.Text = NoDataString(TemperatureFormat);
            }

            double? windSpeed = stationData.FirstOrDefault(d => d.WindSpeed.HasValue)?.WindSpeed;
            int? windDirection = stationData.FirstOrDefault(d => d.WindAngle.HasValue)?.WindAngle;
            if (windSpeed.HasValue)
            {
                WindLabel.Text = string.Format(WindFormat, windSpeed.ToString(), windDirection.ToString());
                SetWindDirection(windDirection);
            }
            else
            {
                WindLabel.Text = NoDataString(WindFormat);
            }

            double? waveHeight = stationData.FirstOrDefault(d => d.SignificantWaveHeight.HasValue)?.SignificantWaveHeight;
            double? wavePeriod = stationData.FirstOrDefault(d => d.DominantWavePeriod.HasValue)?.DominantWavePeriod;
            if (waveHeight.HasValue)
            {
                WaveLabel.Text = string.Format(WaveFormat, waveHeight.ToString(), wavePeriod.ToString());
            }
            else
            {
                WaveLabel.Text = NoDataString(WaveFormat);
            }
        }

        private void UpdateOpenWeatherData(CurrentWeatherResult weatherResult)
        {
            TemperatureLabel.Text = string.Format(TemperatureFormat, weatherResult.Temp);
        }
        private void UpdateForecastData(List<FiveDaysForecastResult> forecastResult)
        {
            var group = forecastResult.GroupBy(f => f.Date.Date);
            List<FiveDaysForecastResult> tomorrow = group.FirstOrDefault(kv => kv.Key.Date == DateTimeOffset.Now.AddDays(1).Date).ToList();
            if (tomorrow != null)
            {
                string date = tomorrow.First().Title;
                string description = tomorrow.First().Description;
                double high = tomorrow.Max(t => t.TempMax);
                double low = tomorrow.Min(t => t.TempMin);

                ForecastLabel.Text = string.Format(ForecastFormat, date, description, high, low);
                SetForecastImage(tomorrow.First().Icon);
            }
            else
            {
                ForecastLabel.Text = NoDataString(ForecastFormat);
            }
        }

        private void SetForecastImage(string icon)
        {
            string iconPath = Path.Combine(OpenWeatherContentPath, icon);
            iconPath = Path.ChangeExtension(iconPath, "png");

            ForecastImage.ImageLocation = iconPath;
        }

        private void SetWindDirection(int? angle)
        {
            if (angle.HasValue)
            {
                WindAddtionalLabel.Text = string.Empty;
            }

            // Sometimes north is displayed as 360 so make it pretty.
            angle = angle % 360;

            string iconPath = Path.Combine(FlatIconPath, WindImageName);

            WindImage.Image = WinFormExtensions.RotateImage(Image.FromFile(iconPath), angle.Value);
            WindAddtionalLabel.Text = string.Format(WindDirectionFormat, angle);
        }
    }
}
