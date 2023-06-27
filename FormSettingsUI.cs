
namespace TextPad_
{
    public partial class SettingsUI : Form
    {
        private bool isLangChanged = false;


        public SettingsUI()
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

        private void saveSettings(object sender, EventArgs e)
        {
            var rtb = Program.mainUI.tabControl.TabPages[Program.mainUI.tabControl.SelectedIndex].Controls.OfType<RichTextBox>().First();

            // Проверка настроек языка
            switch (comboBoxLanguage.SelectedItem)
            {
                case @"English / Английский":
                    if (Properties.Settings.Default.Language != "English")
                        isLangChanged = true;

                    Properties.Settings.Default.Language = "English";
                    break;
                case @"Русский / Russian":
                    if (Properties.Settings.Default.Language != "Russian")
                        isLangChanged = true;

                    Properties.Settings.Default.Language = "Russian";
                    break;
            }

            // Проверка настроек цветовой схемы
            switch (comboTheme.SelectedItem)
            {
                case "Белая / White":
                    Properties.Settings.Default.Theme = "White";

                    // на текущей вкладке
                    Program.mainUI.colorThemeWhite();
                    break;
                case "Тёмная / Dark":
                    Properties.Settings.Default.Theme = "Dark";

                    // на текущей вкладке
                    Program.mainUI.colorThemeDark();
                    break;
            }

            // Проверка настроек строки состояния
            Program.mainUI.statusStrip.Visible = statusStripCheckBox.Checked;
            Properties.Settings.Default.StatusStripVisible = statusStripCheckBox.Checked;

            // Проверка настроек TopMost'а
            Program.mainUI.TopMost = overWindowsCheckBox.Checked;
            Properties.Settings.Default.Topmost = overWindowsCheckBox.Checked;

            // Проверка настроек панели запуска файлов
            Program.mainUI.runFileToolStrip.Visible = runFilesPanelCheckBox.Checked;
            Properties.Settings.Default.RunFileToolbar = runFilesPanelCheckBox.Checked;

            // Проверка настроек переноса слов
            Properties.Settings.Default.WordWarp = wordWarpCheckBox.Checked;
            rtb.WordWrap = wordWarpCheckBox.Checked;

            // Проверка настроек панели инструментов
            Properties.Settings.Default.InstrumentalPanel = instrumentPanelCheckBox.Checked;
            Program.mainUI.toolsStrip.Visible = instrumentPanelCheckBox.Checked;

            // Проверка настроек обозревателя папок
            Properties.Settings.Default.Explorer = explorerCheckBox.Checked;
            Program.mainUI.folderExplorerPanel.Visible = explorerCheckBox.Checked;

            // Сообщение о перезапуске программы
            if (isLangChanged == true)
            {
                MessageBox.Show(Resources.Localization.SettingsLangChange, "TextPad+", MessageBoxButtons.OK, MessageBoxIcon.Information);
                isLangChanged = false;
            }

            this.Close();
        }

        private void changeFont(object sender, EventArgs e)
        {
            Program.mainUI.fontDialog.ShowDialog();
            FontTextBox.Text = Program.mainUI.fontDialog.Font.ToString();

            Properties.Settings.Default.EditorFont = Program.mainUI.fontDialog.Font;
            Properties.Settings.Default.Save();

            var rtb = Program.mainUI.tabControl.TabPages[Program.mainUI.tabControl.SelectedIndex].Controls.OfType<RichTextBox>().First();
            rtb.Font = Program.mainUI.fontDialog.Font;
        }

        private void SettingsUILoad(object sender, EventArgs e)
        {
            statusStripCheckBox.Checked = Properties.Settings.Default.StatusStripVisible;
            overWindowsCheckBox.Checked = Properties.Settings.Default.Topmost;
            runFilesPanelCheckBox.Checked = Properties.Settings.Default.RunFileToolbar;
            wordWarpCheckBox.Checked = Properties.Settings.Default.WordWarp;
            instrumentPanelCheckBox.Checked = Properties.Settings.Default.InstrumentalPanel;
            wordWarpCheckBox.Checked = Properties.Settings.Default.WordWarp;
            explorerCheckBox.Checked = Properties.Settings.Default.Explorer;

            if (Properties.Settings.Default.Theme == "Dark")
            {
                comboTheme.SelectedItem = "Тёмная / Dark";
            }
            else
            {
                comboTheme.SelectedItem = "Белая / White";
            }

            if (Properties.Settings.Default.Language == "Russian")
            {
                comboBoxLanguage.SelectedItem = "Русский / Russian";
            }
            else if (Properties.Settings.Default.Language == "English")
            {
                comboBoxLanguage.SelectedItem = "English / Английский";
            }

            FontTextBox.Text = Properties.Settings.Default.EditorFont.ToString();
        }
    }
}
