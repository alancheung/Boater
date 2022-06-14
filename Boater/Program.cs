using Boater.Models;
using System;
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


            //NoaaRssFeed noaaFeed = new NoaaRssFeed(stations);
            //noaaFeed.Start();


            ViewModel initialModel = new ViewModel()
            {
                IsMainPanel = true
            };
            Application.Run(new MainForm(initialModel));
        }
    }
}
