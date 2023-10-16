namespace TextPad_
{
    partial class FormUpdaterUI
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormUpdaterUI));
            this.UpdaterInfoPanel = new System.Windows.Forms.Panel();
            this.ProgramPathTextBox = new System.Windows.Forms.TextBox();
            this.UpdatePathLabel = new System.Windows.Forms.Label();
            this.UpdateIatestVerL = new System.Windows.Forms.Label();
            this.UpdateIatestVerLabel = new System.Windows.Forms.Label();
            this.UpdateInstalledVerL = new System.Windows.Forms.Label();
            this.UpdateInstalledVerLabel = new System.Windows.Forms.Label();
            this.updateStatusStrip = new System.Windows.Forms.StatusStrip();
            this.UpdateStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.UpdateStatusProgressBar = new System.Windows.Forms.ToolStripProgressBar();
            this.UpdateInfoTextBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.CheckForUpdatesBtn = new System.Windows.Forms.Button();
            this.InstallUpdateManuallyBtn = new System.Windows.Forms.Button();
            this.UpdaterInfoPanel.SuspendLayout();
            this.updateStatusStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // UpdaterInfoPanel
            // 
            this.UpdaterInfoPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.UpdaterInfoPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.UpdaterInfoPanel.Controls.Add(this.ProgramPathTextBox);
            this.UpdaterInfoPanel.Controls.Add(this.UpdatePathLabel);
            this.UpdaterInfoPanel.Controls.Add(this.UpdateIatestVerL);
            this.UpdaterInfoPanel.Controls.Add(this.UpdateIatestVerLabel);
            this.UpdaterInfoPanel.Controls.Add(this.UpdateInstalledVerL);
            this.UpdaterInfoPanel.Controls.Add(this.UpdateInstalledVerLabel);
            this.UpdaterInfoPanel.Location = new System.Drawing.Point(26, 27);
            this.UpdaterInfoPanel.Name = "UpdaterInfoPanel";
            this.UpdaterInfoPanel.Size = new System.Drawing.Size(627, 141);
            this.UpdaterInfoPanel.TabIndex = 0;
            // 
            // ProgramPathTextBox
            // 
            this.ProgramPathTextBox.BackColor = System.Drawing.SystemColors.Control;
            this.ProgramPathTextBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.ProgramPathTextBox.Location = new System.Drawing.Point(173, 89);
            this.ProgramPathTextBox.Name = "ProgramPathTextBox";
            this.ProgramPathTextBox.ReadOnly = true;
            this.ProgramPathTextBox.Size = new System.Drawing.Size(449, 23);
            this.ProgramPathTextBox.TabIndex = 5;
            // 
            // UpdatePathLabel
            // 
            this.UpdatePathLabel.AutoSize = true;
            this.UpdatePathLabel.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.UpdatePathLabel.Location = new System.Drawing.Point(16, 95);
            this.UpdatePathLabel.Name = "UpdatePathLabel";
            this.UpdatePathLabel.Size = new System.Drawing.Size(103, 17);
            this.UpdatePathLabel.TabIndex = 4;
            this.UpdatePathLabel.Text = "Расположение:";
            // 
            // UpdateIatestVerL
            // 
            this.UpdateIatestVerL.AutoSize = true;
            this.UpdateIatestVerL.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.UpdateIatestVerL.Location = new System.Drawing.Point(173, 51);
            this.UpdateIatestVerL.Name = "UpdateIatestVerL";
            this.UpdateIatestVerL.Size = new System.Drawing.Size(21, 17);
            this.UpdateIatestVerL.TabIndex = 3;
            this.UpdateIatestVerL.Text = "—";
            // 
            // UpdateIatestVerLabel
            // 
            this.UpdateIatestVerLabel.AutoSize = true;
            this.UpdateIatestVerLabel.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.UpdateIatestVerLabel.Location = new System.Drawing.Point(16, 51);
            this.UpdateIatestVerLabel.Name = "UpdateIatestVerLabel";
            this.UpdateIatestVerLabel.Size = new System.Drawing.Size(126, 17);
            this.UpdateIatestVerLabel.TabIndex = 2;
            this.UpdateIatestVerLabel.Text = "Последняя версия:";
            // 
            // UpdateInstalledVerL
            // 
            this.UpdateInstalledVerL.AutoSize = true;
            this.UpdateInstalledVerL.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.UpdateInstalledVerL.Location = new System.Drawing.Point(173, 22);
            this.UpdateInstalledVerL.Name = "UpdateInstalledVerL";
            this.UpdateInstalledVerL.Size = new System.Drawing.Size(21, 17);
            this.UpdateInstalledVerL.TabIndex = 1;
            this.UpdateInstalledVerL.Text = "—";
            // 
            // UpdateInstalledVerLabel
            // 
            this.UpdateInstalledVerLabel.AutoSize = true;
            this.UpdateInstalledVerLabel.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.UpdateInstalledVerLabel.Location = new System.Drawing.Point(16, 22);
            this.UpdateInstalledVerLabel.Name = "UpdateInstalledVerLabel";
            this.UpdateInstalledVerLabel.Size = new System.Drawing.Size(151, 17);
            this.UpdateInstalledVerLabel.TabIndex = 0;
            this.UpdateInstalledVerLabel.Text = "Установленная версия:";
            // 
            // updateStatusStrip
            // 
            this.updateStatusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.UpdateStatusLabel,
            this.UpdateStatusProgressBar});
            this.updateStatusStrip.Location = new System.Drawing.Point(0, 454);
            this.updateStatusStrip.Name = "updateStatusStrip";
            this.updateStatusStrip.ShowItemToolTips = true;
            this.updateStatusStrip.Size = new System.Drawing.Size(681, 22);
            this.updateStatusStrip.TabIndex = 1;
            // 
            // UpdateStatusLabel
            // 
            this.UpdateStatusLabel.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.UpdateStatusLabel.Name = "UpdateStatusLabel";
            this.UpdateStatusLabel.Size = new System.Drawing.Size(45, 17);
            this.UpdateStatusLabel.Text = "Готово";
            this.UpdateStatusLabel.ToolTipText = "Фоновые задачи не выполняются.";
            // 
            // UpdateStatusProgressBar
            // 
            this.UpdateStatusProgressBar.Name = "UpdateStatusProgressBar";
            this.UpdateStatusProgressBar.Size = new System.Drawing.Size(100, 16);
            this.UpdateStatusProgressBar.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            // 
            // UpdateInfoTextBox
            // 
            this.UpdateInfoTextBox.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.UpdateInfoTextBox.ImeMode = System.Windows.Forms.ImeMode.Off;
            this.UpdateInfoTextBox.Location = new System.Drawing.Point(26, 216);
            this.UpdateInfoTextBox.Multiline = true;
            this.UpdateInfoTextBox.Name = "UpdateInfoTextBox";
            this.UpdateInfoTextBox.PlaceholderText = "Здесь будут отображаться изменения в новой версии";
            this.UpdateInfoTextBox.ReadOnly = true;
            this.UpdateInfoTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.UpdateInfoTextBox.Size = new System.Drawing.Size(627, 196);
            this.UpdateInfoTextBox.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI Semibold", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.label1.Location = new System.Drawing.Point(26, 180);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(92, 20);
            this.label1.TabIndex = 3;
            this.label1.Text = "Что нового:";
            // 
            // CheckForUpdatesBtn
            // 
            this.CheckForUpdatesBtn.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.CheckForUpdatesBtn.Location = new System.Drawing.Point(26, 418);
            this.CheckForUpdatesBtn.Name = "CheckForUpdatesBtn";
            this.CheckForUpdatesBtn.Size = new System.Drawing.Size(245, 28);
            this.CheckForUpdatesBtn.TabIndex = 4;
            this.CheckForUpdatesBtn.Text = "Проверить наличие обновлений";
            this.CheckForUpdatesBtn.UseVisualStyleBackColor = true;
            this.CheckForUpdatesBtn.Click += new System.EventHandler(this.CheckForUpdates);
            // 
            // InstallUpdateManuallyBtn
            // 
            this.InstallUpdateManuallyBtn.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.InstallUpdateManuallyBtn.Location = new System.Drawing.Point(408, 418);
            this.InstallUpdateManuallyBtn.Name = "InstallUpdateManuallyBtn";
            this.InstallUpdateManuallyBtn.Size = new System.Drawing.Size(245, 28);
            this.InstallUpdateManuallyBtn.TabIndex = 5;
            this.InstallUpdateManuallyBtn.Text = "Установить обновление вручную";
            this.InstallUpdateManuallyBtn.UseVisualStyleBackColor = true;
            this.InstallUpdateManuallyBtn.Click += new System.EventHandler(this.InstallUpdateManually);
            // 
            // FormUpdaterUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(681, 476);
            this.Controls.Add(this.InstallUpdateManuallyBtn);
            this.Controls.Add(this.CheckForUpdatesBtn);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.UpdateInfoTextBox);
            this.Controls.Add(this.updateStatusStrip);
            this.Controls.Add(this.UpdaterInfoPanel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "FormUpdaterUI";
            this.Text = "Обновить программу";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.FormUpdaterUILoad);
            this.UpdaterInfoPanel.ResumeLayout(false);
            this.UpdaterInfoPanel.PerformLayout();
            this.updateStatusStrip.ResumeLayout(false);
            this.updateStatusStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

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
        internal Label UpdateIatestVerL;
        private Button CheckForUpdatesBtn;
        private Button InstallUpdateManuallyBtn;
        internal ToolStripProgressBar UpdateStatusProgressBar;
        private TextBox ProgramPathTextBox;
    }
}