using Boater.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Boater
{
    public partial class MainForm
    {
        /// <summary>
        /// The object is readonly but the fields in the object are not.
        /// </summary>
        private readonly ViewModel state;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="initialModel">The initial state of the UI to display.</param>
        /// <remarks>The this() constructor runs first then this constructor.</remarks>
        public MainForm(ViewModel initialModel) : this()
        {
            state = initialModel;
            UpdateUI();
        }

        private void UpdateUI()
        {
            MainPanel.Visible = state.IsMainPanel;
            ReadingsPanel.Visible = state.IsMainPanel;
            MapPanel.Visible = state.IsMapPanel;
        }

        private void SwapAndUpdatePanels()
        {
            state.SwapRightPanel();
            UpdateUI();
        }
    }
}
