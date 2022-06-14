using Boater.Models;
using Boater.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace Boater
{
    public partial class MainForm : Form
    {
        private static readonly string DateTimeOffsetFormat = $"MM/dd/yyyy{Environment.NewLine}hh:mm:ss";

        /// <summary>
        /// 
        /// </summary>
        /// <remarks>The object is readonly but the fields in the object are not.</remarks>
        private readonly ViewModel State;

        private readonly NoaaRssFeed NOAA;

        private readonly IReadOnlyCollection<BoatingArea> BoatingAreas;

        public MainForm()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="initialModel">The initial state of the UI to display.</param>
        /// <remarks>The this() constructor runs first then this constructor.</remarks>
        public MainForm(ViewModel initialModel, NoaaRssFeed noaaSource, IReadOnlyCollection<BoatingArea> boatingAreas) : this()
        {
            State = initialModel;
            NOAA = noaaSource;
            BoatingAreas = boatingAreas;
            UpdateUI();
        }

        private void DateTimeTimer_Tick(object sender, EventArgs e)
        {
            TimeLabel.Text = DateTimeOffset.Now.ToString(DateTimeOffsetFormat);
        }

        private void ChooseButton_Click(object sender, EventArgs e)
        {
            SwapAndUpdatePanels();
        }

        private void AreaButton_Click(object sender, EventArgs e)
        {
            if (sender is Button)
            {
                bool validSelection = false;
                Button senderButton = (Button)sender;
                if (senderButton.Name == Area1Button.Name)
                {
                    State.ActiveArea = BoatingAreas.ElementAtOrDefault(0);
                }
                else if (senderButton.Name == Area2Button.Name)
                {
                    State.ActiveArea = BoatingAreas.ElementAtOrDefault(1);
                }
                else if (senderButton.Name == Area3Button.Name)
                {
                    State.ActiveArea = BoatingAreas.ElementAtOrDefault(2);
                }

                if (validSelection)
                {
                    NOAA.GetLatestData(State.ActiveArea);
                    SwapAndUpdatePanels();
                }
                else
                {
                    Console.WriteLine($"{senderButton.Name} was not a recognized area!");
                }
            }
            else
            {
                Console.WriteLine($"{nameof(AreaButton_Click)} called with not a button! Type was {sender.GetType()}");
            }
        }
    }
}
