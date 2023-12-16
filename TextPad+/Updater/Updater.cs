
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace TextPad_.Updater
{
    internal static class Updater
    {

        // Logger
        private static readonly LogSystem Logger = new() { UserFolderName = $"{Application.StartupPath}\\logs" };

        // Свойства с сыллками
        public static string TextPadVersionSite { get; private set; } = "https://mr-nichosik.github.io/TextPadVersion/";
        public static string TextPadUpdateInstallerSite { get; private set; } = "https://github.com/Mr-Nichosik/TextPadPlus/releases/download/vCurrent/Setup.exe";
        public static string TextPadNewUpdateRuSite { get; private set; } = "https://mr-nichosik.github.io/TextPadNewUpdateRu/";
        public static string TextPadNewUpdateEnSite { get; private set; } = "https://mr-nichosik.github.io/TextPadNewUpdateEn/";

        // Новая с сервера
        private static string ServerVersion = "";

        // Веб-фигня
        private static readonly HttpClient UpdaterClient = new();

        public static void OpenWebSite()
        {
            try
            {
                Process websiteProcess = new();
                websiteProcess.StartInfo.UseShellExecute = true;
                websiteProcess.StartInfo.FileName = Program.WebSite;
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
                if (InternetConnection() == false) throw new Exception(Resources.Localization.UPDATERErrorCantCheckUpdates);

                ServerVersion = await GetProgramVersion();

                // соответствует
                if (ServerVersion == Program.Version)
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

                    // пробуем получить список изменений
                    GetChangelog(UpdateInfoTextBox);

                    // спрашиваем пользователя, надо ли оно ему

                    // надо
                    if (MessageBox.Show(Resources.Localization.UPDATERInfoUpdateAvailable, Resources.Localization.UPDATERTitle, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        Logger.Debug("Closing the program, downloading the update");

                        Program.UpdateStatus = 1;
                        // Работа с интерфейсом
                        Program.MainForm.StatusLabel.Text = Resources.Localization.PROGRAMStatusUpdating;

                        // Сохранение настроек программы
                        Program.MainForm.SaveParameters();
                        // Работа с интерфейсом
                        UpdateStatusLabel.Text = Resources.Localization.UPDATERStatusUpdateDeleteOldVersion;
                        UpdateStatusProgressBar.Value = 25;
                        // Запуск деинсталлятора текущей версии
                        try { Process.Start("unins000.exe", $"/SILENT"); } catch { }
                        // Работа с интерфейсом
                        UpdateStatusLabel.ToolTipText = Resources.Localization.PROGRAMStatusUpdating;
                        UpdateStatusLabel.Text = Resources.Localization.UPDATERStatusUpdateDownloadAndInstall;
                        UpdateStatusProgressBar.Value = 50;
                        // Загрузка обновления
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
                if (InternetConnection() == false) throw new Exception(Resources.Localization.UPDATERErrorCantCheckUpdates);

                ServerVersion = await GetProgramVersion();

                // соответствует
                if (ServerVersion == Program.Version)
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
                    GetChangelog(UpdateInfoTextBox);

                    // спрашиваем пользователя, надо ли оно ему

                    // надо
                    if (MessageBox.Show(Resources.Localization.UPDATERInfoUpdateAvailable, Resources.Localization.UPDATERTitle, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        Logger.Debug("Closing the program, downloading the update");

                        Program.UpdateStatus = 1;
                        // Работа с интерфейсом
                        Program.MainForm.StatusLabel.Text = Resources.Localization.PROGRAMStatusUpdating;

                        // Сохранение настроек программы
                        Program.MainForm.SaveParameters();
                        // Работа с интерфейсом
                        UpdateStatusLabel.Text = Resources.Localization.UPDATERStatusUpdateDeleteOldVersion;
                        UpdateStatusProgressBar.Value = 25;
                        // Запуск деинсталлятора текущей версии
                        try { Process.Start("unins000.exe", $"/SILENT"); } catch { }
                        // Работа с интерфейсом
                        UpdateStatusLabel.ToolTipText = Resources.Localization.PROGRAMStatusUpdating;
                        UpdateStatusLabel.Text = Resources.Localization.UPDATERStatusUpdateDownloadAndInstall;
                        UpdateStatusProgressBar.Value = 50;
                        // Загрузка обновления
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

        private static bool InternetConnection()
        {
            try
            {
                Dns.GetHostEntryAsync("mr-nichosik.github.io");
                return true;
            }
            catch { return false; }
        }

        private static async Task<string> GetProgramVersion()
        {
            HttpResponseMessage response = await UpdaterClient.GetAsync(TextPadVersionSite);
            return await response.Content.ReadAsStringAsync();
        }

        private static async void GetChangelog(TextBox UpdateInfoTextBox)
        {
            try
            {
                Logger.Debug("Not the latest version of the program is installed, the check was successful");
                // Проверяем текущий язык
                switch (Properties.Settings.Default.Language)
                {
                    // Если русский, то качаем chngelog с русской версии веб страницы
                    case "Russian":
                        HttpResponseMessage responseRu = await UpdaterClient.GetAsync(TextPadNewUpdateRuSite);
                        UpdateInfoTextBox.Text = await responseRu.Content.ReadAsStringAsync();
                        break;
                    // Если английский
                    case "English":
                        HttpResponseMessage responseEn = await UpdaterClient.GetAsync(TextPadNewUpdateEnSite);
                        UpdateInfoTextBox.Text = await responseEn.Content.ReadAsStringAsync();
                        break;
                }
                Logger.Debug("Received update information");
            }
            catch (Exception ex)
            {
                Logger.Error($"{ex} error getting update information");
                MessageBox.Show(Resources.Localization.UPDATERErrorCantCheckUpdates, Resources.Localization.UPDATERTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private static async void DownloadUpdate()
        {
            try
            {
                Logger.Info("Update download starts");
                Directory.CreateDirectory($"{Program.Path}Update\\");

                Stream stream = await UpdaterClient.GetStreamAsync(TextPadUpdateInstallerSite);
                FileStream file = new($"{Program.Path}Update\\Setup.exe", FileMode.CreateNew);
                await stream.CopyToAsync(file);

                Program.UpdateStatus = 2;

                Process.Start("Update\\Setup.exe", $"/SILENT /DIR=\"{Program.Path}\"");
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
