using Boater.Models;
using Boater.Services;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Windows.Forms;

namespace Boater
{
    static class Program
    {
#if DEBUG
        /// <summary>
        /// Is the current program executing in RELEASE mode? Determined with !DEBUG directive.
        /// Static readonly to prevent unreachable code warnings.
        /// </summary>
        public static readonly bool RELEASE = false;
#else
        public static readonly bool RELEASE = true;
#endif

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            string flatIconContentPath = ConfigurationManager.AppSettings["FlatIconContentPath"];
            string openWeatherMapContentPath = ConfigurationManager.AppSettings["OpenWeatherMapContentPath"];

            string openWeatherMapAPIKey = ConfigurationManager.AppSettings["OpenWeatherMapAPIKey"];
            if (string.IsNullOrWhiteSpace(openWeatherMapAPIKey))
            {
                if (RELEASE)
                {
                    throw new SettingsPropertyNotFoundException($"'OpenWeatherMapAPIKey' was not set!");
                }
                openWeatherMapAPIKey = SecretKeys.OpenWeatherMapAPIKey;
            }

            TimeSpan maxMinsBeforeUpdateRequired = TimeSpan.FromMinutes(int.Parse(ConfigurationManager.AppSettings["MaxMinsBeforeUpdateRequired"]));

            OpenWeatherMapClient ollieWilliams = new OpenWeatherMapClient(
                apiKey: openWeatherMapAPIKey,
                maxMinsBeforeUpdateRequired: maxMinsBeforeUpdateRequired, 
                useRealWeather: RELEASE);
            NoaaRssClient noaaFeed = new NoaaRssClient(maxMinsBeforeUpdateRequired: maxMinsBeforeUpdateRequired);

            string boatingAreaConfigPath = ConfigurationManager.AppSettings["BoatingAreaConfigPath"];
            Console.WriteLine($"Boating area config file path: '{boatingAreaConfigPath}'");
            string rawJson = File.ReadAllText(boatingAreaConfigPath);
            List<BoatingArea> boatingAreas = JsonConvert.DeserializeObject<List<BoatingArea>>(rawJson);

            string defaultBoatingAreaTitle = ConfigurationManager.AppSettings["DefaultBoatingAreaTitle"];

            ViewModel initialModel = new ViewModel()
            {
                IsMainPanel = true
            };

            MainForm form = new MainForm(
                initialModel: initialModel, 
                weatherSource: ollieWilliams,
                noaaSource: noaaFeed, 
                boatingAreas: boatingAreas,
                flaticonPath: flatIconContentPath,
                openweatherPath: openWeatherMapContentPath,
                maxMinsBeforeUpdateRequired: maxMinsBeforeUpdateRequired,
                initialAreaTitle: defaultBoatingAreaTitle);
            Application.Run(form);
        }
    }
}
