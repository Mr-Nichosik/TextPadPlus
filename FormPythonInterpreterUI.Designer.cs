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
            radioButton1 = new RadioButton();
            radioButton2 = new RadioButton();
            label1 = new Label();
            pathTextBox = new TextBox();
            browseBtn = new Button();
            doneBtn = new Button();
            openFileDialog = new OpenFileDialog();
            SuspendLayout();
            // 
            // radioButton1
            // 
            resources.ApplyResources(radioButton1, "radioButton1");
            radioButton1.Name = "radioButton1";
            radioButton1.UseVisualStyleBackColor = true;
            radioButton1.CheckedChanged += radioButtonCheckedChanged;
            // 
            // radioButton2
            // 
            resources.ApplyResources(radioButton2, "radioButton2");
            radioButton2.Name = "radioButton2";
            radioButton2.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            resources.ApplyResources(label1, "label1");
            label1.Name = "label1";
            // 
            // pathTextBox
            // 
            resources.ApplyResources(pathTextBox, "pathTextBox");
            pathTextBox.Name = "pathTextBox";
            // 
            // browseBtn
            // 
            resources.ApplyResources(browseBtn, "browseBtn");
            browseBtn.Name = "browseBtn";
            browseBtn.UseVisualStyleBackColor = true;
            browseBtn.Click += browseBtn_Click;
            // 
            // doneBtn
            // 
            resources.ApplyResources(doneBtn, "doneBtn");
            doneBtn.Name = "doneBtn";
            doneBtn.UseVisualStyleBackColor = true;
            doneBtn.Click += doneBtn_Click;
            // 
            // openFileDialog
            // 
            resources.ApplyResources(openFileDialog, "openFileDialog");
            // 
            // FormPythonInterpreterUI
            // 
            resources.ApplyResources(this, "$this");
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(doneBtn);
            Controls.Add(browseBtn);
            Controls.Add(pathTextBox);
            Controls.Add(label1);
            Controls.Add(radioButton2);
            Controls.Add(radioButton1);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "FormPythonInterpreterUI";
            ShowInTaskbar = false;
            Load += FormPythonInterpreterUI_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private RadioButton radioButton1;
        private RadioButton radioButton2;
        private Label label1;
        private TextBox pathTextBox;
        private Button browseBtn;
        private Button doneBtn;
        private OpenFileDialog openFileDialog;
    }
}