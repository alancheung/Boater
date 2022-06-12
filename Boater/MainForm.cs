using Boater.Models;
using System;
using System.Windows.Forms;

namespace Boater
{
    public partial class MainForm : Form
    {
        /// <summary>
        /// The object is readonly but the fields in the object are not.
        /// </summary>
        private readonly ViewModel viewModel;

        private static readonly string DateTimeOffsetFormat = $"MM/dd/yyyy{Environment.NewLine}hh:mm:ss";

        public MainForm()
        {
            InitializeComponent();
        }

        public MainForm(ViewModel initialModel) : this()
        {
            viewModel = initialModel;
            MainPanel.Visible = viewModel.IsMainPanel
        }

        private void Update(ViewModel state = null)
        {
            state = state ?? viewModel;

            MainPanel.Visible = state.IsMainPanel;
            ReadingsPanel.Visible = state.IsMainPanel;
            MapPanel.Visible = state.IsMapPanel;
        }

        private void DateTimeTimer_Tick(object sender, EventArgs e)
        {
            TimeLabel.Text = DateTimeOffset.Now.ToString(DateTimeOffsetFormat);
        }
    }
}
