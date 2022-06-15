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

            string openWeatherMapAPIKey = ConfigurationManager.AppSettings["OpenWeatherMapAPIKey"];
            if (string.IsNullOrWhiteSpace(openWeatherMapAPIKey))
            {
                if (RELEASE)
                {
                    throw new SettingsPropertyNotFoundException($"'OpenWeatherMapAPIKey' was not set!");
                }
                openWeatherMapAPIKey = SecretKeys.OpenWeatherMapAPIKey;
            }
            string openWeatherMapContentPath = ConfigurationManager.AppSettings["OpenWeatherMapContentPath"];
            OpenWeatherMapClient ollieWilliams = new OpenWeatherMapClient(apiKey: openWeatherMapAPIKey, contentPath: openWeatherMapContentPath, useRealWeather: RELEASE);

                ViewModel initialModel = new ViewModel()
            {
                IsMainPanel = true
            };
            NoaaRssClient noaaFeed = new NoaaRssClient();

            string boatingAreaConfigPath = ConfigurationManager.AppSettings["BoatingAreaConfigPath"];
            Console.WriteLine($"Boating area config file path: '{boatingAreaConfigPath}'");
            string rawJson = File.ReadAllText(boatingAreaConfigPath);
            List<BoatingArea> boatingAreas = JsonConvert.DeserializeObject<List<BoatingArea>>(rawJson);

            string defaultBoatingAreaTitle = ConfigurationManager.AppSettings["DefaultBoatingAreaTitle"];

            Application.Run(new MainForm(initialModel, ollieWilliams, noaaFeed, boatingAreas, defaultBoatingAreaTitle));
        }
    }
}
