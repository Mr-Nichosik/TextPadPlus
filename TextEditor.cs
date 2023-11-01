
using System.Text;

namespace TextPad_
{
    /// <summary>
    /// Класс, созданный для того, что бы обрабатывать всю логику работы текстового редактора. Пока что работает только исключительно с MTextBox.
    /// Его методы вызываются из класса главного окна программы (MainForm).
    /// </summary>
    internal class TextEditor : IFileRunner
    {
        private static readonly ILogger LS = new LogSystem($"{Application.StartupPath}\\logs");
        private static MTextBox? mtb;

        // Метод сохранения файла "Сохранить как..."
        public static void SaveAsFile()
        {
            try
            {
                mtb = Program.MainUI.cTabControl.TabPages[Program.MainUI.cTabControl.SelectedIndex].Controls.OfType<MTextBox>().First();
                Program.MainUI.saveFileDialog.Filter = Program.MainUI.saveFileDialog.Filter = Resources.Localization.saveFileDialogFilter;

                if (Program.MainUI.saveFileDialog.ShowDialog() == DialogResult.Cancel)
                    return;

                // В свойство FileName моего Modified TextBox добавляется путь до файла, выбранного пользователем при сохранеyии.
                Program.MainUI.cTabControl.TabPages[Program.MainUI.cTabControl.SelectedIndex].Controls.OfType<MTextBox>().First().FileName = Program.MainUI.saveFileDialog.FileName;

                // В файл сохраняется текст текущей вкладки по пути, указанному в FileName, учитывая значение кодировки из mtb.
                Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

                StreamWriter fileWriter;
                switch (mtb.Encoding)
                {
                    case "windows-1251":
                        fileWriter = new StreamWriter(Program.MainUI.cTabControl.TabPages[Program.MainUI.cTabControl.SelectedIndex].Controls.OfType<MTextBox>().First().FileName, false, Encoding.GetEncoding(1251));
                        fileWriter.Write(mtb.Text);
                        fileWriter.Close();
                        break;
                    case "UTF-8":
                        fileWriter = new StreamWriter(Program.MainUI.cTabControl.TabPages[Program.MainUI.cTabControl.SelectedIndex].Controls.OfType<MTextBox>().First().FileName, false);
                        fileWriter.Write(mtb.Text);
                        fileWriter.Close();
                        break;
                    case "UTF-16":
                        fileWriter = new StreamWriter(Program.MainUI.cTabControl.TabPages[Program.MainUI.cTabControl.SelectedIndex].Controls.OfType<MTextBox>().First().FileName, false, Encoding.GetEncoding(1200));
                        fileWriter.Write(mtb.Text);
                        fileWriter.Close();
                        break;
                    case "UTF-16 BE":
                        fileWriter = new StreamWriter(Program.MainUI.cTabControl.TabPages[Program.MainUI.cTabControl.SelectedIndex].Controls.OfType<MTextBox>().First().FileName, false, Encoding.GetEncoding(1201));
                        fileWriter.Write(mtb.Text);
                        fileWriter.Close();
                        break;
                    case "UTF-32":
                        fileWriter = new StreamWriter(Program.MainUI.cTabControl.TabPages[Program.MainUI.cTabControl.SelectedIndex].Controls.OfType<MTextBox>().First().FileName, false, Encoding.GetEncoding(12000));
                        fileWriter.Write(mtb.Text);
                        fileWriter.Close();
                        break;
                    case "UTF-32 BE":
                        fileWriter = new StreamWriter(Program.MainUI.cTabControl.TabPages[Program.MainUI.cTabControl.SelectedIndex].Controls.OfType<MTextBox>().First().FileName, false, Encoding.GetEncoding(12001));
                        fileWriter.Write(mtb.Text);
                        fileWriter.Close();
                        break;
                }

                // заголовком вкладки становится название файла
                Program.MainUI.cTabControl.SelectedTab.Text = Path.GetFileName(Program.MainUI.cTabControl.TabPages[Program.MainUI.cTabControl.SelectedIndex].Controls.OfType<MTextBox>().First().FileName);

                // Обновление статуса файла
                mtb.IsFileChanged = false;
                Program.MainUI.cTabControl.TabPages[Program.MainUI.cTabControl.SelectedIndex].Text = Program.MainUI.cTabControl.TabPages[Program.MainUI.cTabControl.SelectedIndex].Text.TrimEnd('*');

                // Внос файла в список недавних файлов
                if (Program.MainUI.recentFilesMenuItem.DropDownItems.Count == 10)
                    Program.MainUI.recentFilesMenuItem.DropDownItems.RemoveAt(0);
                ToolStripMenuItem tsmi = new ToolStripMenuItem();
                tsmi.Text = Program.MainUI.cTabControl.TabPages[Program.MainUI.cTabControl.SelectedIndex].Controls.OfType<MTextBox>().First().FileName;
                tsmi.Click += (sender, e) => OpenFile(tsmi.Text);
                Program.MainUI.recentFilesMenuItem.DropDownItems.Add(tsmi);

                Program.MainUI.deletFileFileMenuItem.Enabled = true;

                LS.Info("Saving file: " + Program.MainUI.cTabControl.TabPages[Program.MainUI.cTabControl.SelectedIndex].Controls.OfType<MTextBox>().First().FileName);

            }
            catch (Exception)
            {
                //LS.Error($"{ex} by saving file.");
                //MessageBox.Show(Resources.Localization.MSGErrorWhenSaveFile, "TextPad+", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Метод сохранения файла, открытого на выбранной вкладке
        public static void SaveCurrentFile()
        {
            try
            {
                mtb = Program.MainUI.cTabControl.TabPages[Program.MainUI.cTabControl.SelectedIndex].Controls.OfType<MTextBox>().First();

                // если путь до файла отсутствует (Missing)
                if (Program.MainUI.cTabControl.TabPages[Program.MainUI.cTabControl.SelectedIndex].Controls.OfType<MTextBox>().First().FileName == "Missing")
                {
                    // то вызываем обычный saveAsFile метод, где используется saveFileDialog и добавляется в список под индексом вкладки выранный путь.
                    SaveAsFile();
                }
                // если нет, то значит путь до файла в списке найден, а значит сохраняем через этот самый путь как обычно.
                else
                {
                    // В файл сохраняется текст текущей вкладки по пути, указанному в FileName, учитывая значение кодировки из mtb.
                    StreamWriter fileWriter;
                    switch (mtb.Encoding)
                    {
                        case "UTF-8":
                            fileWriter = new StreamWriter(Program.MainUI.cTabControl.TabPages[Program.MainUI.cTabControl.SelectedIndex].Controls.OfType<MTextBox>().First().FileName, false);
                            fileWriter.Write(mtb.Text);
                            fileWriter.Close();
                            break;
                        case "UTF-16":
                            fileWriter = new StreamWriter(Program.MainUI.cTabControl.TabPages[Program.MainUI.cTabControl.SelectedIndex].Controls.OfType<MTextBox>().First().FileName, false, Encoding.GetEncoding(1200));
                            fileWriter.Write(mtb.Text);
                            fileWriter.Close();
                            break;
                        case "UTF-16 BE":
                            fileWriter = new StreamWriter(Program.MainUI.cTabControl.TabPages[Program.MainUI.cTabControl.SelectedIndex].Controls.OfType<MTextBox>().First().FileName, false, Encoding.GetEncoding(1201));
                            fileWriter.Write(mtb.Text);
                            fileWriter.Close();
                            break;
                        case "UTF-32":
                            fileWriter = new StreamWriter(Program.MainUI.cTabControl.TabPages[Program.MainUI.cTabControl.SelectedIndex].Controls.OfType<MTextBox>().First().FileName, false, Encoding.GetEncoding(12000));
                            fileWriter.Write(mtb.Text);
                            fileWriter.Close();
                            break;
                        case "UTF-32 BE":
                            fileWriter = new StreamWriter(Program.MainUI.cTabControl.TabPages[Program.MainUI.cTabControl.SelectedIndex].Controls.OfType<MTextBox>().First().FileName, false, Encoding.GetEncoding(12001));
                            fileWriter.Write(mtb.Text);
                            fileWriter.Close();
                            break;
                    }

                    // Обновление статуса файла
                    mtb.IsFileChanged = false;
                    Program.MainUI.cTabControl.TabPages[Program.MainUI.cTabControl.SelectedIndex].Text = Program.MainUI.cTabControl.TabPages[Program.MainUI.cTabControl.SelectedIndex].Text.TrimEnd('*');

                    Program.MainUI.deletFileFileMenuItem.Enabled = true;
                }

                LS.Info("Saving file: " + Program.MainUI.cTabControl.TabPages[Program.MainUI.cTabControl.SelectedIndex].Controls.OfType<MTextBox>().First().FileName);
            }
            catch (Exception ex)
            {
                LS.Error($"{ex} by saving current file.");
                MessageBox.Show(Resources.Localization.MSGErrorWhenSaveFile, "TextPad+", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Открытие файла
        public static void OpenFile()
        {
            Program.MainUI.openFileDialog.Filter = Resources.Localization.openFileDialogFilter;
            if (Program.MainUI.openFileDialog.ShowDialog() == DialogResult.Cancel)
                return;

            try
            {
                mtb = Program.MainUI.cTabControl.TabPages[Program.MainUI.cTabControl.SelectedIndex].Controls.OfType<MTextBox>().First();

                // Вызываем openFileDialog и полученный путь до файла (fileName) записываем в свойство MTextBox'а FileName
                Program.MainUI.cTabControl.TabPages[Program.MainUI.cTabControl.SelectedIndex].Controls.OfType<MTextBox>().First().FileName = Program.MainUI.openFileDialog.FileName;

                // Считываем текст этого файла, путь берём из того же свойства. Проверяем кодировку
                StreamReader fileReader;
                switch (mtb.Encoding)
                {
                    case "UTF-8":
                        fileReader = new StreamReader(Program.MainUI.cTabControl.TabPages[Program.MainUI.cTabControl.SelectedIndex].Controls.OfType<MTextBox>().First().FileName);
                        mtb.Text = fileReader.ReadToEnd();
                        fileReader.Close();
                        break;
                    case "UTF-16":
                        fileReader = new StreamReader(Program.MainUI.cTabControl.TabPages[Program.MainUI.cTabControl.SelectedIndex].Controls.OfType<MTextBox>().First().FileName, Encoding.GetEncoding(1200));
                        mtb.Text = fileReader.ReadToEnd();
                        fileReader.Close();
                        break;
                    case "UTF-16 BE":
                        fileReader = new StreamReader(Program.MainUI.cTabControl.TabPages[Program.MainUI.cTabControl.SelectedIndex].Controls.OfType<MTextBox>().First().FileName, Encoding.GetEncoding(1201));
                        mtb.Text = fileReader.ReadToEnd();
                        fileReader.Close();
                        break;
                    case "UTF-32":
                        fileReader = new StreamReader(Program.MainUI.cTabControl.TabPages[Program.MainUI.cTabControl.SelectedIndex].Controls.OfType<MTextBox>().First().FileName, Encoding.GetEncoding(12000));
                        mtb.Text = fileReader.ReadToEnd();
                        fileReader.Close();
                        break;
                    case "UTF-32 BE":
                        fileReader = new StreamReader(Program.MainUI.cTabControl.TabPages[Program.MainUI.cTabControl.SelectedIndex].Controls.OfType<MTextBox>().First().FileName, Encoding.GetEncoding(12001));
                        mtb.Text = fileReader.ReadToEnd();
                        fileReader.Close();
                        break;
                }
                // Обновление статуса файла
                mtb.IsFileChanged = false;
                Program.MainUI.cTabControl.TabPages[Program.MainUI.cTabControl.SelectedIndex].Text = Program.MainUI.cTabControl.TabPages[Program.MainUI.cTabControl.SelectedIndex].Text.TrimEnd('*');

                // Задаём заголовок вкладки с названием файла
                Program.MainUI.cTabControl.SelectedTab.Text = Path.GetFileName(Program.MainUI.cTabControl.TabPages[Program.MainUI.cTabControl.SelectedIndex].Controls.OfType<MTextBox>().First().FileName);

                // Внос файла в список недавних файлов
                foreach (ToolStripMenuItem rfmdi in Program.MainUI.recentFilesMenuItem.DropDownItems)
                {
                    if (rfmdi.Text == Program.MainUI.cTabControl.TabPages[Program.MainUI.cTabControl.SelectedIndex].Controls.OfType<MTextBox>().First().FileName)
                        return;
                }

                if (Program.MainUI.recentFilesMenuItem.DropDownItems.Count == 10)
                    Program.MainUI.recentFilesMenuItem.DropDownItems.RemoveAt(0);
                ToolStripMenuItem tsmi = new();
                tsmi.Text = Program.MainUI.cTabControl.TabPages[Program.MainUI.cTabControl.SelectedIndex].Controls.OfType<MTextBox>().First().FileName;
                tsmi.Click += (sender, e) => OpenFile(tsmi.Text);
                Program.MainUI.recentFilesMenuItem.DropDownItems.Add(tsmi);

                LS.Info("Opening file: " + Program.MainUI.cTabControl.TabPages[Program.MainUI.cTabControl.SelectedIndex].Controls.OfType<MTextBox>().First().FileName);
            }
            catch (Exception ex)
            {
                LS.Error($"{ex} when opening an open file.");
                MessageBox.Show(Resources.Localization.MSGErrorWhenOpenFIle, "TextPad+", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Открытие файла по конкретному пути
        public static void OpenFile(string fileName)
        {
            try
            {
                if (File.Exists(fileName))
                {
                    mtb = Program.MainUI.cTabControl.TabPages[Program.MainUI.cTabControl.SelectedIndex].Controls.OfType<MTextBox>().First();

                    if (mtb.FileName != "Missing" || mtb.Text.Length != 0)
                    {
                        Program.MainUI.CreateTab();
                        Program.MainUI.cTabControl.SelectTab(Program.MainUI.cTabControl.TabPages[Program.MainUI.cTabControl.TabPages.Count - 1]);
                    }

                    mtb = Program.MainUI.cTabControl.TabPages[Program.MainUI.cTabControl.SelectedIndex].Controls.OfType<MTextBox>().First();

                    // В свойство FileName записываем путь до файла, который передаётся методу
                    Program.MainUI.cTabControl.TabPages[Program.MainUI.cTabControl.SelectedIndex].Controls.OfType<MTextBox>().First().FileName = fileName;

                    // Считываем текст этого файла, путь берём из того же свойства. Проверяем кодировку
                    StreamReader fileReader;
                    switch (mtb.Encoding)
                    {
                        case "UTF-8":
                            fileReader = new StreamReader(Program.MainUI.cTabControl.TabPages[Program.MainUI.cTabControl.SelectedIndex].Controls.OfType<MTextBox>().First().FileName);
                            mtb.Text = fileReader.ReadToEnd();
                            fileReader.Close();
                            break;
                        case "UTF-16":
                            fileReader = new StreamReader(Program.MainUI.cTabControl.TabPages[Program.MainUI.cTabControl.SelectedIndex].Controls.OfType<MTextBox>().First().FileName, Encoding.GetEncoding(1200));
                            mtb.Text = fileReader.ReadToEnd();
                            fileReader.Close();
                            break;
                        case "UTF-16 BE":
                            fileReader = new StreamReader(Program.MainUI.cTabControl.TabPages[Program.MainUI.cTabControl.SelectedIndex].Controls.OfType<MTextBox>().First().FileName, Encoding.GetEncoding(1201));
                            mtb.Text = fileReader.ReadToEnd();
                            fileReader.Close();
                            break;
                        case "UTF-32":
                            fileReader = new StreamReader(Program.MainUI.cTabControl.TabPages[Program.MainUI.cTabControl.SelectedIndex].Controls.OfType<MTextBox>().First().FileName, Encoding.GetEncoding(12000));
                            mtb.Text = fileReader.ReadToEnd();
                            fileReader.Close();
                            break;
                        case "UTF-32 BE":
                            fileReader = new StreamReader(Program.MainUI.cTabControl.TabPages[Program.MainUI.cTabControl.SelectedIndex].Controls.OfType<MTextBox>().First().FileName, Encoding.GetEncoding(12001));
                            mtb.Text = fileReader.ReadToEnd();
                            fileReader.Close();
                            break;
                    }

                    // Обновление статуса файла
                    mtb.IsFileChanged = false;

                    // Задаём заголовок вкладки с названием файла
                    Program.MainUI.cTabControl.SelectedTab.Text = Path.GetFileName(Program.MainUI.cTabControl.TabPages[Program.MainUI.cTabControl.SelectedIndex].Controls.OfType<MTextBox>().First().FileName);

                    // Внос файла в список недавних файлов
                    foreach (ToolStripMenuItem rfmdi in Program.MainUI.recentFilesMenuItem.DropDownItems)
                    {
                        if (rfmdi.Text == Program.MainUI.cTabControl.TabPages[Program.MainUI.cTabControl.SelectedIndex].Controls.OfType<MTextBox>().First().FileName)
                            return;
                    }

                    if (Program.MainUI.recentFilesMenuItem.DropDownItems.Count == 10)
                        Program.MainUI.recentFilesMenuItem.DropDownItems.RemoveAt(0);
                    ToolStripMenuItem tsmi = new();
                    tsmi.Text = Program.MainUI.cTabControl.TabPages[Program.MainUI.cTabControl.SelectedIndex].Controls.OfType<MTextBox>().First().FileName;
                    tsmi.Click += (sender, e) => OpenFile(tsmi.Text);
                    Program.MainUI.recentFilesMenuItem.DropDownItems.Add(tsmi);
                }
                else
                    MessageBox.Show(Resources.Localization.MSGErrorFileNotFound, "TextPad+", MessageBoxButtons.OK, MessageBoxIcon.Stop);

                LS.Info("Opening file: " + Program.MainUI.cTabControl.TabPages[Program.MainUI.cTabControl.SelectedIndex].Controls.OfType<MTextBox>().First().FileName);
            }
            catch (Exception ex)
            {
                LS.Error($"{ex} when opening an open file.");
                MessageBox.Show(Resources.Localization.MSGErrorWhenOpenFIle, "TextPad+", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #region Стандартные функции для правки текста, по названиям понятно, чё он делают.
        public static void copyTextFromTB(CTabControl cTabControl)
        {
            mtb = cTabControl.TabPages[cTabControl.SelectedIndex].Controls.OfType<MTextBox>().First();
            if (mtb.TextLength > 0)
            {
                mtb.Copy();
            }
        }

        public static void cutTextFromTB(CTabControl cTabControl)
        {
            mtb = cTabControl.TabPages[cTabControl.SelectedIndex].Controls.OfType<MTextBox>().First();
            if (mtb.TextLength > 0)
            {
                mtb.Cut();
            }
        }

        public static void pasteTextFromTB(CTabControl cTabControl)
        {
            mtb = cTabControl.TabPages[cTabControl.SelectedIndex].Controls.OfType<MTextBox>().First();
            mtb.Paste();
            string newText = mtb.Text;
            mtb.Text = newText.ToString();
            mtb.Font = Properties.Settings.Default.ModifiedTextBox_Font;
        }

        public static void fontTextFromTB(CTabControl cTabControl, FontDialog fontDialog)
        {
            mtb = cTabControl.TabPages[cTabControl.SelectedIndex].Controls.OfType<MTextBox>().First();
            fontDialog.ShowDialog();
            mtb.Font = fontDialog.Font;
            Properties.Settings.Default.ModifiedTextBox_Font = fontDialog.Font;
        }

        public static void selectAllTextFromTB(CTabControl cTabControl)
        {
            mtb = cTabControl.TabPages[cTabControl.SelectedIndex].Controls.OfType<MTextBox>().First();
            if (mtb.TextLength > 0)
            {
                mtb.SelectAll();
            }
        }

        public static void redoTextFromTB(CTabControl cTabControl)
        {
            mtb = cTabControl.TabPages[cTabControl.SelectedIndex].Controls.OfType<MTextBox>().First();
            mtb.Redo();
        }

        public static void undoTextFromTB(CTabControl cTabControl)
        {
            mtb = cTabControl.TabPages[cTabControl.SelectedIndex].Controls.OfType<MTextBox>().First();
            mtb.Undo();
        }

        public static void deleteTextFromTB(CTabControl cTabControl)
        {
            mtb = cTabControl.TabPages[cTabControl.SelectedIndex].Controls.OfType<MTextBox>().First();
            if (mtb.TextLength > 0)
            {
                mtb.SelectedText = string.Empty;
            }
        }

        public static void dateTime(CTabControl cTabControl)
        {
            mtb = cTabControl.TabPages[cTabControl.SelectedIndex].Controls.OfType<MTextBox>().First();
            mtb.AppendText(Convert.ToString(DateTime.Now));
        }
        #endregion

        // Методы запуска файлов
        public void RunScript()
        {
            if (Program.MainUI.startScriptCombobox.SelectedIndex == 0)
            {
                FormPythonInterpreterUI pythonInterpreterUI = new FormPythonInterpreterUI();
                pythonInterpreterUI.ShowDialog(Program.MainUI);
            }
            else if (Program.MainUI.startScriptCombobox.SelectedIndex == 1)
            {
                BatRun();
            }
            else if (Program.MainUI.startScriptCombobox.SelectedIndex == 2)
            {
                VbsRun();
            }
        }

        public void PythonRun()
        {
            LS.Info("Running a Python file");
            mtb = Program.MainUI.cTabControl.TabPages[Program.MainUI.cTabControl.SelectedIndex].Controls.OfType<MTextBox>().First();

            try
            {
                if (mtb.TextLength == 0)
                {
                    MessageBox.Show(Resources.Localization.MSGErrorWhenRunningEmptyScript, "TextPad+", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                SaveCurrentFile();

                if (Program.MainUI.cTabControl.TabPages[Program.MainUI.cTabControl.SelectedIndex].Controls.OfType<MTextBox>().First().FileName == "Missing")
                    return;

                Process.Start(@"C:\Windows\py.exe", Program.MainUI.cTabControl.TabPages[Program.MainUI.cTabControl.SelectedIndex].Controls.OfType<MTextBox>().First().FileName);
            }
            catch (Exception ex)
            {
                LS.Error($"{ex} Error while running python file: {Program.MainUI.cTabControl.TabPages[Program.MainUI.cTabControl.SelectedIndex].Controls.OfType<MTextBox>().First().FileName}");
                MessageBox.Show(Resources.Localization.MSGErrorRunPythonFile, "TextPad+", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void PythonRun(string path)
        {
            LS.Info("Running a Python file");
            mtb = Program.MainUI.cTabControl.TabPages[Program.MainUI.cTabControl.SelectedIndex].Controls.OfType<MTextBox>().First();

            try
            {
                if (mtb.TextLength == 0)
                {
                    MessageBox.Show(Resources.Localization.MSGErrorWhenRunningEmptyScript, "TextPad+", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                SaveCurrentFile();

                if (Program.MainUI.cTabControl.TabPages[Program.MainUI.cTabControl.SelectedIndex].Controls.OfType<MTextBox>().First().FileName == "Missing")
                    return;

                Process.Start(path, Program.MainUI.cTabControl.TabPages[Program.MainUI.cTabControl.SelectedIndex].Controls.OfType<MTextBox>().First().FileName);
            }
            catch (Exception ex)
            {
                LS.Error($"{ex} Error while running python file: {Program.MainUI.cTabControl.TabPages[Program.MainUI.cTabControl.SelectedIndex].Controls.OfType<MTextBox>().First().FileName}");
                MessageBox.Show(Resources.Localization.MSGErrorRunPythonFile, "TextPad+", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void BatRun()
        {
            LS.Info("Running a bat file");

            mtb = Program.MainUI.cTabControl.TabPages[Program.MainUI.cTabControl.SelectedIndex].Controls.OfType<MTextBox>().First();

            try
            {
                if (mtb.TextLength == 0)
                {
                    MessageBox.Show(Resources.Localization.MSGErrorWhenRunningEmptyScript, "TextPad+", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                SaveCurrentFile();

                Process.Start(Program.MainUI.cTabControl.TabPages[Program.MainUI.cTabControl.SelectedIndex].Controls.OfType<MTextBox>().First().FileName);
            }
            catch (Exception ex)
            {
                LS.Error($"{ex} Error while running Windows Script: {Program.MainUI.cTabControl.TabPages[Program.MainUI.cTabControl.SelectedIndex].Controls.OfType<MTextBox>().First().FileName}");
                MessageBox.Show(Resources.Localization.MSGErrorRunBatFile, "TextPad+", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void VbsRun()
        {
            LS.Info("Running a vbs file");

            mtb = Program.MainUI.cTabControl.TabPages[Program.MainUI.cTabControl.SelectedIndex].Controls.OfType<MTextBox>().First();

            try
            {
                if (mtb.TextLength == 0)
                {
                    MessageBox.Show(Resources.Localization.MSGErrorWhenRunningEmptyScript, "TextPad+", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                SaveCurrentFile();

                Process.Start("wscript.exe", Program.MainUI.cTabControl.TabPages[Program.MainUI.cTabControl.SelectedIndex].Controls.OfType<MTextBox>().First().FileName);
            }
            catch (Exception ex)
            {
                LS.Error($"{ex} Error while running VBScript file: {Program.MainUI.cTabControl.TabPages[Program.MainUI.cTabControl.SelectedIndex].Controls.OfType<MTextBox>().First().FileName}");
                MessageBox.Show(Resources.Localization.MSGErrorRunVBSFile, "TextPad+", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
