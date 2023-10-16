
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
            Updater.Updater.GetUpdate(UpdateIatestVerL, UpdateInfoTextBox, UpdateStatusLabel, UpdateStatusProgressBar);
        }

        private void InstallUpdateManually(object sender, EventArgs e)
        {
            Updater.Updater.OpenUpdatesSite();
            this.WindowState = FormWindowState.Minimized;
        }

        private void FormUpdaterUILoad(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Normal;
            ProgramPathTextBox.Text = Program.mainUI.ProgramPath;
            UpdateInstalledVerL.Text = Program.mainUI.GetAssemblyVersion();
        }
    }
}
