
namespace TextPad_
{
    partial class SearchUI
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SearchUI));
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.FindTextBox = new System.Windows.Forms.TextBox();
            this.ReplaceTextBox = new System.Windows.Forms.TextBox();
            this.SearchButton = new System.Windows.Forms.Button();
            this.ReplcaeButton = new System.Windows.Forms.Button();
            this.ReplaceAllButton = new System.Windows.Forms.Button();
            this.GoToLineBtn = new System.Windows.Forms.Button();
            this.numericLineNumber = new System.Windows.Forms.NumericUpDown();
            ((System.ComponentModel.ISupportInitialize)(this.numericLineNumber)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // FindTextBox
            // 
            resources.ApplyResources(this.FindTextBox, "FindTextBox");
            this.FindTextBox.Name = "FindTextBox";
            // 
            // ReplaceTextBox
            // 
            resources.ApplyResources(this.ReplaceTextBox, "ReplaceTextBox");
            this.ReplaceTextBox.Name = "ReplaceTextBox";
            // 
            // SearchButton
            // 
            resources.ApplyResources(this.SearchButton, "SearchButton");
            this.SearchButton.Name = "SearchButton";
            this.SearchButton.UseVisualStyleBackColor = true;
            this.SearchButton.Click += new System.EventHandler(this.SearchButton_Click);
            // 
            // ReplcaeButton
            // 
            resources.ApplyResources(this.ReplcaeButton, "ReplcaeButton");
            this.ReplcaeButton.Name = "ReplcaeButton";
            this.ReplcaeButton.UseVisualStyleBackColor = true;
            this.ReplcaeButton.Click += new System.EventHandler(this.replaceClick);
            // 
            // ReplaceAllButton
            // 
            resources.ApplyResources(this.ReplaceAllButton, "ReplaceAllButton");
            this.ReplaceAllButton.Name = "ReplaceAllButton";
            this.ReplaceAllButton.UseVisualStyleBackColor = true;
            this.ReplaceAllButton.Click += new System.EventHandler(this.ReplaceAllButton_Click);
            // 
            // GoToLineBtn
            // 
            resources.ApplyResources(this.GoToLineBtn, "GoToLineBtn");
            this.GoToLineBtn.Name = "GoToLineBtn";
            this.GoToLineBtn.UseVisualStyleBackColor = true;
            this.GoToLineBtn.Click += new System.EventHandler(this.gotoBtnClick);
            // 
            // numericLineNumber
            // 
            resources.ApplyResources(this.numericLineNumber, "numericLineNumber");
            this.numericLineNumber.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.numericLineNumber.Maximum = new decimal(new int[] {
            268435455,
            1042612833,
            542101086,
            0});
            this.numericLineNumber.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericLineNumber.Name = "numericLineNumber";
            this.numericLineNumber.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // SearchUI
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.numericLineNumber);
            this.Controls.Add(this.GoToLineBtn);
            this.Controls.Add(this.ReplaceAllButton);
            this.Controls.Add(this.ReplcaeButton);
            this.Controls.Add(this.SearchButton);
            this.Controls.Add(this.ReplaceTextBox);
            this.Controls.Add(this.FindTextBox);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SearchUI";
            this.ShowInTaskbar = false;
            ((System.ComponentModel.ISupportInitialize)(this.numericLineNumber)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox FindTextBox;
        private System.Windows.Forms.TextBox ReplaceTextBox;
        private System.Windows.Forms.Button SearchButton;
        private System.Windows.Forms.Button ReplcaeButton;
        private System.Windows.Forms.Button ReplaceAllButton;
        private System.Windows.Forms.Button GoToLineBtn;
        private System.Windows.Forms.NumericUpDown numericLineNumber;
    }
}