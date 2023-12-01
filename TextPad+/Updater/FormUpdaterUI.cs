
namespace TextPad_.Updater
{
    public partial class UpdaterUI : Form
    {
        public UpdaterUI()
        {
            if (Properties.Settings.Default.Language == "English")
            {
                System.Threading.Thread.CurrentThread.CurrentUICulture = System.Globalization.CultureInfo.GetCultureInfo("en-US");
                System.Threading.Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.GetCultureInfo("en-US");
            }
            else if (Properties.Settings.Default.Language == "Russian")
            {
                System.Threading.Thread.CurrentThread.CurrentUICulture = System.Globalization.CultureInfo.GetCultureInfo("ru-RU");
                System.Threading.Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.GetCultureInfo("ru-RU");
            }

            InitializeComponent();
        }

        private void CheckForUpdates(object sender, EventArgs e)
        {
            Updater.GetUpdate(UpdateLatestVerL, UpdateInfoTextBox, UpdateStatusLabel, UpdateStatusProgressBar);
        }

        private void InstallUpdateManually(object sender, EventArgs e)
        {
            Updater.OpenUpdatesSite();
            this.WindowState = FormWindowState.Minimized;
        }

        private void FormUpdaterUILoad(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Normal;
            ProgramPathTextBox.Text = Program.MainForm.ProgramPath;
            UpdateInstalledVerL.Text = MainUI.GetAssemblyVersion();
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
