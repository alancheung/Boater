using Boater.Models;
using System;
using System.Windows.Forms;

namespace Boater
{
    public partial class MainForm : Form
    {
        private static readonly string DateTimeOffsetFormat = $"MM/dd/yyyy{Environment.NewLine}hh:mm:ss";

        public MainForm()
        {
            InitializeComponent();
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
                if (senderButton.Name == Area1Button.Name)
                {

                    SwapAndUpdatePanels();
                }
                else if (senderButton.Name == Area2Button.Name)
                {

                    SwapAndUpdatePanels();
                }
                else if (senderButton.Name == Area3Button.Name)
                {

                    SwapAndUpdatePanels();
                }
            }
        }
    }
}
