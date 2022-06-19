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
                StationLabel.Text += " (STALE)";
                OtherLabel.Text = $"{DateTimeOffset.Now.ToString("HH:mm:ss")} Update FAILED! Exception: {ex.Message}";
                OtherLabel.ForeColor = Color.Red;
            }
        }

        private string NoDataString(string format)
        {
            // string.Format requires at least the number of elements with no upper limit
            return string.Format(format, "--", "--", "--", "--", "--", "--", "--", "--");
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
                UpdateForecastData(area.ForecastResult, 1);
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
            double? windGust = stationData.FirstOrDefault(d => d.WindGust.HasValue)?.WindGust;
            int? windAngle = stationData.FirstOrDefault(d => d.WindAngle.HasValue)?.WindAngle;
            string windDirection= stationData.FirstOrDefault(d => !string.IsNullOrWhiteSpace(d.WindDirection))?.WindDirection;
            if (windSpeed.HasValue)
            {
                WindLabel.Text = string.Format(WindFormat, windSpeed.ToString());
                SetWindDirection(windAngle, windDirection);

                if (windGust.HasValue)
                {
                    WindAdditionalLabel.Text = string.Format(WindGustFormat, windGust.ToString());
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
                worst = forecast.FirstOrDefault(f => WeatherTitleRanking[f.Title] >= 2);
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
                }
                else
                {
                    ForecastLabel.SetText(NoDataString(ForecastFormat));
                }
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

        private void SetWindDirection(int? angle, string direction)
        {
            if (!angle.HasValue)
            {
                WindDirectionLabel.Text = string.Empty;
                return;
            }

            // Sometimes north is displayed as 360 so make it pretty.
            angle %= 360;
            
            string text = string.Format(WindDirectionFormat, angle);
            if (!string.IsNullOrWhiteSpace(direction))
            {
                text += $"({direction})";
            }
            WindDirectionLabel.Text = text;

            string iconPath = Path.Combine(FlatIconPath, WindImageName);
            WindImage.Image = WinFormExtensions.RotateImage(Image.FromFile(iconPath), angle.Value);

        }
    }
}
