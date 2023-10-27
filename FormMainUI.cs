
using Microsoft.VisualBasic.FileIO;
using System.Collections.Generic;
using System.Reflection;

namespace TextPad_
{
    /// <summary>
    /// Класс главного окна программы, где обрабатывается только (как я надеюсь) внешний вид и пользовательский UI.
    /// Всю (наверное) логику работы программы я переместил в класс TextEditor.
    /// <summary>
    public partial class FormMainUI : Form
    {
        // Logger
        private readonly ILogger LS = new LogSystem($"{Application.StartupPath}\\logs");

        // Окно установщика обновлений
        public FormUpdaterUI UpdaterUI { get; private set; } = new();

        // Обработчик запуска файлов
        private IFileRunner FileRunner = new TextEditor();

        // Авто свойства для чтения с инфой о программе
        public string DateOfRelease { get; } = "DEVELOPEMENT";
        public string ProgramPath { get; } = Application.StartupPath;
        public string WebSite { get; private set; } = "https://mr-nichosik.github.io/Main_Page/";

        // Список открытых файлов
        //internal List<string> OpenedFiles { get; set; } = new();

        // Общее поле для Rich Text Box
        private MTextBox? mtb;

        // Поле для настроек программы. Если isLangChanched = true, то при выходе из настроек появится сообщение о необходимости перезапустить программу
        private static bool isLangChanged = false;

        // Запуск программы в обычном режиме
        public FormMainUI()
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

            LS.Info("MainUI Initialization");
            InitializeComponent();

            Program.mainUI = this;

            LS.Debug("Program launched successfully");
        }

