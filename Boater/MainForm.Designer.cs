
namespace Boater
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.MapPanel = new System.Windows.Forms.Panel();
            this.Area1Button = new System.Windows.Forms.Button();
            this.Area2Button = new System.Windows.Forms.Button();
            this.Area3Button = new System.Windows.Forms.Button();
            this.MainPanel = new System.Windows.Forms.Panel();
            this.StationLabel = new System.Windows.Forms.Label();
            this.OtherLabel = new System.Windows.Forms.Label();
            this.ReadingsPanel = new System.Windows.Forms.Panel();
            this.TemperatureLabel = new System.Windows.Forms.Label();
            this.WindLabel = new System.Windows.Forms.Label();
            this.WaveLabel = new System.Windows.Forms.Label();
            this.ForecastLabel = new System.Windows.Forms.Label();
            this.LeftPanel = new System.Windows.Forms.Panel();
            this.TimeLabel = new System.Windows.Forms.Label();
            this.UpdateTextBox = new System.Windows.Forms.TextBox();
            this.ChooseButton = new System.Windows.Forms.Button();
            this.RightPanel = new System.Windows.Forms.Panel();
            this.DateTimeTimer = new System.Windows.Forms.Timer(this.components);
            this.TemperaturePanel = new System.Windows.Forms.Panel();
            this.WindPanel = new System.Windows.Forms.Panel();
            this.WavePanel = new System.Windows.Forms.Panel();
            this.ForecastPanel = new System.Windows.Forms.Panel();
            this.MapPanel.SuspendLayout();
            this.MainPanel.SuspendLayout();
            this.ReadingsPanel.SuspendLayout();
            this.LeftPanel.SuspendLayout();
            this.RightPanel.SuspendLayout();
            this.TemperaturePanel.SuspendLayout();
            this.WindPanel.SuspendLayout();
            this.WavePanel.SuspendLayout();
            this.ForecastPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // MapPanel
            // 
            this.MapPanel.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("MapPanel.BackgroundImage")));
            this.MapPanel.Controls.Add(this.Area1Button);
            this.MapPanel.Controls.Add(this.Area2Button);
            this.MapPanel.Controls.Add(this.Area3Button);
            this.MapPanel.Location = new System.Drawing.Point(0, 0);
            this.MapPanel.Name = "MapPanel";
            this.MapPanel.Size = new System.Drawing.Size(640, 480);
            this.MapPanel.TabIndex = 1;
            this.MapPanel.Visible = false;
            // 
            // Area1Button
            // 
            this.Area1Button.BackColor = System.Drawing.Color.Transparent;
            this.Area1Button.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.Area1Button.Font = new System.Drawing.Font("Microsoft Sans Serif", 30F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Area1Button.Location = new System.Drawing.Point(0, 0);
            this.Area1Button.Name = "Area1Button";
            this.Area1Button.Size = new System.Drawing.Size(640, 160);
            this.Area1Button.TabIndex = 0;
            this.Area1Button.Text = "Baltimore";
            this.Area1Button.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.Area1Button.UseVisualStyleBackColor = false;
            this.Area1Button.Click += new System.EventHandler(this.AreaButton_Click);
            // 
            // Area2Button
            // 
            this.Area2Button.BackColor = System.Drawing.Color.Transparent;
            this.Area2Button.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.Area2Button.Font = new System.Drawing.Font("Microsoft Sans Serif", 30F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Area2Button.ForeColor = System.Drawing.SystemColors.ControlText;
            this.Area2Button.Location = new System.Drawing.Point(0, 160);
            this.Area2Button.Name = "Area2Button";
            this.Area2Button.Size = new System.Drawing.Size(640, 160);
            this.Area2Button.TabIndex = 1;
            this.Area2Button.Text = "Sandy Point";
            this.Area2Button.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.Area2Button.UseVisualStyleBackColor = false;
            this.Area2Button.Click += new System.EventHandler(this.AreaButton_Click);
            // 
            // Area3Button
            // 
            this.Area3Button.BackColor = System.Drawing.Color.Transparent;
            this.Area3Button.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.Area3Button.Font = new System.Drawing.Font("Microsoft Sans Serif", 30F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Area3Button.Location = new System.Drawing.Point(0, 320);
            this.Area3Button.Name = "Area3Button";
            this.Area3Button.Size = new System.Drawing.Size(640, 160);
            this.Area3Button.TabIndex = 2;
            this.Area3Button.Text = "North Beach";
            this.Area3Button.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.Area3Button.UseVisualStyleBackColor = false;
            this.Area3Button.Click += new System.EventHandler(this.AreaButton_Click);
            // 
            // MainPanel
            // 
            this.MainPanel.Controls.Add(this.StationLabel);
            this.MainPanel.Controls.Add(this.OtherLabel);
            this.MainPanel.Controls.Add(this.ReadingsPanel);
            this.MainPanel.Location = new System.Drawing.Point(0, 0);
            this.MainPanel.Name = "MainPanel";
            this.MainPanel.Size = new System.Drawing.Size(640, 480);
            this.MainPanel.TabIndex = 0;
            // 
            // StationLabel
            // 
            this.StationLabel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.StationLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.StationLabel.Location = new System.Drawing.Point(0, 0);
            this.StationLabel.Name = "StationLabel";
            this.StationLabel.Size = new System.Drawing.Size(640, 70);
            this.StationLabel.TabIndex = 0;
            this.StationLabel.Text = "Boating Station: Father\'s Day 2022";
            this.StationLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // OtherLabel
            // 
            this.OtherLabel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.OtherLabel.Location = new System.Drawing.Point(0, 410);
            this.OtherLabel.Name = "OtherLabel";
            this.OtherLabel.Size = new System.Drawing.Size(640, 70);
            this.OtherLabel.TabIndex = 1;
            this.OtherLabel.Text = resources.GetString("OtherLabel.Text");
            this.OtherLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // ReadingsPanel
            // 
            this.ReadingsPanel.Controls.Add(this.TemperaturePanel);
            this.ReadingsPanel.Controls.Add(this.WindPanel);
            this.ReadingsPanel.Controls.Add(this.WavePanel);
            this.ReadingsPanel.Controls.Add(this.ForecastPanel);
            this.ReadingsPanel.Location = new System.Drawing.Point(0, 70);
            this.ReadingsPanel.Margin = new System.Windows.Forms.Padding(0);
            this.ReadingsPanel.Name = "ReadingsPanel";
            this.ReadingsPanel.Size = new System.Drawing.Size(640, 340);
            this.ReadingsPanel.TabIndex = 2;
            // 
            // TemperatureLabel
            // 
            this.TemperatureLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TemperatureLabel.Location = new System.Drawing.Point(0, 0);
            this.TemperatureLabel.Margin = new System.Windows.Forms.Padding(0);
            this.TemperatureLabel.Name = "TemperatureLabel";
            this.TemperatureLabel.Size = new System.Drawing.Size(320, 170);
            this.TemperatureLabel.TabIndex = 0;
            this.TemperatureLabel.Text = "Temperature";
            this.TemperatureLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // WindLabel
            // 
            this.WindLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.WindLabel.Location = new System.Drawing.Point(0, 0);
            this.WindLabel.Margin = new System.Windows.Forms.Padding(0);
            this.WindLabel.Name = "WindLabel";
            this.WindLabel.Size = new System.Drawing.Size(320, 170);
            this.WindLabel.TabIndex = 1;
            this.WindLabel.Text = "Wind Speed\r\nDirection";
            this.WindLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // WaveLabel
            // 
            this.WaveLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.WaveLabel.Location = new System.Drawing.Point(0, 0);
            this.WaveLabel.Margin = new System.Windows.Forms.Padding(0);
            this.WaveLabel.Name = "WaveLabel";
            this.WaveLabel.Size = new System.Drawing.Size(320, 170);
            this.WaveLabel.TabIndex = 2;
            this.WaveLabel.Text = "Wave Height";
            this.WaveLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // ForecastLabel
            // 
            this.ForecastLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ForecastLabel.Location = new System.Drawing.Point(0, 0);
            this.ForecastLabel.Margin = new System.Windows.Forms.Padding(0);
            this.ForecastLabel.Name = "ForecastLabel";
            this.ForecastLabel.Size = new System.Drawing.Size(320, 170);
            this.ForecastLabel.TabIndex = 3;
            this.ForecastLabel.Text = "Forecast";
            this.ForecastLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // LeftPanel
            // 
            this.LeftPanel.Controls.Add(this.TimeLabel);
            this.LeftPanel.Controls.Add(this.UpdateTextBox);
            this.LeftPanel.Controls.Add(this.ChooseButton);
            this.LeftPanel.Location = new System.Drawing.Point(0, 0);
            this.LeftPanel.Name = "LeftPanel";
            this.LeftPanel.Size = new System.Drawing.Size(160, 480);
            this.LeftPanel.TabIndex = 0;
            // 
            // TimeLabel
            // 
            this.TimeLabel.BackColor = System.Drawing.Color.White;
            this.TimeLabel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.TimeLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TimeLabel.Location = new System.Drawing.Point(0, 0);
            this.TimeLabel.Margin = new System.Windows.Forms.Padding(0);
            this.TimeLabel.Name = "TimeLabel";
            this.TimeLabel.Size = new System.Drawing.Size(160, 70);
            this.TimeLabel.TabIndex = 0;
            this.TimeLabel.Text = "06/12/2022\r\n00:00:00";
            this.TimeLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // UpdateTextBox
            // 
            this.UpdateTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.UpdateTextBox.Location = new System.Drawing.Point(0, 70);
            this.UpdateTextBox.Margin = new System.Windows.Forms.Padding(0);
            this.UpdateTextBox.Multiline = true;
            this.UpdateTextBox.Name = "UpdateTextBox";
            this.UpdateTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.UpdateTextBox.Size = new System.Drawing.Size(160, 340);
            this.UpdateTextBox.TabIndex = 1;
            this.UpdateTextBox.Text = resources.GetString("UpdateTextBox.Text");
            // 
            // ChooseButton
            // 
            this.ChooseButton.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.ChooseButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ChooseButton.Location = new System.Drawing.Point(0, 410);
            this.ChooseButton.Margin = new System.Windows.Forms.Padding(0);
            this.ChooseButton.Name = "ChooseButton";
            this.ChooseButton.Size = new System.Drawing.Size(160, 70);
            this.ChooseButton.TabIndex = 2;
            this.ChooseButton.Text = "Choose";
            this.ChooseButton.UseVisualStyleBackColor = false;
            this.ChooseButton.Click += new System.EventHandler(this.ChooseButton_Click);
            // 
            // RightPanel
            // 
            this.RightPanel.Controls.Add(this.MainPanel);
            this.RightPanel.Controls.Add(this.MapPanel);
            this.RightPanel.Location = new System.Drawing.Point(160, 0);
            this.RightPanel.Name = "RightPanel";
            this.RightPanel.Size = new System.Drawing.Size(640, 480);
            this.RightPanel.TabIndex = 1;
            // 
            // DateTimeTimer
            // 
            this.DateTimeTimer.Enabled = true;
            this.DateTimeTimer.Interval = 1000;
            this.DateTimeTimer.Tick += new System.EventHandler(this.DateTimeTimer_Tick);
            // 
            // TemperaturePanel
            // 
            this.TemperaturePanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.TemperaturePanel.Controls.Add(this.TemperatureLabel);
            this.TemperaturePanel.Location = new System.Drawing.Point(0, 0);
            this.TemperaturePanel.Name = "TemperaturePanel";
            this.TemperaturePanel.Size = new System.Drawing.Size(320, 170);
            this.TemperaturePanel.TabIndex = 0;
            // 
            // WindPanel
            // 
            this.WindPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.WindPanel.Controls.Add(this.WindLabel);
            this.WindPanel.Location = new System.Drawing.Point(320, 0);
            this.WindPanel.Name = "WindPanel";
            this.WindPanel.Size = new System.Drawing.Size(320, 170);
            this.WindPanel.TabIndex = 1;
            // 
            // WavePanel
            // 
            this.WavePanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.WavePanel.Controls.Add(this.WaveLabel);
            this.WavePanel.Location = new System.Drawing.Point(0, 170);
            this.WavePanel.Name = "WavePanel";
            this.WavePanel.Size = new System.Drawing.Size(320, 170);
            this.WavePanel.TabIndex = 2;
            // 
            // ForecastPanel
            // 
            this.ForecastPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.ForecastPanel.Controls.Add(this.ForecastLabel);
            this.ForecastPanel.Location = new System.Drawing.Point(320, 170);
            this.ForecastPanel.Name = "ForecastPanel";
            this.ForecastPanel.Size = new System.Drawing.Size(320, 170);
            this.ForecastPanel.TabIndex = 3;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 480);
            this.ControlBox = false;
            this.Controls.Add(this.RightPanel);
            this.Controls.Add(this.LeftPanel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "MainForm";
            this.Text = "Main";
            this.MapPanel.ResumeLayout(false);
            this.MainPanel.ResumeLayout(false);
            this.ReadingsPanel.ResumeLayout(false);
            this.LeftPanel.ResumeLayout(false);
            this.LeftPanel.PerformLayout();
            this.RightPanel.ResumeLayout(false);
            this.TemperaturePanel.ResumeLayout(false);
            this.WindPanel.ResumeLayout(false);
            this.WavePanel.ResumeLayout(false);
            this.ForecastPanel.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel MapPanel;
        private System.Windows.Forms.Panel MainPanel;
        private System.Windows.Forms.Panel LeftPanel;
        private System.Windows.Forms.TextBox UpdateTextBox;
        private System.Windows.Forms.Button ChooseButton;
        private System.Windows.Forms.Button Area1Button;
        private System.Windows.Forms.Button Area2Button;
        private System.Windows.Forms.Button Area3Button;
        private System.Windows.Forms.Panel RightPanel;
        private System.Windows.Forms.Label StationLabel;
        private System.Windows.Forms.Label OtherLabel;
        private System.Windows.Forms.Label TimeLabel;
        private System.Windows.Forms.Panel ReadingsPanel;
        private System.Windows.Forms.Label TemperatureLabel;
        private System.Windows.Forms.Label WindLabel;
        private System.Windows.Forms.Label WaveLabel;
        private System.Windows.Forms.Label ForecastLabel;
        private System.Windows.Forms.Timer DateTimeTimer;
        private System.Windows.Forms.Panel TemperaturePanel;
        private System.Windows.Forms.Panel WindPanel;
        private System.Windows.Forms.Panel WavePanel;
        private System.Windows.Forms.Panel ForecastPanel;
    }
}

