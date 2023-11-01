
namespace TextPad_
{
    public partial class FormUpdaterUI : Form
    {
        public FormUpdaterUI()
        {
            InitializeComponent();
        }

        private void CheckForUpdates(object sender, EventArgs e)
        {
            Updater.GetUpdate(UpdateIatestVerL, UpdateInfoTextBox, UpdateStatusLabel, UpdateStatusProgressBar);
        }

        private void InstallUpdateManually(object sender, EventArgs e)
        {
            Updater.OpenUpdatesSite();
            this.WindowState = FormWindowState.Minimized;
        }

        private void FormUpdaterUILoad(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Normal;
            ProgramPathTextBox.Text = Program.MainUI.ProgramPath;
            UpdateInstalledVerL.Text = Program.MainUI.GetAssemblyVersion();
        }

        private void FormUpdaterUI_FormClosing(object sender, FormClosingEventArgs e)
        {
            //if (Program.isUpdating == true)
            //{
            //    if (MessageBox.Show("Прервать установку обновления?", Resources.Localization.UPDATERTitle, MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
            //        return;
            //    else
            //        e.Cancel = true;
            //}
        }
    }
}
