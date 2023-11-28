
using Microsoft.VisualBasic.FileIO;
using System.Reflection;

namespace TextPad_
{
    /// <summary>
    /// Класс главного окна программы, где обрабатывается внешний вид и пользовательский UI.
    /// Всю (наверное) логику работы программы я переместил в класс TextEditor.
    /// <summary>
    public partial class FormMainUI : Form
    {
        // Logger
        private readonly LogSystem Logger = new($"{Application.StartupPath}\\logs");

        // Авто свойства для чтения с инфой о программе
        public string DateOfRelease { get; } = "Dev";
        public string ProgramPath { get; } = Application.StartupPath;
        public string WebSite { get; private set; } = "https://mr-nichosik.github.io/Main_Page/";

        // Общее поле для Modified TextBox
        private MTextBox? mtb;

        // Поле для настроек программы. Если isLangChanched = true, то при выходе из настроек появится сообщение о необходимости перезапустить программу
        private static bool isLanguageChanged = false;

        // Конструктор окна
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

            Logger.Info("MainUI Initialization");
            InitializeComponent();

            Program.MainUI = this;

            Logger.Debug("Program launched successfully");
        }

        // Переопределение OnKeyDown для добавления сочетаний клавиш
        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);

            if (e.KeyCode == Keys.V && e.Control)
            {
                MTextBoxPaste();
                e.Handled = true;
            }

            if (e.KeyCode == Keys.F && e.Control)
            {
                mtb = cTabControl.TabPages[cTabControl.SelectedIndex].Controls.OfType<MTextBox>().First();
                if (mtb.TextLength > 0)
                {
                    SearchUI searchUI = new();
                    searchUI.ShowDialog(this);

                    mtb = cTabControl.TabPages[cTabControl.SelectedIndex].Controls.OfType<MTextBox>().First();
                }
                e.Handled = true;
            }
        }

        #region Обработка событий разных элементов формы

        // Методы для кнопок панели инструментов, контекстного и главного меню
        private void SaveAsFile(object sender, EventArgs e)
        {
            TextEditor.SaveAsFile();
        }

        private void SaveFile(object sender, EventArgs e)
        {
            TextEditor.SaveCurrentFile();
        }

        private void OpenFile(object sender, EventArgs e)
        {
            TextEditor.OpenFile();
        }

        private void ReopenFile(object sender, EventArgs e)
        {
            mtb = cTabControl.TabPages[cTabControl.SelectedIndex].Controls.OfType<MTextBox>().First();
            TextEditor.ReopenFile();
        }

        private void MTextBoxCopy(object sender, EventArgs e)
        {
            mtb = cTabControl.TabPages[cTabControl.SelectedIndex].Controls.OfType<MTextBox>().First();
            if (mtb.TextLength > 0)
            {
                mtb.Copy();
            }
        }

        private void MTextBoxCut(object sender, EventArgs e)
        {
            mtb = cTabControl.TabPages[cTabControl.SelectedIndex].Controls.OfType<MTextBox>().First();
            if (mtb.TextLength > 0)
            {
                mtb.Cut();
            }
        }

        private void MTextBoxPaste()
        {
            mtb = cTabControl.TabPages[cTabControl.SelectedIndex].Controls.OfType<MTextBox>().First();
            mtb.Paste();
            // Это делается для того, что бы текст встал без форматирования
            mtb.Text.ToString();
            mtb.Font = Properties.Settings.Default.ModifiedTextBox_Font;
        }

        private void MTextBoxPaste(object sender, EventArgs e)
        {
            MTextBoxPaste();
        }

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
            {
                mtb.SelectAll();
            }
        }

        private void MTextBoxUndo(object sender, EventArgs e)
        {
            mtb = cTabControl.TabPages[cTabControl.SelectedIndex].Controls.OfType<MTextBox>().First();
            mtb.Undo();
        }

        private void MTextBoxRedo(object sender, EventArgs e)
        {
            mtb = cTabControl.TabPages[cTabControl.SelectedIndex].Controls.OfType<MTextBox>().First();
            mtb.Redo();
        }

        private void MTextBoxDeleteText(object sender, EventArgs e)
        {
            mtb = cTabControl.TabPages[cTabControl.SelectedIndex].Controls.OfType<MTextBox>().First();
            if (mtb.TextLength > 0)
            {
                mtb.SelectedText = "";
            }
        }

        private void MTextBoxInsertDateAndTime(object sender, EventArgs e)
        {
            mtb = cTabControl.TabPages[cTabControl.SelectedIndex].Controls.OfType<MTextBox>().First();
            mtb.AppendText(Convert.ToString(DateTime.Now));
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
                {
                    MessageBox.Show(Resources.Localization.MSGErrorCantDeleteFile, "TextPad+", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
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
            SearchEditMenuItem.DropDownItems.Clear();

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

            textBoxWebSiteUrl.Text = WebSite;
            ProgramNameLabel.Text = $"{GetAssemblyName()} {GetAssemblyVersion()}";
            VersionLabel.Text = GetAssemblyVersion();
            DateOfReleaseLabel.Text = DateOfRelease;

            LSVersionLabel.Text = LogSystem.GetAssemblyVersion();
            CTCLabelVersion.Text = CTabControl.GetAssemblyVersion();
            MTBVersionLabel.Text = MTextBox.GetAssemblyVersion();

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
            switch (ColorThemeComboBox.SelectedItem)
            {
                case "Белая / White":
                    Properties.Settings.Default.Theme = "White";
                    ColorThemeWhite();
                    break;
                case "Тёмная / Dark":
                    Properties.Settings.Default.Theme = "Dark";
                    ColorThemeDark();
                    break;
            }

            // Шрифт
            FontTextBox.Text = FontDialog_.Font.ToString();
            Properties.Settings.Default.ModifiedTextBox_Font = FontDialog_.Font;
            Properties.Settings.Default.Save();
            mtb.Font = FontDialog_.Font;

            // Стркоа состояния
            StatusBar.Visible = StatusBarCheckBox.Checked;
            Properties.Settings.Default.StatusStrip_Visible = StatusBarCheckBox.Checked;

            // Topmost
            TopMost = TopmostCheckBox.Checked;
            Properties.Settings.Default.FormMainUI_Topmost = TopmostCheckBox.Checked;

            // Панель запуска файлов
            RunFileToolStrip.Visible = RunFileToolBarCheckBox.Checked;
            Properties.Settings.Default.RunFileToolStrip_Visible = RunFileToolBarCheckBox.Checked;

            // Перенос слов
            Properties.Settings.Default.ModifiedTextBox_WordWarp = WordWarpCheckBox.Checked;
            mtb.WordWrap = WordWarpCheckBox.Checked;

            // Панель инструментов
            Properties.Settings.Default.ToolStrip_Visible = ToolBarCheckBox.Checked;
            ToolBar.Visible = ToolBarCheckBox.Checked;

            // Проводник
            Properties.Settings.Default.FolderExplorerPanel_Visible = ExplorerCheckBox.Checked;
            FolderExplorerPanel.Visible = ExplorerCheckBox.Checked;

            // Нужно ли закрывать программу при закрытии последней вкладки
            Properties.Settings.Default.ExitWhenClosingLastTab = ExitWhenClosingLastTabCheckBox.Checked;

            // Нужно ли проверять наличие обновлений при входе
            Properties.Settings.Default.AutoCheckUpdate = AutoChekUpdateCheckBox.Checked;

            // Нужно ли подставлять закрывающие символы
            Properties.Settings.Default.AutoSubstitutionOfClosingCharacters = AutoSubstitutionCheckBox.Checked;

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

        private void ChangeFont(object sender, EventArgs e)
        {
            FontDialog_.ShowDialog();
        }

        private void OpenWebSite(object sender, EventArgs e)
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
                MessageBox.Show(Resources.Localization.MSGErrorWhenCheckingInternet, "TextPad+", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Logger.Error($"The program's website could not be opened. There may be problems with the Internet connection. \n{ex}");
            }
        }

        // Открытие окон и нажатия на кнопки в настройках
        private void NewWindow(object sender, EventArgs e)
        {
            Process.Start("TextPad+.exe");
        }

        private void SearchWindow(object sender, EventArgs e)
        {
            SearchUI searchUI = new();
            searchUI.ShowDialog(this);
        }

        private void GetUpdate(object sender, EventArgs e)
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
            TabPage tpage = new(Resources.Localization.newDocumentTitle);
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
            mtb.TextChanged += new EventHandler(MTextBoxTextChanged!);
            mtb.DragEnter += new DragEventHandler(FileDragEnter!);
            mtb.DragDrop += new DragEventHandler(FileDragDrop!);
            mtb.KeyPress += new KeyPressEventHandler(MTextBoxKeyPress!);

            tpage.Controls.Add(mtb);
            cTabControl.TabPages.Add(tpage);

            if (Properties.Settings.Default.Theme == "White")
                ColorThemeWhite();
            else
                ColorThemeDark();

            mtb.Encoding = Properties.Settings.Default.DefaultEncoding;
            if (cTabControl.TabPages[cTabControl.SelectedIndex] == tpage)
                EncodingStatusLabel.Text = mtb.Encoding;

            CheckFile();
        }

        private void CreateTab(object sender, EventArgs e)
        {
            CreateTab();
        }

        private void CloseTab(object sender, EventArgs e)
        {
            mtb = cTabControl.TabPages[cTabControl.SelectedIndex].Controls.OfType<MTextBox>().First();
            TabPage tab = cTabControl.SelectedTab!;

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

        private void CloseAllWithoutCurrentTab(object sender, EventArgs e)
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

        private void CTabControlSelecting(object sender, TabControlCancelEventArgs e)
        {
            // Этот метод нужен для того, что бы на каждой новой вкладке применялись заданные ранее параметры (вкл/выкл перенос слов, цвет текста, фона, шрифт и т.д.)
            try
            {
                mtb = cTabControl.TabPages[cTabControl.SelectedIndex].Controls.OfType<MTextBox>().First();
                mtb.WordWrap = Properties.Settings.Default.ModifiedTextBox_WordWarp;
                mtb.Font = Properties.Settings.Default.ModifiedTextBox_Font;
                TextLengthLabel.Text = mtb.TextLength.ToString();
                if (mtb.Lines.Length == 0)
                    TextLinesLabel.Text = "1";
                else
                    TextLinesLabel.Text = mtb.Lines.Length.ToString();

                CheckFile();
                EncodingStatusLabel.Text = mtb.Encoding;

                switch (Properties.Settings.Default.Theme)
                {
                    case "Dark":
                        ColorThemeDark();
                        break;
                    case "White":
                        ColorThemeWhite();
                        break;
                    default:
                        ColorThemeWhite();
                        break;
                }
            }
            catch (Exception ex)
            {
                Logger.Error($"Error while selecting tabs (?). Maybe not have any tabs open. If nothing is broken, then ignore it. \n{ex}");
                TextLengthLabel.Text = "0";
                TextLinesLabel.Text = "1";
            }
        }

        // Изменение текста в Modified TextBox
        internal void MTextBoxTextChanged(object sender, EventArgs e)
        {
            mtb = (MTextBox)sender;

            // Подсчёт количества строк и символов
            TextLengthLabel.Text = mtb.Text.Length.ToString();
            if (mtb.Lines.Length == 0)
                TextLinesLabel.Text = "1";
            else
                TextLinesLabel.Text = mtb.Lines.Length.ToString();

            if (mtb.IsFileChanged == false)
                cTabControl.TabPages[cTabControl.SelectedIndex].Text += "*";
            mtb.IsFileChanged = true;

            CheckFile();
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
                }
            }
        }

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
            mtb = cTabControl.TabPages[cTabControl.SelectedIndex].Controls.OfType<MTextBox>().First();

            if (e.Data!.GetDataPresent(DataFormats.Text))
                mtb.Text += e.Data.GetData(DataFormats.Text);

            else if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop)!;
                TextEditor.OpenFile(files[0]);
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
            {
                return;
            }

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
                {
                    return;
                }

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
                {
                    return;
                }

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
            {
                TextEditor.OpenFile(path);
            }
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
                    MessageBox.Show(Resources.Localization.MSGErrorCantDeleteFile, "TextPad+", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
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

        private void RunPythonScript(object sender, EventArgs e)
        {
            FormPythonInterpreterUI pythonInterpreterUI = new();
            pythonInterpreterUI.ShowDialog(this);
        }

        private void RunWindowsScript(object sender, EventArgs e)
        {
            FileRunner.RunWindowsScript();
        }

        private void RunVbsJsScript(object sender, EventArgs e)
        {
            FileRunner.RunVbsJsScript();
        }

        private void RunHTMLPage(object sender, EventArgs e)
        {
            FileRunner.RunHTMLPage();
        }

        // Просто выход
        private void Exit(object sender, EventArgs e)
        {
            Application.Exit();
        }

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

        // Цветовые схемы
        internal void ColorThemeWhite()
        {
            // цвет mtb
            cTabControl.TabPages[cTabControl.SelectedIndex].BackColor = Color.Transparent;
            mtb = cTabControl.TabPages[cTabControl.SelectedIndex].Controls.OfType<MTextBox>().First();

            mtb.ForeColor = Color.Black;
            mtb.BackColor = SystemColors.Window;
            mtb.ForeColor = SystemColors.ControlText;
        }

        internal void ColorThemeDark()
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
                    ToolStripMenuItem tsmi = new();
                    tsmi.Text = Properties.RecentFiles.Default.RecentFile0;
                    tsmi.Click += (sender, e) => TextEditor.OpenFile(tsmi.Text);
                    RecentFilesListFileItem.DropDownItems.Add(tsmi);
                }
                if (Properties.RecentFiles.Default.RecentFile1 != "Missing")
                {
                    ToolStripMenuItem tsmi = new();
                    tsmi.Text = Properties.RecentFiles.Default.RecentFile1;
                    tsmi.Click += (sender, e) => TextEditor.OpenFile(tsmi.Text);
                    RecentFilesListFileItem.DropDownItems.Add(tsmi);
                }
                if (Properties.RecentFiles.Default.RecentFile2 != "Missing")
                {
                    ToolStripMenuItem tsmi = new();
                    tsmi.Text = Properties.RecentFiles.Default.RecentFile2;
                    tsmi.Click += (sender, e) => TextEditor.OpenFile(tsmi.Text);
                    RecentFilesListFileItem.DropDownItems.Add(tsmi);
                }
                if (Properties.RecentFiles.Default.RecentFile3 != "Missing")
                {
                    ToolStripMenuItem tsmi = new();
                    tsmi.Text = Properties.RecentFiles.Default.RecentFile3;
                    tsmi.Click += (sender, e) => TextEditor.OpenFile(tsmi.Text);
                    RecentFilesListFileItem.DropDownItems.Add(tsmi);
                }
                if (Properties.RecentFiles.Default.RecentFile4 != "Missing")
                {
                    ToolStripMenuItem tsmi = new();
                    tsmi.Text = Properties.RecentFiles.Default.RecentFile4;
                    tsmi.Click += (sender, e) => TextEditor.OpenFile(tsmi.Text);
                    RecentFilesListFileItem.DropDownItems.Add(tsmi);
                }
                if (Properties.RecentFiles.Default.RecentFile5 != "Missing")
                {
                    ToolStripMenuItem tsmi = new();
                    tsmi.Text = Properties.RecentFiles.Default.RecentFile5;
                    tsmi.Click += (sender, e) => TextEditor.OpenFile(tsmi.Text);
                    RecentFilesListFileItem.DropDownItems.Add(tsmi);
                }
                if (Properties.RecentFiles.Default.RecentFile6 != "Missing")
                {
                    ToolStripMenuItem tsmi = new();
                    tsmi.Text = Properties.RecentFiles.Default.RecentFile6;
                    tsmi.Click += (sender, e) => TextEditor.OpenFile(tsmi.Text);
                    RecentFilesListFileItem.DropDownItems.Add(tsmi);
                }
                if (Properties.RecentFiles.Default.RecentFile7 != "Missing")
                {
                    ToolStripMenuItem tsmi = new();
                    tsmi.Text = Properties.RecentFiles.Default.RecentFile7;
                    tsmi.Click += (sender, e) => TextEditor.OpenFile(tsmi.Text);
                    RecentFilesListFileItem.DropDownItems.Add(tsmi);
                }
                if (Properties.RecentFiles.Default.RecentFile8 != "Missing")
                {
                    ToolStripMenuItem tsmi = new();
                    tsmi.Text = Properties.RecentFiles.Default.RecentFile8;
                    tsmi.Click += (sender, e) => TextEditor.OpenFile(tsmi.Text);
                    RecentFilesListFileItem.DropDownItems.Add(tsmi);
                }
                if (Properties.RecentFiles.Default.RecentFile9 != "Missing")
                {
                    ToolStripMenuItem tsmi = new();
                    tsmi.Text = Properties.RecentFiles.Default.RecentFile9;
                    tsmi.Click += (sender, e) => TextEditor.OpenFile(tsmi.Text);
                    RecentFilesListFileItem.DropDownItems.Add(tsmi);
                }
            }
            catch (Exception ex) { Logger.Error($"Error loading list of recent files. \n{ex}"); }
        }

        // Сохранение параметров программы
        internal void SaveParameters()
        {
            Logger.Debug("Loading settings...");

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
                if (RecentFilesListFileItem.DropDownItems[0] != null)
                {
                    Properties.RecentFiles.Default.RecentFile0 = RecentFilesListFileItem.DropDownItems[0].Text;
                }
                if (RecentFilesListFileItem.DropDownItems[1] != null)
                {
                    Properties.RecentFiles.Default.RecentFile1 = RecentFilesListFileItem.DropDownItems[1].Text;
                }
                if (RecentFilesListFileItem.DropDownItems[2] != null)
                {
                    Properties.RecentFiles.Default.RecentFile2 = RecentFilesListFileItem.DropDownItems[2].Text;
                }
                if (RecentFilesListFileItem.DropDownItems[3] != null)
                {
                    Properties.RecentFiles.Default.RecentFile3 = RecentFilesListFileItem.DropDownItems[3].Text;
                }
                if (RecentFilesListFileItem.DropDownItems[4] != null)
                {
                    Properties.RecentFiles.Default.RecentFile4 = RecentFilesListFileItem.DropDownItems[4].Text;
                }
                if (RecentFilesListFileItem.DropDownItems[5] != null)
                {
                    Properties.RecentFiles.Default.RecentFile5 = RecentFilesListFileItem.DropDownItems[5].Text;
                }
                if (RecentFilesListFileItem.DropDownItems[6] != null)
                {
                    Properties.RecentFiles.Default.RecentFile6 = RecentFilesListFileItem.DropDownItems[6].Text;
                }
                if (RecentFilesListFileItem.DropDownItems[7] != null)
                {
                    Properties.RecentFiles.Default.RecentFile7 = RecentFilesListFileItem.DropDownItems[7].Text;
                }
                if (RecentFilesListFileItem.DropDownItems[8] != null)
                {
                    Properties.RecentFiles.Default.RecentFile8 = RecentFilesListFileItem.DropDownItems[8].Text;
                }
                if (RecentFilesListFileItem.DropDownItems[9] != null)
                {
                    Properties.RecentFiles.Default.RecentFile9 = RecentFilesListFileItem.DropDownItems[9].Text;
                }
            }
            catch (Exception ex) { Logger.Error($"An error occurred while saving the list of recent files. \n{ex}"); }

            Properties.RecentFiles.Default.Save();

            Logger.Debug("Settings saved");
        }

        #region Методы для получения информации о сборке

        internal static string GetAssemblyName()
        {
            string assembly = Assembly.GetExecutingAssembly().GetName().Name!.ToString();
            return assembly;
        }

        internal static string GetAssemblyVersion()
        {
            string assembly = Assembly.GetExecutingAssembly().GetName().Version!.ToString();
            assembly = assembly.Remove(assembly.Length - 2);
            return assembly;
        }

        internal static string GetAssemblyCompany()
        {
            object[] assemblyCustomAttributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyCompanyAttribute), false);
            return ((AssemblyCompanyAttribute)assemblyCustomAttributes[0]).Company.ToString();
        }

        #endregion

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
                    cTabControl.TabPages[cTabControl.SelectedIndex].Controls.OfType<MTextBox>().First().FileName = args[1];
                    // помещаем текст
                    File.ReadAllText(args[1]);
                    // задаём даголовок вкладки
                    cTabControl.SelectedTab!.Text = Path.GetFileName(cTabControl.TabPages[cTabControl.SelectedIndex].Controls.OfType<MTextBox>().First().FileName);

                    if (RecentFilesListFileItem.DropDownItems.Count == 10)
                        RecentFilesListFileItem.DropDownItems.RemoveAt(0);
                    ToolStripMenuItem tsmi = new();
                    tsmi.Text = cTabControl.TabPages[cTabControl.SelectedIndex].Controls.OfType<MTextBox>().First().FileName;
                    tsmi.Click += (sender, e) => TextEditor.OpenFile(tsmi.Text);
                    RecentFilesListFileItem.DropDownItems.Add(tsmi);
                }
                catch (Exception ex)
                {
                    Logger.Error($"Error while trying to create a tab while running a program through a file: {cTabControl.TabPages[cTabControl.SelectedIndex].Controls.OfType<MTextBox>().First().FileName} \n{ex}");
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
            ToolBar.Visible = Properties.Settings.Default.ToolStrip_Visible;
            RunFileToolStrip.Visible = Properties.Settings.Default.RunFileToolStrip_Visible;
            StatusBar.Visible = Properties.Settings.Default.StatusStrip_Visible;
            FolderExplorerPanel.Visible = Properties.Settings.Default.FolderExplorerPanel_Visible;
            RunScriptCombobox.SelectedItem = Properties.Settings.Default.ScriptTypeToRun;
            FolderExplorerPanel.Width = Properties.Settings.Default.FolderExplorerPanel_Size;
            switch (Properties.Settings.Default.Theme)
            {
                case "White":
                    ColorThemeWhite();
                    ColorThemeWhite();
                    break;

                case "Dark":
                    ColorThemeDark();
                    ColorThemeDark();
                    break;
            }
            this.WindowState = Properties.Settings.Default.FormMainUI_WindowState switch
            {
                "Normal" => FormWindowState.Normal,
                "Maximized" => FormWindowState.Maximized,
                "Minimized" => FormWindowState.Normal,
                _ => FormWindowState.Normal,
            };
            if (Properties.Settings.Default.AutoCheckUpdate == true)
            {
                StatusLabel.Text = Resources.Localization.PROGRAMStatusCheckForUpdates;
                Updater.Updater.GetUpdateQuiet(Program.UpdaterUI.UpdateLatestVerL, Program.UpdaterUI.UpdateInfoTextBox, Program.UpdaterUI.UpdateStatusLabel, Program.UpdaterUI.UpdateStatusProgressBar);
            }

            CheckFile();

            if (Program.UpdateStatus > 0)
                StatusLabel.Text = Resources.Localization.PROGRAMStatusUpdating;
            else
                StatusLabel.Text = Resources.Localization.PROGRAMStatusReady;

            if (this.Width >= 900)
                FileNameToolTextBox.Width = 610;

            Logger.Info($"Program path: {ProgramPath}");
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

                    if (mtb.IsFileChanged == true)
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
