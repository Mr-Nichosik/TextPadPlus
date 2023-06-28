
using Microsoft.VisualBasic.FileIO;
using System.Collections.Generic;
using System.IO;
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
        private IFileRunner FileRunner = new TextEditor();

        // Авто свойства для чтения с инфой о программе
        public string DateOfRelease { get; } = "IN DEVELOPING";
        public string ProgramPath { get; } = Application.StartupPath;
        public string UpdaterPath { get; } = Application.StartupPath + "Updater.exe";
        public string WebSite { get; private set; } = "https://mr-nichosik.github.io/Main_Page/";

        // Списоки открытых файлов и папок (для обозревателя проекта)
        internal List<string> OpenedFiles { get; set; } = new();

        // Общая переменная для Rich Text Box
        private RichTextBox? rtb;

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

            InitializeComponent();
            LS.Info("Program Initialization");

            Program.mainUI = this;

            CreateTab();

            LS.Info("Program launched successfully");
            LS.Debug($"Memory Consumed: {Process.GetProcessesByName("TextPad+")[0].WorkingSet64} Bytes");
        }

        // Обрботка некоторых нажатий клавиш
        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);

            if (e.KeyCode == Keys.V && e.Control)
            {
                TextEditor.pasteTextFromTB(tabControl);
                e.Handled = true;
            }

            if (e.KeyCode == Keys.F && e.Control)
            {
                rtb = tabControl.TabPages[tabControl.SelectedIndex].Controls.OfType<RichTextBox>().First();
                if (rtb.TextLength > 0)
                {
                    SearchUI search_ui = new SearchUI();
                    search_ui.ShowDialog(this);

                    rtb = tabControl.TabPages[tabControl.SelectedIndex].Controls.OfType<RichTextBox>().First();
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
            TextEditor.OpenFile(tabControl, openFileDialog);
        }

        private void copyTextFromTBButton(object sender, EventArgs e)
        {
            TextEditor.copyTextFromTB(tabControl);
        }

        private void cutTextFromTBButton(object sender, EventArgs e)
        {
            TextEditor.cutTextFromTB(tabControl);
        }

        private void pasteTextFromTBButton(object sender, EventArgs e)
        {
            TextEditor.pasteTextFromTB(tabControl);
        }

        private void fontTextFromTBButton(object sender, EventArgs e)
        {
            TextEditor.fontTextFromTB(tabControl, fontDialog);
        }

        private void selectAllTextFromTBButton(object sender, EventArgs e)
        {
            TextEditor.selectAllTextFromTB(tabControl);
        }

        private void undoTextFromTBButton(object sender, EventArgs e)
        {
            TextEditor.undoTextFromTB(tabControl);
        }

        private void redoTextFromTBButton(object sender, EventArgs e)
        {
            TextEditor.redoTextFromTB(tabControl);
        }

        private void deleteTextFromTBButton(object sender, EventArgs e)
        {
            TextEditor.deleteTextFromTB(tabControl);
        }

        private void dateAndTime(object sender, EventArgs e)
        {
            TextEditor.dateTime(tabControl);
        }

        // Методы для TabControl
        private void createTabClick(object sender, EventArgs e)
        {
            CreateTab();
        }

        private void CloseTab(object sender, EventArgs e)
        {
            rtb = tabControl.TabPages[tabControl.SelectedIndex].Controls.OfType<RichTextBox>().First();
            if (rtb.TextLength != 0)
            {
                DialogResult dr = MessageBox.Show(Resources.Localization.MSGQuestionTextInFTB, "TextPad+", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                if (dr == DialogResult.Yes)
                {
                    if (saveFileDialog.ShowDialog() == DialogResult.Yes)
                    {
                        File.WriteAllText(saveFileDialog.FileName, rtb.Text);
                        OpenedFiles.RemoveAt(tabControl.SelectedIndex);
                        tabControl.TabPages.Remove(tabControl.SelectedTab);
                        return;
                    }
                    else
                    {
                        return;
                    }
                }
                else if (dr == DialogResult.No)
                {
                    OpenedFiles.RemoveAt(tabControl.SelectedIndex);
                    tabControl.TabPages.Remove(tabControl.SelectedTab);
                }
                else if (dr == DialogResult.Cancel)
                {
                    return;
                }
            }
            else
            {
                OpenedFiles.RemoveAt(tabControl.SelectedIndex);
                tabControl.TabPages.Remove(tabControl.SelectedTab);
            }

            if (tabControl.TabPages.Count <= 0)
            {
                CreateTab();
            }
        }

        private void TabControlSelecting(object sender, TabControlCancelEventArgs e)
        {
            // Этот метод нужен для того, что бы на каждой новой вкладке применялись заданные ранее параметры (вкл/выкл перенос слов, цвет текста, фона, шрифт и т.д.)
            try
            {
                rtb = tabControl.TabPages[tabControl.SelectedIndex].Controls.OfType<RichTextBox>().First();

                rtb.WordWrap = Properties.Settings.Default.WordWarp;
                rtb.Font = Properties.Settings.Default.EditorFont;
                textLengthLabel.Text = rtb.TextLength.ToString();
                if (rtb.Lines.Length == 0)
                {
                    textLinesLabel.Text = "1";
                }
                else
                {
                    textLinesLabel.Text = rtb.Lines.Length.ToString();
                }

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
                LS.Error($"{ex} Error while selecting tabs (?). Maybe not have any tabs open.");
                textLengthLabel.Text = "0";
                textLinesLabel.Text = "1";
            }
        }

        // Обработка события TextChanged для каждого rich text box'а
        private void TextBoxTextChanged()
        {
            rtb = tabControl.TabPages[tabControl.SelectedIndex].Controls.OfType<RichTextBox>().First();
            textLengthLabel.Text = rtb.Text.Length.ToString();
            if (rtb.Lines.Length == 0)
            {
                textLinesLabel.Text = "1";
            }
            else
            {
                textLinesLabel.Text = rtb.Lines.Length.ToString();
            }

            LS.Trace("Handling the \"Text Changed\" event");
            LS.Debug($"Memory Consumed: {Process.GetProcessesByName("TextPad+")[0].WorkingSet64} Bytes");
        }

        // Методы для кнопок панели проводника
        private void OpenFolderBtnClick(object sender, EventArgs e)
        {
            if (folderBrowserDialog.ShowDialog() == DialogResult.Cancel)
            {
                return;
            }

            workFolderLabel.Text = new DirectoryInfo(folderBrowserDialog.SelectedPath).Name;
            listView.Clear();

            // Папки
            foreach (string item in Directory.GetDirectories(folderBrowserDialog.SelectedPath))
            {
                ListViewItem lvi = new ListViewItem(new DirectoryInfo(item).Name, 0);
                lvi.ToolTipText = item;
                listView.Items.Add(lvi);

            }

            // Файлы
            foreach (string item in Directory.GetFiles(folderBrowserDialog.SelectedPath))
            {
                ListViewItem lvi = new ListViewItem(new DirectoryInfo(item).Name, 1);
                lvi.ToolTipText = item;
                listView.Items.Add(lvi);
            }
        }

        private void AboveFolderBtnClick(object sender, EventArgs e)
        {
            try
            {
                string newDirectory = Directory.GetParent(folderBrowserDialog.SelectedPath).ToString();
                folderBrowserDialog.SelectedPath = newDirectory;

                workFolderLabel.Text = new DirectoryInfo(newDirectory).Name;
                listView.Clear();

                // Папки
                foreach (string item in Directory.GetDirectories(newDirectory))
                {
                    ListViewItem lvi = new ListViewItem(new DirectoryInfo(item).Name, 0);
                    lvi.ToolTipText = item;
                    listView.Items.Add(lvi);

                }

                // Файлы
                foreach (string item in Directory.GetFiles(newDirectory))
                {
                    ListViewItem lvi = new ListViewItem(new DirectoryInfo(item).Name, 1);
                    lvi.ToolTipText = item;
                    listView.Items.Add(lvi);
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

            listView.Clear();

            // Папки
            foreach (string item in Directory.GetDirectories(folderBrowserDialog.SelectedPath))
            {
                ListViewItem lvi = new ListViewItem(new DirectoryInfo(item).Name, 0);
                lvi.ToolTipText = item;
                listView.Items.Add(lvi);
            }

            // Файлы
            foreach (string item in Directory.GetFiles(folderBrowserDialog.SelectedPath))
            {
                ListViewItem lvi = new ListViewItem(new DirectoryInfo(item).Name, 1);
                lvi.ToolTipText = item;
                listView.Items.Add(lvi);
            }
        }

        private void listView_DoubleClick(object sender, EventArgs e)
        {
            string path = listView.Items[listView.FocusedItem.Index].ToolTipText;

            //Проверка существования файла
            if (Directory.Exists(path))
            {
                folderBrowserDialog.SelectedPath = path;

                workFolderLabel.Text = new DirectoryInfo(path).Name;
                listView.Clear();

                // Папки
                foreach (string item in Directory.GetDirectories(path))
                {
                    ListViewItem lvi = new ListViewItem(new DirectoryInfo(item).Name, 0);
                    lvi.ToolTipText = item;
                    listView.Items.Add(lvi);

                }

                // Файлы
                foreach (string item in Directory.GetFiles(path))
                {
                    ListViewItem lvi = new ListViewItem(new DirectoryInfo(item).Name, 1);
                    lvi.ToolTipText = item;
                    listView.Items.Add(lvi);
                }
            }

            else if (File.Exists(path))
            {
                CreateTab(new DirectoryInfo(path).Name);
                OpenedFiles.Insert(tabControl.TabPages.Count - 1, path);
                string fileText = File.ReadAllText(path);

                rtb = tabControl.TabPages[tabControl.TabPages.Count - 1].Controls.OfType<RichTextBox>().First();
                rtb.Text = fileText;

                tabControl.SelectTab(tabControl.TabPages[tabControl.TabPages.Count - 1]);
            }
        }

        private void UpSizeListView(object sender, EventArgs e)
        {
            if (listView.Width < 410)
            {
                listView.Width += 30;
                folderExplorerPanel.Width += 30;
            }
        }

        private void DownSizeListView(object sender, EventArgs e)
        {
            if (listView.Width > 110)
            {
                listView.Width -= 30;
                folderExplorerPanel.Width -= 30;
            }
        }

        private void closeFolderToolBtnClick(object sender, EventArgs e)
        {
            listView.Clear();
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
        private void deleteFile(object sender, EventArgs e)
        {
            if (listView.Items.Count < 1)
                return;

            try
            {
                if (File.Exists(listView.Items[listView.FocusedItem.Index].ToolTipText))
                {
                    if (MessageBox.Show(Resources.Localization.MSGQestionMoveFileToTrash, "TextPad+", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                    {
                        return;
                    }
                    FileSystem.DeleteFile(listView.Items[listView.FocusedItem.Index].ToolTipText, UIOption.OnlyErrorDialogs, RecycleOption.SendToRecycleBin);
                }
                else if (Directory.Exists(listView.Items[listView.FocusedItem.Index].ToolTipText))
                {
                    if (MessageBox.Show(Resources.Localization.MSGQestionMoveFileToTrash, "TextPad+", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                    {
                        return;
                    }

                    FileSystem.DeleteDirectory(listView.Items[listView.FocusedItem.Index].ToolTipText, UIOption.OnlyErrorDialogs, RecycleOption.SendToRecycleBin);
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
        private void CreateTab()
        {
            /* Создаётся экземпляр вкладки, в rtb закидывается новоиспечённый RichTextBox (rtb, потому что раньше это был RichTextBox, а мне лень менять названия),
            * задаются кое-какие параметры, затем в tabControl, который накинут на форму впринципе, добавляется новая вкладка и кней автоматичсеки присваивается ивент TextChanged,
            * метод для которого (TbTextChanged) я создал заранее. Это нужно для динамичного изменения настроек программы в текущей вкладке, например, юзер может выключить функцию
            * переноса слов.
            */
            TabPage tpage = new TabPage(Resources.Localization.newDocumentTitle);
            var rtb = new RichTextBox();
            rtb.Dock = DockStyle.Fill;
            rtb.BorderStyle = BorderStyle.None;
            rtb.WordWrap = Properties.Settings.Default.WordWarp;
            rtb.ContextMenuStrip = contextMenuStrip;
            rtb.Font = Properties.Settings.Default.EditorFont;
            rtb.TextChanged += (sender, args) => TextBoxTextChanged();
            rtb.AcceptsTab = true;

            tpage.Controls.Add(rtb);
            tabControl.TabPages.Add(tpage);

            OpenedFiles.Insert(tabControl.TabPages.IndexOf(tpage), "Missing");
        }

        private void CreateTab(string tabName)
        {
            /* Создаётся экземпляр вкладки, в rtb закидывается новоиспечённый RichTextBox (rtb, потому что раньше это был RichTextBox, а мне лень менять названия),
            * задаются кое-какие параметры, затем в tabControl, который накинут на форму впринципе, добавляется новая вкладка и кней автоматичсеки присваивается ивент TextChanged,
            * метод для которого (TbTextChanged) я создал заранее. Это нужно для динамичного изменения настроек программы в текущей вкладке, например, юзер может выключить функцию
            * переноса слов.
            */
            TabPage tpage = new TabPage(tabName);
            var rtb = new RichTextBox();
            rtb.Dock = DockStyle.Fill;
            rtb.BorderStyle = BorderStyle.None;
            rtb.WordWrap = Properties.Settings.Default.WordWarp;
            rtb.ContextMenuStrip = contextMenuStrip;
            rtb.Font = Properties.Settings.Default.EditorFont;
            rtb.TextChanged += (sender, args) => TextBoxTextChanged();
            rtb.AcceptsTab = true;

            tpage.Controls.Add(rtb);
            tabControl.TabPages.Add(tpage);

            OpenedFiles.Insert(tabControl.TabPages.IndexOf(tpage), "Missing");
        }

        // Цветовые схемы
        internal void colorThemeWhite()
        {
            // Цвет основного интерфейса
            MainMenuStrip.BackColor = SystemColors.Control;
            toolsStrip.BackColor = SystemColors.Control;
            statusStrip.BackColor = SystemColors.Control;
            runFileToolStrip.BackColor = SystemColors.Control;
            BackColor = SystemColors.Control;
            runFileToolStrip.BackColor = SystemColors.Control;

            MainMenuStrip.ForeColor = SystemColors.ControlText;
            runFileToolStrip.ForeColor = SystemColors.ControlText;

            fileMenuItem.BackColor = SystemColors.Control;

            textLabelStatus.ForeColor = SystemColors.ControlText;
            textLengthLabel.ForeColor = SystemColors.ControlText;

            // цвет rtb
            tabControl.TabPages[tabControl.SelectedIndex].BackColor = Color.Transparent;
            rtb = tabControl.TabPages[tabControl.SelectedIndex].Controls.OfType<RichTextBox>().First();

            rtb.ForeColor = Color.Black;
            rtb.BackColor = SystemColors.Window;
            rtb.ForeColor = SystemColors.ControlText;
        }

        internal void colorThemeDark()
        {
            // Цвет основного интерфейса
            MainMenuStrip.BackColor = SystemColors.ControlDarkDark;
            toolsStrip.BackColor = SystemColors.ControlDarkDark;
            statusStrip.BackColor = SystemColors.ControlDarkDark;
            runFileToolStrip.BackColor = SystemColors.ControlDarkDark;
            BackColor = SystemColors.ControlDarkDark;

            MainMenuStrip.ForeColor = SystemColors.ControlLight;
            runFileToolStrip.ForeColor = SystemColors.ControlLight;

            fileMenuItem.BackColor = SystemColors.ControlDarkDark;

            textLabelStatus.ForeColor = SystemColors.ControlLight;
            textLengthLabel.ForeColor = SystemColors.ControlLight;

            // цвет rtb
            tabControl.TabPages[tabControl.SelectedIndex].BackColor = Color.Black;
            rtb = tabControl.TabPages[tabControl.SelectedIndex].Controls.OfType<RichTextBox>().First();

            rtb.ForeColor = Color.White;
            rtb.BackColor = Color.DimGray;
            rtb.ForeColor = Color.Cyan;
        }

        #region Вызов окон

        private void aboutWindow(object sender, EventArgs e)
        {
            FormInfoUI info_ui = new FormInfoUI();
            info_ui.ShowDialog(this);
        }

        private void search(object sender, EventArgs e)
        {
            SearchUI search_ui = new SearchUI();
            search_ui.ShowDialog(this);
        }

        private void settingsWindow(object sender, EventArgs e)
        {
            SettingsUI setwin = new SettingsUI();
            setwin.ShowDialog(this);
        }

        private void newWindow(object sender, EventArgs e)
        {
            Process.Start("TextPad+.exe");
        }

        private void getupdate(object sender, EventArgs e)
        {
            try
            {
                Process.Start(UpdaterPath);
            }
            catch
            {
                MessageBox.Show(Resources.Localization.MSGErrorStartUpdater, "TextPad+", MessageBoxButtons.OK, MessageBoxIcon.Error);
                LS.Error("An error occurred while launching the update installer. Updater.exe is missing.");
            }
        }

        #endregion

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

            // Количество аргументов запуска программы
            string[] args = Environment.GetCommandLineArgs();
            if (args.Length > 1)
            {
                try
                {
                    rtb = tabControl.TabPages[tabControl.SelectedIndex].Controls.OfType<RichTextBox>().First();
                    // в список помещаем под индексом новой вкладки путь до файла

                    OpenedFiles.Insert(tabControl.SelectedIndex, args[1]);
                    // в text box помещаем текст
                    rtb.Text = File.ReadAllText(args[1]);
                    // задаём даголовок вкладки
                    tabControl.SelectedTab.Text = Path.GetFileName(OpenedFiles.ElementAt(Program.mainUI.tabControl.SelectedIndex));
                }
                catch (Exception ex)
                {
                    LS.Error($"{ex} Error when trying to create a tab while running a program through a file: {OpenedFiles.ElementAt(Program.mainUI.tabControl.SelectedIndex)}");
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

            listView.Clear();

            /*
             * Здесь ваыставляется состояние окна, которое было в момент последнего запуска. По умолчанию это обычное.
             * Но если окно было свёрнуто, то всё равно ставится обычное.
            */
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

            // Xml конфигурация
            try
            {
                /*
                 * В файле Config.xml есть корневой элемент configuration. В нм содержатся 2 элемента: program и web.
                 * В первый элемент программа сама записывает инфу о себе, а во втором находятся неизменяемая информация о моём сайте (url адреса).
                 */

                // Загрузка Xml файла
                XmlDocument xmlConfig = new XmlDocument();
                xmlConfig.Load(Application.StartupPath + @"Config.xml");

                // В корневом элементе (DocumentElement) загруженного файла (xmlConfig) идёт обзор элементов в корневом элементе.
                foreach (XmlNode node in xmlConfig.DocumentElement)
                {
                    // Если один из двух элементов в корневом называется program, то в его атрибут name устанавливается название сборки проекта через метод GetAssemblyName();
                    if (node.Name == "program")
                    {
                        node.Attributes.GetNamedItem("name").Value = GetAssemblyName();

                        // обход дочерних узлов из узла node (program) и установка нужных значений
                        foreach (XmlNode childNode in node.ChildNodes)
                        {
                            if (childNode.Name == "version")
                            {
                                childNode.InnerText = GetAssemblyVersion().ToString();
                            }
                            if (childNode.Name == "path")
                            {
                                childNode.InnerText = ProgramPath;
                            }
                            if (childNode.Name == "language")
                            {
                                childNode.InnerText = Properties.Settings.Default.Language;
                            }
                        }
                    }

                    if (node.Name == "web")
                    {
                        foreach (XmlNode childNode in node.ChildNodes)
                        {
                            if (childNode.Name == "site")
                            {
                                WebSite = childNode.InnerText;
                            }
                        }
                    }
                }
                // Сохранение документа
                xmlConfig.Save(Application.StartupPath + @"Config.xml");
            }
            catch (Exception ex)
            {
                LS.Error($"{ex} Missing configuration file");
                MessageBox.Show(Resources.Localization.MissingXmlFile, "TextPad+", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            LS.Debug("Options loaded");
            LS.Debug($"Program path: {ProgramPath}");
            LS.Debug($"Memory Consumed: {Process.GetProcessesByName("TextPad+")[0].WorkingSet64} Bytes");
        }

        private void MainFormClosing(object sender, FormClosingEventArgs e)
        {
            Properties.Settings.Default.MainWindowState = this.WindowState.ToString();
            Properties.Settings.Default.FormWidth = this.Width;
            Properties.Settings.Default.FormHeight = this.Height;
            Properties.Settings.Default.StartScriptsConfig = startScriptCombobox.SelectedItem.ToString();
            Properties.Settings.Default.ExplorerSize = folderExplorerPanel.Width;
            Properties.Settings.Default.Save();

            LS.Info("Exiting the program and saving parameters");
            LS.Debug($"Memory Consumed: {Process.GetProcessesByName("TextPad+")[0].WorkingSet64} Bytes");
            LS.Debug("Total tabs: " + tabControl.TabPages.Count.ToString());
        }
    }
}
