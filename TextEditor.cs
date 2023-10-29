
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
                mtb = Program.mainUI.cTabControl.TabPages[Program.mainUI.cTabControl.SelectedIndex].Controls.OfType<MTextBox>().First();
                Program.mainUI.saveFileDialog.Filter = Program.mainUI.saveFileDialog.Filter = Resources.Localization.saveFileDialogFilter;

                if (Program.mainUI.saveFileDialog.ShowDialog() == DialogResult.Cancel)
                    return;

                // В свойство FileName моего Modified TextBox добавляется путь до файла, выбранного пользователем при сохранеyии.
                Program.mainUI.cTabControl.TabPages[Program.mainUI.cTabControl.SelectedIndex].Controls.OfType<MTextBox>().First().FileName = Program.mainUI.saveFileDialog.FileName;

                // В файл сохраняется текст текущей вкладки по пути, указанному в FileName, учитывая значение кодировки из mtb.
                Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

                StreamWriter fileWriter;
                switch (mtb.Encoding)
                {
                    case "windows-1251":
                        fileWriter = new StreamWriter(Program.mainUI.cTabControl.TabPages[Program.mainUI.cTabControl.SelectedIndex].Controls.OfType<MTextBox>().First().FileName, false, Encoding.GetEncoding(1251));
                        fileWriter.Write(mtb.Text);
                        fileWriter.Close();
                        break;
                    case "UTF-8":
                        fileWriter = new StreamWriter(Program.mainUI.cTabControl.TabPages[Program.mainUI.cTabControl.SelectedIndex].Controls.OfType<MTextBox>().First().FileName, false);
                        fileWriter.Write(mtb.Text);
                        fileWriter.Close();
                        break;
                    case "UTF-16":
                        fileWriter = new StreamWriter(Program.mainUI.cTabControl.TabPages[Program.mainUI.cTabControl.SelectedIndex].Controls.OfType<MTextBox>().First().FileName, false, Encoding.GetEncoding(1200));
                        fileWriter.Write(mtb.Text);
                        fileWriter.Close();
                        break;
                    case "UTF-16 BE":
                        fileWriter = new StreamWriter(Program.mainUI.cTabControl.TabPages[Program.mainUI.cTabControl.SelectedIndex].Controls.OfType<MTextBox>().First().FileName, false, Encoding.GetEncoding(1201));
                        fileWriter.Write(mtb.Text);
                        fileWriter.Close();
                        break;
                    case "UTF-32":
                        fileWriter = new StreamWriter(Program.mainUI.cTabControl.TabPages[Program.mainUI.cTabControl.SelectedIndex].Controls.OfType<MTextBox>().First().FileName, false, Encoding.GetEncoding(12000));
                        fileWriter.Write(mtb.Text);
                        fileWriter.Close();
                        break;
                    case "UTF-32 BE":
                        fileWriter = new StreamWriter(Program.mainUI.cTabControl.TabPages[Program.mainUI.cTabControl.SelectedIndex].Controls.OfType<MTextBox>().First().FileName, false, Encoding.GetEncoding(12001));
                        fileWriter.Write(mtb.Text);
                        fileWriter.Close();
                        break;
                }

                // заголовком вкладки становится название файла
                Program.mainUI.cTabControl.SelectedTab.Text = Path.GetFileName(Program.mainUI.cTabControl.TabPages[Program.mainUI.cTabControl.SelectedIndex].Controls.OfType<MTextBox>().First().FileName);

                // Обновление статуса файла
                mtb.IsFileChanged = false;
                Program.mainUI.cTabControl.TabPages[Program.mainUI.cTabControl.SelectedIndex].Text = Program.mainUI.cTabControl.TabPages[Program.mainUI.cTabControl.SelectedIndex].Text.TrimEnd('*');

                // Внос файла в список недавних файлов
                if (Program.mainUI.recentFilesMenuItem.DropDownItems.Count == 10)
                    Program.mainUI.recentFilesMenuItem.DropDownItems.RemoveAt(0);
                ToolStripMenuItem tsmi = new ToolStripMenuItem();
                tsmi.Text = Program.mainUI.cTabControl.TabPages[Program.mainUI.cTabControl.SelectedIndex].Controls.OfType<MTextBox>().First().FileName;
                tsmi.Click += (sender, e) => OpenFile(tsmi.Text);
                Program.mainUI.recentFilesMenuItem.DropDownItems.Add(tsmi);

                Program.mainUI.deletFileFileMenuItem.Enabled = true;

                LS.Info("Saving file: " + Program.mainUI.cTabControl.TabPages[Program.mainUI.cTabControl.SelectedIndex].Controls.OfType<MTextBox>().First().FileName);

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
                mtb = Program.mainUI.cTabControl.TabPages[Program.mainUI.cTabControl.SelectedIndex].Controls.OfType<MTextBox>().First();

                // если путь до файла отсутствует (Missing)
                if (Program.mainUI.cTabControl.TabPages[Program.mainUI.cTabControl.SelectedIndex].Controls.OfType<MTextBox>().First().FileName == "Missing")
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
                            fileWriter = new StreamWriter(Program.mainUI.cTabControl.TabPages[Program.mainUI.cTabControl.SelectedIndex].Controls.OfType<MTextBox>().First().FileName, false);
                            fileWriter.Write(mtb.Text);
                            fileWriter.Close();
                            break;
                        case "UTF-16":
                            fileWriter = new StreamWriter(Program.mainUI.cTabControl.TabPages[Program.mainUI.cTabControl.SelectedIndex].Controls.OfType<MTextBox>().First().FileName, false, Encoding.GetEncoding(1200));
                            fileWriter.Write(mtb.Text);
                            fileWriter.Close();
                            break;
                        case "UTF-16 BE":
                            fileWriter = new StreamWriter(Program.mainUI.cTabControl.TabPages[Program.mainUI.cTabControl.SelectedIndex].Controls.OfType<MTextBox>().First().FileName, false, Encoding.GetEncoding(1201));
                            fileWriter.Write(mtb.Text);
                            fileWriter.Close();
                            break;
                        case "UTF-32":
                            fileWriter = new StreamWriter(Program.mainUI.cTabControl.TabPages[Program.mainUI.cTabControl.SelectedIndex].Controls.OfType<MTextBox>().First().FileName, false, Encoding.GetEncoding(12000));
                            fileWriter.Write(mtb.Text);
                            fileWriter.Close();
                            break;
                        case "UTF-32 BE":
                            fileWriter = new StreamWriter(Program.mainUI.cTabControl.TabPages[Program.mainUI.cTabControl.SelectedIndex].Controls.OfType<MTextBox>().First().FileName, false, Encoding.GetEncoding(12001));
                            fileWriter.Write(mtb.Text);
                            fileWriter.Close();
                            break;
                    }

                    // Обновление статуса файла
                    mtb.IsFileChanged = false;
                    Program.mainUI.cTabControl.TabPages[Program.mainUI.cTabControl.SelectedIndex].Text = Program.mainUI.cTabControl.TabPages[Program.mainUI.cTabControl.SelectedIndex].Text.TrimEnd('*');

                    Program.mainUI.deletFileFileMenuItem.Enabled = true;
                }

                LS.Info("Saving file: " + Program.mainUI.cTabControl.TabPages[Program.mainUI.cTabControl.SelectedIndex].Controls.OfType<MTextBox>().First().FileName);
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
            Program.mainUI.openFileDialog.Filter = Resources.Localization.openFileDialogFilter;
            if (Program.mainUI.openFileDialog.ShowDialog() == DialogResult.Cancel)
                return;

            try
            {
                mtb = Program.mainUI.cTabControl.TabPages[Program.mainUI.cTabControl.SelectedIndex].Controls.OfType<MTextBox>().First();

                // Вызываем openFileDialog и полученный путь до файла (fileName) записываем в свойство MTextBox'а FileName
                Program.mainUI.cTabControl.TabPages[Program.mainUI.cTabControl.SelectedIndex].Controls.OfType<MTextBox>().First().FileName = Program.mainUI.openFileDialog.FileName;

                // Считываем текст этого файла, путь берём из того же свойства. Проверяем кодировку
                StreamReader fileReader;
                switch (mtb.Encoding)
                {
                    case "UTF-8":
                        fileReader = new StreamReader(Program.mainUI.cTabControl.TabPages[Program.mainUI.cTabControl.SelectedIndex].Controls.OfType<MTextBox>().First().FileName);
                        mtb.Text = fileReader.ReadToEnd();
                        fileReader.Close();
                        break;
                    case "UTF-16":
                        fileReader = new StreamReader(Program.mainUI.cTabControl.TabPages[Program.mainUI.cTabControl.SelectedIndex].Controls.OfType<MTextBox>().First().FileName, Encoding.GetEncoding(1200));
                        mtb.Text = fileReader.ReadToEnd();
                        fileReader.Close();
                        break;
                    case "UTF-16 BE":
                        fileReader = new StreamReader(Program.mainUI.cTabControl.TabPages[Program.mainUI.cTabControl.SelectedIndex].Controls.OfType<MTextBox>().First().FileName, Encoding.GetEncoding(1201));
                        mtb.Text = fileReader.ReadToEnd();
                        fileReader.Close();
                        break;
                    case "UTF-32":
                        fileReader = new StreamReader(Program.mainUI.cTabControl.TabPages[Program.mainUI.cTabControl.SelectedIndex].Controls.OfType<MTextBox>().First().FileName, Encoding.GetEncoding(12000));
                        mtb.Text = fileReader.ReadToEnd();
                        fileReader.Close();
                        break;
                    case "UTF-32 BE":
                        fileReader = new StreamReader(Program.mainUI.cTabControl.TabPages[Program.mainUI.cTabControl.SelectedIndex].Controls.OfType<MTextBox>().First().FileName, Encoding.GetEncoding(12001));
                        mtb.Text = fileReader.ReadToEnd();
                        fileReader.Close();
                        break;
                }
                // Обновление статуса файла
                mtb.IsFileChanged = false;
                Program.mainUI.cTabControl.TabPages[Program.mainUI.cTabControl.SelectedIndex].Text = Program.mainUI.cTabControl.TabPages[Program.mainUI.cTabControl.SelectedIndex].Text.TrimEnd('*');

                // Задаём заголовок вкладки с названием файла
                Program.mainUI.cTabControl.SelectedTab.Text = Path.GetFileName(Program.mainUI.cTabControl.TabPages[Program.mainUI.cTabControl.SelectedIndex].Controls.OfType<MTextBox>().First().FileName);

                // Внос файла в список недавних файлов
                if (Program.mainUI.recentFilesMenuItem.DropDownItems.Count == 10)
                    Program.mainUI.recentFilesMenuItem.DropDownItems.RemoveAt(0);
                ToolStripMenuItem tsmi = new ToolStripMenuItem();
                tsmi.Text = Program.mainUI.cTabControl.TabPages[Program.mainUI.cTabControl.SelectedIndex].Controls.OfType<MTextBox>().First().FileName;
                tsmi.Click += (sender, e) => OpenFile(tsmi.Text);
                Program.mainUI.recentFilesMenuItem.DropDownItems.Add(tsmi);

                LS.Info("Opening file: " + Program.mainUI.cTabControl.TabPages[Program.mainUI.cTabControl.SelectedIndex].Controls.OfType<MTextBox>().First().FileName);
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
                Program.mainUI.CreateTab();
                Program.mainUI.cTabControl.SelectTab(Program.mainUI.cTabControl.TabPages[Program.mainUI.cTabControl.TabPages.Count - 1]);

                mtb = Program.mainUI.cTabControl.TabPages[Program.mainUI.cTabControl.SelectedIndex].Controls.OfType<MTextBox>().First();

                // В свойство FileName записываем путь до файла, который передаётся методу
                Program.mainUI.cTabControl.TabPages[Program.mainUI.cTabControl.SelectedIndex].Controls.OfType<MTextBox>().First().FileName = fileName;

                // Считываем текст этого файла, путь берём из того же свойства. Проверяем кодировку
                StreamReader fileReader;
                switch (mtb.Encoding)
                {
                    case "UTF-8":
                        fileReader = new StreamReader(Program.mainUI.cTabControl.TabPages[Program.mainUI.cTabControl.SelectedIndex].Controls.OfType<MTextBox>().First().FileName);
                        mtb.Text = fileReader.ReadToEnd();
                        fileReader.Close();
                        break;
                    case "UTF-16":
                        fileReader = new StreamReader(Program.mainUI.cTabControl.TabPages[Program.mainUI.cTabControl.SelectedIndex].Controls.OfType<MTextBox>().First().FileName, Encoding.GetEncoding(1200));
                        mtb.Text = fileReader.ReadToEnd();
                        fileReader.Close();
                        break;
                    case "UTF-16 BE":
                        fileReader = new StreamReader(Program.mainUI.cTabControl.TabPages[Program.mainUI.cTabControl.SelectedIndex].Controls.OfType<MTextBox>().First().FileName, Encoding.GetEncoding(1201));
                        mtb.Text = fileReader.ReadToEnd();
                        fileReader.Close();
                        break;
                    case "UTF-32":
                        fileReader = new StreamReader(Program.mainUI.cTabControl.TabPages[Program.mainUI.cTabControl.SelectedIndex].Controls.OfType<MTextBox>().First().FileName, Encoding.GetEncoding(12000));
                        mtb.Text = fileReader.ReadToEnd();
                        fileReader.Close();
                        break;
                    case "UTF-32 BE":
                        fileReader = new StreamReader(Program.mainUI.cTabControl.TabPages[Program.mainUI.cTabControl.SelectedIndex].Controls.OfType<MTextBox>().First().FileName, Encoding.GetEncoding(12001));
                        mtb.Text = fileReader.ReadToEnd();
                        fileReader.Close();
                        break;
                }

                // Обновление статуса файла
                mtb.IsFileChanged = false;

                // Задаём заголовок вкладки с названием файла
                Program.mainUI.cTabControl.SelectedTab.Text = Path.GetFileName(Program.mainUI.cTabControl.TabPages[Program.mainUI.cTabControl.SelectedIndex].Controls.OfType<MTextBox>().First().FileName);

                LS.Info("Opening file: " + Program.mainUI.cTabControl.TabPages[Program.mainUI.cTabControl.SelectedIndex].Controls.OfType<MTextBox>().First().FileName);
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
            if (Program.mainUI.startScriptCombobox.SelectedIndex == 0)
            {
                FormPythonInterpreterUI pythonInterpreterUI = new FormPythonInterpreterUI();
                pythonInterpreterUI.ShowDialog(Program.mainUI);
            }
            else if (Program.mainUI.startScriptCombobox.SelectedIndex == 1)
            {
                BatRun();
            }
            else if (Program.mainUI.startScriptCombobox.SelectedIndex == 2)
            {
                VbsRun();
            }
        }

        public void PythonRun()
        {
            LS.Info("Running a Python file");
            mtb = Program.mainUI.cTabControl.TabPages[Program.mainUI.cTabControl.SelectedIndex].Controls.OfType<MTextBox>().First();

            try
            {
                if (mtb.TextLength == 0)
                {
                    MessageBox.Show(Resources.Localization.MSGErrorWhenRunningEmptyScript, "TextPad+", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                SaveCurrentFile();

                Process.Start(@"C:\Windows\py.exe", Program.mainUI.cTabControl.TabPages[Program.mainUI.cTabControl.SelectedIndex].Controls.OfType<MTextBox>().First().FileName);
            }
            catch (Exception ex)
            {
                LS.Error($"{ex} Error while running python file: {Program.mainUI.cTabControl.TabPages[Program.mainUI.cTabControl.SelectedIndex].Controls.OfType<MTextBox>().First().FileName}");
                MessageBox.Show(Resources.Localization.MSGErrorRunPythonFile, "TextPad+", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void PythonRun(string path)
        {
            LS.Info("Running a Python file");
            mtb = Program.mainUI.cTabControl.TabPages[Program.mainUI.cTabControl.SelectedIndex].Controls.OfType<MTextBox>().First();

            try
            {
                if (mtb.TextLength == 0)
                {
                    MessageBox.Show(Resources.Localization.MSGErrorWhenRunningEmptyScript, "TextPad+", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                SaveCurrentFile();

                Process.Start(path, Program.mainUI.cTabControl.TabPages[Program.mainUI.cTabControl.SelectedIndex].Controls.OfType<MTextBox>().First().FileName);
            }
            catch (Exception ex)
            {
                LS.Error($"{ex} Error while running python file: {Program.mainUI.cTabControl.TabPages[Program.mainUI.cTabControl.SelectedIndex].Controls.OfType<MTextBox>().First().FileName}");
                MessageBox.Show(Resources.Localization.MSGErrorRunPythonFile, "TextPad+", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void BatRun()
        {
            LS.Info("Running a bat file");

            mtb = Program.mainUI.cTabControl.TabPages[Program.mainUI.cTabControl.SelectedIndex].Controls.OfType<MTextBox>().First();

            try
            {
                if (mtb.TextLength == 0)
                {
                    MessageBox.Show(Resources.Localization.MSGErrorWhenRunningEmptyScript, "TextPad+", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                SaveCurrentFile();

                Process.Start(Program.mainUI.cTabControl.TabPages[Program.mainUI.cTabControl.SelectedIndex].Controls.OfType<MTextBox>().First().FileName);
            }
            catch (Exception ex)
            {
                LS.Error($"{ex} Error while running Windows Script: {Program.mainUI.cTabControl.TabPages[Program.mainUI.cTabControl.SelectedIndex].Controls.OfType<MTextBox>().First().FileName}");
                MessageBox.Show(Resources.Localization.MSGErrorRunBatFile, "TextPad+", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void VbsRun()
        {
            LS.Info("Running a vbs file");

            mtb = Program.mainUI.cTabControl.TabPages[Program.mainUI.cTabControl.SelectedIndex].Controls.OfType<MTextBox>().First();

            try
            {
                if (mtb.TextLength == 0)
                {
                    MessageBox.Show(Resources.Localization.MSGErrorWhenRunningEmptyScript, "TextPad+", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                SaveCurrentFile();

                Process.Start("wscript.exe", Program.mainUI.cTabControl.TabPages[Program.mainUI.cTabControl.SelectedIndex].Controls.OfType<MTextBox>().First().FileName);
            }
            catch (Exception ex)
            {
                LS.Error($"{ex} Error while running VBScript file: {Program.mainUI.cTabControl.TabPages[Program.mainUI.cTabControl.SelectedIndex].Controls.OfType<MTextBox>().First().FileName}");
                MessageBox.Show(Resources.Localization.MSGErrorRunVBSFile, "TextPad+", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
