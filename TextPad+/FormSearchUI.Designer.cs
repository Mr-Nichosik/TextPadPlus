
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
            label1 = new Label();
            label2 = new Label();
            FindTextBox = new TextBox();
            ReplaceTextBox = new TextBox();
            SearchButton = new Button();
            ReplcaeButton = new Button();
            ReplaceAllButton = new Button();
            GoToLineBtn = new Button();
            numericLineNumber = new NumericUpDown();
            ((System.ComponentModel.ISupportInitialize)numericLineNumber).BeginInit();
            SuspendLayout();
            // 
            // label1
            // 
            resources.ApplyResources(label1, "label1");
            label1.Name = "label1";
            // 
            // label2
            // 
            resources.ApplyResources(label2, "label2");
            label2.Name = "label2";
            // 
            // FindTextBox
            // 
            resources.ApplyResources(FindTextBox, "FindTextBox");
            FindTextBox.Name = "FindTextBox";
            // 
            // ReplaceTextBox
            // 
            resources.ApplyResources(ReplaceTextBox, "ReplaceTextBox");
            ReplaceTextBox.Name = "ReplaceTextBox";
            // 
            // SearchButton
            // 
            resources.ApplyResources(SearchButton, "SearchButton");
            SearchButton.Name = "SearchButton";
            SearchButton.UseVisualStyleBackColor = true;
            SearchButton.Click += Search;
            // 
            // ReplcaeButton
            // 
            resources.ApplyResources(ReplcaeButton, "ReplcaeButton");
            ReplcaeButton.Name = "ReplcaeButton";
            ReplcaeButton.UseVisualStyleBackColor = true;
            ReplcaeButton.Click += Replace;
            // 
            // ReplaceAllButton
            // 
            resources.ApplyResources(ReplaceAllButton, "ReplaceAllButton");
            ReplaceAllButton.Name = "ReplaceAllButton";
            ReplaceAllButton.UseVisualStyleBackColor = true;
            ReplaceAllButton.Click += ReplaceAll;
            // 
            // GoToLineBtn
            // 
            resources.ApplyResources(GoToLineBtn, "GoToLineBtn");
            GoToLineBtn.Name = "GoToLineBtn";
            GoToLineBtn.UseVisualStyleBackColor = true;
            GoToLineBtn.Click += GoToLine;
            // 
            // numericLineNumber
            // 
            resources.ApplyResources(numericLineNumber, "numericLineNumber");
            numericLineNumber.BorderStyle = BorderStyle.FixedSingle;
            numericLineNumber.Maximum = new decimal(new int[] { 268435455, 1042612833, 542101086, 0 });
            numericLineNumber.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            numericLineNumber.Name = "numericLineNumber";
            numericLineNumber.Value = new decimal(new int[] { 1, 0, 0, 0 });
            // 
            // SearchUI
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = AutoScaleMode.Font;
            this.Controls.Add(numericLineNumber);
            this.Controls.Add(GoToLineBtn);
            this.Controls.Add(ReplaceAllButton);
            this.Controls.Add(ReplcaeButton);
            this.Controls.Add(SearchButton);
            this.Controls.Add(ReplaceTextBox);
            this.Controls.Add(FindTextBox);
            this.Controls.Add(label2);
            this.Controls.Add(label1);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SearchUI";
            this.ShowInTaskbar = false;
            ((System.ComponentModel.ISupportInitialize)numericLineNumber).EndInit();
            ResumeLayout(false);
            PerformLayout();
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