
namespace TextPad_.Updater
{
    /// <summary>
    /// Класс окна установщика обновлений
    /// </summary>
    internal sealed partial class UpdaterUI : Form
    {
        public UpdaterUI()
        {
            if (Properties.Settings.Default.Language == "English")
            {
                Thread.CurrentThread.CurrentUICulture = System.Globalization.CultureInfo.GetCultureInfo("en-US");
                Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.GetCultureInfo("en-US");
            }
            else if (Properties.Settings.Default.Language == "Russian")
            {
                Thread.CurrentThread.CurrentUICulture = System.Globalization.CultureInfo.GetCultureInfo("ru-RU");
                Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.GetCultureInfo("ru-RU");
            }

            InitializeComponent();
        }

        private void CheckForUpdates(object sender, EventArgs e) => Updater.GetUpdate(UpdateLatestVerL, UpdateInfoTextBox, UpdateStatusLabel, UpdateStatusProgressBar);

        private void InstallUpdateManually(object sender, EventArgs e)
        {
            Updater.OpenWebSite();
            this.WindowState = FormWindowState.Minimized;
        }

        private void FormUpdaterUILoad(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Normal;
            PathTextBox.Text = Program.Path;
            UpdateInstalledVerL.Text = Program.Version;
        }
    }
}
