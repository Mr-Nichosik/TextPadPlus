namespace TextPad_
{
    partial class FormPythonInterpreterUI
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormPythonInterpreterUI));
            AutoSearchRadioButton = new RadioButton();
            ManuallySearchRadioButton = new RadioButton();
            label1 = new Label();
            PathTextBox = new TextBox();
            BrowseBtn = new Button();
            DoneBtn = new Button();
            openFileDialog = new OpenFileDialog();
            SuspendLayout();
            // 
            // AutoSearchRadioButton
            // 
            resources.ApplyResources(AutoSearchRadioButton, "AutoSearchRadioButton");
            AutoSearchRadioButton.Name = "AutoSearchRadioButton";
            AutoSearchRadioButton.UseVisualStyleBackColor = true;
            AutoSearchRadioButton.CheckedChanged += RadioButtonCheckedChanged;
            // 
            // ManuallySearchRadioButton
            // 
            resources.ApplyResources(ManuallySearchRadioButton, "ManuallySearchRadioButton");
            ManuallySearchRadioButton.Name = "ManuallySearchRadioButton";
            ManuallySearchRadioButton.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            resources.ApplyResources(label1, "label1");
            label1.Name = "label1";
            // 
            // PathTextBox
            // 
            resources.ApplyResources(PathTextBox, "PathTextBox");
            PathTextBox.Name = "PathTextBox";
            // 
            // BrowseBtn
            // 
            resources.ApplyResources(BrowseBtn, "BrowseBtn");
            BrowseBtn.Name = "BrowseBtn";
            BrowseBtn.UseVisualStyleBackColor = true;
            BrowseBtn.Click += Browse;
            // 
            // DoneBtn
            // 
            resources.ApplyResources(DoneBtn, "DoneBtn");
            DoneBtn.Name = "DoneBtn";
            DoneBtn.UseVisualStyleBackColor = true;
            DoneBtn.Click += Start;
            // 
            // openFileDialog
            // 
            resources.ApplyResources(openFileDialog, "openFileDialog");
            // 
            // FormPythonInterpreterUI
            // 
            resources.ApplyResources(this, "$this");
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(DoneBtn);
            Controls.Add(BrowseBtn);
            Controls.Add(PathTextBox);
            Controls.Add(label1);
            Controls.Add(ManuallySearchRadioButton);
            Controls.Add(AutoSearchRadioButton);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "FormPythonInterpreterUI";
            ShowInTaskbar = false;
            Load += FormPythonInterpreterUILoad;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private RadioButton AutoSearchRadioButton;
        private RadioButton ManuallySearchRadioButton;
        private Label label1;
        private TextBox PathTextBox;
        private Button BrowseBtn;
        private Button DoneBtn;
        private OpenFileDialog openFileDialog;
    }
}