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

        private readonly NoaaRssClient NOAA;

        private readonly IReadOnlyCollection<BoatingArea> BoatingAreas;

        public MainForm()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="initialModel">The initial state of the UI to display.</param>
        /// <param name="noaaSource">A helper preconstructed to retrieve data from NOAA</param>
        /// <param name="boatingAreas">A list of selectable <see cref="BoatingArea"/></param>
        /// <param name="initialArea">The initial selection from <paramref name="boatingAreas"/></param>
        /// <remarks>The this() constructor runs first then this constructor.</remarks>
        public MainForm(ViewModel initialModel, NoaaRssClient noaaSource, IReadOnlyCollection<BoatingArea> boatingAreas, string initialAreaTitle = null) : this()
        {
            State = initialModel;
            NOAA = noaaSource;
            BoatingAreas = boatingAreas;
            UpdateUI();

            if (!string.IsNullOrWhiteSpace(initialAreaTitle))
            {
                SetActiveArea(boatingAreas.SingleOrDefault(a => a.Title == initialAreaTitle));
            }
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
                Button senderButton = (Button)sender;

                BoatingArea newArea = null;
                if (senderButton.Name == Area1Button.Name)
                {
                    newArea = BoatingAreas.ElementAtOrDefault(0);
                }
                else if (senderButton.Name == Area2Button.Name)
                {
                    newArea = BoatingAreas.ElementAtOrDefault(1);
                }
                else if (senderButton.Name == Area3Button.Name)
                {
                    newArea = BoatingAreas.ElementAtOrDefault(2);
                }

                SetActiveArea(newArea);
                SwapAndUpdatePanels();
            }
            else
            {
                Console.WriteLine($"{nameof(AreaButton_Click)} called with not a button! Type was {sender.GetType()}");
            }
        }
    }
}
