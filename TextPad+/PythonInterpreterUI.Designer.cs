namespace TextPad_
{
    internal sealed partial class PythonInterpreterUI
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PythonInterpreterUI));
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
            // PythonInterpreterUI
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = AutoScaleMode.Font;
            this.Controls.Add(DoneBtn);
            this.Controls.Add(BrowseBtn);
            this.Controls.Add(PathTextBox);
            this.Controls.Add(label1);
            this.Controls.Add(ManuallySearchRadioButton);
            this.Controls.Add(AutoSearchRadioButton);
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "PythonInterpreterUI";
            this.ShowInTaskbar = false;
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