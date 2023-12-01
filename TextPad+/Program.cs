
global using CustomTabControl;
global using LoggingSystem;
global using ModifiedTextBox;
global using System;
global using System.Diagnostics;
global using System.Drawing;
global using System.IO;
global using System.Linq;
global using System.Windows.Forms;

namespace TextPad_
{
    /// <summary>
    /// Главный класс программы
    /// </summary>
    internal static class Program
    {
        // Окна программы
        internal static MainUI MainForm = new();
        internal static Updater.UpdaterUI UpdaterForm = new();
        internal static SearchUI SearchForm = new();
        internal static PythonInterpreterUI PythonInterpreterForm = new();
        // Статус обновления программы
        // 0 - программа не обновляется; 1 - программа обновляется и зкарывать нельзя; 2 - программа обновляется и нуобходимо её закрыть
        public static byte UpdateStatus = 0;

        /// <summary>
        /// Точка входа в программу
        /// </summary>
        [STAThread]
        static void Main()
        {
            ApplicationConfiguration.Initialize();
            Application.Run(MainForm);
        }
    }
}