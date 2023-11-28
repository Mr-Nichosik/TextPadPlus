
using System.Net;
using System.Threading.Tasks;

namespace TextPad_.Updater
{
    internal class Updater
    {

        // Logger
        private static readonly LogSystem Logger = new($"{Application.StartupPath}\\logs");

        // Свойства с сыллками
        public static string ProgramVersion { get; set; } = FormMainUI.GetAssemblyVersion();
        public static string WebSite { get; private set; } = "https://mr-nichosik.github.io/Main_Page/";
        public static string TextPadVersionSite { get; private set; } = "https://mr-nichosik.github.io/TextPadVersion/";
        public static string TextPadUpdateInstallerSite { get; private set; } = "https://github.com/Mr-Nichosik/TextPadPlus/releases/download/vCurrent/Setup.exe";
        public static string TextPadNewUpdateRuSite { get; private set; } = "https://mr-nichosik.github.io/TextPadNewUpdateRu/";
        public static string TextPadNewUpdateEnSite { get; private set; } = "https://mr-nichosik.github.io/TextPadNewUpdateEn/";

        // Новая версия с ервера
        private static string ServerVersion = "";

        // Веб-фигня
        private static readonly WebClient UpdaterClient = new();

        public static void OpenUpdatesSite()
        {
            try
            {
                Process websiteProcess = new();
                websiteProcess.StartInfo.UseShellExecute = true;
                websiteProcess.StartInfo.FileName = WebSite;
                websiteProcess.Start();
            }
            catch (Exception ex)
            {
                MessageBox.Show(Resources.Localization.UPDATERErrorCantCheckUpdates, Resources.Localization.UPDATERTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
                Logger.Error($"{ex} when starting web site from InfoUI window.");
            }
        }

        public static async void GetUpdate(Label UpdateIatestVerL, TextBox UpdateInfoTextBox, ToolStripStatusLabel UpdateStatusLabel, ToolStripProgressBar UpdateStatusProgressBar)
        {
            //Соответствует ли версия программы версии на сервере?
            Logger.Info("Checking the program version");

            try
            {
                // соответствует
                ServerVersion = UpdaterClient.DownloadString(TextPadVersionSite);

                if (ServerVersion.Contains(ProgramVersion))
                {
                    Logger.Debug("The latest version of the program is installed, the test was successful");

                    UpdateIatestVerL.Text = ServerVersion;
                    MessageBox.Show(Resources.Localization.UPDATERInfoLatestVersion, Resources.Localization.UPDATERTitle, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                // не соответствует (можно обновлять)
                else
                {
                    // Выводим это на в интерфейс
                    UpdateIatestVerL.Text = ServerVersion;
                    UpdateIatestVerL.Text = ServerVersion;

                    // пробуем получить список изменений
                    if (GetChangelog(ref UpdateInfoTextBox) == 1)
                        return;

                    // спрашиваем пользователя, надо ли оно ему

                    // надо
                    if (MessageBox.Show(Resources.Localization.UPDATERInfoUpdateAvailable, Resources.Localization.UPDATERTitle, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        Logger.Debug("Closing the program, downloading the update");

                        Program.UpdateStatus = 1;
                        Program.MainUI.StatusLabel.Text = Resources.Localization.PROGRAMStatusUpdating;

                        Program.MainUI.SaveParameters();
                        StartUninstaller();
                        UpdateStatusLabel.Text = Resources.Localization.UPDATERStatusUpdateDeleteOldVersion;
                        UpdateStatusProgressBar.Value = 25;
                        UpdateStatusLabel.ToolTipText = Resources.Localization.PROGRAMStatusUpdating;

                        UpdateStatusLabel.Text = Resources.Localization.UPDATERStatusUpdateDownloadAndInstall;
                        UpdateStatusProgressBar.Value = 50;
                        await Task.Run(() => DownloadUpdate());
                    }
                    else return;
                }
            }
            catch
            {
                Logger.Error("Error checking update");
                MessageBox.Show(Resources.Localization.UPDATERErrorCantCheckUpdates, Resources.Localization.UPDATERTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public static async void GetUpdateQuiet(Label UpdateIatestVerL, TextBox UpdateInfoTextBox, ToolStripStatusLabel UpdateStatusLabel, ToolStripProgressBar UpdateStatusProgressBar)
        {
            //Соответствует ли версия программы версии на сервере?
            Logger.Info("Checking the program version quiet");

            try
            {
                if (await Task.Run(() => CheckProgramVersion()) == 1)
                    throw new Exception("Не удалось проверить наличие обновлений");

                // соответствует
                if (ServerVersion.Contains(ProgramVersion))
                {
                    Logger.Debug("The latest version of the program is installed, the test was successful");

                    UpdateIatestVerL.Text = ServerVersion;
                    return;
                }
                // не соответствует (можно обновлять)
                else
                {
                    // Выводим это в интерфейс
                    UpdateIatestVerL.Text = ServerVersion;

                    // пробуем получить список изменений
                    if (GetChangelog(ref UpdateInfoTextBox) == 1)
                        return;

                    // спрашиваем пользователя, надо ли оно ему

                    // надо
                    if (MessageBox.Show(Resources.Localization.UPDATERInfoUpdateAvailable, Resources.Localization.UPDATERTitle, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        Logger.Debug("Closing the program, downloading the update");

                        Program.UpdateStatus = 1;
                        Program.MainUI.SaveParameters();
                        StartUninstaller();
                        UpdateStatusLabel.ToolTipText = "Выполняется обновление...";
                        UpdateStatusLabel.Text = Resources.Localization.UPDATERStatusUpdateDeleteOldVersion;
                        UpdateStatusProgressBar.Value = 25;

                        await Task.Run(() => DownloadUpdate());
                        UpdateStatusLabel.Text = Resources.Localization.UPDATERStatusUpdateDownloadAndInstall;
                        UpdateStatusProgressBar.Value = 50;
                    }
                    else return;
                }
            }
            catch
            {
                Logger.Error("Error checking update");
                MessageBox.Show(Resources.Localization.UPDATERErrorCantCheckUpdates, Resources.Localization.UPDATERTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private static byte CheckProgramVersion()
        {
            try
            {
                ServerVersion = UpdaterClient.DownloadString(TextPadVersionSite);
            }
            catch
            {
                return 1;
            }

            return 0;
        }

        private static int GetChangelog(ref TextBox UpdateInfoTextBox)
        {
            try
            {
                Logger.Debug("Not the latest version of the program is installed, the check was successful");
                // Проверяем текущий язык
                switch (Properties.Settings.Default.Language)
                {
                    // Если русский, то качаем chngelog с русской версии веб страницы
                    case "Russian":
                        UpdateInfoTextBox.Text = UpdaterClient.DownloadString(TextPadNewUpdateRuSite);
                        break;
                    // Если английский
                    case "English":
                        UpdateInfoTextBox.Text = UpdaterClient.DownloadString(TextPadNewUpdateEnSite);
                        break;
                }
                Logger.Debug("Received update information");

                return 0;
            }
            catch (Exception ex)
            {
                Logger.Error($"{ex} error getting update information");
                MessageBox.Show(Resources.Localization.UPDATERErrorCantCheckUpdates, Resources.Localization.UPDATERTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);

                return 1;
            }
        }

        private static void StartUninstaller()
        {
            try { Process.Start("unins000.exe", $"/SILENT"); }
            catch { }
        }

        private static void DownloadUpdate()
        {
            try
            {
                Logger.Info("Update download starts");
                Directory.CreateDirectory($"{Program.MainUI.ProgramPath}Update\\");

                UpdaterClient.DownloadFile(TextPadUpdateInstallerSite, $"{Program.MainUI.ProgramPath}Update\\Setup.exe");

                Program.UpdateStatus = 2;

                Process.Start("Update\\Setup.exe", $"/SILENT /DIR=\"{Program.MainUI.ProgramPath}\"");
                Application.Exit();
            }
            catch
            {
                Logger.Error("Error downloading update");
                MessageBox.Show(Resources.Localization.UPDATERErrorCantCheckUpdates, Resources.Localization.UPDATERTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
