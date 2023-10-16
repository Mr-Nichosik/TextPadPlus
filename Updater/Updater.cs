﻿
using System.Net;
using System.Threading;

namespace TextPad_.Updater
{
    internal class Updater
    {

        // Logger
        private static readonly ILogger LS = new LogSystem($"{Application.StartupPath}\\logs");

        // Свойства с сыллками
        public static string ProgramVersion { get; set; } = Program.mainUI.GetAssemblyVersion();
        public static string WebSite { get; private set; } = "https://mr-nichosik.github.io/Main_Page/";
        public static string TextPadVersionSite { get; private set; } = "https://mr-nichosik.github.io/TextPadVersion/";
        public static string TextPadUpdateInstallerSite { get; private set; } = "https://github.com/Mr-Nichosik/TextPadPlus/releases/download/vCurrent/Setup.exe";
        public static string TextPadNewUpdateRuSite { get; private set; } = "https://mr-nichosik.github.io/TextPadNewUpdateRu/";
        public static string TextPadNewUpdateEnSite { get; private set; } = "https://mr-nichosik.github.io/TextPadNewUpdateEn/";

        public static void OpenUpdatesSite()
        {
            try
            {
                Process websiteProcess = new Process();
                websiteProcess.StartInfo.UseShellExecute = true;
                websiteProcess.StartInfo.FileName = WebSite;
                websiteProcess.Start();
            }
            catch (Exception ex)
            {
                MessageBox.Show(Resources.Localization.UPDATERErrorCantCheckUpdates, "TextPad+", MessageBoxButtons.OK, MessageBoxIcon.Error);
                LS.Error($"{ex} when starting web site from InfoUI window.");
            }
        }

        // Веб-фигня
        private static WebClient UpdaterClient = new();

        public static void GetUpdate(Label UpdateIatestVerL, TextBox UpdateInfoTextBox, ToolStripStatusLabel UpdateStatusLabel, ToolStripProgressBar UpdateStatusProgressBar)
        {
            //Соответствует ли версия программы версии на сервере?
            LS.Info("Checking the program version");

            try
            {
                // соответствует
                string latestVer = UpdaterClient.DownloadString(TextPadVersionSite);

                if (UpdaterClient.DownloadString(TextPadVersionSite).Contains(ProgramVersion))
                {
                    LS.Debug("The latest version of the program is installed, the test was successful");
                    MessageBox.Show(Resources.Localization.UPDATERInfoLatestVersion, "TextPad+", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                // не соответствует (можно обновлять)
                else
                {
                    // Выводим это на в интерфейс
                    UpdateIatestVerL.Text = latestVer;

                    // пробуем получить список изменений
                    if (GetChangelog(UpdateInfoTextBox) == 1)
                        return;

                    // спрашиваем пользователя, надо ли оно ему

                    // надо
                    if (MessageBox.Show(Resources.Localization.UPDATERInfoUpdateAvailable, "Updater", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        LS.Debug("Closing the program, downloading the update");

                        Program.mainUI.SaveParsameters();
                        StartUninstaller();
                        UpdateStatusLabel.ToolTipText = "Выполняется обновление...";
                        UpdateStatusLabel.Text = Resources.Localization.UPDATERStatusUpdateDeleteOldVersion;
                        UpdateStatusProgressBar.Value = 25;

                        Thread threadDownloader = new Thread(new ThreadStart(DownloadUpdate));
                        threadDownloader.Start();
                        UpdateStatusLabel.Text = Resources.Localization.UPDATERStatusUpdateDownloadAndInstall;
                        UpdateStatusProgressBar.Value = 50;
                    }
                    else return;
                }
            }
            catch
            {
                LS.Error("Error checking update");
                MessageBox.Show(Resources.Localization.UPDATERErrorCantCheckUpdates, "TextPad+", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private static int GetChangelog(TextBox UpdateInfoTextBox)
        {
            try
            {
                LS.Debug("Not the latest version of the program is installed, the check was successful");
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
                LS.Debug("Received update information");

                return 0;
            }
            catch (Exception ex)
            {
                LS.Error($"{ex} error getting update information");
                MessageBox.Show(Resources.Localization.UPDATERErrorCantCheckUpdates, "TextPad+", MessageBoxButtons.OK, MessageBoxIcon.Error);

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
            //try
            // {
            LS.Info("Update download starts");
            Directory.CreateDirectory($"{Program.mainUI.ProgramPath}Update\\");

            UpdaterClient.DownloadFile(TextPadUpdateInstallerSite, $"{Program.mainUI.ProgramPath}Update\\Setup.exe");

            Process.Start("Update\\Setup.exe", $"/SILENT /DIR=\"{Program.mainUI.ProgramPath}\"");

            Program.isUpdating = true;
            Application.Exit();
            //   }
            //  catch
            //   {
            //        LS.Error("Error downloading update");
            //         MessageBox.Show(Resources.Localization.UPDATERErrorCantCheckUpdates, "TextPad+", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //     }
        }

    }
}