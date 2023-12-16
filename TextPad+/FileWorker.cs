
using System.Text;

namespace TextPad_
{
    /// <summary>
    /// Класс, созданный для того, что бы обрабатывать всю логику работы текстового редактора. Пока что работает только исключительно с MTextBox.
    /// Его методы вызываются из класса главного окна программы (MainForm).
    /// </summary>
    internal class FileWorker
    {
        private static readonly LogSystem Logger = new() { UserFolderName = $"{Application.StartupPath}\\logs" };
        private static MTextBox? mtb;

        // Методы для сохранения уже существующего файла
        internal static void SaveFile(bool useFileDialog = true)
        {
            try
            {
                // ModifiedTextBox на выбранной вкладке
                mtb = Program.MainForm.cTabControl.TabPages[Program.MainForm.cTabControl.SelectedIndex].Controls.OfType<MTextBox>().First();

                // Если путь до файла отсутствует (Missing)
                if (mtb.FileName == "Missing")
                {
                    if (useFileDialog == true)
                    {
                        // Вызываем обычный SaveAsFile метод, с saveFileDialog и завершаем работу
                        SaveAsFile();
                        return;
                    }
                    else
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
                Program.MainForm.CheckFile();
                Program.MainForm.cTabControl.TabPages[Program.MainForm.cTabControl.SelectedIndex].Text = Path.GetFileName(mtb.FileName);

                GC.Collect();
                Logger.Info("Saving file: " + mtb.FileName);
            }
            catch (Exception ex)
            {
                Logger.Error($"An error occurred while saving an existing file. \n{ex}");
                //MessageBox.Show(Resources.Localization.MSGErrorWhenSaveFile, "TextPad+", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        internal static void SaveFile(int tabIndex, bool useFileDialog = true)
        {
            try
            {
                // ModifiedTextBox на выбранной вкладке
                mtb = Program.MainForm.cTabControl.TabPages[tabIndex].Controls.OfType<MTextBox>().First();

                // Если путь до файла отсутствует (Missing)
                if (mtb.FileName == "Missing")
                {
                    if (useFileDialog == true)
                    {
                        // Вызываем обычный SaveAsFile метод, с SaveFileDialog_ и завершаем работу
                        SaveAsFile();
                        return;
                    }
                    else
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
                Program.MainForm.CheckFile(tabIndex);
                Program.MainForm.cTabControl.TabPages[tabIndex].Text = Path.GetFileName(mtb.FileName);

                GC.Collect();
                Logger.Info("Saving file: " + mtb.FileName);
            }
            catch (Exception ex)
            {
                Logger.Error($"An error occurred while saving an existing file. \n{ex}");
                //MessageBox.Show(Resources.Localization.MSGErrorWhenSaveFile, "TextPad+", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Методы сохранения файла "Сохранить как..."
        internal static void SaveAsFile()
        {
            try
            {
                // ModifiedTextBox на выбранной вкладке и фльтра для диалога
                mtb = Program.MainForm.cTabControl.TabPages[Program.MainForm.cTabControl.SelectedIndex].Controls.OfType<MTextBox>().First();
                Program.MainForm.SaveFileDialog_.Filter = Program.MainForm.SaveFileDialog_.Filter = Resources.Localization.SaveFileDialogFilter;
                if (Program.MainForm.SaveFileDialog_.ShowDialog() == DialogResult.Cancel)
                    return;
                mtb.FileName = Program.MainForm.SaveFileDialog_.FileName;

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
                Program.MainForm.CheckFile();
                mtb.IsFileChanged = false;
                Program.MainForm.cTabControl.TabPages[Program.MainForm.cTabControl.SelectedIndex].Text = Path.GetFileName(mtb.FileName);
                Program.MainForm.SaveFileDialog_.FileName = "";

                // Внос файла в список недавних файлов
                AddFileToRecentList(mtb);

                GC.Collect();
                Logger.Info("Saving file: " + mtb.FileName);
            }
            catch (Exception ex)
            {
                Logger.Error($"Error when saving a file through the dialog box. \n{ex}");
                //MessageBox.Show(Resources.Localization.MSGErrorWhenSaveFile, "TextPad+", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        internal static void SaveAsFile(int tabIndex)
        {
            try
            {
                // ModifiedTextBox на выбранной вкладке и фльтра для диалога
                mtb = Program.MainForm.cTabControl.TabPages[tabIndex].Controls.OfType<MTextBox>().First();
                Program.MainForm.SaveFileDialog_.Filter = Program.MainForm.SaveFileDialog_.Filter = Resources.Localization.SaveFileDialogFilter;
                if (Program.MainForm.SaveFileDialog_.ShowDialog() == DialogResult.Cancel)
                    return;
                mtb.FileName = Program.MainForm.SaveFileDialog_.FileName;

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
                Program.MainForm.CheckFile(tabIndex);
                mtb.IsFileChanged = false;
                Program.MainForm.cTabControl.TabPages[tabIndex].Text = Path.GetFileName(mtb.FileName);
                Program.MainForm.SaveFileDialog_.FileName = "";

                // Внос файла в список недавних файлов
                AddFileToRecentList(mtb);

                GC.Collect();
                Logger.Info("Saving file: " + mtb.FileName);
            }
            catch (Exception ex)
            {
                Logger.Error($"Error when saving a file through the dialog box. \n{ex}");
                //MessageBox.Show(Resources.Localization.MSGErrorWhenSaveFile, "TextPad+", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Метод сохранения всех файлов

        internal static void SaveAllFiles(bool useFileDialog = true)
        {
            for (int i = 0; i < Program.MainForm.cTabControl.TabPages.Count; i++)
                SaveFile(i, useFileDialog);
        }

        // Методы открытия файла
        internal static void OpenFile()
        {
            try
            {
                mtb = Program.MainForm.cTabControl.TabPages[Program.MainForm.cTabControl.SelectedIndex].Controls.OfType<MTextBox>().First();

                // Вызов openFileDialog
                Program.MainForm.OpenFileDialog_.Filter = Resources.Localization.OpenFileDialogFilter;
                if (Program.MainForm.OpenFileDialog_.ShowDialog() == DialogResult.Cancel)
                    return;

                // Если текущая вкладка пуста, открываем в ней файл, а если нет, то создаём новую
                if (mtb.FileName != "Missing" || mtb.Text.Length > 0)
                {
                    Program.MainForm.CreateTab();
                    Program.MainForm.cTabControl.SelectTab(Program.MainForm.cTabControl.TabPages[Program.MainForm.cTabControl.TabPages.Count - 1]);
                    mtb = Program.MainForm.cTabControl.TabPages[Program.MainForm.cTabControl.SelectedIndex].Controls.OfType<MTextBox>().First();
                }

                mtb.FileName = Program.MainForm.OpenFileDialog_.FileName;

                // Считываем текст этого файла, путь берём из того же свойства, проверяем кодировку
                StreamReader fileReader;
                Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
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
                Program.MainForm.CheckFile();
                Program.MainForm.cTabControl.SelectedTab!.Text = Path.GetFileName(mtb.FileName);
                mtb.IsFileChanged = false;

                // Внос файла в список недавних файлов
                AddFileToRecentList(mtb);

                GC.Collect();
                Logger.Info("Opening file: " + mtb.FileName);
            }
            catch (Exception ex)
            {
                Logger.Error($"An error occurred while opening the file. \n{ex}");
                MessageBox.Show(Resources.Localization.MSGErrorWhenOpenFIle, "TextPad+", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        internal static void OpenFile(string fileName)
        {
            try
            {
                if (File.Exists(fileName) == false)
                {
                    MessageBox.Show(Resources.Localization.MSGErrorFileNotFound, "TextPad+", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                mtb = Program.MainForm.cTabControl.TabPages[Program.MainForm.cTabControl.SelectedIndex].Controls.OfType<MTextBox>().First();

                // Если текущая вкладка пуста, открываем в ней файл, а если нет, то создаём новую
                if (mtb.FileName != "Missing" || mtb.Text.Length > 0)
                {
                    Program.MainForm.CreateTab();
                    Program.MainForm.cTabControl.SelectTab(Program.MainForm.cTabControl.TabPages[Program.MainForm.cTabControl.TabPages.Count - 1]);
                    mtb = Program.MainForm.cTabControl.TabPages[Program.MainForm.cTabControl.SelectedIndex].Controls.OfType<MTextBox>().First();
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
                Program.MainForm.CheckFile();
                Program.MainForm.cTabControl.SelectedTab!.Text = Path.GetFileName(mtb.FileName);
                mtb.IsFileChanged = false;

                // Внос файла в список недавних файлов
                AddFileToRecentList(mtb);

                GC.Collect();
                Logger.Info("Opening file: " + mtb.FileName);
            }
            catch (Exception ex)
            {
                Logger.Error($"An error occurred while opening a file at the specified path. \n{ex}");
                MessageBox.Show(Resources.Localization.MSGErrorWhenOpenFIle, "TextPad+", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Повторное открытие файла
        internal static void ReopenFile()
        {
            try
            {
                mtb = Program.MainForm.cTabControl.TabPages[Program.MainForm.cTabControl.SelectedIndex].Controls.OfType<MTextBox>().First();

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
                Program.MainForm.CheckFile();

                // Изменение заголовка вкладки и обновление статуса файла
                Program.MainForm.cTabControl.SelectedTab!.Text = Path.GetFileName(mtb.FileName);
                mtb.IsFileChanged = false;
                Program.MainForm.CheckFile();

                GC.Collect();
                Logger.Info("Reopening file: " + Program.MainForm.cTabControl.TabPages[Program.MainForm.cTabControl.SelectedIndex].Controls.OfType<MTextBox>().First().FileName);
            }
            catch (Exception ex)
            {
                Logger.Error($"An error occurred while reopening a file. \n{ex}");
                MessageBox.Show(Resources.Localization.MSGErrorWhenOpenFIle, "TextPad+", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        //
        // Метод для добавления файла в список последних
        //
        internal static void AddFileToRecentList(MTextBox mtb)
        {
            foreach (ToolStripMenuItem rftsmi in Program.MainForm.RecentFilesListFileItem.DropDownItems)
                if (rftsmi.Text == mtb.FileName)
                    return;
            if (Program.MainForm.RecentFilesListFileItem.DropDownItems.Count == 10)
                Program.MainForm.RecentFilesListFileItem.DropDownItems.RemoveAt(0);
            ToolStripMenuItem tsmi = new();
            tsmi.Text = mtb.FileName;
            tsmi.Click += (sender, e) => OpenFile(tsmi.Text);
            Program.MainForm.RecentFilesListFileItem.DropDownItems.Add(tsmi);
        }

        //
        // Методы для запуска скриптов
        //
        internal static void RunScript()
        {
            if (Program.MainForm.RunScriptCombobox.SelectedIndex == 0)
                Program.PythonInterpreterForm.ShowDialog(Program.MainForm);
            else if (Program.MainForm.RunScriptCombobox.SelectedIndex == 1)
                RunWindowsScript();
            else if (Program.MainForm.RunScriptCombobox.SelectedIndex == 2)
                RunVbsJsScript();
            else if (Program.MainForm.RunScriptCombobox.SelectedIndex == 3)
                RunHTMLPage();
        }

        internal static void RunPythonScript()
        {
            Logger.Info("Running a Python file");
            mtb = Program.MainForm.cTabControl.TabPages[Program.MainForm.cTabControl.SelectedIndex].Controls.OfType<MTextBox>().First();

            try
            {
                if (mtb.TextLength == 0)
                {
                    MessageBox.Show(Resources.Localization.MSGErrorWhenRunningEmptyScript, "TextPad+", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                SaveFile();

                if (Program.MainForm.cTabControl.TabPages[Program.MainForm.cTabControl.SelectedIndex].Controls.OfType<MTextBox>().First().FileName == "Missing")
                    return;

                Process.Start(@"C:\Windows\py.exe", Program.MainForm.cTabControl.TabPages[Program.MainForm.cTabControl.SelectedIndex].Controls.OfType<MTextBox>().First().FileName);
            }
            catch (Exception ex)
            {
                Logger.Error($"An error occurred while running the Python script: {Program.MainForm.cTabControl.TabPages[Program.MainForm.cTabControl.SelectedIndex].Controls.OfType<MTextBox>().First().FileName} \n{ex}");
                MessageBox.Show(Resources.Localization.MSGErrorRunPythonFile, "TextPad+", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        internal static void RunPythonScript(string path)
        {
            Logger.Info("Running a Python file");
            mtb = Program.MainForm.cTabControl.TabPages[Program.MainForm.cTabControl.SelectedIndex].Controls.OfType<MTextBox>().First();

            try
            {
                if (mtb.TextLength == 0)
                {
                    MessageBox.Show(Resources.Localization.MSGErrorWhenRunningEmptyScript, "TextPad+", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                SaveFile();

                if (mtb.FileName == "Missing")
                    return;

                Process.Start(path, Program.MainForm.cTabControl.TabPages[Program.MainForm.cTabControl.SelectedIndex].Controls.OfType<MTextBox>().First().FileName);
            }
            catch (Exception ex)
            {
                Logger.Error($"An error occurred while running the Python script at the specified path: {Program.MainForm.cTabControl.TabPages[Program.MainForm.cTabControl.SelectedIndex].Controls.OfType<MTextBox>().First().FileName} \n{ex}");
                MessageBox.Show(Resources.Localization.MSGErrorRunPythonFile, "TextPad+", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        internal static void RunWindowsScript()
        {
            Logger.Info("Running a bat/cmd file");

            mtb = Program.MainForm.cTabControl.TabPages[Program.MainForm.cTabControl.SelectedIndex].Controls.OfType<MTextBox>().First();

            try
            {
                if (mtb.TextLength == 0)
                {
                    MessageBox.Show(Resources.Localization.MSGErrorWhenRunningEmptyScript, "TextPad+", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                SaveFile();
                Process.Start(Program.MainForm.cTabControl.TabPages[Program.MainForm.cTabControl.SelectedIndex].Controls.OfType<MTextBox>().First().FileName);
            }
            catch (Exception ex)
            {
                Logger.Error($"Error while running Windows script: {Program.MainForm.cTabControl.TabPages[Program.MainForm.cTabControl.SelectedIndex].Controls.OfType<MTextBox>().First().FileName} \n{ex}");
                MessageBox.Show(Resources.Localization.MSGErrorRunBatCmdFile, "TextPad+", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        internal static void RunVbsJsScript()
        {
            Logger.Info("Running a VBS / JS script");

            mtb = Program.MainForm.cTabControl.TabPages[Program.MainForm.cTabControl.SelectedIndex].Controls.OfType<MTextBox>().First();

            try
            {
                if (mtb.TextLength == 0)
                {
                    MessageBox.Show(Resources.Localization.MSGErrorWhenRunningEmptyScript, "TextPad+", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                SaveFile();
                Process.Start("wscript.exe", mtb.FileName);
            }
            catch (Exception ex)
            {
                Logger.Error($"Error while running VBS / JS script: {Program.MainForm.cTabControl.TabPages[Program.MainForm.cTabControl.SelectedIndex].Controls.OfType<MTextBox>().First().FileName} \n{ex}");
                MessageBox.Show(Resources.Localization.MSGErrorRunVBSJSFile, "TextPad+", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        internal static void RunHTMLPage()
        {
            try
            {
                SaveFile();
                mtb = Program.MainForm.cTabControl.TabPages[Program.MainForm.cTabControl.SelectedIndex].Controls.OfType<MTextBox>().First();
                Process websiteProcess = new();
                websiteProcess.StartInfo.UseShellExecute = true;
                websiteProcess.StartInfo.FileName = mtb.FileName;
                websiteProcess.Start();
            }
            catch { }
        }
    }
}
