using Boater.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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

            List<Tuple<string, string>> stations = new List<Tuple<string, string>>()
            {
                new Tuple<string, string>("bltm2", "Station BLTM2 - 8574680 - Baltimore, MD"),
                new Tuple<string, string>("fsnm2", "NDBC - Station FSNM2 - 8574729 - Francis Scott Key Bridge NE Tower, MD Observations"),
                new Tuple<string, string>("fskm2", "NDBC - Station FSKM2 - 8574728 - Francis Scott Key Bridge, MD Observations"),
                new Tuple<string, string>("44063", "Station 44063 - Annapolis, MD"),
            };

            //NoaaRssFeed noaaFeed = new NoaaRssFeed(stations);
            //noaaFeed.Start();



            Application.Run(new MainForm());
        }
    }
}
