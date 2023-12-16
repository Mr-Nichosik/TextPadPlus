
using Microsoft.VisualBasic.FileIO;

namespace TextPad_
{
    /// <summary>
    /// Класс главного окна программы, где обрабатывается внешний вид и пользовательский UI.
    /// Всю (наверное) логику работы программы я переместил в класс TextEditor.
    /// <summary>
    internal sealed partial class MainUI : Form
    {
        // Logger
        private readonly LogSystem Logger = new() { UserFolderName = $"{Application.StartupPath}\\logs" };

        // Общее поле для Modified TextBox
        private MTextBox? mtb;

        // Поле для настроек программы. Если равно true, то при выходе из настроек появится сообщение о необходимости перезапустить программу
        private bool isLanguageChanged = false;

        // Конструктор класса
        public MainUI()
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

            Logger.Info("MainUI Initialization");
            InitializeComponent();

            Logger.Debug("Program launched successfully");
        }

        // Переопределение OnKeyDown для обработки сочетаний клавиш
        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);

            if (e.KeyCode == Keys.V && e.Control)
            {
                e.Handled = true;
                MTextBoxPaste();
            }

            if (e.KeyCode == Keys.F && e.Control && MainUIPanel.Visible == true && SearchPanel.Visible == false)
            {
                if (cTabControl.TabPages[cTabControl.SelectedIndex].Controls.OfType<MTextBox>().First().TextLength > 0)
                {
                    SearchPanel.Visible = true;
                }
                e.Handled = true;
            }
            else if (e.KeyCode == Keys.F && e.Control && MainUIPanel.Visible == true && SearchPanel.Visible == true)
                SearchPanel.Visible = false;

            if (e.KeyCode == Keys.Z && e.Control && e.Shift)
            {
                cTabControl.TabPages[cTabControl.SelectedIndex].Controls.OfType<MTextBox>().First().Redo();
            }
        }

        #region Обработка событий разных элементов формы

        // Методы для кнопок панели инструментов, контекстного и главного меню
        private void SaveAsFile(object sender, EventArgs e) => FileWorker.SaveAsFile();

        private void SaveFile(object sender, EventArgs e) => FileWorker.SaveFile();

        private void SaveAllFiles(object sender, EventArgs e) => FileWorker.SaveAllFiles();

        private void AutoSave(object sender, EventArgs e) => FileWorker.SaveAllFiles(false);

        private void OpenFile(object sender, EventArgs e) => FileWorker.OpenFile();

        private void ReopenFile(object sender, EventArgs e) => FileWorker.ReopenFile();

        private void MTextBoxCopy(object sender, EventArgs e)
        {
            mtb = cTabControl.TabPages[cTabControl.SelectedIndex].Controls.OfType<MTextBox>().First();
            if (mtb.TextLength > 0)
                mtb.Copy();
        }

        private void MTextBoxCut(object sender, EventArgs e)
        {
            mtb = cTabControl.TabPages[cTabControl.SelectedIndex].Controls.OfType<MTextBox>().First();
            if (mtb.TextLength > 0)
                mtb.Cut();
        }

        private void MTextBoxPaste()
        {
            cTabControl.TabPages[cTabControl.SelectedIndex].Controls.OfType<MTextBox>().First().Paste(DataFormats.GetFormat(DataFormats.Text));
        }

        private void MTextBoxPaste(object sender, EventArgs e) => MTextBoxPaste();

        private void MTextBoxSelectFont(object sender, EventArgs e)
        {
            mtb = cTabControl.TabPages[cTabControl.SelectedIndex].Controls.OfType<MTextBox>().First();
            FontDialog_.ShowDialog();
            mtb.Font = FontDialog_.Font;
            Properties.Settings.Default.ModifiedTextBox_Font = FontDialog_.Font;
        }

        private void MTextBoxSelectAllText(object sender, EventArgs e)
        {
            mtb = cTabControl.TabPages[cTabControl.SelectedIndex].Controls.OfType<MTextBox>().First();
            if (mtb.TextLength > 0)
                mtb.SelectAll();
        }

        private void MTextBoxUndo(object sender, EventArgs e)
        {
            cTabControl.TabPages[cTabControl.SelectedIndex].Controls.OfType<MTextBox>().First().Undo();
        }

        private void MTextBoxRedo(object sender, EventArgs e)
        {
            cTabControl.TabPages[cTabControl.SelectedIndex].Controls.OfType<MTextBox>().First().Redo();
        }

        private void MTextBoxDeleteText(object sender, EventArgs e)
        {
            mtb = cTabControl.TabPages[cTabControl.SelectedIndex].Controls.OfType<MTextBox>().First();
            if (mtb.TextLength > 0)
                mtb.SelectedText = "";
        }

        private void MTextBoxInsertDateAndTime(object sender, EventArgs e)
        {
            cTabControl.TabPages[cTabControl.SelectedIndex].Controls.OfType<MTextBox>().First().SelectedText = DateTime.Now.ToString();
        }

        private void OpenFileFolder(object sender, EventArgs e)
        {
            try
            {
                if (cTabControl.TabPages[cTabControl.SelectedIndex].Controls.OfType<MTextBox>().First().FileName != "Missing")
                    Process.Start("explorer.exe", Path.GetDirectoryName(cTabControl.TabPages[cTabControl.SelectedIndex].Controls.OfType<MTextBox>().First().FileName)!);
            }
            catch { }
        }

        private void DeleteFile(object sender, EventArgs e)
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
                    MessageBox.Show(Resources.Localization.MSGErrorCantDeleteFile, "TextPad+", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            catch (Exception ex) { Logger.Error($"An error while deleting the file (?). \n{ex}"); }
        }

        private void CopyFullPath(object sender, EventArgs e)
        {
            if (cTabControl.TabPages[cTabControl.SelectedIndex].Controls.OfType<MTextBox>().First().FileName != "Missing")
                Clipboard.SetText(cTabControl.TabPages[cTabControl.SelectedIndex].Controls.OfType<MTextBox>().First().FileName);
            else
                return;
        }

        private void CopyFileName(object sender, EventArgs e)
        {
            if (cTabControl.TabPages[cTabControl.SelectedIndex].Controls.OfType<MTextBox>().First().FileName != "Missing")
                Clipboard.SetText(Path.GetFileName(cTabControl.TabPages[cTabControl.SelectedIndex].Controls.OfType<MTextBox>().First().FileName));
            else
                return;
        }

        private void ClearRecentFilesList(object sender, EventArgs e)
        {
            RecentFilesListFileItem.DropDownItems.Clear();

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
            Properties.RecentFiles.Default.Save();
        }

        private void RegisterToUpper(object sender, EventArgs e)
        {
            mtb = cTabControl.TabPages[cTabControl.SelectedIndex].Controls.OfType<MTextBox>().First();
            mtb.SelectedText = mtb.SelectedText.ToUpper();
        }

        private void RegisterToLower(object sender, EventArgs e)
        {
            mtb = cTabControl.TabPages[cTabControl.SelectedIndex].Controls.OfType<MTextBox>().First();
            mtb.SelectedText = mtb.SelectedText.ToLower();
        }

        // Методы для настроек
        private void OpenSettings(object sender, EventArgs e)
        {
            MainUIPanel.Visible = false;
            SettingsUIPanel.Visible = true;

            textBoxWebSiteUrl.Text = Program.WebSite;
            ProgramNameLabel.Text = $"{Program.Name} {Program.Version}";
            VersionLabel.Text = Program.Version;
            DateOfReleaseLabel.Text = Program.DateOfRelease;

            LSVersionLabel.Text = LogSystem.Version;
            CTCLabelVersion.Text = CTabControl.Version;
            MTBVersionLabel.Text = MTextBox.Version;

            // Загрузка настроек
            StatusBarCheckBox.Checked = Properties.Settings.Default.StatusStrip_Visible;
            TopmostCheckBox.Checked = Properties.Settings.Default.FormMainUI_Topmost;
            RunFileToolBarCheckBox.Checked = Properties.Settings.Default.RunFileToolStrip_Visible;
            WordWarpCheckBox.Checked = Properties.Settings.Default.ModifiedTextBox_WordWarp;
            ToolBarCheckBox.Checked = Properties.Settings.Default.ToolStrip_Visible;
            WordWarpCheckBox.Checked = Properties.Settings.Default.ModifiedTextBox_WordWarp;
            ExplorerCheckBox.Checked = Properties.Settings.Default.FolderExplorerPanel_Visible;
            ExitWhenClosingLastTabCheckBox.Checked = Properties.Settings.Default.ExitWhenClosingLastTab;
            AutoChekUpdateCheckBox.Checked = Properties.Settings.Default.AutoCheckUpdate;
            AutoSubstitutionCheckBox.Checked = Properties.Settings.Default.AutoSubstitutionOfClosingCharacters;
            FontTextBox.Text = Properties.Settings.Default.ModifiedTextBox_Font.ToString();
            FontDialog_.Font = Properties.Settings.Default.ModifiedTextBox_Font;
            DetectUrlsCheckBox.Checked = Properties.Settings.Default.MTextBox_DetectUrls;

            AutoSaveTimeComboBox.SelectedItem = Properties.Settings.Default.AutoSaveTime switch
            {
                1 => "При каждом изменении текста (On text changed)",
                60000 => "1 Минута (Minute)",
                120000 => "2 Минуты (Minutes)",
                300000 => "5 Минут (Minutes)",
                _ => "Выключить (Disable)",
            };
            ColorThemeComboBox.SelectedItem = Properties.Settings.Default.Theme switch
            {
                "Dark" => "Тёмная / Dark",
                _ => "Белая / White",
            };

            LanguageComboBox.SelectedItem = Properties.Settings.Default.Language switch
            {
                "English" => "English / Английский",
                _ => "Русский / Russian"
            };
        }

        private void SaveSettings(object sender, EventArgs e)
        {
            mtb = cTabControl.TabPages[cTabControl.SelectedIndex].Controls.OfType<MTextBox>().First();

            // Языка
            switch (LanguageComboBox.SelectedItem)
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

            // Цветовая схема
            if (ColorThemeComboBox.SelectedIndex == 1)
            {
                Properties.Settings.Default.Theme = "Dark";
                ColorThemeDark();
            }
            else
            {
                Properties.Settings.Default.Theme = "White";
                ColorThemeWhite();
            }

            // Шрифт
            Properties.Settings.Default.ModifiedTextBox_Font = FontDialog_.Font;
            Properties.Settings.Default.Save();
            FontTextBox.Text = FontDialog_.Font.ToString();
            mtb.Font = FontDialog_.Font;

            // Стркоа состояния
            Properties.Settings.Default.StatusStrip_Visible = StatusBarCheckBox.Checked;
            StatusBar.Visible = StatusBarCheckBox.Checked;

            // Topmost
            Properties.Settings.Default.FormMainUI_Topmost = TopmostCheckBox.Checked;
            TopMost = TopmostCheckBox.Checked;

            // Панель запуска файлов
            Properties.Settings.Default.RunFileToolStrip_Visible = RunFileToolBarCheckBox.Checked;
            RunFileToolStrip.Visible = RunFileToolBarCheckBox.Checked;

            // Перенос слов
            Properties.Settings.Default.ModifiedTextBox_WordWarp = WordWarpCheckBox.Checked;
            mtb.WordWrap = WordWarpCheckBox.Checked;

            // Панель инструментов
            Properties.Settings.Default.ToolStrip_Visible = ToolBarCheckBox.Checked;
            ToolBar.Visible = ToolBarCheckBox.Checked;

            // Проводник
            Properties.Settings.Default.FolderExplorerPanel_Visible = ExplorerCheckBox.Checked;
            FolderExplorerPanel.Visible = ExplorerCheckBox.Checked;
            ExplorerSplitter.Visible = ExplorerCheckBox.Checked;

            // Нужно ли закрывать программу при закрытии последней вкладки
            Properties.Settings.Default.ExitWhenClosingLastTab = ExitWhenClosingLastTabCheckBox.Checked;

            // Нужно ли проверять наличие обновлений при входе
            Properties.Settings.Default.AutoCheckUpdate = AutoChekUpdateCheckBox.Checked;

            // Нужно ли подставлять закрывающие символы
            Properties.Settings.Default.AutoSubstitutionOfClosingCharacters = AutoSubstitutionCheckBox.Checked;

            // Время автосохранения
            switch (AutoSaveTimeComboBox.SelectedItem)
            {
                case "При каждом изменении текста (On text changed)":
                    Properties.Settings.Default.AutoSaveTime = 1;
                    AutoSaveTimer.Enabled = false;
                    break;
                case "1 Минута (Minute)":
                    Properties.Settings.Default.AutoSaveTime = 60000;
                    AutoSaveTimer.Interval = Properties.Settings.Default.AutoSaveTime;
                    AutoSaveTimer.Enabled = true;
                    break;

                case "2 Минуты (Minutes)":
                    Properties.Settings.Default.AutoSaveTime = 120000;
                    AutoSaveTimer.Interval = Properties.Settings.Default.AutoSaveTime;
                    AutoSaveTimer.Enabled = true;
                    break;

                case "5 Минут (Minutes)":
                    Properties.Settings.Default.AutoSaveTime = 300000;
                    AutoSaveTimer.Interval = Properties.Settings.Default.AutoSaveTime;
                    AutoSaveTimer.Enabled = true;
                    break;

                default:
                    Properties.Settings.Default.AutoSaveTime = 0;
                    AutoSaveTimer.Enabled = false;
                    break;
            }

            // Подсветка ссылок
            Properties.Settings.Default.MTextBox_DetectUrls = DetectUrlsCheckBox.Checked;
            mtb.DetectUrls = DetectUrlsCheckBox.Checked;

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

        private void CloseSettings(object sender, EventArgs e)
        {
            MainUIPanel.Visible = true;
            SettingsUIPanel.Visible = false;
        }

        private void ChangeFont(object sender, EventArgs e) => FontDialog_.ShowDialog();

        private void OpenWebSite(object sender, EventArgs e)
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
                MessageBox.Show(Resources.Localization.MSGErrorWhenCheckingInternet, "TextPad+", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Logger.Error($"The program's website could not be opened. There may be problems with the Internet connection. \n{ex}");
            }
        }

        private void NewWindow(object sender, EventArgs e) => Process.Start("TextPad+.exe");

        private void GetUpdate(object sender, EventArgs e) => Program.UpdaterForm.ShowDialog(this);

        // Методы для cTabControl
        internal void CreateTab()
        {
            /*
             * Создаётся экземпляр вкладки, в mtb закидывается новоиспечённый MTextBox (mtb, потому что раньше это был MTextBox, а мне лень менять названия),
             * задаются кое-какие параметры, затем в cTabControl, который накинут на форму впринципе, добавляется новая вкладка и кней автоматичсеки присваивается ивент TextChanged,
             * метод для которого (TbTextChanged) я создал заранее. Это нужно для динамичного изменения настроек программы в текущей вкладке, например, юзер может выключить функцию
             * переноса слов.
            */
            TabPage tpage = new(Resources.Localization.NewDocumentTitle);
            MTextBox mtb = new MTextBox
            {
                AllowDrop = true,
                EnableAutoDragDrop = false,
                Dock = DockStyle.Fill,
                BorderStyle = BorderStyle.None,
                WordWrap = Properties.Settings.Default.ModifiedTextBox_WordWarp,
                ContextMenuStrip = ContextMenu,
                Font = Properties.Settings.Default.ModifiedTextBox_Font,
                AcceptsTab = true
            };
            mtb.TextChanged += MTextBoxTextChanged!;
            mtb.DragEnter += FileDragEnter!;
            mtb.DragDrop += FileDragDrop!;
            mtb.KeyPress += MTextBoxKeyPress!;
            mtb.LinkClicked += MTextBoxLinkClicked!;
            tpage.Paint += cTabControl.OnDrawPage!;

            tpage.Controls.Add(mtb);
            cTabControl.TabPages.Add(tpage);

            if (Properties.Settings.Default.Theme == "Dark")
                ColorThemeDark();
            else
                ColorThemeWhite();
            mtb.DetectUrls = Properties.Settings.Default.MTextBox_DetectUrls;
            mtb.Encoding = Properties.Settings.Default.DefaultEncoding;
            if (cTabControl.TabPages[cTabControl.SelectedIndex] == tpage)
                EncodingStatusLabel.Text = mtb.Encoding;

            CheckFile();
        }

        private void CreateTab(object sender, EventArgs e) => CreateTab();

        private void CloseTab(object sender, EventArgs e)
        {
            mtb = cTabControl.TabPages[cTabControl.SelectedIndex].Controls.OfType<MTextBox>().First();
            TabPage tab = cTabControl.SelectedTab!;

            if (mtb.IsFileChanged == true)
            {
                DialogResult dr = MessageBox.Show($"{Resources.Localization.MSGQuestionSaveFile} \"{tab.Text}\"?", "TextPad+", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                if (dr == DialogResult.Yes)
                    FileWorker.SaveFile();
                else if (dr == DialogResult.No)
                    cTabControl.TabPages.Remove(tab);
                else if (dr == DialogResult.Cancel)
                    return;
            }
            else
                cTabControl.TabPages.Remove(tab);

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

            GC.Collect();
        }

        private void CloseAllTabs(object sender, EventArgs e)
        {
            foreach (TabPage tab in cTabControl.TabPages)
            {
                mtb = cTabControl.TabPages[cTabControl.TabPages.IndexOf(tab)].Controls.OfType<MTextBox>().First();

                if (mtb.IsFileChanged == true)
                {
                    DialogResult dr = MessageBox.Show($"{Resources.Localization.MSGQuestionSaveFile} \"{tab.Text}\"?", "TextPad+", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                    if (dr == DialogResult.Yes)
                    {
                        FileWorker.SaveFile();
                        cTabControl.TabPages.Remove(tab);
                    }
                    else if (dr == DialogResult.No)
                        cTabControl.TabPages.Remove(tab);
                    else if (dr == DialogResult.Cancel)
                        return;
                }
                else
                    cTabControl.TabPages.Remove(tab);
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

            GC.Collect();
        }

        private void CloseAllWithoutCurrentTab(object sender, EventArgs e)
        {
            foreach (TabPage tab in cTabControl.TabPages)
            {
                if (tab == cTabControl.TabPages[cTabControl.SelectedIndex])
                    continue;

                mtb = cTabControl.TabPages[cTabControl.TabPages.IndexOf(tab)].Controls.OfType<MTextBox>().First();

                if (mtb.TextLength != 0)
                {
                    DialogResult dr = MessageBox.Show($"{Resources.Localization.MSGQuestionSaveFile} \"{tab.Text}\"?", "TextPad+", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                    if (dr == DialogResult.Yes)
                    {
                        FileWorker.SaveFile();
                        cTabControl.TabPages.Remove(tab);
                    }
                    else if (dr == DialogResult.No)
                        cTabControl.TabPages.Remove(tab);
                    else if (dr == DialogResult.Cancel)
                        return;
                }
                else
                    cTabControl.TabPages.Remove(tab);
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

            GC.Collect();
        }

        private void CTabControlSelected(object sender, TabControlEventArgs e)
        {
            // Этот метод нужен для того, что бы на каждой новой вкладке применялись заданные ранее параметры (вкл/выкл перенос слов, цвет текста, фона, шрифт и т.д.)
            try
            {
                mtb = cTabControl.TabPages[cTabControl.SelectedIndex].Controls.OfType<MTextBox>().First();
                mtb.WordWrap = Properties.Settings.Default.ModifiedTextBox_WordWarp;
                if (mtb.IsFileChanged == false)
                {
                    mtb.Font = Properties.Settings.Default.ModifiedTextBox_Font;
                    if (mtb.FileName != "Missing")
                        cTabControl.SelectedTab!.Text = cTabControl.SelectedTab!.Text.TrimEnd('*');
                    mtb.IsFileChanged = false;
                }

                TextLengthLabel.Text = mtb.TextLength.ToString();
                if (mtb.Lines.Length == 0)
                    TextLinesLabel.Text = "1";
                else
                    TextLinesLabel.Text = mtb.Lines.Length.ToString();

                CheckFile();
                EncodingStatusLabel.Text = mtb.Encoding;

                mtb.DetectUrls = DetectUrlsCheckBox.Checked;

                if (Properties.Settings.Default.Theme == "Dark")
                    ColorThemeDark();
                else
                    ColorThemeWhite();

                GC.Collect();
            }
            catch (Exception ex)
            {
                Logger.Error($"Error while selecting tabs (?). If nothing is broken, then ignore it. \n{ex}");
                TextLengthLabel.Text = "0";
                TextLinesLabel.Text = "1";
            }
        }

        // Изменение текста в Modified TextBox
        internal void MTextBoxTextChanged(object sender, EventArgs e)
        {
            mtb = cTabControl.TabPages[cTabControl.SelectedIndex].Controls.OfType<MTextBox>().First();

            TextLengthLabel.Text = mtb.Text.Length.ToString();
            if (mtb.Lines.Length == 0)
                TextLinesLabel.Text = "1";
            else
                TextLinesLabel.Text = mtb.Lines.Length.ToString();

            if (Properties.Settings.Default.AutoSaveTime == 1 & mtb.FileName != "Missing")
                FileWorker.SaveFile(false);
            else if (mtb.IsFileChanged == false)
            {
                mtb.IsFileChanged = true;
                cTabControl.TabPages[cTabControl.SelectedIndex].Text += "*";
            }

            CheckFile();
            GC.Collect();
        }

        private void MTextBoxKeyPress(object sender, KeyPressEventArgs e)
        {
            if (Properties.Settings.Default.AutoSubstitutionOfClosingCharacters == true)
            {
                switch (e.KeyChar)
                {
                    case '(':
                        e.Handled = true;
                        mtb = cTabControl.TabPages[cTabControl.SelectedIndex].Controls.OfType<MTextBox>().First();
                        mtb.SelectedText = "()";
                        mtb.SelectionStart--;
                        break;
                    case '"':
                        e.Handled = true;
                        mtb = cTabControl.TabPages[cTabControl.SelectedIndex].Controls.OfType<MTextBox>().First();
                        mtb.SelectedText = "\"\"";
                        mtb.SelectionStart--;
                        break;
                    case '[':
                        e.Handled = true;
                        mtb = cTabControl.TabPages[cTabControl.SelectedIndex].Controls.OfType<MTextBox>().First();
                        mtb.SelectedText = "[]";
                        mtb.SelectionStart--;
                        break;
                    case '{':
                        e.Handled = true;
                        mtb = cTabControl.TabPages[cTabControl.SelectedIndex].Controls.OfType<MTextBox>().First();
                        mtb.SelectedText = "{}";
                        mtb.SelectionStart--;
                        break;
                    case '\'':
                        e.Handled = true;
                        mtb = cTabControl.TabPages[cTabControl.SelectedIndex].Controls.OfType<MTextBox>().First();
                        mtb.SelectedText = "\'\'";
                        mtb.SelectionStart--;
                        break;
                    case (char)Keys.Tab:
                        e.Handled = true;
                        mtb = cTabControl.TabPages[cTabControl.SelectedIndex].Controls.OfType<MTextBox>().First();
                        mtb.SelectedText = "    ";
                        break;
                }
            }
        }

        private void MTextBoxLinkClicked(object sender, LinkClickedEventArgs e)
        {
            mtb = Program.MainForm.cTabControl.TabPages[Program.MainForm.cTabControl.SelectedIndex].Controls.OfType<MTextBox>().First();
            Process linkProcess = new();
            linkProcess.StartInfo.UseShellExecute = true;
            linkProcess.StartInfo.FileName = e.LinkText;
            linkProcess.Start();
        }

        //private void MTextBoxMouseWheel(object sender, MouseEventArgs e)
        //{
        //    mtb = cTabControl.TabPages[cTabControl.SelectedIndex].Controls.OfType<MTextBox>().First();
        //    ZoomFactorStatusLabel.Text = Math.Round(mtb.ZoomFactor * 100, 1) + "%";
        //}

        // Методы для перетаскивания и скидывания файлов и текста

        private void FileDragEnter(object sender, DragEventArgs e)
        {
            if (e.Data!.GetDataPresent(DataFormats.Text))
                e.Effect = DragDropEffects.Copy;

            if (e.Data.GetDataPresent(DataFormats.FileDrop))
                e.Effect = DragDropEffects.Copy;
        }

        private void FileDragDrop(object sender, DragEventArgs e)
        {
            if (e.Data!.GetDataPresent(DataFormats.Text))
                cTabControl.TabPages[cTabControl.SelectedIndex].Controls.OfType<MTextBox>().First().Text += e.Data.GetData(DataFormats.Text);

            else if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop)!;
                FileWorker.OpenFile(files[0]);
            }
        }

        // Выбор кодировки
        private void ChangeEncodingToUTF32BE(object sender, EventArgs e)
        {
            mtb = cTabControl.TabPages[cTabControl.SelectedIndex].Controls.OfType<MTextBox>().First();
            mtb.Encoding = "UTF-32 BE";
            EncodingStatusLabel.Text = mtb.Encoding.ToString();
        }
        private void ChangeEncodingToUTF32(object sender, EventArgs e)
        {
            mtb = cTabControl.TabPages[cTabControl.SelectedIndex].Controls.OfType<MTextBox>().First();
            mtb.Encoding = "UTF-32";
            EncodingStatusLabel.Text = mtb.Encoding.ToString();
        }
        private void ChangeEncodingToUTF16BE(object sender, EventArgs e)
        {
            mtb = cTabControl.TabPages[cTabControl.SelectedIndex].Controls.OfType<MTextBox>().First();
            mtb.Encoding = "UTF-16 BE";
            EncodingStatusLabel.Text = mtb.Encoding.ToString();
        }
        private void ChangeEncodingToUTF16(object sender, EventArgs e)
        {
            mtb = cTabControl.TabPages[cTabControl.SelectedIndex].Controls.OfType<MTextBox>().First();
            mtb.Encoding = "UTF-16";
            EncodingStatusLabel.Text = mtb.Encoding.ToString();
        }
        private void ChangeEncodingToUTF8BOM(object sender, EventArgs e)
        {
            mtb = cTabControl.TabPages[cTabControl.SelectedIndex].Controls.OfType<MTextBox>().First();
            mtb.Encoding = "UTF-8 BOM";
            EncodingStatusLabel.Text = mtb.Encoding.ToString();
        }
        private void ChangeEncodingToUTF8(object sender, EventArgs e)
        {
            mtb = cTabControl.TabPages[cTabControl.SelectedIndex].Controls.OfType<MTextBox>().First();
            mtb.Encoding = "UTF-8";
            EncodingStatusLabel.Text = mtb.Encoding.ToString();
        }
        private void ChangeEncodingToWindows1251(object sender, EventArgs e)
        {
            mtb = cTabControl.TabPages[cTabControl.SelectedIndex].Controls.OfType<MTextBox>().First();
            mtb.Encoding = "Windows-1251";
            EncodingStatusLabel.Text = mtb.Encoding.ToString();
        }
        private void ChangeEncodingToASCII(object sender, EventArgs e)
        {
            mtb = cTabControl.TabPages[cTabControl.SelectedIndex].Controls.OfType<MTextBox>().First();
            mtb.Encoding = "ASCII";
            EncodingStatusLabel.Text = mtb.Encoding.ToString();
        }
        private void ChangeEncodingToKOI8R(object sender, EventArgs e)
        {
            mtb = cTabControl.TabPages[cTabControl.SelectedIndex].Controls.OfType<MTextBox>().First();
            mtb.Encoding = "KOI8-R";
            EncodingStatusLabel.Text = mtb.Encoding.ToString();
        }
        private void ChangeEncodingToKOI8U(object sender, EventArgs e)
        {
            mtb = cTabControl.TabPages[cTabControl.SelectedIndex].Controls.OfType<MTextBox>().First();
            mtb.Encoding = "KOI8-U";
            EncodingStatusLabel.Text = mtb.Encoding.ToString();
        }
        private void ChangeEncodingToCP866(object sender, EventArgs e)
        {
            mtb = cTabControl.TabPages[cTabControl.SelectedIndex].Controls.OfType<MTextBox>().First();
            mtb.Encoding = "CP866";
            EncodingStatusLabel.Text = mtb.Encoding.ToString();
        }

        // Методы для кнопок панели проводника
        private void OpenFolder(object sender, EventArgs e)
        {
            if (FolderBrowserDialog_.ShowDialog() == DialogResult.Cancel)
                return;

            workFolderLabel.Text = new DirectoryInfo(FolderBrowserDialog_.SelectedPath).Name;
            FilesListView.Clear();

            // Папки
            foreach (string item in Directory.GetDirectories(FolderBrowserDialog_.SelectedPath))
            {
                ListViewItem lvi = new(new DirectoryInfo(item).Name, 0);
                lvi.ToolTipText = item;
                FilesListView.Items.Add(lvi);

            }

            // Файлы
            foreach (string item in Directory.GetFiles(FolderBrowserDialog_.SelectedPath))
            {
                ListViewItem lvi = new(new DirectoryInfo(item).Name, 1);
                lvi.ToolTipText = item;
                FilesListView.Items.Add(lvi);
            }
        }
        
        private void OpenFileFolderAsProject(object sender, EventArgs e)
        {
            if (cTabControl.TabPages[cTabControl.SelectedIndex].Controls.OfType<MTextBox>().First().FileName == "Missing")
                return;

            workFolderLabel.Text = new DirectoryInfo(Path.GetDirectoryName(cTabControl.TabPages[cTabControl.SelectedIndex].Controls.OfType<MTextBox>().First().FileName)!).Name;
            FilesListView.Clear();

            // Папки
            foreach (string item in Directory.GetDirectories(Path.GetDirectoryName(cTabControl.TabPages[cTabControl.SelectedIndex].Controls.OfType<MTextBox>().First().FileName)!))
            {
                ListViewItem lvi = new(new DirectoryInfo(item).Name, 0);
                lvi.ToolTipText = item;
                FilesListView.Items.Add(lvi);

            }

            // Файлы
            foreach (string item in Directory.GetFiles(Path.GetDirectoryName(cTabControl.TabPages[cTabControl.SelectedIndex].Controls.OfType<MTextBox>().First().FileName)!))
            {
                ListViewItem lvi = new(new DirectoryInfo(item).Name, 1);
                lvi.ToolTipText = item;
                FilesListView.Items.Add(lvi);
            }

            FolderExplorerPanel.Visible = true;
        }

        private void AboveFolder(object sender, EventArgs e)
        {
            try
            {
                string parentDirectory = Directory.GetParent(FolderBrowserDialog_.SelectedPath)!.ToString();
                if (!Directory.Exists(parentDirectory))
                    return;

                FolderBrowserDialog_.SelectedPath = parentDirectory;

                workFolderLabel.Text = new DirectoryInfo(parentDirectory).Name;
                FilesListView.Clear();

                // Папки
                foreach (string item in Directory.GetDirectories(parentDirectory))
                {
                    ListViewItem lvi = new(new DirectoryInfo(item).Name, 0);
                    lvi.ToolTipText = item;
                    FilesListView.Items.Add(lvi);
                }

                // Файлы
                foreach (string item in Directory.GetFiles(parentDirectory))
                {
                    ListViewItem lvi = new(new DirectoryInfo(item).Name, 1);
                    lvi.ToolTipText = item;
                    FilesListView.Items.Add(lvi);
                }
            }
            catch { }
        }

        private void RefreshFolder(object sender, EventArgs e)
        {
            try
            {
                if (!Directory.Exists(FolderBrowserDialog_.SelectedPath))
                    return;
                FilesListView.Clear();

                // Папки
                foreach (string item in Directory.GetDirectories(FolderBrowserDialog_.SelectedPath))
                {
                    ListViewItem lvi = new(new DirectoryInfo(item).Name, 0);
                    lvi.ToolTipText = item;
                    FilesListView.Items.Add(lvi);
                }

                // Файлы
                foreach (string item in Directory.GetFiles(FolderBrowserDialog_.SelectedPath))
                {
                    ListViewItem lvi = new(new DirectoryInfo(item).Name, 1);
                    lvi.ToolTipText = item;
                    FilesListView.Items.Add(lvi);
                }
            }
            catch
            {
                MessageBox.Show("Папка не найдена", "TextPad+", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                CloseFolder(sender, e);
            }
        }

        private void FilesListViewDoubleClick(object sender, EventArgs e)
        {
            string path = FilesListView.Items[FilesListView.FocusedItem!.Index].ToolTipText;

            //Проверка существования файла
            if (Directory.Exists(path))
            {
                FolderBrowserDialog_.SelectedPath = path;

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
                FileWorker.OpenFile(path);

            GC.Collect();
        }

        private void CloseFolder(object sender, EventArgs e)
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
        private void DeleteFileInExplorer(object sender, EventArgs e)
        {
            if (FilesListView.Items.Count < 1)
                return;

            try
            {
                if (File.Exists(FilesListView.Items[FilesListView.FocusedItem!.Index].ToolTipText))
                {
                    if (MessageBox.Show(Resources.Localization.MSGQestionMoveFileToTrash, "TextPad+", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                        return;
                    FileSystem.DeleteFile(FilesListView.Items[FilesListView.FocusedItem.Index].ToolTipText, UIOption.OnlyErrorDialogs, RecycleOption.SendToRecycleBin);
                }
                else if (Directory.Exists(FilesListView.Items[FilesListView.FocusedItem.Index].ToolTipText))
                {
                    if (MessageBox.Show(Resources.Localization.MSGQestionMoveFileToTrash, "TextPad+", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                        return;

                    FileSystem.DeleteDirectory(FilesListView.Items[FilesListView.FocusedItem.Index].ToolTipText, UIOption.OnlyErrorDialogs, RecycleOption.SendToRecycleBin);
                }
                else
                    MessageBox.Show(Resources.Localization.MSGErrorCantDeleteFile, "TextPad+", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

                RefreshFolder(sender, e);
            }
            catch { }
        }

        // Методы для кнопок панели запуска файлов
        private void RunScript(object sender, EventArgs e) => FileWorker.RunScript();

        private void RunPythonScript(object sender, EventArgs e) => Program.PythonInterpreterForm.ShowDialog(this);

        private void RunWindowsScript(object sender, EventArgs e) => FileWorker.RunWindowsScript();

        private void RunVbsJsScript(object sender, EventArgs e) => FileWorker.RunVbsJsScript();

        private void RunHTMLPage(object sender, EventArgs e) => FileWorker.RunHTMLPage();

        // Поиск
        private void SearchPanelControl(object sender, EventArgs e)
        {
            if (SearchPanel.Visible == false)
                SearchPanel.Visible = true;
            else
                SearchPanel.Visible = false;
        }

        private void Find(object sender, EventArgs e) => Searcher.Search(ref FindTextBox);

        private void Replace(object sender, EventArgs e) => Searcher.Replace(ref FindTextBox, ref ReplaceTextBox);

        private void ReplaceAll(object sender, EventArgs e) => Searcher.ReplaceAll(ref FindTextBox, ref ReplaceTextBox);

        private void GoToLine(object sender, EventArgs e) => Searcher.GoToLine(ref GoToLineNumericUpDown);

        private void FindTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                Searcher.Search(ref FindTextBox);
        }

        // Просто выход
        private void Exit(object sender, EventArgs e) => Application.Exit();

        #endregion

        // Проверяет некоторые свойства файла, что бы включить/выключить часть кнопок
        internal void CheckFile()
        {
            mtb = cTabControl.TabPages[cTabControl.SelectedIndex].Controls.OfType<MTextBox>().First();
            if (mtb.Text.Length == 0)
            {
                SearchEditMenuItem.Enabled = false;
                SearchToolStripItem.Enabled = false;
            }
            else
            {
                SearchEditMenuItem.Enabled = true;
                SearchToolStripItem.Enabled = true;
            }
            if (mtb.FileName == "Missing")
            {
                OpenFileFolderFileMenuItem.Enabled = false;
                DeleteFileMenuItem.Enabled = false;
                OpenFolderAsProjectMenuFileItem.Enabled = false;
                ReopenFileMenuItem.Enabled = false;
            }
            else
            {
                OpenFileFolderFileMenuItem.Enabled = true;
                DeleteFileMenuItem.Enabled = true;
                OpenFolderAsProjectMenuFileItem.Enabled = true;
                ReopenFileMenuItem.Enabled = true;
            }

            FileNameToolTextBox.Text = mtb.FileName == "Missing" ? "" : mtb.FileName;
        }

        internal void CheckFile(int tabIndex)
        {
            mtb = cTabControl.TabPages[tabIndex].Controls.OfType<MTextBox>().First();
            if (mtb.Text.Length == 0)
            {
                SearchEditMenuItem.Enabled = false;
                SearchToolStripItem.Enabled = false;
            }
            else
            {
                SearchEditMenuItem.Enabled = true;
                SearchToolStripItem.Enabled = true;
            }
            if (mtb.FileName == "Missing")
            {
                OpenFileFolderFileMenuItem.Enabled = false;
                DeleteFileMenuItem.Enabled = false;
                OpenFolderAsProjectMenuFileItem.Enabled = false;
                ReopenFileMenuItem.Enabled = false;
            }
            else
            {
                OpenFileFolderFileMenuItem.Enabled = true;
                DeleteFileMenuItem.Enabled = true;
                OpenFolderAsProjectMenuFileItem.Enabled = true;
                ReopenFileMenuItem.Enabled = true;
            }
        }

        // Цветовые схемы
        internal void ColorThemeWhite()
        {
            cTabControl.TabPages[cTabControl.SelectedIndex].BackColor = Color.Transparent;
            mtb = cTabControl.TabPages[cTabControl.SelectedIndex].Controls.OfType<MTextBox>().First();

            mtb.BackColor = SystemColors.Window;
            mtb.ForeColor = SystemColors.ControlText;
        }

        internal void ColorThemeDark()
        {
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
                    ToolStripMenuItem tsmi = new();
                    tsmi.Text = Properties.RecentFiles.Default.RecentFile0;
                    tsmi.Click += (sender, e) => FileWorker.OpenFile(tsmi.Text);
                    RecentFilesListFileItem.DropDownItems.Add(tsmi);
                }
                if (Properties.RecentFiles.Default.RecentFile1 != "Missing")
                {
                    ToolStripMenuItem tsmi = new();
                    tsmi.Text = Properties.RecentFiles.Default.RecentFile1;
                    tsmi.Click += (sender, e) => FileWorker.OpenFile(tsmi.Text);
                    RecentFilesListFileItem.DropDownItems.Add(tsmi);
                }
                if (Properties.RecentFiles.Default.RecentFile2 != "Missing")
                {
                    ToolStripMenuItem tsmi = new();
                    tsmi.Text = Properties.RecentFiles.Default.RecentFile2;
                    tsmi.Click += (sender, e) => FileWorker.OpenFile(tsmi.Text);
                    RecentFilesListFileItem.DropDownItems.Add(tsmi);
                }
                if (Properties.RecentFiles.Default.RecentFile3 != "Missing")
                {
                    ToolStripMenuItem tsmi = new();
                    tsmi.Text = Properties.RecentFiles.Default.RecentFile3;
                    tsmi.Click += (sender, e) => FileWorker.OpenFile(tsmi.Text);
                    RecentFilesListFileItem.DropDownItems.Add(tsmi);
                }
                if (Properties.RecentFiles.Default.RecentFile4 != "Missing")
                {
                    ToolStripMenuItem tsmi = new();
                    tsmi.Text = Properties.RecentFiles.Default.RecentFile4;
                    tsmi.Click += (sender, e) => FileWorker.OpenFile(tsmi.Text);
                    RecentFilesListFileItem.DropDownItems.Add(tsmi);
                }
                if (Properties.RecentFiles.Default.RecentFile5 != "Missing")
                {
                    ToolStripMenuItem tsmi = new();
                    tsmi.Text = Properties.RecentFiles.Default.RecentFile5;
                    tsmi.Click += (sender, e) => FileWorker.OpenFile(tsmi.Text);
                    RecentFilesListFileItem.DropDownItems.Add(tsmi);
                }
                if (Properties.RecentFiles.Default.RecentFile6 != "Missing")
                {
                    ToolStripMenuItem tsmi = new();
                    tsmi.Text = Properties.RecentFiles.Default.RecentFile6;
                    tsmi.Click += (sender, e) => FileWorker.OpenFile(tsmi.Text);
                    RecentFilesListFileItem.DropDownItems.Add(tsmi);
                }
                if (Properties.RecentFiles.Default.RecentFile7 != "Missing")
                {
                    ToolStripMenuItem tsmi = new();
                    tsmi.Text = Properties.RecentFiles.Default.RecentFile7;
                    tsmi.Click += (sender, e) => FileWorker.OpenFile(tsmi.Text);
                    RecentFilesListFileItem.DropDownItems.Add(tsmi);
                }
                if (Properties.RecentFiles.Default.RecentFile8 != "Missing")
                {
                    ToolStripMenuItem tsmi = new();
                    tsmi.Text = Properties.RecentFiles.Default.RecentFile8;
                    tsmi.Click += (sender, e) => FileWorker.OpenFile(tsmi.Text);
                    RecentFilesListFileItem.DropDownItems.Add(tsmi);
                }
                if (Properties.RecentFiles.Default.RecentFile9 != "Missing")
                {
                    ToolStripMenuItem tsmi = new();
                    tsmi.Text = Properties.RecentFiles.Default.RecentFile9;
                    tsmi.Click += (sender, e) => FileWorker.OpenFile(tsmi.Text);
                    RecentFilesListFileItem.DropDownItems.Add(tsmi);
                }
            }
            catch (Exception ex) { Logger.Error($"Error loading list of recent files. \n{ex}"); }
        }

        // Сохранение параметров программы
        internal void SaveParameters()
        {
            Logger.Debug("Saving settings...");

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
            Properties.Settings.Default.ScriptTypeToRun = RunScriptCombobox.SelectedItem!.ToString();
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
                if (RecentFilesListFileItem.DropDownItems.Count > 0 && RecentFilesListFileItem.DropDownItems[0] != null)
                    Properties.RecentFiles.Default.RecentFile0 = RecentFilesListFileItem.DropDownItems[0].Text;
                if (RecentFilesListFileItem.DropDownItems.Count > 1 && RecentFilesListFileItem.DropDownItems[1] != null)
                    Properties.RecentFiles.Default.RecentFile1 = RecentFilesListFileItem.DropDownItems[1].Text;
                if (RecentFilesListFileItem.DropDownItems.Count > 2 && RecentFilesListFileItem.DropDownItems[2] != null)
                    Properties.RecentFiles.Default.RecentFile2 = RecentFilesListFileItem.DropDownItems[2].Text;
                if (RecentFilesListFileItem.DropDownItems.Count > 3 && RecentFilesListFileItem.DropDownItems[3] != null)
                    Properties.RecentFiles.Default.RecentFile3 = RecentFilesListFileItem.DropDownItems[3].Text;
                if (RecentFilesListFileItem.DropDownItems.Count > 4 && RecentFilesListFileItem.DropDownItems[4] != null)
                    Properties.RecentFiles.Default.RecentFile4 = RecentFilesListFileItem.DropDownItems[4].Text;
                if (RecentFilesListFileItem.DropDownItems.Count > 5 && RecentFilesListFileItem.DropDownItems[5] != null)
                    Properties.RecentFiles.Default.RecentFile5 = RecentFilesListFileItem.DropDownItems[5].Text;
                if (RecentFilesListFileItem.DropDownItems.Count > 6 && RecentFilesListFileItem.DropDownItems[6] != null)
                    Properties.RecentFiles.Default.RecentFile6 = RecentFilesListFileItem.DropDownItems[6].Text;
                if (RecentFilesListFileItem.DropDownItems.Count > 7 && RecentFilesListFileItem.DropDownItems[7] != null)
                    Properties.RecentFiles.Default.RecentFile7 = RecentFilesListFileItem.DropDownItems[7].Text;
                if (RecentFilesListFileItem.DropDownItems.Count > 8 && RecentFilesListFileItem.DropDownItems[8] != null)
                    Properties.RecentFiles.Default.RecentFile8 = RecentFilesListFileItem.DropDownItems[8].Text;
                if (RecentFilesListFileItem.DropDownItems.Count > 9 && RecentFilesListFileItem.DropDownItems[9] != null)
                    Properties.RecentFiles.Default.RecentFile9 = RecentFilesListFileItem.DropDownItems[9].Text;
            }
            catch (Exception ex) { Logger.Error($"An error occurred while saving the list of recent files. \n{ex}"); }

            Properties.RecentFiles.Default.Save();

            Logger.Debug("Settings saved");
        }

        // Обработчики загрузки и закрытия формы
        private void MainFormLoad(object sender, EventArgs e)
        {
            Logger.Debug("Loading the program...");

            StatusLabel.Text = Resources.Localization.PROGRAMStatusLoading;

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
                    mtb.FileName = args[1];
                    // помещаем текст
                    File.ReadAllText(args[1]);
                    // задаём даголовок вкладки
                    cTabControl.SelectedTab!.Text = Path.GetFileName(mtb.FileName);

                    if (RecentFilesListFileItem.DropDownItems.Count == 10)
                        RecentFilesListFileItem.DropDownItems.RemoveAt(0);
                    ToolStripMenuItem tsmi = new();
                    tsmi.Text = cTabControl.TabPages[cTabControl.SelectedIndex].Controls.OfType<MTextBox>().First().FileName;
                    tsmi.Click += (sender, e) => FileWorker.OpenFile(tsmi.Text);
                    RecentFilesListFileItem.DropDownItems.Add(tsmi);
                }
                catch (Exception ex)
                {
                    Logger.Error($"Error while trying to create a tab while running a program through a file: {args[1]} \n{ex}");
                    MessageBox.Show(ex.Message, "TextPad+", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

            // Загрузка параметров из файла Settings.settings.
            Width = Properties.Settings.Default.FormMainUI_Width;
            Height = Properties.Settings.Default.FormMainUI_Height;
            if (Properties.Settings.Default.FormMainUI_Topmost == true)
                TopMost = true;
            else
                TopMost = false;
            ToolBar.Visible = Properties.Settings.Default.ToolStrip_Visible;
            RunFileToolStrip.Visible = Properties.Settings.Default.RunFileToolStrip_Visible;
            StatusBar.Visible = Properties.Settings.Default.StatusStrip_Visible;
            FolderExplorerPanel.Visible = Properties.Settings.Default.FolderExplorerPanel_Visible;
            RunScriptCombobox.SelectedItem = Properties.Settings.Default.ScriptTypeToRun;
            FolderExplorerPanel.Width = Properties.Settings.Default.FolderExplorerPanel_Size;
            if (Properties.Settings.Default.Theme == "Dark")
                ColorThemeDark();
            else
                ColorThemeWhite();
            WindowState = Properties.Settings.Default.FormMainUI_WindowState switch
            {
                "Normal" => FormWindowState.Normal,
                "Maximized" => FormWindowState.Maximized,
                "Minimized" => FormWindowState.Normal,
                _ => FormWindowState.Normal,
            };
            if (Properties.Settings.Default.AutoCheckUpdate == true)
            {
                StatusLabel.Text = Resources.Localization.PROGRAMStatusCheckForUpdates;
                Updater.Updater.GetUpdateQuiet(Program.UpdaterForm.UpdateLatestVerL, Program.UpdaterForm.UpdateInfoTextBox, Program.UpdaterForm.UpdateStatusLabel, Program.UpdaterForm.UpdateStatusProgressBar);
            }
            if (Properties.Settings.Default.AutoSaveTime > 1)
            {
                AutoSaveTimer.Enabled = true;
                AutoSaveTimer.Interval = Properties.Settings.Default.AutoSaveTime;
            }
            else
                AutoSaveTimer.Enabled = false;

            Logger.Warning(AutoSaveTimer.Enabled.ToString() + " : " + Properties.Settings.Default.AutoSaveTime);

            CheckFile();

            if (Program.UpdateStatus > 0)
                StatusLabel.Text = Resources.Localization.PROGRAMStatusUpdating;
            else
                StatusLabel.Text = Resources.Localization.PROGRAMStatusReady;

            if (Width >= 900)
                FileNameToolTextBox.Width = 610;

            Logger.Info($"Program path: {Program.Path}");
            Logger.Debug("loading is complete");
        }

        private void MainFormClosing(object sender, FormClosingEventArgs e)
        {
            Logger.Debug("Closing the main window...");

            // Проверка статуса обновления
            if (Program.UpdateStatus == 1)
            {
                if (MessageBox.Show(Resources.Localization.UPDATERAbortUpdate, Resources.Localization.UPDATERTitle, MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
                    return;
                else
                    e.Cancel = true; return;
            }
            else if (Program.UpdateStatus == 2)
                Process.GetCurrentProcess().Kill();
            else
            {
                // Закрытие всех вкладок
                foreach (TabPage tab in cTabControl.TabPages)
                {
                    mtb = cTabControl.TabPages[cTabControl.TabPages.IndexOf(tab)].Controls.OfType<MTextBox>().First();

                    if (mtb.IsFileChanged == true)
                    {
                        DialogResult dr = MessageBox.Show($"{Resources.Localization.MSGQuestionSaveFile} \"{tab.Text}\"?", "TextPad+", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                        if (dr == DialogResult.Yes)
                            FileWorker.SaveFile();

                        else if (dr == DialogResult.No)
                            cTabControl.TabPages.Remove(tab);

                        else if (dr == DialogResult.Cancel)
                            e.Cancel = true; return;
                    }
                    else
                        cTabControl.TabPages.Remove(tab);
                }
            }

            SaveParameters();
            Logger.Debug("Exit completed");
        }

        private void FormMainUiSizeChanged(object sender, EventArgs e)
        {
            if (this.Width < 900)
                FileNameToolTextBox.Width = this.Width - 295;
        }
    }
}
