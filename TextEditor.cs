
namespace TextPad_
{
    /// <summary>
    /// Класс, созданный для того, что бы обрабатывать всю логику работы текстового редактора. Пока что работает только исключительно с RichTextBox.
    /// Его методы вызываются из класса главного окна программы (MainForm).
    /// </summary>
    internal class TextEditor : IFileRunner
    {
        private static readonly ILogger Ls = new LogSystem($"{Application.StartupPath}\\logs");
        private static RichTextBox? rtb;

        // Метод сохранения файла "сохранить как..."
        public static void SaveAsFile()
        {
            try
            {
                rtb = Program.mainUI.cTabControl.TabPages[Program.mainUI.cTabControl.SelectedIndex].Controls.OfType<RichTextBox>().First();
                Program.mainUI.saveFileDialog.Filter = Program.mainUI.saveFileDialog.Filter = Resources.Localization.saveFileDialogFilter;

                if (rtb.TextLength == 0)
                    return;
                if (Program.mainUI.saveFileDialog.ShowDialog() == DialogResult.Cancel)
                    return;

                try
                {
                    /* 
                     * в список openedFiles добавляется путь до файла, выбранного пользователем при сохранеии.
                     * А индекс элемента равен вкладке, из которой взят текст для файла.
                    */
                    Program.mainUI.OpenedFiles.Insert(Program.mainUI.cTabControl.SelectedIndex, Program.mainUI.saveFileDialog.FileName);

                    // в файл сохраняется текст текущей вкладки по пути, указанному пользователем и ранее добавленного в список.
                    File.WriteAllText(Program.mainUI.OpenedFiles.ElementAt(Program.mainUI.cTabControl.SelectedIndex), rtb.Text);

                    // заголовком вкладки становится название файла, путь до которого взят из списка openedFiles с индексом этой вкладки
                    Program.mainUI.cTabControl.SelectedTab.Text = Path.GetFileName(Program.mainUI.OpenedFiles.ElementAt(Program.mainUI.cTabControl.SelectedIndex));

                    if (Program.mainUI.recentFilesMenuItem.DropDownItems.Count == 10)
                        Program.mainUI.recentFilesMenuItem.DropDownItems.RemoveAt(0);
                    ToolStripMenuItem tsmi = new ToolStripMenuItem();
                    tsmi.Text = Program.mainUI.OpenedFiles.ElementAt(Program.mainUI.cTabControl.SelectedIndex);
                    tsmi.Click += (sender, e) => OpenFile(tsmi.Text);
                    Program.mainUI.recentFilesMenuItem.DropDownItems.Add(tsmi);

                    Ls.Debug("Saving file: " + Program.mainUI.OpenedFiles.ElementAt(Program.mainUI.cTabControl.SelectedIndex));
                }
                catch
                {
                    MessageBox.Show(Resources.Localization.MSGErrorWhenSaveFile, "TextPad+", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Ls.Error("Handling an error when saving a file: " + Program.mainUI.OpenedFiles.ElementAt(Program.mainUI.cTabControl.SelectedIndex));
                }

            }
            catch (Exception ex)
            {
                Ls.Error($"{ex} by saving file.");
                MessageBox.Show(Resources.Localization.MSGErrorWhenSaveFile, "TextPad+", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Метод сохранения файла, открытого на выбранной вкладке
        public static void SaveCurrentFile()
        {
            try
            {
                rtb = Program.mainUI.cTabControl.TabPages[Program.mainUI.cTabControl.SelectedIndex].Controls.OfType<RichTextBox>().First();
                if (rtb.TextLength == 0)
                    return;

                // если путь до файла с индексом открытой вкладки отсутствует (Missing)
                if (Program.mainUI.OpenedFiles.ElementAt(Program.mainUI.cTabControl.SelectedIndex) == "Missing")
                {
                    // то вызываем обычный saveAsFile метод, где используется saveFileDialog и добавляется в список под индексом вкладки выранный путь.
                    SaveAsFile();
                }
                else
                {
                    // иначе путь до файла в списке найден, а значит сохраняем через этот самый путь как обычно.
                    try
                    {
                        File.WriteAllText(Program.mainUI.OpenedFiles.ElementAt(Program.mainUI.cTabControl.SelectedIndex), rtb.Text);
                    }
                    catch
                    {
                        MessageBox.Show(Resources.Localization.MSGErrorWhenSaveFile, "TextPad+", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        Ls.Error("Error when saving an open file: " + Program.mainUI.OpenedFiles.ElementAt(Program.mainUI.cTabControl.SelectedIndex));
                    }
                }

                Ls.Debug("Saving file: " + Program.mainUI.OpenedFiles.ElementAt(Program.mainUI.cTabControl.SelectedIndex));
            }
            catch (Exception ex)
            {
                Ls.Error($"{ex} by saving current file.");
                MessageBox.Show(Resources.Localization.MSGErrorWhenSaveFile, "TextPad+", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Открытие файла
        public static void OpenFile(OpenFileDialog openFileDialog)
        {
            openFileDialog.Filter = Resources.Localization.openFileDialogFilter;
            if (openFileDialog.ShowDialog() == DialogResult.Cancel)
                return;

            try
            {
                rtb = Program.mainUI.cTabControl.TabPages[Program.mainUI.cTabControl.SelectedIndex].Controls.OfType<RichTextBox>().First();
                // Вызываем openFileDialog и полученный путь до файла (fileName) записываем в список openedFiles в индекс текущей вкладки
                Program.mainUI.OpenedFiles.Insert(Program.mainUI.cTabControl.SelectedIndex, openFileDialog.FileName);
                // Считываем текст этого файла, путь берём из того же списка
                rtb.Text = File.ReadAllText(Program.mainUI.OpenedFiles.ElementAt(Program.mainUI.cTabControl.SelectedIndex));
                // Задаём заголовок вкладки с названием файла
                Program.mainUI.cTabControl.SelectedTab.Text = Path.GetFileName(Program.mainUI.OpenedFiles.ElementAt(Program.mainUI.cTabControl.SelectedIndex));

                if (Program.mainUI.recentFilesMenuItem.DropDownItems.Count == 10)
                    Program.mainUI.recentFilesMenuItem.DropDownItems.RemoveAt(0);
                ToolStripMenuItem tsmi = new ToolStripMenuItem();
                tsmi.Text = Program.mainUI.OpenedFiles.ElementAt(Program.mainUI.cTabControl.SelectedIndex);
                tsmi.Click += (sender, e) => OpenFile(tsmi.Text);
                Program.mainUI.recentFilesMenuItem.DropDownItems.Add(tsmi);

                Ls.Debug("Opening file: " + Program.mainUI.OpenedFiles.ElementAt(Program.mainUI.cTabControl.SelectedIndex));
            }
            catch (Exception ex)
            {
                Ls.Error($"{ex} when opening an open file.");
                MessageBox.Show(Resources.Localization.MSGErrorWhenOpenFIle, "TextPad+", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public static void OpenFile(string fileName)
        {
            try
            {
                Program.mainUI.CreateTab();
                Program.mainUI.cTabControl.SelectTab(Program.mainUI.cTabControl.TabPages[Program.mainUI.cTabControl.TabPages.Count - 1]);

                rtb = Program.mainUI.cTabControl.TabPages[Program.mainUI.cTabControl.SelectedIndex].Controls.OfType<RichTextBox>().First();
                // Вызываем openFileDialog и полученный путь до файла (fileName) записываем в список openedFiles в индекс текущей вкладки
                Program.mainUI.OpenedFiles.Insert(Program.mainUI.cTabControl.SelectedIndex, fileName);
                // Считываем текст этого файла, путь берём из того же списка
                rtb.Text = File.ReadAllText(Program.mainUI.OpenedFiles.ElementAt(Program.mainUI.cTabControl.SelectedIndex));
                // Задаём заголовок вкладки с названием файла
                Program.mainUI.cTabControl.SelectedTab.Text = Path.GetFileName(Program.mainUI.OpenedFiles.ElementAt(Program.mainUI.cTabControl.SelectedIndex));

                Ls.Debug("Opening file: " + Program.mainUI.OpenedFiles.ElementAt(Program.mainUI.cTabControl.SelectedIndex));
            }
            catch (Exception ex)
            {
                Ls.Error($"{ex} when opening an open file.");
                MessageBox.Show(Resources.Localization.MSGErrorWhenOpenFIle, "TextPad+", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #region Стандартные функции для правки текста, по названиям понятно, чё он делают.
        public static void copyTextFromTB(CTabControl cTabControl)
        {
            rtb = cTabControl.TabPages[cTabControl.SelectedIndex].Controls.OfType<RichTextBox>().First();
            if (rtb.TextLength > 0)
            {
                rtb.Copy();
            }
        }

        public static void cutTextFromTB(CTabControl cTabControl)
        {
            rtb = cTabControl.TabPages[cTabControl.SelectedIndex].Controls.OfType<RichTextBox>().First();
            if (rtb.TextLength > 0)
            {
                rtb.Cut();
            }
        }

        public static void pasteTextFromTB(CTabControl cTabControl)
        {
            rtb = cTabControl.TabPages[cTabControl.SelectedIndex].Controls.OfType<RichTextBox>().First();
            rtb.Paste();
            string newText = rtb.Text;
            rtb.Text = newText.ToString();
            rtb.Font = Properties.Settings.Default.EditorFont;
        }

        public static void fontTextFromTB(CTabControl cTabControl, FontDialog fontDialog)
        {
            rtb = cTabControl.TabPages[cTabControl.SelectedIndex].Controls.OfType<RichTextBox>().First();
            fontDialog.ShowDialog();
            rtb.Font = fontDialog.Font;
            Properties.Settings.Default.EditorFont = fontDialog.Font;
        }

        public static void selectAllTextFromTB(CTabControl cTabControl)
        {
            rtb = cTabControl.TabPages[cTabControl.SelectedIndex].Controls.OfType<RichTextBox>().First();
            if (rtb.TextLength > 0)
            {
                rtb.SelectAll();
            }
        }

        public static void redoTextFromTB(CTabControl cTabControl)
        {
            rtb = cTabControl.TabPages[cTabControl.SelectedIndex].Controls.OfType<RichTextBox>().First();
            rtb.Redo();
        }

        public static void undoTextFromTB(CTabControl cTabControl)
        {
            rtb = cTabControl.TabPages[cTabControl.SelectedIndex].Controls.OfType<RichTextBox>().First();
            rtb.Undo();
        }

        public static void deleteTextFromTB(CTabControl cTabControl)
        {
            rtb = cTabControl.TabPages[cTabControl.SelectedIndex].Controls.OfType<RichTextBox>().First();
            if (rtb.TextLength > 0)
            {
                rtb.SelectedText = string.Empty;
            }
        }

        public static void dateTime(CTabControl cTabControl)
        {
            rtb = cTabControl.TabPages[cTabControl.SelectedIndex].Controls.OfType<RichTextBox>().First();
            rtb.AppendText(Convert.ToString(DateTime.Now));
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
            Ls.Debug("Running a Python file");
            rtb = Program.mainUI.cTabControl.TabPages[Program.mainUI.cTabControl.SelectedIndex].Controls.OfType<RichTextBox>().First();

            try
            {
                if (rtb.TextLength == 0)
                {
                    MessageBox.Show(Resources.Localization.MSGErrorWhenRunningEmptyScript, "TextPad+", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                SaveCurrentFile();

                Process.Start(@"C:\Windows\py.exe", Program.mainUI.OpenedFiles.ElementAt(Program.mainUI.cTabControl.SelectedIndex));
            }
            catch (Exception ex)
            {
                Ls.Error($"{ex} Error while running python file: {Program.mainUI.OpenedFiles.ElementAt(Program.mainUI.cTabControl.SelectedIndex)}");
                MessageBox.Show(Resources.Localization.MSGErrorRunPythonFile, "TextPad+", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            Ls.Debug($"Memory Consumed: {Process.GetProcessesByName("TextPad+")[0].WorkingSet64} Bytes");
        }

        public void PythonRun(string path)
        {
            Ls.Debug("Running a Python file");
            rtb = Program.mainUI.cTabControl.TabPages[Program.mainUI.cTabControl.SelectedIndex].Controls.OfType<RichTextBox>().First();

            try
            {
                if (rtb.TextLength == 0)
                {
                    MessageBox.Show(Resources.Localization.MSGErrorWhenRunningEmptyScript, "TextPad+", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                SaveCurrentFile();

                Process.Start(path, Program.mainUI.OpenedFiles.ElementAt(Program.mainUI.cTabControl.SelectedIndex));
            }
            catch (Exception ex)
            {
                Ls.Error($"{ex} Error while running python file: {Program.mainUI.OpenedFiles.ElementAt(Program.mainUI.cTabControl.SelectedIndex)}");
                MessageBox.Show(Resources.Localization.MSGErrorRunPythonFile, "TextPad+", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            Ls.Debug($"Memory Consumed: {Process.GetProcessesByName("TextPad+")[0].WorkingSet64} Bytes");
        }

        public void BatRun()
        {
            Ls.Debug("Running a bat file");

            rtb = Program.mainUI.cTabControl.TabPages[Program.mainUI.cTabControl.SelectedIndex].Controls.OfType<RichTextBox>().First();

            try
            {
                if (rtb.TextLength == 0)
                {
                    MessageBox.Show(Resources.Localization.MSGErrorWhenRunningEmptyScript, "TextPad+", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                SaveCurrentFile();

                Process.Start(Program.mainUI.OpenedFiles.ElementAt(Program.mainUI.cTabControl.SelectedIndex));
            }
            catch (Exception ex)
            {
                Ls.Error($"{ex} Error while running Windows Script: {Program.mainUI.OpenedFiles.ElementAt(Program.mainUI.cTabControl.SelectedIndex)}");
                MessageBox.Show(Resources.Localization.MSGErrorRunBatFile, "TextPad+", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            Ls.Debug($"Memory Consumed: {Process.GetProcessesByName("TextPad+")[0].WorkingSet64} Bytes");
        }

        public void VbsRun()
        {
            Ls.Debug("Running a vbs file");

            rtb = Program.mainUI.cTabControl.TabPages[Program.mainUI.cTabControl.SelectedIndex].Controls.OfType<RichTextBox>().First();

            try
            {
                if (rtb.TextLength == 0)
                {
                    MessageBox.Show(Resources.Localization.MSGErrorWhenRunningEmptyScript, "TextPad+", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                TextEditor.SaveCurrentFile();

                Process.Start("wscript.exe", Program.mainUI.OpenedFiles.ElementAt(Program.mainUI.cTabControl.SelectedIndex));
            }
            catch (Exception ex)
            {
                Ls.Error($"{ex} Error while running VBScript file: {Program.mainUI.OpenedFiles.ElementAt(Program.mainUI.cTabControl.SelectedIndex)}");
                MessageBox.Show(Resources.Localization.MSGErrorRunVBSFile, "TextPad+", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            Ls.Debug($"Memory Consumed: {Process.GetProcessesByName("TextPad+")[0].WorkingSet64} Bytes");
        }
    }
}
