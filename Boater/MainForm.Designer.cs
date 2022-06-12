
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.MapPanel = new System.Windows.Forms.Panel();
            this.MainPanel = new System.Windows.Forms.Panel();
            this.LeftPanel = new System.Windows.Forms.Panel();
            this.TimeTextBox = new System.Windows.Forms.TextBox();
            this.UpdateTextBox = new System.Windows.Forms.TextBox();
            this.ChooseButton = new System.Windows.Forms.Button();
            this.Area1Button = new System.Windows.Forms.Button();
            this.Area2Button = new System.Windows.Forms.Button();
            this.Area3Button = new System.Windows.Forms.Button();
            this.MapPanel.SuspendLayout();
            this.LeftPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // MapPanel
            // 
            this.MapPanel.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("MapPanel.BackgroundImage")));
            this.MapPanel.Controls.Add(this.Area1Button);
            this.MapPanel.Controls.Add(this.Area2Button);
            this.MapPanel.Controls.Add(this.Area3Button);
            this.MapPanel.Location = new System.Drawing.Point(160, 0);
            this.MapPanel.Name = "MapPanel";
            this.MapPanel.Size = new System.Drawing.Size(640, 480);
            this.MapPanel.TabIndex = 0;
            // 
            // MainPanel
            // 
            this.MainPanel.Location = new System.Drawing.Point(160, 0);
            this.MainPanel.Name = "MainPanel";
            this.MainPanel.Size = new System.Drawing.Size(640, 480);
            this.MainPanel.TabIndex = 0;
            this.MainPanel.Visible = false;
            // 
            // LeftPanel
            // 
            this.LeftPanel.Controls.Add(this.TimeTextBox);
            this.LeftPanel.Controls.Add(this.UpdateTextBox);
            this.LeftPanel.Controls.Add(this.ChooseButton);
            this.LeftPanel.Location = new System.Drawing.Point(0, 0);
            this.LeftPanel.Name = "LeftPanel";
            this.LeftPanel.Size = new System.Drawing.Size(160, 480);
            this.LeftPanel.TabIndex = 1;
            // 
            // TimeTextBox
            // 
            this.TimeTextBox.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.TimeTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TimeTextBox.Location = new System.Drawing.Point(0, 0);
            this.TimeTextBox.Margin = new System.Windows.Forms.Padding(0);
            this.TimeTextBox.Multiline = true;
            this.TimeTextBox.Name = "TimeTextBox";
            this.TimeTextBox.ReadOnly = true;
            this.TimeTextBox.Size = new System.Drawing.Size(160, 71);
            this.TimeTextBox.TabIndex = 0;
            this.TimeTextBox.Text = "06/12/2022\r\n00:00:00";
            this.TimeTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // UpdateTextBox
            // 
            this.UpdateTextBox.Location = new System.Drawing.Point(0, 74);
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
            this.ChooseButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ChooseButton.Location = new System.Drawing.Point(0, 420);
            this.ChooseButton.Name = "ChooseButton";
            this.ChooseButton.Size = new System.Drawing.Size(160, 60);
            this.ChooseButton.TabIndex = 2;
            this.ChooseButton.Text = "Choose";
            this.ChooseButton.UseVisualStyleBackColor = false;
            // 
            // Area1Button
            // 
            this.Area1Button.BackColor = System.Drawing.Color.Transparent;
            this.Area1Button.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.Area1Button.Font = new System.Drawing.Font("Microsoft Sans Serif", 30F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Area1Button.Location = new System.Drawing.Point(0, 0);
            this.Area1Button.Name = "Area1Button";
            this.Area1Button.Size = new System.Drawing.Size(640, 160);
            this.Area1Button.TabIndex = 1;
            this.Area1Button.Text = "Baltimore";
            this.Area1Button.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.Area1Button.UseVisualStyleBackColor = false;
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
            this.Area2Button.TabIndex = 2;
            this.Area2Button.Text = "Sandy Point";
            this.Area2Button.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.Area2Button.UseVisualStyleBackColor = false;
            // 
            // Area3Button
            // 
            this.Area3Button.BackColor = System.Drawing.Color.Transparent;
            this.Area3Button.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.Area3Button.Font = new System.Drawing.Font("Microsoft Sans Serif", 30F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Area3Button.Location = new System.Drawing.Point(0, 320);
            this.Area3Button.Name = "Area3Button";
            this.Area3Button.Size = new System.Drawing.Size(640, 160);
            this.Area3Button.TabIndex = 3;
            this.Area3Button.Text = "North Beach";
            this.Area3Button.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.Area3Button.UseVisualStyleBackColor = false;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 480);
            this.ControlBox = false;
            this.Controls.Add(this.LeftPanel);
            this.Controls.Add(this.MapPanel);
            this.Controls.Add(this.MainPanel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "MainForm";
            this.Text = "Main";
            this.MapPanel.ResumeLayout(false);
            this.LeftPanel.ResumeLayout(false);
            this.LeftPanel.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel MapPanel;
        private System.Windows.Forms.Panel MainPanel;
        private System.Windows.Forms.Panel LeftPanel;
        private System.Windows.Forms.TextBox TimeTextBox;
        private System.Windows.Forms.TextBox UpdateTextBox;
        private System.Windows.Forms.Button ChooseButton;
        private System.Windows.Forms.Button Area1Button;
        private System.Windows.Forms.Button Area2Button;
        private System.Windows.Forms.Button Area3Button;
    }
}

