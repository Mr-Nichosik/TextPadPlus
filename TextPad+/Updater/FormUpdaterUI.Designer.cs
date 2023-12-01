namespace TextPad_.Updater
{
    partial class UpdaterUI
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UpdaterUI));
            UpdaterInfoPanel = new Panel();
            ProgramPathTextBox = new TextBox();
            UpdatePathLabel = new Label();
            UpdateLatestVerL = new Label();
            UpdateIatestVerLabel = new Label();
            UpdateInstalledVerL = new Label();
            UpdateInstalledVerLabel = new Label();
            updateStatusStrip = new StatusStrip();
            UpdateStatusLabel = new ToolStripStatusLabel();
            UpdateStatusProgressBar = new ToolStripProgressBar();
            UpdateInfoTextBox = new TextBox();
            label1 = new Label();
            CheckForUpdatesBtn = new Button();
            InstallUpdateManuallyBtn = new Button();
            UpdaterInfoPanel.SuspendLayout();
            updateStatusStrip.SuspendLayout();
            SuspendLayout();
            // 
            // UpdaterInfoPanel
            // 
            resources.ApplyResources(UpdaterInfoPanel, "UpdaterInfoPanel");
            UpdaterInfoPanel.BorderStyle = BorderStyle.FixedSingle;
            UpdaterInfoPanel.Controls.Add(ProgramPathTextBox);
            UpdaterInfoPanel.Controls.Add(UpdatePathLabel);
            UpdaterInfoPanel.Controls.Add(UpdateLatestVerL);
            UpdaterInfoPanel.Controls.Add(UpdateIatestVerLabel);
            UpdaterInfoPanel.Controls.Add(UpdateInstalledVerL);
            UpdaterInfoPanel.Controls.Add(UpdateInstalledVerLabel);
            UpdaterInfoPanel.Name = "UpdaterInfoPanel";
            // 
            // ProgramPathTextBox
            // 
            resources.ApplyResources(ProgramPathTextBox, "ProgramPathTextBox");
            ProgramPathTextBox.BackColor = SystemColors.Control;
            ProgramPathTextBox.BorderStyle = BorderStyle.FixedSingle;
            ProgramPathTextBox.Name = "ProgramPathTextBox";
            ProgramPathTextBox.ReadOnly = true;
            // 
            // UpdatePathLabel
            // 
            resources.ApplyResources(UpdatePathLabel, "UpdatePathLabel");
            UpdatePathLabel.Name = "UpdatePathLabel";
            // 
            // UpdateLatestVerL
            // 
            resources.ApplyResources(UpdateLatestVerL, "UpdateLatestVerL");
            UpdateLatestVerL.Name = "UpdateLatestVerL";
            // 
            // UpdateIatestVerLabel
            // 
            resources.ApplyResources(UpdateIatestVerLabel, "UpdateIatestVerLabel");
            UpdateIatestVerLabel.Name = "UpdateIatestVerLabel";
            // 
            // UpdateInstalledVerL
            // 
            resources.ApplyResources(UpdateInstalledVerL, "UpdateInstalledVerL");
            UpdateInstalledVerL.Name = "UpdateInstalledVerL";
            // 
            // UpdateInstalledVerLabel
            // 
            resources.ApplyResources(UpdateInstalledVerLabel, "UpdateInstalledVerLabel");
            UpdateInstalledVerLabel.Name = "UpdateInstalledVerLabel";
            // 
            // updateStatusStrip
            // 
            resources.ApplyResources(updateStatusStrip, "updateStatusStrip");
            updateStatusStrip.Items.AddRange(new ToolStripItem[] { UpdateStatusLabel, UpdateStatusProgressBar });
            updateStatusStrip.Name = "updateStatusStrip";
            updateStatusStrip.ShowItemToolTips = true;
            // 
            // UpdateStatusLabel
            // 
            resources.ApplyResources(UpdateStatusLabel, "UpdateStatusLabel");
            UpdateStatusLabel.Name = "UpdateStatusLabel";
            // 
            // UpdateStatusProgressBar
            // 
            resources.ApplyResources(UpdateStatusProgressBar, "UpdateStatusProgressBar");
            UpdateStatusProgressBar.Name = "UpdateStatusProgressBar";
            UpdateStatusProgressBar.Style = ProgressBarStyle.Continuous;
            // 
            // UpdateInfoTextBox
            // 
            resources.ApplyResources(UpdateInfoTextBox, "UpdateInfoTextBox");
            UpdateInfoTextBox.Name = "UpdateInfoTextBox";
            UpdateInfoTextBox.ReadOnly = true;
            // 
            // label1
            // 
            resources.ApplyResources(label1, "label1");
            label1.Name = "label1";
            // 
            // CheckForUpdatesBtn
            // 
            resources.ApplyResources(CheckForUpdatesBtn, "CheckForUpdatesBtn");
            CheckForUpdatesBtn.Name = "CheckForUpdatesBtn";
            CheckForUpdatesBtn.UseVisualStyleBackColor = true;
            CheckForUpdatesBtn.Click += CheckForUpdates;
            // 
            // InstallUpdateManuallyBtn
            // 
            resources.ApplyResources(InstallUpdateManuallyBtn, "InstallUpdateManuallyBtn");
            InstallUpdateManuallyBtn.Name = "InstallUpdateManuallyBtn";
            InstallUpdateManuallyBtn.UseVisualStyleBackColor = true;
            InstallUpdateManuallyBtn.Click += InstallUpdateManually;
            // 
            // UpdaterUI
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = AutoScaleMode.Font;
            this.Controls.Add(InstallUpdateManuallyBtn);
            this.Controls.Add(CheckForUpdatesBtn);
            this.Controls.Add(label1);
            this.Controls.Add(UpdateInfoTextBox);
            this.Controls.Add(updateStatusStrip);
            this.Controls.Add(UpdaterInfoPanel);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.Name = "UpdaterUI";
            this.TopMost = true;
            FormClosing += FormUpdaterUI_FormClosing;
            Load += FormUpdaterUILoad;
            UpdaterInfoPanel.ResumeLayout(false);
            UpdaterInfoPanel.PerformLayout();
            updateStatusStrip.ResumeLayout(false);
            updateStatusStrip.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Panel UpdaterInfoPanel;
        private Label UpdateInstalledVerLabel;
        private Label UpdateIatestVerLabel;
        private Label UpdatePathLabel;
        private StatusStrip updateStatusStrip;
        private Label label1;
        internal TextBox UpdateInfoTextBox;
        internal ToolStripStatusLabel UpdateStatusLabel;
        internal Label UpdateInstalledVerL;
        internal Label UpdateLatestVerL;
        private Button CheckForUpdatesBtn;
        private Button InstallUpdateManuallyBtn;
        internal ToolStripProgressBar UpdateStatusProgressBar;
        private TextBox ProgramPathTextBox;
    }
}