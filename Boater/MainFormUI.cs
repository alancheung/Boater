using Boater.Common;
using Boater.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
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
        private const string WindGustFormat = "Gust: {0} knots";
        private const string WaveFormat = "{0} feet";
        private const string WavePeriodFormat = "Every {0} seconds";
        private const string ForecastFormat = "{0}\n{1}\n{2}\u00B0 F / {3}\u00B0 F\n{4}%";

        private const string WindImageName = "wind-scaled-north.png";
        private Dictionary<string, int> WeatherTitleRanking = new Dictionary<string, int>()
        {
            { "Clear",  0 },
            { "Clouds",  1 },
            { "Drizzle",  2 },
            { "Rain",  3 },
            { "Thunderstorm",  4 },
            { "Snow",  5 },
            { "Atmosphere",  6 },
        };

        private static readonly Color OkColor = Color.MediumSeaGreen;
        private static readonly Color WarningColor = Color.Orange;
        private static readonly Color AlertColor = Color.Red;

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
            StationLabel.ForeColor = Color.Black;
            TemperatureLabel.Text = NoDataString(TemperatureFormat);
            TemperatureAdditionalLabel.Text = NoDataString(TemperatureFormat);
            WindLabel.Text = NoDataString(WindFormat);
            WindDirectionLabel.Text = NoDataString(WindDirectionFormat);
            WindAdditionalLabel.Text = NoDataString(WindGustFormat);
            WaveLabel.Text = NoDataString(WaveFormat);
            WavePeriodLabel.Text = NoDataString(WavePeriodFormat);
            ForecastLabel.Text = NoDataString(ForecastFormat);
            OtherLabel.Text = string.Empty;
            OtherLabel.ForeColor = Color.Black;
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
                    Task<bool> textTask = NOAA.UpdateLatestTextData(State.ActiveArea);
                    Task<bool> weatherTask = OWM.UpdateWeather(State.ActiveArea);
                    Task<bool> forecastTask = OWM.UpdateForecast(State.ActiveArea);

                    Task updateTasks = Task.WhenAll(noaaTask, textTask, weatherTask, forecastTask);
                    Task<Task> running = Task.WhenAny(updateTasks, Task.Delay(timeoutMs));
                    
                    await running;
                    
                    if (running.Result != updateTasks)
                    {
                        // If all updates failed then call the task failed, otherwise update what we can
                        if (!noaaTask.IsCompleted && !textTask.IsCompleted && !weatherTask.IsCompleted && !forecastTask.IsCompleted)
                        {
                            throw new TimeoutException($"Updating tasks did not finish within {timeoutMs / 1000} seconds. Showing stale data...");
                        }
                    }

                    // Trickery here to only attempt to get the result if the task is completed. Otherwise waiting for the result causes it to hang.
                    AreaChanged(State.ActiveArea,
                        noaaUpdateSuccess: noaaTask.IsCompleted && noaaTask.Result,
                        noaaTextUpdateSuccess: textTask.IsCompleted && textTask.Result,
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
                OtherLabel.Text = $"{DateTimeOffset.Now.ToString("HH:mm:ss")} Update FAILED! Exception: {ex.Message}";
                OtherLabel.ForeColor = Color.Red;
            }
        }

        private string NoDataString(string format)
        {
            // string.Format requires at least the number of elements with no upper limit
            return string.Format(format, "--", "--", "--", "--", "--", "--", "--", "--");
        }

        private void SetLastUpdateText(DateTimeOffset oldestUpdate)
        {
            // Setup text if not already showing an error
            if (OtherLabel.ForeColor != Color.Red)
            {
                DateTimeOffset nextUpdate = DateTimeOffset.Now + (MaxMinsBeforeUpdateRequired - (DateTimeOffset.Now - oldestUpdate));
                OtherLabel.Text = $"Last Update: {oldestUpdate.ToString("HH:mm:ss")}. Next update at {nextUpdate.ToString("HH:mm:ss")}";
            }
        }

        private void AreaChanged(BoatingArea area, bool noaaUpdateSuccess, bool noaaTextUpdateSuccess, bool weatherSuccess, bool forecastSuccess)
        {
            StationLabel.Text = area.Title;
            
            if (noaaUpdateSuccess)
            {
                UpdateNoaaData(area.StationData);
            }

            if (noaaTextUpdateSuccess)
            {
                UpdateNoaaTextUpdate(area.TextUpdate);
            }

            if (weatherSuccess)
            {
                UpdateOpenWeatherData(area.WeatherResult);
            }

            if (forecastSuccess)
            {
                State.ForecastDaysOut = 1;
                UpdateForecastData(area.ForecastResult, State.ForecastDaysOut);
            }

            SetLastUpdateText(area.OldestUpdate);
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
            double? windGust = stationData.FirstOrDefault(d => d.WindGust.HasValue)?.WindGust;
            int? windAngle = stationData.FirstOrDefault(d => d.WindAngle.HasValue)?.WindAngle;
            string windDirection= stationData.FirstOrDefault(d => !string.IsNullOrWhiteSpace(d.WindDirection))?.WindDirection;
            if (windSpeed.HasValue)
            {
                WindLabel.Text = string.Format(WindFormat, windSpeed);
                SetWindDirection(windAngle, windDirection);
                SetWindImageAlert(windSpeed.Value, windGust);

                if (windGust.HasValue)
                {
                    WindAdditionalLabel.Text = string.Format(WindGustFormat, windGust);
                }
                else
                {
                    WindAdditionalLabel.Text = string.Empty;
                }
            }
            else
            {
                WindLabel.Text = NoDataString(WindFormat);
            }

            double? waveHeight = stationData.FirstOrDefault(d => d.SignificantWaveHeight.HasValue)?.SignificantWaveHeight;
            double? wavePeriod = stationData.FirstOrDefault(d => d.DominantWavePeriod.HasValue)?.DominantWavePeriod;
            if (waveHeight.HasValue)
            {
                WaveLabel.Text = string.Format(WaveFormat, waveHeight.ToString());
                SetWaveImageAlert(waveHeight.Value, wavePeriod);

                if (wavePeriod.HasValue)
                {
                    WavePeriodLabel.Text = string.Format(WavePeriodFormat, wavePeriod.ToString());
                }
                else
                {
                    WavePeriodLabel.Text = string.Empty;
                }
            }
            else
            {
                WaveLabel.Text = NoDataString(WaveFormat);
            }
        }

        private void UpdateNoaaTextUpdate(string textUpdate)
        {
            UpdateTextBox.Text = textUpdate;

            if (textUpdate.Contains("SMALL CRAFT ADVISORY IN EFFECT"))
            {
                string text = StationLabel.Text + "\nSMALL CRAFT ADVISORY";
                StationLabel.SetText(text);
                StationLabel.ForeColor = Color.Red;
            }
        }

        private void UpdateOpenWeatherData(CurrentWeatherResult weatherResult)
        {
            TemperatureLabel.Text = string.Format(TemperatureFormat, weatherResult.Temp);
            SetTemperatureImageAlert(weatherResult.Temp);

            if (WindLabel.Text == NoDataString(WindFormat))
            {
                WindLabel.Text = string.Format(WindFormat, weatherResult.WindSpeed);
            }
        }
        private void UpdateForecastData(IEnumerable<IGrouping<DateTime, FiveDaysForecastResult>> forecastResult, int days)
        {
            List<FiveDaysForecastResult> forecast = forecastResult
                .FirstOrDefault(kv => kv.Key.Date == DateTimeOffset.Now.AddDays(days).Date)
                .ToList();

            if (forecast != null && forecast.Any())
            {
                FiveDaysForecastResult worst = null;
                // Will there be anything other than clear or clouds tomorrow?
                worst = forecast.FirstOrDefault(f => (WeatherTitleRanking.ContainsKey(f.Title) ? WeatherTitleRanking[f.Title] : 99) >= 2);
                if (worst == null)
                {
                    // No bad weather, just get the weather at 8AM
                    worst = forecast.FirstOrDefault(f => f.Date.Hour > 8);
                }

                if (worst != null)
                {
                    string date = worst.Date.ToString("ddd MM/dd");
                    string title = worst.Title;

                    double high = forecast.Max(t => t.TempMax);
                    double low = forecast.Min(t => t.TempMin);
                    double humidity = forecast.Average(t => t.Humidity);

                    ForecastLabel.SetText(string.Format(ForecastFormat, date, title, high, low, humidity));
                    SetForecastImage(worst.Icon);
                    SetForecastImageAlert(title);
                }
                else
                {
                    ForecastLabel.Text = string.Format(ForecastFormat, DateTimeOffset.Now.AddDays(days).ToString("ddd MM/dd"), "--", "--", "--", "--");
                }
            }
            else
            {
                ForecastLabel.Text = string.Format(ForecastFormat, DateTimeOffset.Now.AddDays(days).ToString("ddd MM/dd"), "--", "--", "--", "--");
            }
        }

        public void SetTemperatureImageAlert(double temperature)
        {
            if (temperature < 32)
            {
                TemperatureStatusImage.BackColor = Color.SteelBlue;
            }
            else if (temperature < 50)
            {
                TemperatureStatusImage.BackColor = Color.LightSteelBlue;
            }
            else if (temperature < 80)
            {
                TemperatureStatusImage.BackColor = OkColor;
            }
            else if (temperature < 90)
            {
                TemperatureStatusImage.BackColor = WarningColor;
            }
            else
            {
                TemperatureStatusImage.BackColor = AlertColor;
            }
        }

        public void SetWindImageAlert(double speed, double? gust)
        {
            double gustDifference = 0;
            if (gust.HasValue && speed < gust)
            {
                gustDifference = gust.Value - speed;
            }

            if (speed < 7)
            {
                if (gustDifference < 3)
                {
                    WindImage.BackColor = OkColor;
                }
                else
                {
                    WindImage.BackColor = WarningColor;
                }
            }
            else if (speed < 10)
            {
                if (gustDifference < 3)
                {
                    WindImage.BackColor = WarningColor;
                }
                else
                {
                    WindImage.BackColor = Color.Red;
                }
            }
            else
            {
                WindImage.BackColor = Color.Red;
            }
        }

        public void SetWaveImageAlert(double wave, double? p)
        {
            double period = p.HasValue ? p.Value : 0;

            if (wave == 0)
            {
                WaveImage.BackColor = OkColor;
            }
            else if (wave < 1)
            {
                if (period < 5)
                {
                    WaveImage.BackColor = OkColor;
                }
                else
                {
                    WaveImage.BackColor = WarningColor;
                }
            }
            else
            {
                WaveImage.BackColor = AlertColor;
            }
        }

        public void SetForecastImageAlert(string title)
        {
            int rank = 99;
            if (WeatherTitleRanking.ContainsKey(title))
            {
                rank = WeatherTitleRanking[title];
            }

            if (rank < 2)
            {
                ForecastImage.BackColor = OkColor;
            }
            else if (rank < 4)
            {
                ForecastImage.BackColor = WarningColor;
            }
            else
            {
                ForecastImage.BackColor = AlertColor;
            }
        }

        private void SetForecastImage(string icon)
        {
            string iconPath = Path.Combine(OpenWeatherContentPath, icon);
            iconPath = Path.ChangeExtension(iconPath, "png");

            ForecastImage.ImageLocation = iconPath;
        }

        private void SetWindDirection(int? angle, string direction)
        {
            if (!angle.HasValue)
            {
                WindDirectionLabel.Text = string.Empty;
                return;
            }

            // Sometimes north is displayed as 360 so make it pretty.
            angle %= 360;
            
            if (!string.IsNullOrWhiteSpace(direction))
            {
                WindLabel.Text += $"\n{direction}";
            }
            WindDirectionLabel.Text = string.Format(WindDirectionFormat, angle);

            string iconPath = Path.Combine(FlatIconPath, WindImageName);
            WindImage.Image = WinFormExtensions.RotateImage(Image.FromFile(iconPath), angle.Value);

        }
    }
}
