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
            UpdaterInfoPanel = new Panel();
            ProgramPathTextBox = new TextBox();
            UpdatePathLabel = new Label();
            UpdateIatestVerL = new Label();
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
            UpdaterInfoPanel.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            UpdaterInfoPanel.BorderStyle = BorderStyle.FixedSingle;
            UpdaterInfoPanel.Controls.Add(ProgramPathTextBox);
            UpdaterInfoPanel.Controls.Add(UpdatePathLabel);
            UpdaterInfoPanel.Controls.Add(UpdateIatestVerL);
            UpdaterInfoPanel.Controls.Add(UpdateIatestVerLabel);
            UpdaterInfoPanel.Controls.Add(UpdateInstalledVerL);
            UpdaterInfoPanel.Controls.Add(UpdateInstalledVerLabel);
            UpdaterInfoPanel.Location = new Point(26, 27);
            UpdaterInfoPanel.Name = "UpdaterInfoPanel";
            UpdaterInfoPanel.Size = new Size(627, 141);
            UpdaterInfoPanel.TabIndex = 0;
            // 
            // ProgramPathTextBox
            // 
            ProgramPathTextBox.BackColor = SystemColors.Control;
            ProgramPathTextBox.BorderStyle = BorderStyle.FixedSingle;
            ProgramPathTextBox.Location = new Point(173, 89);
            ProgramPathTextBox.Name = "ProgramPathTextBox";
            ProgramPathTextBox.ReadOnly = true;
            ProgramPathTextBox.Size = new Size(449, 23);
            ProgramPathTextBox.TabIndex = 5;
            // 
            // UpdatePathLabel
            // 
            UpdatePathLabel.AutoSize = true;
            UpdatePathLabel.Font = new Font("Segoe UI Semibold", 9.75F, FontStyle.Bold, GraphicsUnit.Point);
            UpdatePathLabel.Location = new Point(16, 95);
            UpdatePathLabel.Name = "UpdatePathLabel";
            UpdatePathLabel.Size = new Size(103, 17);
            UpdatePathLabel.TabIndex = 4;
            UpdatePathLabel.Text = "Расположение:";
            // 
            // UpdateIatestVerL
            // 
            UpdateIatestVerL.AutoSize = true;
            UpdateIatestVerL.Font = new Font("Segoe UI Semibold", 9.75F, FontStyle.Bold, GraphicsUnit.Point);
            UpdateIatestVerL.Location = new Point(173, 51);
            UpdateIatestVerL.Name = "UpdateIatestVerL";
            UpdateIatestVerL.Size = new Size(21, 17);
            UpdateIatestVerL.TabIndex = 3;
            UpdateIatestVerL.Text = "—";
            // 
            // UpdateIatestVerLabel
            // 
            UpdateIatestVerLabel.AutoSize = true;
            UpdateIatestVerLabel.Font = new Font("Segoe UI Semibold", 9.75F, FontStyle.Bold, GraphicsUnit.Point);
            UpdateIatestVerLabel.Location = new Point(16, 51);
            UpdateIatestVerLabel.Name = "UpdateIatestVerLabel";
            UpdateIatestVerLabel.Size = new Size(126, 17);
            UpdateIatestVerLabel.TabIndex = 2;
            UpdateIatestVerLabel.Text = "Последняя версия:";
            // 
            // UpdateInstalledVerL
            // 
            UpdateInstalledVerL.AutoSize = true;
            UpdateInstalledVerL.Font = new Font("Segoe UI Semibold", 9.75F, FontStyle.Bold, GraphicsUnit.Point);
            UpdateInstalledVerL.Location = new Point(173, 22);
            UpdateInstalledVerL.Name = "UpdateInstalledVerL";
            UpdateInstalledVerL.Size = new Size(21, 17);
            UpdateInstalledVerL.TabIndex = 1;
            UpdateInstalledVerL.Text = "—";
            // 
            // UpdateInstalledVerLabel
            // 
            UpdateInstalledVerLabel.AutoSize = true;
            UpdateInstalledVerLabel.Font = new Font("Segoe UI Semibold", 9.75F, FontStyle.Bold, GraphicsUnit.Point);
            UpdateInstalledVerLabel.Location = new Point(16, 22);
            UpdateInstalledVerLabel.Name = "UpdateInstalledVerLabel";
            UpdateInstalledVerLabel.Size = new Size(151, 17);
            UpdateInstalledVerLabel.TabIndex = 0;
            UpdateInstalledVerLabel.Text = "Установленная версия:";
            // 
            // updateStatusStrip
            // 
            updateStatusStrip.Items.AddRange(new ToolStripItem[] { UpdateStatusLabel, UpdateStatusProgressBar });
            updateStatusStrip.Location = new Point(0, 454);
            updateStatusStrip.Name = "updateStatusStrip";
            updateStatusStrip.ShowItemToolTips = true;
            updateStatusStrip.Size = new Size(681, 22);
            updateStatusStrip.TabIndex = 1;
            // 
            // UpdateStatusLabel
            // 
            UpdateStatusLabel.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold, GraphicsUnit.Point);
            UpdateStatusLabel.Name = "UpdateStatusLabel";
            UpdateStatusLabel.Size = new Size(45, 17);
            UpdateStatusLabel.Text = "Готово";
            UpdateStatusLabel.ToolTipText = "Фоновые задачи не выполняются.";
            // 
            // UpdateStatusProgressBar
            // 
            UpdateStatusProgressBar.Name = "UpdateStatusProgressBar";
            UpdateStatusProgressBar.Size = new Size(100, 16);
            UpdateStatusProgressBar.Style = ProgressBarStyle.Continuous;
            // 
            // UpdateInfoTextBox
            // 
            UpdateInfoTextBox.Font = new Font("Segoe UI Semibold", 9.75F, FontStyle.Bold, GraphicsUnit.Point);
            UpdateInfoTextBox.ImeMode = ImeMode.Off;
            UpdateInfoTextBox.Location = new Point(26, 216);
            UpdateInfoTextBox.Multiline = true;
            UpdateInfoTextBox.Name = "UpdateInfoTextBox";
            UpdateInfoTextBox.PlaceholderText = "Здесь будут отображаться изменения в новой версии";
            UpdateInfoTextBox.ReadOnly = true;
            UpdateInfoTextBox.ScrollBars = ScrollBars.Vertical;
            UpdateInfoTextBox.Size = new Size(627, 196);
            UpdateInfoTextBox.TabIndex = 2;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI Semibold", 11.25F, FontStyle.Bold, GraphicsUnit.Point);
            label1.Location = new Point(26, 180);
            label1.Name = "label1";
            label1.Size = new Size(92, 20);
            label1.TabIndex = 3;
            label1.Text = "Что нового:";
            // 
            // CheckForUpdatesBtn
            // 
            CheckForUpdatesBtn.Font = new Font("Segoe UI Semibold", 9.75F, FontStyle.Bold, GraphicsUnit.Point);
            CheckForUpdatesBtn.Location = new Point(26, 418);
            CheckForUpdatesBtn.Name = "CheckForUpdatesBtn";
            CheckForUpdatesBtn.Size = new Size(245, 28);
            CheckForUpdatesBtn.TabIndex = 4;
            CheckForUpdatesBtn.Text = "Проверить наличие обновлений";
            CheckForUpdatesBtn.UseVisualStyleBackColor = true;
            CheckForUpdatesBtn.Click += CheckForUpdates;
            // 
            // InstallUpdateManuallyBtn
            // 
            InstallUpdateManuallyBtn.Font = new Font("Segoe UI Semibold", 9.75F, FontStyle.Bold, GraphicsUnit.Point);
            InstallUpdateManuallyBtn.Location = new Point(408, 418);
            InstallUpdateManuallyBtn.Name = "InstallUpdateManuallyBtn";
            InstallUpdateManuallyBtn.Size = new Size(245, 28);
            InstallUpdateManuallyBtn.TabIndex = 5;
            InstallUpdateManuallyBtn.Text = "Установить обновление вручную";
            InstallUpdateManuallyBtn.UseVisualStyleBackColor = true;
            InstallUpdateManuallyBtn.Click += InstallUpdateManually;
            // 
            // FormUpdaterUI
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(681, 476);
            Controls.Add(InstallUpdateManuallyBtn);
            Controls.Add(CheckForUpdatesBtn);
            Controls.Add(label1);
            Controls.Add(UpdateInfoTextBox);
            Controls.Add(updateStatusStrip);
            Controls.Add(UpdaterInfoPanel);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            Name = "FormUpdaterUI";
            Text = "Обновить программу";
            TopMost = true;
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
        internal Label UpdateIatestVerL;
        private Button CheckForUpdatesBtn;
        private Button InstallUpdateManuallyBtn;
        internal ToolStripProgressBar UpdateStatusProgressBar;
        private TextBox ProgramPathTextBox;
    }
}