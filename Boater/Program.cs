﻿using Boater.Models;
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
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            ViewModel initialModel = new ViewModel()
            {
                IsMainPanel = true
            };
            NoaaRssFeed noaaFeed = new NoaaRssFeed();

            string boatingAreaConfigPath = ConfigurationManager.AppSettings["BoatingAreaConfigPath"];
            Console.WriteLine($"Boating area config file path: '{boatingAreaConfigPath}'");
            string rawJson = File.ReadAllText(boatingAreaConfigPath);
            List<BoatingArea> boatingAreas = JsonConvert.DeserializeObject<List<BoatingArea>>(rawJson);
            
            Application.Run(new MainForm(initialModel, noaaFeed, boatingAreas));
        }
    }
}