        // Переопределение OnKeyDown для добавления сочетаний клавиш
        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);

            if (e.KeyCode == Keys.V && e.Control)
            {
                TextEditor.pasteTextFromTB(cTabControl);
                e.Handled = true;
            }

            if (e.KeyCode == Keys.F && e.Control)
            {
                mtb = cTabControl.TabPages[cTabControl.SelectedIndex].Controls.OfType<MTextBox>().First();
                if (mtb.TextLength > 0)
                {
                    SearchUI search_ui = new SearchUI();
                    search_ui.ShowDialog(this);

                    mtb = cTabControl.TabPages[cTabControl.SelectedIndex].Controls.OfType<MTextBox>().First();
                }
                e.Handled = true;
            }
        }

        #region Обработка событий разных элементов формы

        // Методы для кнопок панели инструментов, контекстного и главного меню
        private void saveAsFileButton(object sender, EventArgs e)
        {
            TextEditor.SaveAsFile();
        }

        private void saveFileButton(object sender, EventArgs e)
        {
            TextEditor.SaveCurrentFile();
        }

        private void openFileButton(object sender, EventArgs e)
        {
            TextEditor.OpenFile();
        }

        private void copyTextFromTBButton(object sender, EventArgs e)
        {
            TextEditor.copyTextFromTB(cTabControl);
        }

        private void cutTextFromTBButton(object sender, EventArgs e)
        {
            TextEditor.cutTextFromTB(cTabControl);
        }

        private void pasteTextFromTBButton(object sender, EventArgs e)
        {
            TextEditor.pasteTextFromTB(cTabControl);
        }

        private void fontTextFromTBButton(object sender, EventArgs e)
        {
            TextEditor.fontTextFromTB(cTabControl, fontDialog);
        }

        private void selectAllTextFromTBButton(object sender, EventArgs e)
        {
            TextEditor.selectAllTextFromTB(cTabControl);
        }

        private void undoTextFromTBButton(object sender, EventArgs e)
        {
            TextEditor.undoTextFromTB(cTabControl);
        }

        private void redoTextFromTBButton(object sender, EventArgs e)
        {
            TextEditor.redoTextFromTB(cTabControl);
        }

        private void deleteTextFromTBButton(object sender, EventArgs e)
        {
            TextEditor.deleteTextFromTB(cTabControl);
        }

        private void dateAndTime(object sender, EventArgs e)
        {
            TextEditor.dateTime(cTabControl);
        }

        private void openFileFolder(object sender, EventArgs e)
        {
            try
            {
                if (cTabControl.TabPages[cTabControl.SelectedIndex].Controls.OfType<MTextBox>().First().FileName != "Missing")
                    Process.Start("explorer.exe", Path.GetDirectoryName(cTabControl.TabPages[cTabControl.SelectedIndex].Controls.OfType<MTextBox>().First().FileName)!);
            }
            catch { }
        }

        private void deleteFile(object sender, EventArgs e)
        {
            try
            {
                if (File.Exists(cTabControl.TabPages[cTabControl.SelectedIndex].Controls.OfType<MTextBox>().First().FileName))
                {
                    FileSystem.DeleteFile(cTabControl.TabPages[cTabControl.SelectedIndex].Controls.OfType<MTextBox>().First().FileName, UIOption.OnlyErrorDialogs, RecycleOption.SendToRecycleBin);

                    cTabControl.TabPages.Remove(cTabControl.TabPages[cTabControl.SelectedIndex]);
                    if (cTabControl.TabPages.Count == 0)
                    {
                        switch (Properties.Settings.Default.ExitWhenClosingLastTab)
                        {
                            case true:
                                Application.Exit();
                                break;
                            case false:
                                CreateTab();
                                break;
                        }
                    }
                }
                else
                {
                    MessageBox.Show(Resources.Localization.MSGErrorCantDeleteFile, "TextPad+", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch { }
        }

        private void copyFullPath(object sender, EventArgs e)
        {
            if (cTabControl.TabPages[cTabControl.SelectedIndex].Controls.OfType<MTextBox>().First().FileName != "Missing")
                Clipboard.SetText(cTabControl.TabPages[cTabControl.SelectedIndex].Controls.OfType<MTextBox>().First().FileName);
            else
                return;
        }

        private void copyFileName(object sender, EventArgs e)
        {
            if (cTabControl.TabPages[cTabControl.SelectedIndex].Controls.OfType<MTextBox>().First().FileName != "Missing")
                Clipboard.SetText(Path.GetFileName(cTabControl.TabPages[cTabControl.SelectedIndex].Controls.OfType<MTextBox>().First().FileName));
            else
                return;
        }

        // Методы для настроек
        private void openSettings(object sender, EventArgs e)
        {
            MainUIPanel.Visible = false;
            SettingsUIPanel.Visible = true;

            textBoxWebSiteUrl.Text = WebSite;
            ProgramNameLabel.Text = $"{GetAssemblyName()} {GetAssemblyVersion()}";
            VersionLabel.Text = GetAssemblyVersion();
            DateOfReleaseLabel.Text = DateOfRelease;
            DeveloperLabel.Text = GetAssemblyCompany();

            LSVersionLabel.Text = LogSystem.GetAssemblyVersion();
            CTCLabelVersion.Text = CTabControl.GetAssemblyVersion();
            MTBVersionLabel.Text = MTextBox.GetAssemblyVersion();

            // Загрузка настроек
            statusStripCheckBox.Checked = Properties.Settings.Default.StatusStripVisible;
            overWindowsCheckBox.Checked = Properties.Settings.Default.Topmost;
            runFilesPanelCheckBox.Checked = Properties.Settings.Default.RunFileToolbar;
            wordWarpCheckBox.Checked = Properties.Settings.Default.WordWarp;
            instrumentPanelCheckBox.Checked = Properties.Settings.Default.InstrumentalPanel;
            wordWarpCheckBox.Checked = Properties.Settings.Default.WordWarp;
            explorerCheckBox.Checked = Properties.Settings.Default.Explorer;
            exitWhenClosingLastTabCheckBox.Checked = Properties.Settings.Default.ExitWhenClosingLastTab;

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

            switch (Properties.Settings.Default.DefaultEncoding)
            {
                case "ASCII":
                    encodingComboBox.SelectedItem = "ASCII";
                    break;
                case "UTF-7":
                    encodingComboBox.SelectedItem = "UTF-7";
                    break;
                case "UTF-8":
                    encodingComboBox.SelectedItem = "UTF-8";
                    break;
                case "UTF-16 (Unicode)":
                    encodingComboBox.SelectedItem = "UTF-16 (Unicode)";
                    break;
                case "UTF-32":
                    encodingComboBox.SelectedItem = "UTF-32";
                    break;
            }

            FontTextBox.Text = Properties.Settings.Default.EditorFont.ToString();
        }

        private void saveSettings(object sender, EventArgs e)
        {
            var mtb = Program.mainUI.cTabControl.TabPages[Program.mainUI.cTabControl.SelectedIndex].Controls.OfType<MTextBox>().First();

            // Проверка настроек языка
            switch (Program.mainUI.comboBoxLanguage.SelectedItem)
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
            switch (Program.mainUI.comboTheme.SelectedItem)
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

            // Проверка кодировки
            switch (encodingComboBox.SelectedItem)
            {
                case "ASCII":
                    Properties.Settings.Default.DefaultEncoding = "ASCII";
                    break;
                case "UTF-7":
                    Properties.Settings.Default.DefaultEncoding = "UTF-7";
                    break;
                case "UTF-8":
                    Properties.Settings.Default.DefaultEncoding = "UTF-8";
                    break;
                case "UTF-16 (Unicode)":
                    Properties.Settings.Default.DefaultEncoding = "UTF-16 (Unicode)";
                    break;
                case "UTF-32":
                    Properties.Settings.Default.DefaultEncoding = "UTF-32";
                    break;
            }

            // Проверка настроек строки состояния
            Program.mainUI.statusStrip.Visible = Program.mainUI.statusStripCheckBox.Checked;
            Properties.Settings.Default.StatusStripVisible = Program.mainUI.statusStripCheckBox.Checked;

            // Проверка настроек TopMost'а
            Program.mainUI.TopMost = Program.mainUI.overWindowsCheckBox.Checked;
            Properties.Settings.Default.Topmost = Program.mainUI.overWindowsCheckBox.Checked;

            // Проверка настроек панели запуска файлов
            Program.mainUI.runFileToolStrip.Visible = Program.mainUI.runFilesPanelCheckBox.Checked;
            Properties.Settings.Default.RunFileToolbar = Program.mainUI.runFilesPanelCheckBox.Checked;

            // Проверка настроек переноса слов
            Properties.Settings.Default.WordWarp = Program.mainUI.wordWarpCheckBox.Checked;
            mtb.WordWrap = Program.mainUI.wordWarpCheckBox.Checked;

            // Проверка настроек панели инструментов
            Properties.Settings.Default.InstrumentalPanel = Program.mainUI.instrumentPanelCheckBox.Checked;
            Program.mainUI.toolsStrip.Visible = Program.mainUI.instrumentPanelCheckBox.Checked;

            // Проверка настроек обозревателя папок
            Properties.Settings.Default.Explorer = Program.mainUI.explorerCheckBox.Checked;
            Program.mainUI.folderExplorerPanel.Visible = Program.mainUI.explorerCheckBox.Checked;

            // Нужно ли закрывать программу при закрытии последней вкладки
            Properties.Settings.Default.ExitWhenClosingLastTab = Program.mainUI.exitWhenClosingLastTabCheckBox.Checked;

            // Сообщение о перезапуске программы
            if (isLangChanged == true)
            {
                MessageBox.Show(Resources.Localization.SettingsLangChange, "TextPad+", MessageBoxButtons.OK, MessageBoxIcon.Information);
                isLangChanged = false;
            }

            Properties.Settings.Default.Save();

            Program.mainUI.SettingsUIPanel.Visible = false;
            Program.mainUI.MainUIPanel.Visible = true;
        }

        private void closeSettings(object sender, EventArgs e)
        {
            MainUIPanel.Visible = true;
            SettingsUIPanel.Visible = false;
        }

        private void fontSettings(object sender, EventArgs e)
        {
            Program.mainUI.fontDialog.ShowDialog();
            Program.mainUI.FontTextBox.Text = Program.mainUI.fontDialog.Font.ToString();

            Properties.Settings.Default.EditorFont = Program.mainUI.fontDialog.Font;
            Properties.Settings.Default.Save();

            var mtb = Program.mainUI.cTabControl.TabPages[Program.mainUI.cTabControl.SelectedIndex].Controls.OfType<MTextBox>().First();
            mtb.Font = Program.mainUI.fontDialog.Font;
        }

        private void openWebSite(object sender, EventArgs e)
        {
            try
            {
                Process websiteProcess = new Process();
                websiteProcess.StartInfo.UseShellExecute = true;
                websiteProcess.StartInfo.FileName = Program.mainUI.WebSite;
                websiteProcess.Start();
            }
            catch (Exception ex)
            {
                MessageBox.Show(Resources.Localization.MSGErrorWhenCheckingInternet, "TextPad+", MessageBoxButtons.OK, MessageBoxIcon.Error);
                LS.Error($"{ex} when starting web site from InfoUI window.");
            }
        }

        // Открытие окон и нажатия на кнопки в настройках

        private void newWindow(object sender, EventArgs e)
        {
            Process.Start("TextPad+.exe");
        }

        private void search(object sender, EventArgs e)
        {
            SearchUI search_ui = new SearchUI();
            search_ui.ShowDialog(this);
        }

        private void getUpdate(object sender, EventArgs e)
        {
            UpdaterUI.ShowDialog(this);
        }

        // Методы для cTabControl
        internal void CreateTab()
        {
            /*
             * Создаётся экземпляр вкладки, в mtb закидывается новоиспечённый MTextBox (mtb, потому что раньше это был MTextBox, а мне лень менять названия),
             * задаются кое-какие параметры, затем в cTabControl, который накинут на форму впринципе, добавляется новая вкладка и кней автоматичсеки присваивается ивент TextChanged,
             * метод для которого (TbTextChanged) я создал заранее. Это нужно для динамичного изменения настроек программы в текущей вкладке, например, юзер может выключить функцию
             * переноса слов.
            */
            TabPage tpage = new TabPage(Resources.Localization.newDocumentTitle);
            var mtb = new MTextBox
            {
                Dock = DockStyle.Fill,
                BorderStyle = BorderStyle.None,
                WordWrap = Properties.Settings.Default.WordWarp,
                ContextMenuStrip = contextMenuStrip,
                Font = Properties.Settings.Default.EditorFont
            };
            mtb.TextChanged += (sender, args) => TextBoxTextChanged();
            mtb.AcceptsTab = true;

            tpage.Controls.Add(mtb);
            cTabControl.TabPages.Add(tpage);

            if (Properties.Settings.Default.Theme == "White")
                colorThemeWhite();
            else
                colorThemeDark();

            if (cTabControl.TabPages[cTabControl.SelectedIndex].Controls.OfType<MTextBox>().First().FileName == "Missing")
            {
                fileFolderFileMenuItem.Enabled = false;
                deletFileFileMenuItem.Enabled = false;
            }
            else
            {
                fileFolderFileMenuItem.Enabled = true;
                deletFileFileMenuItem.Enabled = true;
            }

            mtb.Encoding = Properties.Settings.Default.DefaultEncoding;
            if (cTabControl.TabPages[cTabControl.SelectedIndex] == tpage)
                encodingStatusLabel.Text = mtb.Encoding;
        }

        internal void CreateTab(string tabName)
        {
            /* Создаётся экземпляр вкладки, в mtb закидывается новоиспечённый MTextBox (mtb, потому что раньше это был MTextBox, а мне лень менять названия),
            * задаются кое-какие параметры, затем в cTabControl, который накинут на форму впринципе, добавляется новая вкладка и кней автоматичсеки присваивается ивент TextChanged,
            * метод для которого (TbTextChanged) я создал заранее. Это нужно для динамичного изменения настроек программы в текущей вкладке, например, юзер может выключить функцию
            * переноса слов.
            */
            TabPage tpage = new TabPage(tabName);
            var mtb = new MTextBox();
            mtb.Dock = DockStyle.Fill;
            mtb.BorderStyle = BorderStyle.None;
            mtb.WordWrap = Properties.Settings.Default.WordWarp;
            mtb.ContextMenuStrip = contextMenuStrip;
            mtb.Font = Properties.Settings.Default.EditorFont;
            mtb.TextChanged += (sender, args) => TextBoxTextChanged();
            mtb.AcceptsTab = true;

            tpage.Controls.Add(mtb);
            cTabControl.TabPages.Add(tpage);

            cTabControl.TabPages[cTabControl.SelectedIndex].Controls.OfType<MTextBox>().First().FileName = "Missing";

            if (Properties.Settings.Default.Theme == "White")
            {
                colorThemeWhite();
            }
            else
            {
                colorThemeDark();
            }

            if (cTabControl.TabPages[cTabControl.SelectedIndex].Controls.OfType<MTextBox>().First().FileName == "Missing")
            {
                fileFolderFileMenuItem.Enabled = false;
                deletFileFileMenuItem.Enabled = false;
            }
            else
            {
                fileFolderFileMenuItem.Enabled = true;
                deletFileFileMenuItem.Enabled = true;
            }

            mtb.Encoding = Properties.Settings.Default.DefaultEncoding;
            if (cTabControl.TabPages[cTabControl.SelectedIndex] == tpage)
            {
                encodingStatusLabel.Text = mtb.Encoding;
            }
        }

        private void createTabClick(object sender, EventArgs e)
        {
            CreateTab();
        }

        private void CloseTab(object sender, EventArgs e)
        {
            mtb = cTabControl.TabPages[cTabControl.SelectedIndex].Controls.OfType<MTextBox>().First();
            TabPage tab = cTabControl.SelectedTab;

            if (mtb.TextLength != 0)
            {
                DialogResult dr = MessageBox.Show($"Сохранить текст в файл \"{tab.Text}\"?", "TextPad+", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                if (dr == DialogResult.Yes)
                {
                    TextEditor.SaveCurrentFile();
                }
                else if (dr == DialogResult.No)
                {
                    cTabControl.TabPages.Remove(tab);
                }
                else if (dr == DialogResult.Cancel)
                {
                    return;
                }
            }
            else
            {
                cTabControl.TabPages.Remove(tab);
            }

            if (cTabControl.TabPages.Count == 0)
            {
                switch (Properties.Settings.Default.ExitWhenClosingLastTab)
                {
                    case true:
                        Application.Exit();
                        break;
                    case false:
                        CreateTab();
                        break;
                }
            }
        }

        private void closeAllTabs(object sender, EventArgs e)
        {
            foreach (TabPage tab in cTabControl.TabPages)
            {
                mtb = cTabControl.TabPages[cTabControl.TabPages.IndexOf(tab)].Controls.OfType<MTextBox>().First();

                if (mtb.IsFileSaved == false)
                {
                    DialogResult dr = MessageBox.Show($"{Resources.Localization.MSGQuestionSaveFile} \"{tab.Text}\"?", "TextPad+", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                    if (dr == DialogResult.Yes)
                    {
                        TextEditor.SaveCurrentFile();
                        cTabControl.TabPages.Remove(tab);

                    }
                    else if (dr == DialogResult.No)
                    {
                        cTabControl.TabPages.Remove(tab);

                    }
                    else if (dr == DialogResult.Cancel)
                    {
                        return;
                    }
                }
                else
                {
                    cTabControl.TabPages.Remove(tab);
                }
            }

            // Проверяем, нужно ли выходить из программы, если заакрывается последняя вкладка
            if (cTabControl.TabPages.Count == 0)
            {
                switch (Properties.Settings.Default.ExitWhenClosingLastTab)
                {
                    case true:
                        Application.Exit();
                        break;
                    case false:
                        CreateTab();
                        break;
                }
            }
        }

        private void closeAllWithoutCurrentTab(object sender, EventArgs e)
        {
            foreach (TabPage tab in cTabControl.TabPages)
            {
                if (tab == cTabControl.TabPages[cTabControl.SelectedIndex])
                {
                    continue;
                }

                mtb = cTabControl.TabPages[cTabControl.TabPages.IndexOf(tab)].Controls.OfType<MTextBox>().First();

                if (mtb.TextLength != 0)
                {
                    DialogResult dr = MessageBox.Show($"{Resources.Localization.MSGQuestionSaveFile} \"{tab.Text}\"?", "TextPad+", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                    if (dr == DialogResult.Yes)
                    {
                        TextEditor.SaveCurrentFile();
                        cTabControl.TabPages.Remove(tab);

                    }
                    else if (dr == DialogResult.No)
                    {
                        cTabControl.TabPages.Remove(tab);

                    }
                    else if (dr == DialogResult.Cancel)
                    {
                        return;
                    }
                }
                else
                {
                    cTabControl.TabPages.Remove(tab);
                }
            }

            // Проверяем, нужно ли выходить из программы, если заакрывается последняя вкладка
            if (cTabControl.TabPages.Count == 0)
            {
                switch (Properties.Settings.Default.ExitWhenClosingLastTab)
                {
                    case true:
                        Application.Exit();
                        break;
                    case false:
                        CreateTab();
                        break;
                }
            }
        }

        private void cTabControlSelecting(object sender, TabControlCancelEventArgs e)
        {
            // Этот метод нужен для того, что бы на каждой новой вкладке применялись заданные ранее параметры (вкл/выкл перенос слов, цвет текста, фона, шрифт и т.д.)
            try
            {
                mtb = cTabControl.TabPages[cTabControl.SelectedIndex].Controls.OfType<MTextBox>().First();

                mtb.WordWrap = Properties.Settings.Default.WordWarp;
                mtb.Font = Properties.Settings.Default.EditorFont;
                textLengthLabel.Text = mtb.TextLength.ToString();
                if (mtb.Lines.Length == 0)
                {
                    textLinesLabel.Text = "1";
                }
                else
                {
                    textLinesLabel.Text = mtb.Lines.Length.ToString();
                }

                checkFiles();
                encodingStatusLabel.Text = mtb.Encoding;

                switch (Properties.Settings.Default.Theme)
                {
                    case "Dark":
                        colorThemeDark();
                        break;
                    case "White":
                        colorThemeWhite();
                        break;
                    default:
                        colorThemeWhite();
                        break;
                }
            }
            catch (Exception ex)
            {
                LS.Error($"{ex} Error while selecting tabs (?). Maybe not have any tabs open. If nothing is broken, then ignore it.");
                textLengthLabel.Text = "0";
                textLinesLabel.Text = "1";
            }

        }

        // Обработка события TextChanged для каждого rich text box'а
        private void TextBoxTextChanged()
        {
            mtb = cTabControl.TabPages[cTabControl.SelectedIndex].Controls.OfType<MTextBox>().First();

            // Подсчёт количества строк и символов
            textLengthLabel.Text = mtb.Text.Length.ToString();
            if (mtb.Lines.Length == 0)
                textLinesLabel.Text = "1";
            else
                textLinesLabel.Text = mtb.Lines.Length.ToString();

            //mtb.IsFileSaved = false;
            if (mtb.IsFileChanged == false)
            {
                cTabControl.TabPages[cTabControl.SelectedIndex].Text += "*";
                mtb.IsFileChanged = true;
            }

            checkFiles();
        }

        // Выбор кодировки
        private void ChangeToASCII(object sender, EventArgs e)
        {
            mtb = Program.mainUI.cTabControl.TabPages[Program.mainUI.cTabControl.SelectedIndex].Controls.OfType<MTextBox>().First();
            mtb.Encoding = "ASCII";
            encodingStatusLabel.Text = mtb.Encoding.ToString();
        }
        private void ChangeToUTF7(object sender, EventArgs e)
        {
            mtb = Program.mainUI.cTabControl.TabPages[Program.mainUI.cTabControl.SelectedIndex].Controls.OfType<MTextBox>().First();
            mtb.Encoding = "UTF-7";
            encodingStatusLabel.Text = mtb.Encoding.ToString();
        }
        private void ChangeToUTF8(object sender, EventArgs e)
        {
            mtb = Program.mainUI.cTabControl.TabPages[Program.mainUI.cTabControl.SelectedIndex].Controls.OfType<MTextBox>().First();
            mtb.Encoding = "UTF-8";
            encodingStatusLabel.Text = mtb.Encoding.ToString();
        }
        private void ChangeToUTF16(object sender, EventArgs e)
        {
            mtb = Program.mainUI.cTabControl.TabPages[Program.mainUI.cTabControl.SelectedIndex].Controls.OfType<MTextBox>().First();
            mtb.Encoding = "UTF-16 (Unicode)";
            encodingStatusLabel.Text = mtb.Encoding.ToString();
        }
        private void ChangeToUTF32(object sender, EventArgs e)
        {
            mtb = Program.mainUI.cTabControl.TabPages[Program.mainUI.cTabControl.SelectedIndex].Controls.OfType<MTextBox>().First();
            mtb.Encoding = "UTF-32";
            encodingStatusLabel.Text = mtb.Encoding.ToString();
        }

        // Методы для кнопок панели проводника
        private void OpenFolderBtnClick(object sender, EventArgs e)
        {
            if (folderBrowserDialog.ShowDialog() == DialogResult.Cancel)
            {
                return;
            }

            workFolderLabel.Text = new DirectoryInfo(folderBrowserDialog.SelectedPath).Name;
            FilesListView.Clear();

            // Папки
            foreach (string item in Directory.GetDirectories(folderBrowserDialog.SelectedPath))
            {
                ListViewItem lvi = new ListViewItem(new DirectoryInfo(item).Name, 0);
                lvi.ToolTipText = item;
                FilesListView.Items.Add(lvi);

            }

            // Файлы
            foreach (string item in Directory.GetFiles(folderBrowserDialog.SelectedPath))
            {
                ListViewItem lvi = new ListViewItem(new DirectoryInfo(item).Name, 1);
                lvi.ToolTipText = item;
                FilesListView.Items.Add(lvi);
            }
        }

        private void AboveFolderBtnClick(object sender, EventArgs e)
        {
            try
            {
                string newDirectory = Directory.GetParent(folderBrowserDialog.SelectedPath).ToString();
                folderBrowserDialog.SelectedPath = newDirectory;

                workFolderLabel.Text = new DirectoryInfo(newDirectory).Name;
                FilesListView.Clear();

                // Папки
                foreach (string item in Directory.GetDirectories(newDirectory))
                {
                    ListViewItem lvi = new ListViewItem(new DirectoryInfo(item).Name, 0);
                    lvi.ToolTipText = item;
                    FilesListView.Items.Add(lvi);

                }

                // Файлы
                foreach (string item in Directory.GetFiles(newDirectory))
                {
                    ListViewItem lvi = new ListViewItem(new DirectoryInfo(item).Name, 1);
                    lvi.ToolTipText = item;
                    FilesListView.Items.Add(lvi);
                }
            }
            catch { }
        }

        private void RefreshFolder(object sender, EventArgs e)
        {
            if (folderBrowserDialog.SelectedPath == null)
            {
                return;
            }

            FilesListView.Clear();

            // Папки
            foreach (string item in Directory.GetDirectories(folderBrowserDialog.SelectedPath))
            {
                ListViewItem lvi = new ListViewItem(new DirectoryInfo(item).Name, 0);
                lvi.ToolTipText = item;
                FilesListView.Items.Add(lvi);
            }

            // Файлы
            foreach (string item in Directory.GetFiles(folderBrowserDialog.SelectedPath))
            {
                ListViewItem lvi = new ListViewItem(new DirectoryInfo(item).Name, 1);
                lvi.ToolTipText = item;
                FilesListView.Items.Add(lvi);
            }
        }

        private void FilesListViewDoubleClick(object sender, EventArgs e)
        {
            string path = FilesListView.Items[FilesListView.FocusedItem.Index].ToolTipText;

            //Проверка существования файла
            if (Directory.Exists(path))
            {
                folderBrowserDialog.SelectedPath = path;

                workFolderLabel.Text = new DirectoryInfo(path).Name;
                FilesListView.Clear();

                // Папки
                foreach (string item in Directory.GetDirectories(path))
                {
                    ListViewItem lvi = new ListViewItem(new DirectoryInfo(item).Name, 0);
                    lvi.ToolTipText = item;
                    FilesListView.Items.Add(lvi);

                }

                // Файлы
                foreach (string item in Directory.GetFiles(path))
                {
                    ListViewItem lvi = new ListViewItem(new DirectoryInfo(item).Name, 1);
                    lvi.ToolTipText = item;
                    FilesListView.Items.Add(lvi);
                }
            }

            else if (File.Exists(path))
            {
                CreateTab(new DirectoryInfo(path).Name);
                cTabControl.TabPages[cTabControl.TabPages.Count - 1].Controls.OfType<MTextBox>().First().FileName = path;
                string fileText = File.ReadAllText(path);

                mtb = cTabControl.TabPages[cTabControl.TabPages.Count - 1].Controls.OfType<MTextBox>().First();
                mtb.Text = fileText;

                cTabControl.SelectTab(cTabControl.TabPages[cTabControl.TabPages.Count - 1]);

                if (recentFilesMenuItem.DropDownItems.Count == 10)
                    recentFilesMenuItem.DropDownItems.RemoveAt(0);
                ToolStripMenuItem tsmi = new ToolStripMenuItem();
                tsmi.Text = cTabControl.TabPages[cTabControl.SelectedIndex].Controls.OfType<MTextBox>().First().FileName;
                tsmi.Click += (sender, e) => TextEditor.OpenFile(tsmi.Text);
                recentFilesMenuItem.DropDownItems.Add(tsmi);
            }
        }

        private void UpSizeListView(object sender, EventArgs e)
        {
            if (FilesListView.Width < 410)
            {
                FilesListView.Width += 30;
                folderExplorerPanel.Width += 30;
            }
        }

        private void DownSizeListView(object sender, EventArgs e)
        {
            if (FilesListView.Width > 110)
            {
                FilesListView.Width -= 30;
                folderExplorerPanel.Width -= 30;
            }
        }

        private void closeFolderToolBtnClick(object sender, EventArgs e)
        {
            FilesListView.Clear();
            switch (Properties.Settings.Default.Language)
            {
                case "Russian":
                    workFolderLabel.Text = "Рабочая папка";
                    break;
                case "English":
                    workFolderLabel.Text = "Work folder";
                    break;
            }
        }

        // Контекстное меню проводника
        private void deleteFileInExplorer(object sender, EventArgs e)
        {
            if (FilesListView.Items.Count < 1)
                return;

            try
            {
                if (File.Exists(FilesListView.Items[FilesListView.FocusedItem.Index].ToolTipText))
                {
                    if (MessageBox.Show(Resources.Localization.MSGQestionMoveFileToTrash, "TextPad+", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                    {
                        return;
                    }
                    FileSystem.DeleteFile(FilesListView.Items[FilesListView.FocusedItem.Index].ToolTipText, UIOption.OnlyErrorDialogs, RecycleOption.SendToRecycleBin);
                }
                else if (Directory.Exists(FilesListView.Items[FilesListView.FocusedItem.Index].ToolTipText))
                {
                    if (MessageBox.Show(Resources.Localization.MSGQestionMoveFileToTrash, "TextPad+", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                    {
                        return;
                    }

                    FileSystem.DeleteDirectory(FilesListView.Items[FilesListView.FocusedItem.Index].ToolTipText, UIOption.OnlyErrorDialogs, RecycleOption.SendToRecycleBin);
                }
                else
                {
                    MessageBox.Show(Resources.Localization.MSGErrorCantDeleteFile, "TextPad+", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                RefreshFolder(sender, e);
            }
            catch { }
        }

        // Методы для кнопок панели запуска файлов
        private void RunScript(object sender, EventArgs e)
        {
            FileRunner.RunScript();
        }

        private void PythonRun(object sender, EventArgs e)
        {
            FormPythonInterpreterUI pythonInterpreterUI = new FormPythonInterpreterUI();
            pythonInterpreterUI.ShowDialog(Program.mainUI);
        }

        private void BatRun(object sender, EventArgs e)
        {
            FileRunner.BatRun();
        }

        private void VbsRun(object sender, EventArgs e)
        {
            FileRunner.VbsRun();
        }

        // Просто выход
        private void exit(object sender, EventArgs e)
        {
            Application.Exit();
        }

        #endregion

        // Прочие отдельные методы
        private void checkFiles()
        {
            mtb = cTabControl.TabPages[cTabControl.SelectedIndex].Controls.OfType<MTextBox>().First();
            if (mtb.Text.Length == 0)
            {
                searchEditMenuItem.Enabled = false;
                searchToolStripItem.Enabled = false;
            }
            else
            {
                searchEditMenuItem.Enabled = true;
                searchToolStripItem.Enabled = true;
            }
            if (cTabControl.TabPages[cTabControl.SelectedIndex].Controls.OfType<MTextBox>().First().FileName == "Missing")
            {
                fileFolderFileMenuItem.Enabled = false;
                deletFileFileMenuItem.Enabled = false;
            }
            else
            {
                fileFolderFileMenuItem.Enabled = true;
                deletFileFileMenuItem.Enabled = true;
            }
        }

        // Цветовые схемы
        internal void colorThemeWhite()
        {
            // цвет mtb
            cTabControl.TabPages[cTabControl.SelectedIndex].BackColor = Color.Transparent;
            mtb = cTabControl.TabPages[cTabControl.SelectedIndex].Controls.OfType<MTextBox>().First();

            mtb.ForeColor = Color.Black;
            mtb.BackColor = SystemColors.Window;
            mtb.ForeColor = SystemColors.ControlText;
        }

        internal void colorThemeDark()
        {
            // цвет mtb
            cTabControl.TabPages[cTabControl.SelectedIndex].BackColor = Color.Black;
            mtb = cTabControl.TabPages[cTabControl.SelectedIndex].Controls.OfType<MTextBox>().First();

            mtb.BackColor = Color.FromArgb(64, 64, 64);
            mtb.ForeColor = Color.Cyan;
        }

        // Немного говнокода для сохранения загрузки списка последних файлов в файле Properties.RecentFiles
        private void LoadRecentFiles()
        {
            try
            {
                if (Properties.RecentFiles.Default.element0 != "Missing")
                {
                    ToolStripMenuItem tsmi = new ToolStripMenuItem();
                    tsmi.Text = Properties.RecentFiles.Default.element0;
                    tsmi.Click += (sender, e) => TextEditor.OpenFile(tsmi.Text);
                    recentFilesMenuItem.DropDownItems.Add(tsmi);
                }
                if (Properties.RecentFiles.Default.element1 != "Missing")
                {
                    ToolStripMenuItem tsmi = new ToolStripMenuItem();
                    tsmi.Text = Properties.RecentFiles.Default.element1;
                    tsmi.Click += (sender, e) => TextEditor.OpenFile(tsmi.Text);
                    recentFilesMenuItem.DropDownItems.Add(tsmi);
                }
                if (Properties.RecentFiles.Default.element2 != "Missing")
                {
                    ToolStripMenuItem tsmi = new ToolStripMenuItem();
                    tsmi.Text = Properties.RecentFiles.Default.element2;
                    tsmi.Click += (sender, e) => TextEditor.OpenFile(tsmi.Text);
                    recentFilesMenuItem.DropDownItems.Add(tsmi);
                }
                if (Properties.RecentFiles.Default.element3 != "Missing")
                {
                    ToolStripMenuItem tsmi = new ToolStripMenuItem();
                    tsmi.Text = Properties.RecentFiles.Default.element3;
                    tsmi.Click += (sender, e) => TextEditor.OpenFile(tsmi.Text);
                    recentFilesMenuItem.DropDownItems.Add(tsmi);
                }
                if (Properties.RecentFiles.Default.element4 != "Missing")
                {
                    ToolStripMenuItem tsmi = new ToolStripMenuItem();
                    tsmi.Text = Properties.RecentFiles.Default.element4;
                    tsmi.Click += (sender, e) => TextEditor.OpenFile(tsmi.Text);
                    recentFilesMenuItem.DropDownItems.Add(tsmi);
                }
                if (Properties.RecentFiles.Default.element5 != "Missing")
                {
                    ToolStripMenuItem tsmi = new ToolStripMenuItem();
                    tsmi.Text = Properties.RecentFiles.Default.element5;
                    tsmi.Click += (sender, e) => TextEditor.OpenFile(tsmi.Text);
                    recentFilesMenuItem.DropDownItems.Add(tsmi);
                }
                if (Properties.RecentFiles.Default.element6 != "Missing")
                {
                    ToolStripMenuItem tsmi = new ToolStripMenuItem();
                    tsmi.Text = Properties.RecentFiles.Default.element6;
                    tsmi.Click += (sender, e) => TextEditor.OpenFile(tsmi.Text);
                    recentFilesMenuItem.DropDownItems.Add(tsmi);
                }
                if (Properties.RecentFiles.Default.element7 != "Missing")
                {
                    ToolStripMenuItem tsmi = new ToolStripMenuItem();
                    tsmi.Text = Properties.RecentFiles.Default.element7;
                    tsmi.Click += (sender, e) => TextEditor.OpenFile(tsmi.Text);
                    recentFilesMenuItem.DropDownItems.Add(tsmi);
                }
                if (Properties.RecentFiles.Default.element8 != "Missing")
                {
                    ToolStripMenuItem tsmi = new ToolStripMenuItem();
                    tsmi.Text = Properties.RecentFiles.Default.element8;
                    tsmi.Click += (sender, e) => TextEditor.OpenFile(tsmi.Text);
                    recentFilesMenuItem.DropDownItems.Add(tsmi);
                }
                if (Properties.RecentFiles.Default.element9 != "Missing")
                {
                    ToolStripMenuItem tsmi = new ToolStripMenuItem();
                    tsmi.Text = Properties.RecentFiles.Default.element9;
                    tsmi.Click += (sender, e) => TextEditor.OpenFile(tsmi.Text);
                    recentFilesMenuItem.DropDownItems.Add(tsmi);
                }
            }
            catch { }
        }

        // Сохранение параметров программы
        internal void SaveParsameters()
        {
            // Сохранение настроек и недавних файлов
            Properties.Settings.Default.MainWindowState = this.WindowState.ToString();
            Properties.Settings.Default.FormWidth = this.Width;
            Properties.Settings.Default.FormHeight = this.Height;
            Properties.Settings.Default.StartScriptsConfig = startScriptCombobox.SelectedItem.ToString();
            Properties.Settings.Default.ExplorerSize = folderExplorerPanel.Width;
            Properties.Settings.Default.Save();

            Properties.RecentFiles.Default.element0 = "Missing";
            Properties.RecentFiles.Default.element1 = "Missing";
            Properties.RecentFiles.Default.element2 = "Missing";
            Properties.RecentFiles.Default.element3 = "Missing";
            Properties.RecentFiles.Default.element4 = "Missing";
            Properties.RecentFiles.Default.element5 = "Missing";
            Properties.RecentFiles.Default.element6 = "Missing";
            Properties.RecentFiles.Default.element7 = "Missing";
            Properties.RecentFiles.Default.element8 = "Missing";
            Properties.RecentFiles.Default.element9 = "Missing";

            try
            {
                if (recentFilesMenuItem.DropDownItems[0] != null)
                {
                    Properties.RecentFiles.Default.element0 = recentFilesMenuItem.DropDownItems[0].Text;
                }
                if (recentFilesMenuItem.DropDownItems[1] != null)
                {
                    Properties.RecentFiles.Default.element1 = recentFilesMenuItem.DropDownItems[1].Text;
                }
                if (recentFilesMenuItem.DropDownItems[2] != null)
                {
                    Properties.RecentFiles.Default.element2 = recentFilesMenuItem.DropDownItems[2].Text;
                }
                if (recentFilesMenuItem.DropDownItems[3] != null)
                {
                    Properties.RecentFiles.Default.element3 = recentFilesMenuItem.DropDownItems[3].Text;
                }
                if (recentFilesMenuItem.DropDownItems[4] != null)
                {
                    Properties.RecentFiles.Default.element4 = recentFilesMenuItem.DropDownItems[4].Text;
                }
                if (recentFilesMenuItem.DropDownItems[5] != null)
                {
                    Properties.RecentFiles.Default.element5 = recentFilesMenuItem.DropDownItems[5].Text;
                }
                if (recentFilesMenuItem.DropDownItems[6] != null)
                {
                    Properties.RecentFiles.Default.element6 = recentFilesMenuItem.DropDownItems[6].Text;
                }
                if (recentFilesMenuItem.DropDownItems[7] != null)
                {
                    Properties.RecentFiles.Default.element7 = recentFilesMenuItem.DropDownItems[7].Text;
                }
                if (recentFilesMenuItem.DropDownItems[8] != null)
                {
                    Properties.RecentFiles.Default.element8 = recentFilesMenuItem.DropDownItems[8].Text;
                }
                if (recentFilesMenuItem.DropDownItems[9] != null)
                {
                    Properties.RecentFiles.Default.element9 = recentFilesMenuItem.DropDownItems[9].Text;
                }
            }
            catch { }

            Properties.RecentFiles.Default.Save();
            LS.Debug("Saving parameters");
        }

        #region Методы для получения информации о сборке

        internal string GetAssemblyName()
        {
            string assembly = Assembly.GetExecutingAssembly().GetName().Name.ToString();
            return assembly;
        }

        internal string GetAssemblyVersion()
        {
            string assembly = Assembly.GetExecutingAssembly().GetName().Version.ToString();
            assembly = assembly.Remove(assembly.Length - 2);
            return assembly;
        }

        internal string GetAssemblyCompany()
        {
            object[] assemblyCustomAttributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyCompanyAttribute), false);
            return ((AssemblyCompanyAttribute)assemblyCustomAttributes[0]).Company.ToString();
        }

        #endregion

        // Обработчики загрузки и закрытия формы
        private void MainFormLoad(object sender, EventArgs e)
        {
            LS.Debug("Loading parameters");

            CreateTab();
            LoadRecentFiles();

            // Количество аргументов запуска программы
            string[] args = Environment.GetCommandLineArgs();
            if (args.Length > 1)
            {
                try
                {
                    mtb = cTabControl.TabPages[cTabControl.SelectedIndex].Controls.OfType<MTextBox>().First();
                    // вв свойство TextBox'a FileName помещаем путь до файла
                    cTabControl.TabPages[cTabControl.SelectedIndex].Controls.OfType<MTextBox>().First().FileName = args[1];
                    // помещаем текст
                    mtb.Text = File.ReadAllText(args[1]);
                    // задаём даголовок вкладки
                    cTabControl.SelectedTab.Text = Path.GetFileName(cTabControl.TabPages[cTabControl.SelectedIndex].Controls.OfType<MTextBox>().First().FileName);

                    if (recentFilesMenuItem.DropDownItems.Count == 10)
                        recentFilesMenuItem.DropDownItems.RemoveAt(0);
                    ToolStripMenuItem tsmi = new ToolStripMenuItem();
                    tsmi.Text = cTabControl.TabPages[cTabControl.SelectedIndex].Controls.OfType<MTextBox>().First().FileName;
                    tsmi.Click += (sender, e) => TextEditor.OpenFile(tsmi.Text);
                    recentFilesMenuItem.DropDownItems.Add(tsmi);
                }
                catch (Exception ex)
                {
                    LS.Error($"{ex} Error when trying to create a tab while running a program through a file: {cTabControl.TabPages[cTabControl.SelectedIndex].Controls.OfType<MTextBox>().First().FileName}");
                    MessageBox.Show(ex.Message, "TextPad+", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

            // Загрузка параметров из файла Propertis/Default/Settings.settings.
            Width = Properties.Settings.Default.FormWidth;
            Height = Properties.Settings.Default.FormHeight;
            switch (Properties.Settings.Default.Topmost)
            {
                case true:
                    TopMost = true;
                    break;
                case false:
                    TopMost = false;
                    break;
            }
            toolsStrip.Visible = Properties.Settings.Default.InstrumentalPanel;
            runFileToolStrip.Visible = Properties.Settings.Default.RunFileToolbar;
            statusStrip.Visible = Properties.Settings.Default.StatusStripVisible;
            folderExplorerPanel.Visible = Properties.Settings.Default.Explorer;
            startScriptCombobox.SelectedItem = Properties.Settings.Default.StartScriptsConfig;
            folderExplorerPanel.Width = Properties.Settings.Default.ExplorerSize;
            switch (Properties.Settings.Default.Theme)
            {
                case "White":
                    colorThemeWhite();
                    colorThemeWhite();
                    break;

                case "Dark":
                    colorThemeDark();
                    colorThemeDark();
                    break;
            }
            switch (Properties.Settings.Default.MainWindowState)
            {
                case "Normal":
                    this.WindowState = FormWindowState.Normal;
                    break;
                case "Maximized":
                    this.WindowState = FormWindowState.Maximized;
                    break;
                case "Minimized":
                    this.WindowState = FormWindowState.Normal;
                    break;
                default:
                    this.WindowState = FormWindowState.Normal;
                    break;
            }

            checkFiles();

            LS.Debug("Options loaded");
            LS.Info($"Program path: {ProgramPath}");
        }

        private void MainFormClosing(object sender, FormClosingEventArgs e)
        {

            // Проверка статуса обновления
            if (Program.isUpdating == true)
            {
                return;
            }
            else
            {
                // Закрытие всех вкладок
                foreach (TabPage tab in cTabControl.TabPages)
                {
                    mtb = cTabControl.TabPages[cTabControl.TabPages.IndexOf(tab)].Controls.OfType<MTextBox>().First();

                    if (mtb.TextLength != 0)
                    {
                        DialogResult dr = MessageBox.Show($"{Resources.Localization.MSGQuestionSaveFile} \"{tab.Text}\"?", "TextPad+", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                        if (dr == DialogResult.Yes)
                        {
                            TextEditor.SaveCurrentFile();

                        }
                        else if (dr == DialogResult.No)
                        {
                            cTabControl.TabPages.Remove(tab);

                        }
                        else if (dr == DialogResult.Cancel)
                        {
                            e.Cancel = true;
                            return;
                        }
                    }
                    else
                    {
                        cTabControl.TabPages.Remove(tab);
                    }
                }
            }

            SaveParsameters();
            Process.GetCurrentProcess().Kill();

            LS.Debug("Exiting the program");
        }
    }
}
