
namespace TextPad_
{
    public partial class FormPythonInterpreterUI : Form
    {
        private readonly IFileRunner FileRunner = new TextEditor();

        public FormPythonInterpreterUI()
        {
            InitializeComponent();
        }

        private void doneBtn_Click(object sender, EventArgs e)
        {
            if (radioButton1.Checked == true)
            {
                FileRunner.PythonRun();
            }
            else if (radioButton2.Checked == true)
            {
                if (pathTextBox.Text == "")
                {
                    MessageBox.Show(Resources.Localization.MSGErrorPythonInterPath, "TextPad+", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                FileRunner.PythonRun(pathTextBox.Text);
            }

            this.Close();
        }

        private void browseBtn_Click(object sender, EventArgs e)
        {
            if (openFileDialog.ShowDialog() == DialogResult.Cancel) 
            {
                return;
            }

            pathTextBox.Text = openFileDialog.FileName;
        }

        private void radioButtonCheckedChanged(object sender, EventArgs e)
        {
            switch (radioButton1.Checked)
            {
                case true:
                    pathTextBox.Enabled = false;
                    browseBtn.Enabled = false;
                    break;
                case false:
                    pathTextBox.Enabled = true;
                    browseBtn.Enabled = true;
                    break;
            }

        }

        private void FormPythonInterpreterUI_Load(object sender, EventArgs e)
        {
            radioButton1.Checked = true;
            openFileDialog.Filter = "*.exe|*.exe";
        }
    }
}
