
using System.Text;

namespace TextPad_
{
    /// <summary>
    /// Класс, созданный для того, что бы обрабатывать всю логику работы текстового редактора. Пока что работает только исключительно с MTextBox.
    /// Его методы вызываются из класса главного окна программы (MainForm).
    /// </summary>
    internal static class TextEditor
    {
        private static readonly LogSystem Logger = new($"{Application.StartupPath}\\logs");
        private static MTextBox? mtb;

        // Метод сохранения файла "Сохранить как..."
        public static void SaveAsFile()
        {
            try
            {
                // ModifiedTextBox на выбранной вкладке и фльтра для диалога
                mtb = Program.MainUI.cTabControl.TabPages[Program.MainUI.cTabControl.SelectedIndex].Controls.OfType<MTextBox>().First();
                Program.MainUI.SaveFileDialog_.Filter = Program.MainUI.SaveFileDialog_.Filter = Resources.Localization.saveFileDialogFilter;
                if (Program.MainUI.SaveFileDialog_.ShowDialog() == DialogResult.Cancel)
                    return;
                mtb.FileName = Program.MainUI.SaveFileDialog_.FileName;

                // В файл сохраняется текст mtb по пути, указанному в FileName, учитывая значение кодировки из mtb.
                Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
                StreamWriter fileWriter;
                switch (mtb.Encoding)
                {
                    case "CP866":
                        fileWriter = new StreamWriter(mtb.FileName, false, Encoding.GetEncoding(866));
                        fileWriter.Write(mtb.Text);
                        fileWriter.Close();
                        break;
                    case "KOI8-U":
                        fileWriter = new StreamWriter(mtb.FileName, false, Encoding.GetEncoding(21866));
                        fileWriter.Write(mtb.Text);
                        fileWriter.Close();
                        break;
                    case "KOI8-R":
                        fileWriter = new StreamWriter(mtb.FileName, false, Encoding.GetEncoding(20866));
                        fileWriter.Write(mtb.Text);
                        fileWriter.Close();
                        break;
                    case "ASCII":
                        fileWriter = new StreamWriter(mtb.FileName, false, Encoding.GetEncoding(20127));
                        fileWriter.Write(mtb.Text);
                        fileWriter.Close();
                        break;
                    case "Windows-1251":
                        fileWriter = new StreamWriter(mtb.FileName, false, Encoding.GetEncoding(1251));
                        fileWriter.Write(mtb.Text);
                        fileWriter.Close();
                        break;
                    case "UTF-8":
                        fileWriter = new StreamWriter(mtb.FileName, false);
                        fileWriter.Write(mtb.Text);
                        fileWriter.Close();
                        break;
                    case "UTF-8 BOM":
                        fileWriter = new StreamWriter(mtb.FileName, false, Encoding.GetEncoding(65001));
                        fileWriter.Write(mtb.Text);
                        fileWriter.Close();
                        break;
                    case "UTF-16":
                        fileWriter = new StreamWriter(mtb.FileName, false, Encoding.GetEncoding(1200));
                        fileWriter.Write(mtb.Text);
                        fileWriter.Close();
                        break;
                    case "UTF-16 BE":
                        fileWriter = new StreamWriter(mtb.FileName, false, Encoding.GetEncoding(1201));
                        fileWriter.Write(mtb.Text);
                        fileWriter.Close();
                        break;
                    case "UTF-32":
                        fileWriter = new StreamWriter(mtb.FileName, false, Encoding.GetEncoding(12000));
                        fileWriter.Write(mtb.Text);
                        fileWriter.Close();
                        break;
                    case "UTF-32 BE":
                        fileWriter = new StreamWriter(mtb.FileName, false, Encoding.GetEncoding(12001));
                        fileWriter.Write(mtb.Text);
                        fileWriter.Close();
                        break;
                }

                // Изменение заголовка вкладки и обновление статуса файла
                mtb.IsFileChanged = false;
                Program.MainUI.cTabControl.SelectedTab!.Text = Program.MainUI.cTabControl.SelectedTab!.Text.TrimEnd('*');
                Program.MainUI.CheckFile();

                // Лог
                Logger.Info("Saving file: " + mtb.FileName);

                // Внос файла в список недавних файлов
                foreach (ToolStripMenuItem rfmdi in Program.MainUI.RecentFilesListFileItem.DropDownItems)
                {
                    if (rfmdi.Text == mtb.FileName)
                        return;
                }
                if (Program.MainUI.RecentFilesListFileItem.DropDownItems.Count == 10)
                    Program.MainUI.RecentFilesListFileItem.DropDownItems.RemoveAt(0);
                ToolStripMenuItem tsmi = new();
                tsmi.Text = mtb.FileName;
                tsmi.Click += (sender, e) => OpenFile(tsmi.Text);
                Program.MainUI.RecentFilesListFileItem.DropDownItems.Add(tsmi);
            }
            catch (Exception ex)
            {
                Logger.Error($"Error when saving a file through the dialog box. \n{ex}");
                MessageBox.Show(Resources.Localization.MSGErrorWhenSaveFile, "TextPad+", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Метод сохранения файла, открытого на выбранной вкладке
        public static void SaveCurrentFile()
        {
            try
            {
                // ModifiedTextBox на выбранной вкладке
                mtb = Program.MainUI.cTabControl.TabPages[Program.MainUI.cTabControl.SelectedIndex].Controls.OfType<MTextBox>().First();

                // Если путь до файла отсутствует (Missing)
                if (mtb.FileName == "Missing")
                {
                    // Вызываем обычный SaveAsFile метод, с saveFileDialog и завершаем работу
                    SaveAsFile();
                    return;
                }

                // Если нет, то значит путь до файла файл есть, поэтому сохраняем по пути как обычно
                // В файл сохраняется текст текущей вкладки по пути, указанному в FileName, учитывая значение кодировки из mtb.
                StreamWriter fileWriter;
                switch (mtb.Encoding)
                {
                    case "CP866":
                        fileWriter = new StreamWriter(mtb.FileName, false, Encoding.GetEncoding(866));
                        fileWriter.Write(mtb.Text);
                        fileWriter.Close();
                        break;
                    case "KOI8-U":
                        fileWriter = new StreamWriter(mtb.FileName, false, Encoding.GetEncoding(21866));
                        fileWriter.Write(mtb.Text);
                        fileWriter.Close();
                        break;
                    case "KOI8-R":
                        fileWriter = new StreamWriter(mtb.FileName, false, Encoding.GetEncoding(20866));
                        fileWriter.Write(mtb.Text);
                        fileWriter.Close();
                        break;
                    case "ASCII":
                        fileWriter = new StreamWriter(mtb.FileName, false, Encoding.GetEncoding(20127));
                        fileWriter.Write(mtb.Text);
                        fileWriter.Close();
                        break;
                    case "Windows-1251":
                        fileWriter = new StreamWriter(mtb.FileName, false, Encoding.GetEncoding(1251));
                        fileWriter.Write(mtb.Text);
                        fileWriter.Close();
                        break;
                    case "UTF-8":
                        fileWriter = new StreamWriter(mtb.FileName, false);
                        fileWriter.Write(mtb.Text);
                        fileWriter.Close();
                        break;
                    case "UTF-8 BOM":
                        fileWriter = new StreamWriter(mtb.FileName, false, Encoding.GetEncoding(65001));
                        fileWriter.Write(mtb.Text);
                        fileWriter.Close();
                        break;
                    case "UTF-16":
                        fileWriter = new StreamWriter(mtb.FileName, false, Encoding.GetEncoding(1200));
                        fileWriter.Write(mtb.Text);
                        fileWriter.Close();
                        break;
                    case "UTF-16 BE":
                        fileWriter = new StreamWriter(mtb.FileName, false, Encoding.GetEncoding(1201));
                        fileWriter.Write(mtb.Text);
                        fileWriter.Close();
                        break;
                    case "UTF-32":
                        fileWriter = new StreamWriter(mtb.FileName, false, Encoding.GetEncoding(12000));
                        fileWriter.Write(mtb.Text);
                        fileWriter.Close();
                        break;
                    case "UTF-32 BE":
                        fileWriter = new StreamWriter(mtb.FileName, false, Encoding.GetEncoding(12001));
                        fileWriter.Write(mtb.Text);
                        fileWriter.Close();
                        break;
                }

                // Обновление статуса файла
                mtb.IsFileChanged = false;
                Program.MainUI.cTabControl.SelectedTab!.Text = Program.MainUI.cTabControl.SelectedTab!.Text.TrimEnd('*');
                Program.MainUI.CheckFile();

                Logger.Info("Saving file: " + mtb.FileName);
            }
            catch (Exception ex)
            {
                Logger.Error($"An error occurred while saving an existing file. \n{ex}");
                MessageBox.Show(Resources.Localization.MSGErrorWhenSaveFile, "TextPad+", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Открытие файла
        public static void OpenFile()
        {
            try
            {
                // Вызов openFileDialog
                Program.MainUI.OpenFileDialog_.Filter = Resources.Localization.openFileDialogFilter;
                if (Program.MainUI.OpenFileDialog_.ShowDialog() == DialogResult.Cancel)
                    return;

                // ModifiedTextBox на выбранной вкладке и фльтра для диалога
                mtb = Program.MainUI.cTabControl.TabPages[Program.MainUI.cTabControl.SelectedIndex].Controls.OfType<MTextBox>().First();
                mtb.FileName = Program.MainUI.OpenFileDialog_.FileName;

                // Считываем текст этого файла, путь берём из того же свойства, проверяем кодировку
                StreamReader fileReader;
                switch (mtb.Encoding)
                {
                    case "CP866":
                        fileReader = new StreamReader(mtb.FileName, Encoding.GetEncoding(866));
                        mtb.Text = fileReader.ReadToEnd();
                        fileReader.Close();
                        break;
                    case "KOI8-U":
                        fileReader = new StreamReader(mtb.FileName, Encoding.GetEncoding(21866));
                        mtb.Text = fileReader.ReadToEnd();
                        fileReader.Close();
                        break;
                    case "KOI8-R":
                        fileReader = new StreamReader(mtb.FileName, Encoding.GetEncoding(20866));
                        mtb.Text = fileReader.ReadToEnd();
                        fileReader.Close();
                        break;
                    case "ASCII":
                        fileReader = new StreamReader(mtb.FileName, Encoding.GetEncoding(20127));
                        mtb.Text = fileReader.ReadToEnd();
                        fileReader.Close();
                        break;
                    case "Windows-1251":
                        fileReader = new StreamReader(mtb.FileName, Encoding.GetEncoding(1251));
                        mtb.Text = fileReader.ReadToEnd();
                        fileReader.Close();
                        break;
                    case "UTF-8":
                        fileReader = new StreamReader(mtb.FileName);
                        mtb.Text = fileReader.ReadToEnd();
                        fileReader.Close();
                        break;
                    case "UTF-8 BOM":
                        fileReader = new StreamReader(mtb.FileName, Encoding.GetEncoding(65001));
                        mtb.Text = fileReader.ReadToEnd();
                        fileReader.Close();
                        break;
                    case "UTF-16":
                        fileReader = new StreamReader(mtb.FileName, Encoding.GetEncoding(1200));
                        mtb.Text = fileReader.ReadToEnd();
                        fileReader.Close();
                        break;
                    case "UTF-16 BE":
                        fileReader = new StreamReader(mtb.FileName, Encoding.GetEncoding(1201));
                        mtb.Text = fileReader.ReadToEnd();
                        fileReader.Close();
                        break;
                    case "UTF-32":
                        fileReader = new StreamReader(mtb.FileName, Encoding.GetEncoding(12000));
                        mtb.Text = fileReader.ReadToEnd();
                        fileReader.Close();
                        break;
                    case "UTF-32 BE":
                        fileReader = new StreamReader(mtb.FileName, Encoding.GetEncoding(12001));
                        mtb.Text = fileReader.ReadToEnd();
                        fileReader.Close();
                        break;
                }

                // Изменение заголовка вкладки и обновление статуса файла
                Program.MainUI.cTabControl.SelectedTab!.Text = Path.GetFileName(mtb.FileName);
                mtb.IsFileChanged = false;
                Program.MainUI.CheckFile();

                // Лог
                Logger.Info("Opening file: " + mtb.FileName);

                // Внос файла в список недавних файлов
                foreach (ToolStripMenuItem rftsmi in Program.MainUI.RecentFilesListFileItem.DropDownItems)
                {
                    if (rftsmi.Text == mtb.FileName)
                        return;
                }
                if (Program.MainUI.RecentFilesListFileItem.DropDownItems.Count == 10)
                    Program.MainUI.RecentFilesListFileItem.DropDownItems.RemoveAt(0);
                ToolStripMenuItem tsmi = new();
                tsmi.Text = mtb.FileName;
                tsmi.Click += (sender, e) => OpenFile(tsmi.Text);
                Program.MainUI.RecentFilesListFileItem.DropDownItems.Add(tsmi);
            }
            catch (Exception ex)
            {
                Logger.Error($"An error occurred while opening the file. \n{ex}");
                MessageBox.Show(Resources.Localization.MSGErrorWhenOpenFIle, "TextPad+", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Открытие файла по конкретному пути
        public static void OpenFile(string fileName)
        {
            try
            {
                if (File.Exists(fileName) == false)
                {
                    MessageBox.Show(Resources.Localization.MSGErrorFileNotFound, "TextPad+", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    return;
                }

                mtb = Program.MainUI.cTabControl.TabPages[Program.MainUI.cTabControl.SelectedIndex].Controls.OfType<MTextBox>().First();

                // Если текущая вкладка пуста, открываем в ней файл, а если нет, то создаём новую
                if (mtb.FileName != "Missing" || mtb.Text.Length > 0)
                {
                    Program.MainUI.CreateTab();
                    Program.MainUI.cTabControl.SelectTab(Program.MainUI.cTabControl.TabPages[Program.MainUI.cTabControl.TabPages.Count - 1]);
                    mtb = Program.MainUI.cTabControl.TabPages[Program.MainUI.cTabControl.SelectedIndex].Controls.OfType<MTextBox>().First();
                }

                // В свойство FileName записываем путь до файла, который передаётся методу
                mtb.FileName = fileName;

                // Считываем текст этого файла, проверяем кодировку
                Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
                StreamReader fileReader;
                switch (mtb.Encoding)
                {
                    case "CP866":
                        fileReader = new StreamReader(mtb.FileName, Encoding.GetEncoding(866));
                        mtb.Text = fileReader.ReadToEnd();
                        fileReader.Close();
                        break;
                    case "KOI8-U":
                        fileReader = new StreamReader(fileName, Encoding.GetEncoding(21866));
                        mtb.Text = fileReader.ReadToEnd();
                        fileReader.Close();
                        break;
                    case "KOI8-R":
                        fileReader = new StreamReader(fileName, Encoding.GetEncoding(20866));
                        mtb.Text = fileReader.ReadToEnd();
                        fileReader.Close();
                        break;
                    case "ASCII":
                        fileReader = new StreamReader(fileName, Encoding.GetEncoding(20127));
                        mtb.Text = fileReader.ReadToEnd();
                        fileReader.Close();
                        break;
                    case "Windows-1251":
                        fileReader = new StreamReader(fileName, Encoding.GetEncoding(1251));
                        mtb.Text = fileReader.ReadToEnd();
                        fileReader.Close();
                        break;
                    case "UTF-8":
                        fileReader = new StreamReader(fileName);
                        mtb.Text = fileReader.ReadToEnd();
                        fileReader.Close();
                        break;
                    case "UTF-8 BOM":
                        fileReader = new StreamReader(fileName, Encoding.GetEncoding(65001));
                        mtb.Text = fileReader.ReadToEnd();
                        fileReader.Close();
                        break;
                    case "UTF-16":
                        fileReader = new StreamReader(fileName, Encoding.GetEncoding(1200));
                        mtb.Text = fileReader.ReadToEnd();
                        fileReader.Close();
                        break;
                    case "UTF-16 BE":
                        fileReader = new StreamReader(fileName, Encoding.GetEncoding(1201));
                        mtb.Text = fileReader.ReadToEnd();
                        fileReader.Close();
                        break;
                    case "UTF-32":
                        fileReader = new StreamReader(fileName, Encoding.GetEncoding(12000));
                        mtb.Text = fileReader.ReadToEnd();
                        fileReader.Close();
                        break;
                    case "UTF-32 BE":
                        fileReader = new StreamReader(fileName, Encoding.GetEncoding(12001));
                        mtb.Text = fileReader.ReadToEnd();
                        fileReader.Close();
                        break;
                }

                // Изменение заголовка вкладки и обновление статуса файла
                Program.MainUI.cTabControl.SelectedTab!.Text = Path.GetFileName(mtb.FileName);
                mtb.IsFileChanged = false;
                Program.MainUI.CheckFile();

                // Лог
                Logger.Info("Opening file: " + mtb.FileName);

                // Внос файла в список недавних файлов
                foreach (ToolStripMenuItem rfmdi in Program.MainUI.RecentFilesListFileItem.DropDownItems)
                {
                    if (rfmdi.Text == mtb.FileName)
                        return;
                }
                if (Program.MainUI.RecentFilesListFileItem.DropDownItems.Count == 10)
                    Program.MainUI.RecentFilesListFileItem.DropDownItems.RemoveAt(0);
                ToolStripMenuItem tsmi = new();
                tsmi.Text = mtb.FileName;
                tsmi.Click += (sender, e) => OpenFile(tsmi.Text);
                Program.MainUI.RecentFilesListFileItem.DropDownItems.Add(tsmi);
            }
            catch (Exception ex)
            {
                Logger.Error($"An error occurred while opening a file at the specified path. \n{ex}");
                MessageBox.Show(Resources.Localization.MSGErrorWhenOpenFIle, "TextPad+", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Повторное открытие файла
        public static void ReopenFile()
        {
            try
            {
                mtb = Program.MainUI.cTabControl.TabPages[Program.MainUI.cTabControl.SelectedIndex].Controls.OfType<MTextBox>().First();

                if (File.Exists(mtb.FileName) == false)
                {
                    MessageBox.Show(Resources.Localization.MSGErrorFileNotFound, "TextPad+", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    return;
                }

                // Считываем текст этого файла, путь берём из того же свойства. Проверяем кодировку
                Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
                StreamReader fileReader;
                switch (mtb.Encoding)
                {
                    case "CP866":
                        fileReader = new StreamReader(mtb.FileName, Encoding.GetEncoding(866));
                        mtb.Text = fileReader.ReadToEnd();
                        fileReader.Close();
                        break;
                    case "KOI8-U":
                        fileReader = new StreamReader(mtb.FileName, Encoding.GetEncoding(21866));
                        mtb.Text = fileReader.ReadToEnd();
                        fileReader.Close();
                        break;
                    case "KOI8-R":
                        fileReader = new StreamReader(mtb.FileName, Encoding.GetEncoding(20866));
                        mtb.Text = fileReader.ReadToEnd();
                        fileReader.Close();
                        break;
                    case "ASCII":
                        fileReader = new StreamReader(mtb.FileName, Encoding.GetEncoding(20127));
                        mtb.Text = fileReader.ReadToEnd();
                        fileReader.Close();
                        break;
                    case "Windows-1251":
                        fileReader = new StreamReader(mtb.FileName, Encoding.GetEncoding(1251));
                        mtb.Text = fileReader.ReadToEnd();
                        fileReader.Close();
                        break;
                    case "UTF-8":
                        fileReader = new StreamReader(mtb.FileName);
                        mtb.Text = fileReader.ReadToEnd();
                        fileReader.Close();
                        break;
                    case "UTF-8 BOM":
                        fileReader = new StreamReader(mtb.FileName, Encoding.GetEncoding(65001));
                        mtb.Text = fileReader.ReadToEnd();
                        fileReader.Close();
                        break;
                    case "UTF-16":
                        fileReader = new StreamReader(mtb.FileName, Encoding.GetEncoding(1200));
                        mtb.Text = fileReader.ReadToEnd();
                        fileReader.Close();
                        break;
                    case "UTF-16 BE":
                        fileReader = new StreamReader(mtb.FileName, Encoding.GetEncoding(1201));
                        mtb.Text = fileReader.ReadToEnd();
                        fileReader.Close();
                        break;
                    case "UTF-32":
                        fileReader = new StreamReader(mtb.FileName, Encoding.GetEncoding(12000));
                        mtb.Text = fileReader.ReadToEnd();
                        fileReader.Close();
                        break;
                    case "UTF-32 BE":
                        fileReader = new StreamReader(mtb.FileName, Encoding.GetEncoding(12001));
                        mtb.Text = fileReader.ReadToEnd();
                        fileReader.Close();
                        break;
                }
                Program.MainUI.CheckFile();

                // Изменение заголовка вкладки и обновление статуса файла
                Program.MainUI.cTabControl.SelectedTab!.Text = Path.GetFileName(mtb.FileName);
                mtb.IsFileChanged = false;
                Program.MainUI.CheckFile();

                Logger.Info("Reopening file: " + Program.MainUI.cTabControl.TabPages[Program.MainUI.cTabControl.SelectedIndex].Controls.OfType<MTextBox>().First().FileName);
            }
            catch (Exception ex)
            {
                Logger.Error($"An error occurred while reopening a file. \n{ex}");
                MessageBox.Show(Resources.Localization.MSGErrorWhenOpenFIle, "TextPad+", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }

    // Структура с методами запуска файлов
    internal struct FileRunner
    {
        private static readonly LogSystem Logger = new($"{Application.StartupPath}\\logs");
        private static MTextBox? mtb;

        // Методы запуска файлов
        public static void RunScript()
        {
            if (Program.MainUI.RunScriptCombobox.SelectedIndex == 0)
            {
                FormPythonInterpreterUI pythonInterpreterUI = new();
                pythonInterpreterUI.ShowDialog(Program.MainUI);
            }
            else if (Program.MainUI.RunScriptCombobox.SelectedIndex == 1)
            {
                RunWindowsScript();
            }
            else if (Program.MainUI.RunScriptCombobox.SelectedIndex == 2)
            {
                RunVbsJsScript();
            }
            else if (Program.MainUI.RunScriptCombobox.SelectedIndex == 3)
            {
                RunHTMLPage();
            }
        }

        public static void RunPythonScript()
        {
            Logger.Info("Running a Python file");
            mtb = Program.MainUI.cTabControl.TabPages[Program.MainUI.cTabControl.SelectedIndex].Controls.OfType<MTextBox>().First();

            try
            {
                if (mtb.TextLength == 0)
                {
                    MessageBox.Show(Resources.Localization.MSGErrorWhenRunningEmptyScript, "TextPad+", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                TextEditor.SaveCurrentFile();

                if (Program.MainUI.cTabControl.TabPages[Program.MainUI.cTabControl.SelectedIndex].Controls.OfType<MTextBox>().First().FileName == "Missing")
                    return;

                Process.Start(@"C:\Windows\py.exe", Program.MainUI.cTabControl.TabPages[Program.MainUI.cTabControl.SelectedIndex].Controls.OfType<MTextBox>().First().FileName);
            }
            catch (Exception ex)
            {
                Logger.Error($"An error occurred while running the Python script: {Program.MainUI.cTabControl.TabPages[Program.MainUI.cTabControl.SelectedIndex].Controls.OfType<MTextBox>().First().FileName} \n{ex}");
                MessageBox.Show(Resources.Localization.MSGErrorRunPythonFile, "TextPad+", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public static void RunPythonScript(string path)
        {
            Logger.Info("Running a Python file");
            mtb = Program.MainUI.cTabControl.TabPages[Program.MainUI.cTabControl.SelectedIndex].Controls.OfType<MTextBox>().First();

            try
            {
                if (mtb.TextLength == 0)
                {
                    MessageBox.Show(Resources.Localization.MSGErrorWhenRunningEmptyScript, "TextPad+", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                TextEditor.SaveCurrentFile();

                if (Program.MainUI.cTabControl.TabPages[Program.MainUI.cTabControl.SelectedIndex].Controls.OfType<MTextBox>().First().FileName == "Missing")
                    return;

                Process.Start(path, Program.MainUI.cTabControl.TabPages[Program.MainUI.cTabControl.SelectedIndex].Controls.OfType<MTextBox>().First().FileName);
            }
            catch (Exception ex)
            {
                Logger.Error($"An error occurred while running the Python script at the specified path: {Program.MainUI.cTabControl.TabPages[Program.MainUI.cTabControl.SelectedIndex].Controls.OfType<MTextBox>().First().FileName} \n{ex}");
                MessageBox.Show(Resources.Localization.MSGErrorRunPythonFile, "TextPad+", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public static void RunWindowsScript()
        {
            Logger.Info("Running a bat/cmd file");

            mtb = Program.MainUI.cTabControl.TabPages[Program.MainUI.cTabControl.SelectedIndex].Controls.OfType<MTextBox>().First();

            try
            {
                if (mtb.TextLength == 0)
                {
                    MessageBox.Show(Resources.Localization.MSGErrorWhenRunningEmptyScript, "TextPad+", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                TextEditor.SaveCurrentFile();

                Process.Start(Program.MainUI.cTabControl.TabPages[Program.MainUI.cTabControl.SelectedIndex].Controls.OfType<MTextBox>().First().FileName);
            }
            catch (Exception ex)
            {
                Logger.Error($"Error while running Windows script: {Program.MainUI.cTabControl.TabPages[Program.MainUI.cTabControl.SelectedIndex].Controls.OfType<MTextBox>().First().FileName} \n{ex}");
                MessageBox.Show(Resources.Localization.MSGErrorRunBatCmdFile, "TextPad+", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public static void RunVbsJsScript()
        {
            Logger.Info("Running a VBS / JS script");

            mtb = Program.MainUI.cTabControl.TabPages[Program.MainUI.cTabControl.SelectedIndex].Controls.OfType<MTextBox>().First();

            try
            {
                if (mtb.TextLength == 0)
                {
                    MessageBox.Show(Resources.Localization.MSGErrorWhenRunningEmptyScript, "TextPad+", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                TextEditor.SaveCurrentFile();

                Process.Start("wscript.exe", mtb.FileName);
            }
            catch (Exception ex)
            {
                Logger.Error($"Error while running VBS / JS script: {Program.MainUI.cTabControl.TabPages[Program.MainUI.cTabControl.SelectedIndex].Controls.OfType<MTextBox>().First().FileName} \n{ex}");
                MessageBox.Show(Resources.Localization.MSGErrorRunVBSJSFile, "TextPad+", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public static void RunHTMLPage()
        {
            mtb = Program.MainUI.cTabControl.TabPages[Program.MainUI.cTabControl.SelectedIndex].Controls.OfType<MTextBox>().First();

            Process websiteProcess = new();
            websiteProcess.StartInfo.UseShellExecute = true;
            websiteProcess.StartInfo.FileName = mtb.FileName;
            websiteProcess.Start();
        }
    }
}
