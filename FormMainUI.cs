
using Microsoft.VisualBasic.FileIO;
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

        // Обработчик запуска файлов
        private readonly IFileRunner FileRunner = new TextEditor();

        // Авто свойства для чтения с инфой о программе
        public string DateOfRelease { get; } = "DEVELOPEMENT";
        public string ProgramPath { get; } = Application.StartupPath;
        public string WebSite { get; private set; } = "https://mr-nichosik.github.io/Main_Page/";

        // Общее поле для Rich Text Box
        private MTextBox? mtb;

        // Поле для настроек программы. Если isLangChanched = true, то при выходе из настроек появится сообщение о необходимости перезапустить программу
        private static bool isLanguageChanged = false;
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

            Program.MainUI = this;

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

            LSVersionLabel.Text = LogSystem.GetAssemblyVersion();
            CTCLabelVersion.Text = CTabControl.GetAssemblyVersion();
            MTBVersionLabel.Text = MTextBox.GetAssemblyVersion();

            // Загрузка настроек
            statusStripCheckBox.Checked = Properties.Settings.Default.StatusStrip_Visible;
            overWindowsCheckBox.Checked = Properties.Settings.Default.FormMainUI_Topmost;
            runFilesPanelCheckBox.Checked = Properties.Settings.Default.RunFileToolStrip_Visible;
            wordWarpCheckBox.Checked = Properties.Settings.Default.ModifiedTextBox_WordWarp;
            instrumentPanelCheckBox.Checked = Properties.Settings.Default.ToolStrip_Visible;
            wordWarpCheckBox.Checked = Properties.Settings.Default.ModifiedTextBox_WordWarp;
            explorerCheckBox.Checked = Properties.Settings.Default.FolderExplorerPanel_Visible;
            exitWhenClosingLastTabCheckBox.Checked = Properties.Settings.Default.ExitWhenClosingLastTab;
            autoUpdateCheckBox.Checked = Properties.Settings.Default.AutoCheckUpdate;

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

            FontTextBox.Text = Properties.Settings.Default.ModifiedTextBox_Font.ToString();
        }

        private void saveSettings(object sender, EventArgs e)
        {
            var mtb = cTabControl.TabPages[cTabControl.SelectedIndex].Controls.OfType<MTextBox>().First();

            // Проверка настроек языка
            switch (comboBoxLanguage.SelectedItem)
            {
                case @"English / Английский":
                    if (Properties.Settings.Default.Language != "English")
                        isLanguageChanged = true;

                    Properties.Settings.Default.Language = "English";
                    break;
                case @"Русский / Russian":
                    if (Properties.Settings.Default.Language != "Russian")
                        isLanguageChanged = true;

                    Properties.Settings.Default.Language = "Russian";
                    break;
            }

            // Проверка настроек цветовой схемы
            switch (comboTheme.SelectedItem)
            {
                case "Белая / White":
                    Properties.Settings.Default.Theme = "White";

                    // на текущей вкладке
                    colorThemeWhite();
                    break;
                case "Тёмная / Dark":
                    Properties.Settings.Default.Theme = "Dark";

                    // на текущей вкладке
                    colorThemeDark();
                    break;
            }

            // Проверка настроек строки состояния
            StatusStrip.Visible = statusStripCheckBox.Checked;
            Properties.Settings.Default.StatusStrip_Visible = statusStripCheckBox.Checked;

            // Проверка настроек TopMost'а
            TopMost = overWindowsCheckBox.Checked;
            Properties.Settings.Default.FormMainUI_Topmost = overWindowsCheckBox.Checked;

            // Проверка настроек панели запуска файлов
            RunFileToolStrip.Visible = runFilesPanelCheckBox.Checked;
            Properties.Settings.Default.RunFileToolStrip_Visible = runFilesPanelCheckBox.Checked;

            // Проверка настроек переноса слов
            Properties.Settings.Default.ModifiedTextBox_WordWarp = wordWarpCheckBox.Checked;
            mtb.WordWrap = wordWarpCheckBox.Checked;

            // Проверка настроек панели инструментов
            Properties.Settings.Default.ToolStrip_Visible = instrumentPanelCheckBox.Checked;
            ToolStrip.Visible = instrumentPanelCheckBox.Checked;

            // Проверка настроек обозревателя папок
            Properties.Settings.Default.FolderExplorerPanel_Visible = explorerCheckBox.Checked;
            FolderExplorerPanel.Visible = explorerCheckBox.Checked;

            // Нужно ли закрывать программу при закрытии последней вкладки
            Properties.Settings.Default.ExitWhenClosingLastTab = exitWhenClosingLastTabCheckBox.Checked;

            // Нужно ли проверять наличие обновлений при входе
            Properties.Settings.Default.AutoCheckUpdate = autoUpdateCheckBox.Checked;

            // Сообщение о перезапуске программы
            if (isLanguageChanged == true)
            {
                MessageBox.Show(Resources.Localization.SettingsLangChange, "TextPad+", MessageBoxButtons.OK, MessageBoxIcon.Information);
                isLanguageChanged = false;
            }

            Properties.Settings.Default.Save();

            SettingsUIPanel.Visible = false;
            MainUIPanel.Visible = true;
        }

        private void closeSettings(object sender, EventArgs e)
        {
            MainUIPanel.Visible = true;
            SettingsUIPanel.Visible = false;
        }

        private void fontSettings(object sender, EventArgs e)
        {
            fontDialog.ShowDialog();
            FontTextBox.Text = fontDialog.Font.ToString();

            Properties.Settings.Default.ModifiedTextBox_Font = fontDialog.Font;
            Properties.Settings.Default.Save();

            var mtb = cTabControl.TabPages[cTabControl.SelectedIndex].Controls.OfType<MTextBox>().First();
            mtb.Font = fontDialog.Font;
        }

        private void openWebSite(object sender, EventArgs e)
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
            Program.UpdaterUI.ShowDialog(this);
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
                AllowDrop = true,
                EnableAutoDragDrop = false,
                Dock = DockStyle.Fill,
                BorderStyle = BorderStyle.None,
                WordWrap = Properties.Settings.Default.ModifiedTextBox_WordWarp,
                ContextMenuStrip = contextMenuStrip,
                Font = Properties.Settings.Default.ModifiedTextBox_Font
            };
            mtb.TextChanged += (sender, args) => TextBoxTextChanged();
            mtb.AcceptsTab = true;
            mtb.DragEnter += new DragEventHandler(FileDragEnter!);
            mtb.DragDrop += new DragEventHandler(FileDragDrop!);
            mtb.KeyPress += new KeyPressEventHandler(TextBoxKeyPress!);

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
            var mtb = new MTextBox
            {
                AllowDrop = true,
                EnableAutoDragDrop = false,
                Dock = DockStyle.Fill,
                BorderStyle = BorderStyle.None,
                WordWrap = Properties.Settings.Default.ModifiedTextBox_WordWarp,
                ContextMenuStrip = contextMenuStrip,
                Font = Properties.Settings.Default.ModifiedTextBox_Font
            };
            mtb.TextChanged += (sender, args) => TextBoxTextChanged();
            mtb.AcceptsTab = true;
            mtb.DragEnter += new DragEventHandler(FileDragEnter!);
            mtb.DragDrop += new DragEventHandler(FileDragDrop!);
            mtb.KeyPress += new KeyPressEventHandler(TextBoxKeyPress!);

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

                mtb.WordWrap = Properties.Settings.Default.ModifiedTextBox_WordWarp;
                mtb.Font = Properties.Settings.Default.ModifiedTextBox_Font;
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

        // Изменение текста в Modified TextBox
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

        private void TextBoxKeyPress(object sender, KeyPressEventArgs e)
        {

        }

        // Методы для перетаскивания и скидывания файлов и текста
        private void FileDragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.Text))
                e.Effect = DragDropEffects.Copy;

            if (e.Data.GetDataPresent(DataFormats.FileDrop))
                e.Effect = DragDropEffects.Copy;
        }

        private void FileDragDrop(object sender, DragEventArgs e)
        {
            mtb = cTabControl.TabPages[cTabControl.SelectedIndex].Controls.OfType<MTextBox>().First();

            if (e.Data.GetDataPresent(DataFormats.Text))
                mtb.Text += e.Data.GetData(DataFormats.Text);

            else if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
                TextEditor.OpenFile(files[0]);
            }
        }

        // Выбор кодировки
        private void ChangeToUTF32BE(object sender, EventArgs e)
        {
            mtb = cTabControl.TabPages[cTabControl.SelectedIndex].Controls.OfType<MTextBox>().First();
            mtb.Encoding = "UTF-32 BE";
            encodingStatusLabel.Text = mtb.Encoding.ToString();
        }
        private void ChangeToUTF32(object sender, EventArgs e)
        {
            mtb = cTabControl.TabPages[cTabControl.SelectedIndex].Controls.OfType<MTextBox>().First();
            mtb.Encoding = "UTF-32";
            encodingStatusLabel.Text = mtb.Encoding.ToString();
        }
        private void ChangeToUTF16BE(object sender, EventArgs e)
        {
            mtb = cTabControl.TabPages[cTabControl.SelectedIndex].Controls.OfType<MTextBox>().First();
            mtb.Encoding = "UTF-16 BE";
            encodingStatusLabel.Text = mtb.Encoding.ToString();
        }
        private void ChangeToUTF16(object sender, EventArgs e)
        {
            mtb = cTabControl.TabPages[cTabControl.SelectedIndex].Controls.OfType<MTextBox>().First();
            mtb.Encoding = "UTF-16";
            encodingStatusLabel.Text = mtb.Encoding.ToString();
        }
        private void ChangeToUTFWindows1251(object sender, EventArgs e)
        {
            mtb = cTabControl.TabPages[cTabControl.SelectedIndex].Controls.OfType<MTextBox>().First();
            mtb.Encoding = "windows-1251";
            encodingStatusLabel.Text = mtb.Encoding.ToString();
        }
        private void ChangeToUTF8(object sender, EventArgs e)
        {
            mtb = cTabControl.TabPages[cTabControl.SelectedIndex].Controls.OfType<MTextBox>().First();
            mtb.Encoding = "UTF-8";
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
                    ListViewItem lvi = new(new DirectoryInfo(item).Name, 0);
                    lvi.ToolTipText = item;
                    FilesListView.Items.Add(lvi);

                }

                // Файлы
                foreach (string item in Directory.GetFiles(path))
                {
                    ListViewItem lvi = new(new DirectoryInfo(item).Name, 1);
                    lvi.ToolTipText = item;
                    FilesListView.Items.Add(lvi);
                }
            }

            else if (File.Exists(path))
            {
                TextEditor.OpenFile(path);
            }
        }

        private void UpSizeListView(object sender, EventArgs e)
        {
            if (FilesListView.Width < 410)
            {
                FilesListView.Width += 30;
                FolderExplorerPanel.Width += 30;
            }
        }

        private void DownSizeListView(object sender, EventArgs e)
        {
            if (FilesListView.Width > 110)
            {
                FilesListView.Width -= 30;
                FolderExplorerPanel.Width -= 30;
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
            FormPythonInterpreterUI pythonInterpreterUI = new();
            pythonInterpreterUI.ShowDialog(this);
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

        // Проверяет некоторые свойства файла, что бы включить/выключить часть кнопок
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

        // Немного говнокода для загрузки списка последних файлов в файле Properties.RecentFiles
        private void LoadRecentFiles()
        {
            try
            {
                if (Properties.RecentFiles.Default.RecentFile0 != "Missing")
                {
                    ToolStripMenuItem tsmi = new ToolStripMenuItem();
                    tsmi.Text = Properties.RecentFiles.Default.RecentFile0;
                    tsmi.Click += (sender, e) => TextEditor.OpenFile(tsmi.Text);
                    recentFilesMenuItem.DropDownItems.Add(tsmi);
                }
                if (Properties.RecentFiles.Default.RecentFile1 != "Missing")
                {
                    ToolStripMenuItem tsmi = new ToolStripMenuItem();
                    tsmi.Text = Properties.RecentFiles.Default.RecentFile1;
                    tsmi.Click += (sender, e) => TextEditor.OpenFile(tsmi.Text);
                    recentFilesMenuItem.DropDownItems.Add(tsmi);
                }
                if (Properties.RecentFiles.Default.RecentFile2 != "Missing")
                {
                    ToolStripMenuItem tsmi = new ToolStripMenuItem();
                    tsmi.Text = Properties.RecentFiles.Default.RecentFile2;
                    tsmi.Click += (sender, e) => TextEditor.OpenFile(tsmi.Text);
                    recentFilesMenuItem.DropDownItems.Add(tsmi);
                }
                if (Properties.RecentFiles.Default.RecentFile3 != "Missing")
                {
                    ToolStripMenuItem tsmi = new ToolStripMenuItem();
                    tsmi.Text = Properties.RecentFiles.Default.RecentFile3;
                    tsmi.Click += (sender, e) => TextEditor.OpenFile(tsmi.Text);
                    recentFilesMenuItem.DropDownItems.Add(tsmi);
                }
                if (Properties.RecentFiles.Default.RecentFile4 != "Missing")
                {
                    ToolStripMenuItem tsmi = new ToolStripMenuItem();
                    tsmi.Text = Properties.RecentFiles.Default.RecentFile4;
                    tsmi.Click += (sender, e) => TextEditor.OpenFile(tsmi.Text);
                    recentFilesMenuItem.DropDownItems.Add(tsmi);
                }
                if (Properties.RecentFiles.Default.RecentFile5 != "Missing")
                {
                    ToolStripMenuItem tsmi = new ToolStripMenuItem();
                    tsmi.Text = Properties.RecentFiles.Default.RecentFile5;
                    tsmi.Click += (sender, e) => TextEditor.OpenFile(tsmi.Text);
                    recentFilesMenuItem.DropDownItems.Add(tsmi);
                }
                if (Properties.RecentFiles.Default.RecentFile6 != "Missing")
                {
                    ToolStripMenuItem tsmi = new ToolStripMenuItem();
                    tsmi.Text = Properties.RecentFiles.Default.RecentFile6;
                    tsmi.Click += (sender, e) => TextEditor.OpenFile(tsmi.Text);
                    recentFilesMenuItem.DropDownItems.Add(tsmi);
                }
                if (Properties.RecentFiles.Default.RecentFile7 != "Missing")
                {
                    ToolStripMenuItem tsmi = new ToolStripMenuItem();
                    tsmi.Text = Properties.RecentFiles.Default.RecentFile7;
                    tsmi.Click += (sender, e) => TextEditor.OpenFile(tsmi.Text);
                    recentFilesMenuItem.DropDownItems.Add(tsmi);
                }
                if (Properties.RecentFiles.Default.RecentFile8 != "Missing")
                {
                    ToolStripMenuItem tsmi = new ToolStripMenuItem();
                    tsmi.Text = Properties.RecentFiles.Default.RecentFile8;
                    tsmi.Click += (sender, e) => TextEditor.OpenFile(tsmi.Text);
                    recentFilesMenuItem.DropDownItems.Add(tsmi);
                }
                if (Properties.RecentFiles.Default.RecentFile9 != "Missing")
                {
                    ToolStripMenuItem tsmi = new ToolStripMenuItem();
                    tsmi.Text = Properties.RecentFiles.Default.RecentFile9;
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
            Properties.Settings.Default.FormMainUI_WindowState = this.WindowState.ToString();
            switch (this.WindowState.ToString())
            {
                case "Maximized":
                    break;
                default:
                    Properties.Settings.Default.FormMainUI_Width = this.Width;
                    Properties.Settings.Default.FormMainUI_Height = this.Height;
                    break;
            }
            Properties.Settings.Default.ScriptTypeToRun = startScriptCombobox.SelectedItem.ToString();
            Properties.Settings.Default.FolderExplorerPanel_Size = FolderExplorerPanel.Width;
            Properties.Settings.Default.Save();

            Properties.RecentFiles.Default.RecentFile0 = "Missing";
            Properties.RecentFiles.Default.RecentFile1 = "Missing";
            Properties.RecentFiles.Default.RecentFile2 = "Missing";
            Properties.RecentFiles.Default.RecentFile3 = "Missing";
            Properties.RecentFiles.Default.RecentFile4 = "Missing";
            Properties.RecentFiles.Default.RecentFile5 = "Missing";
            Properties.RecentFiles.Default.RecentFile6 = "Missing";
            Properties.RecentFiles.Default.RecentFile7 = "Missing";
            Properties.RecentFiles.Default.RecentFile8 = "Missing";
            Properties.RecentFiles.Default.RecentFile9 = "Missing";

            try
            {
                if (recentFilesMenuItem.DropDownItems[0] != null)
                {
                    Properties.RecentFiles.Default.RecentFile0 = recentFilesMenuItem.DropDownItems[0].Text;
                }
                if (recentFilesMenuItem.DropDownItems[1] != null)
                {
                    Properties.RecentFiles.Default.RecentFile1 = recentFilesMenuItem.DropDownItems[1].Text;
                }
                if (recentFilesMenuItem.DropDownItems[2] != null)
                {
                    Properties.RecentFiles.Default.RecentFile2 = recentFilesMenuItem.DropDownItems[2].Text;
                }
                if (recentFilesMenuItem.DropDownItems[3] != null)
                {
                    Properties.RecentFiles.Default.RecentFile3 = recentFilesMenuItem.DropDownItems[3].Text;
                }
                if (recentFilesMenuItem.DropDownItems[4] != null)
                {
                    Properties.RecentFiles.Default.RecentFile4 = recentFilesMenuItem.DropDownItems[4].Text;
                }
                if (recentFilesMenuItem.DropDownItems[5] != null)
                {
                    Properties.RecentFiles.Default.RecentFile5 = recentFilesMenuItem.DropDownItems[5].Text;
                }
                if (recentFilesMenuItem.DropDownItems[6] != null)
                {
                    Properties.RecentFiles.Default.RecentFile6 = recentFilesMenuItem.DropDownItems[6].Text;
                }
                if (recentFilesMenuItem.DropDownItems[7] != null)
                {
                    Properties.RecentFiles.Default.RecentFile7 = recentFilesMenuItem.DropDownItems[7].Text;
                }
                if (recentFilesMenuItem.DropDownItems[8] != null)
                {
                    Properties.RecentFiles.Default.RecentFile8 = recentFilesMenuItem.DropDownItems[8].Text;
                }
                if (recentFilesMenuItem.DropDownItems[9] != null)
                {
                    Properties.RecentFiles.Default.RecentFile9 = recentFilesMenuItem.DropDownItems[9].Text;
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

            StatusLabel.Text = "Загрузка...";

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
            Width = Properties.Settings.Default.FormMainUI_Width;
            Height = Properties.Settings.Default.FormMainUI_Height;
            switch (Properties.Settings.Default.FormMainUI_Topmost)
            {
                case true:
                    TopMost = true;
                    break;
                case false:
                    TopMost = false;
                    break;
            }
            ToolStrip.Visible = Properties.Settings.Default.ToolStrip_Visible;
            RunFileToolStrip.Visible = Properties.Settings.Default.RunFileToolStrip_Visible;
            StatusStrip.Visible = Properties.Settings.Default.StatusStrip_Visible;
            FolderExplorerPanel.Visible = Properties.Settings.Default.FolderExplorerPanel_Visible;
            startScriptCombobox.SelectedItem = Properties.Settings.Default.ScriptTypeToRun;
            FolderExplorerPanel.Width = Properties.Settings.Default.FolderExplorerPanel_Size;
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
            switch (Properties.Settings.Default.FormMainUI_WindowState)
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
            if (Properties.Settings.Default.AutoCheckUpdate == true)
            {
                StatusLabel.Text = "Проверка обновлений...";
                Updater.GetUpdateQuiet(Program.UpdaterUI.UpdateIatestVerL, Program.UpdaterUI.UpdateInfoTextBox, Program.UpdaterUI.UpdateStatusLabel, Program.UpdaterUI.UpdateStatusProgressBar);
            }

            checkFiles();

            if (Program.UpdateStatus > 0)
                StatusLabel.Text = "Выполняется обновление...";
            else
                StatusLabel.Text = "Готово";

            LS.Debug("Options loaded");
            LS.Info($"Program path: {ProgramPath}");
        }

        private void MainFormClosing(object sender, FormClosingEventArgs e)
        {

            // Проверка статуса обновления
            if (Program.UpdateStatus == 1)
            {
                if (MessageBox.Show("Прервать установку обновления?", Resources.Localization.UPDATERTitle, MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
                    return;
                else
                {
                    e.Cancel = true;
                    return;
                }

            }
            else if (Program.UpdateStatus == 2)
            {
                Process.GetCurrentProcess().Kill();
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
