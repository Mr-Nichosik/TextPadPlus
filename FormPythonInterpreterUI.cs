
namespace TextPad_
{
    public partial class FormPythonInterpreterUI : Form
    {
        public FormPythonInterpreterUI()
        {
            InitializeComponent();
        }

        private void Start(object sender, EventArgs e)
        {
            if (AutoSearchRadioButton.Checked == true)
            {
                FileRunner.RunPythonScript();
            }
            else if (ManuallySearchRadioButton.Checked == true)
            {
                if (PathTextBox.Text == "")
                {
                    MessageBox.Show(Resources.Localization.MSGErrorPythonInterPath, "TextPad+", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                FileRunner.RunPythonScript(PathTextBox.Text);
            }

            this.Close();
        }

        private void Browse(object sender, EventArgs e)
        {
            if (openFileDialog.ShowDialog() == DialogResult.Cancel)
            {
                return;
            }

            PathTextBox.Text = openFileDialog.FileName;
            Properties.Settings.Default.PythonInterpreterPath = openFileDialog.FileName;
        }

        private void RadioButtonCheckedChanged(object sender, EventArgs e)
        {
            switch (AutoSearchRadioButton.Checked)
            {
                case true:
                    PathTextBox.Enabled = false;
                    BrowseBtn.Enabled = false;
                    break;
                case false:
                    PathTextBox.Enabled = true;
                    BrowseBtn.Enabled = true;
                    break;
            }

        }

        private void FormPythonInterpreterUILoad(object sender, EventArgs e)
        {
            if (Properties.Settings.Default.PythonInterpreterPath != "Empty File")
            {
                ManuallySearchRadioButton.Checked = true;
                PathTextBox.Text = Properties.Settings.Default.PythonInterpreterPath;
            }
            else
            {
                AutoSearchRadioButton.Checked = true;
            }

            openFileDialog.Filter = "*.exe|*.exe";
        }
    }
}
