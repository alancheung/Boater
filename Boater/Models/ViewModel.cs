namespace Boater.Models
{
    /// <summary>
    /// A representation of the UI state and any objects required for the UI.
    /// </summary>
    public class ViewModel
    {
        #region MainPanelControls
        /// <summary>
        /// Is the main panel (with the readings) being displayed?
        /// </summary>
        public bool IsMainPanel = true;

        /// <summary>
        /// Is the map panel being displayed?
        /// </summary>
        public bool IsMapPanel => !IsMainPanel;

        /// <summary>
        /// Switch the right panel between readings view and map view.
        /// </summary>
        public void SwapRightPanel()
        {
            IsMainPanel = !IsMainPanel;
        }
        #endregion

        #region AreaControls
        public BoatingArea ActiveArea { get; set; }
        #endregion
    }
}
