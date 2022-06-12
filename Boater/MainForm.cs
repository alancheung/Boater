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
        private readonly ViewModel state;

        private static readonly string DateTimeOffsetFormat = $"MM/dd/yyyy{Environment.NewLine}hh:mm:ss";

        public MainForm()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="initialModel"></param>
        /// <remarks>The this() constructor runs first then this constructor.</remarks>
        public MainForm(ViewModel initialModel) : this()
        {
            state = initialModel;
            Update();
        }

        private void Update()
        {
            MainPanel.Visible = state.IsMainPanel;
            ReadingsPanel.Visible = state.IsMainPanel;
            MapPanel.Visible = state.IsMapPanel;
        }

        private void DateTimeTimer_Tick(object sender, EventArgs e)
        {
            TimeLabel.Text = DateTimeOffset.Now.ToString(DateTimeOffsetFormat);
        }

        private void ChooseButton_Click(object sender, EventArgs e)
        {
            state.IsMainPanel = !state.IsMainPanel;
            Update();
        }
    }
}
