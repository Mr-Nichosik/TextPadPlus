
global using CustomTabControl;
global using LoggingSystem;
global using ModifiedTextBox;
global using System;
global using System.Diagnostics;
global using System.Drawing;
global using System.IO;
global using System.Linq;
global using System.Windows.Forms;
global using System.Reflection;

namespace TextPad_
{
    /// <summary>
    /// Главный класс программы
    /// </summary>
    internal static class Program
    {
        // Свойства для чтения с инфой о программе
        public static readonly string Name = Assembly.GetExecutingAssembly().GetName().Name!.ToString();
        public static readonly string Version = Assembly.GetExecutingAssembly().GetName().Version!.ToString(3);
        public static readonly string DateOfRelease = "Dev";
        public static readonly string Path = Application.StartupPath;
        public static readonly string WebSite = "https://mr-nichosik.github.io/Main_Page/";

        // Окна программы
        internal static readonly MainUI MainForm = new();
        internal static readonly Updater.UpdaterUI UpdaterForm = new();
        internal static readonly PythonInterpreterUI PythonInterpreterForm = new();
        // Статус обновления программы
        // 0 - программа не обновляется; 1 - программа обновляется и закрывать нельзя; 2 - программа обновляется и необходимо её закрыть
        public static byte UpdateStatus { get; set; } = 0;

        /// <summary>
        /// Точка входа в программу
        /// </summary>
        [STAThread]
        private static void Main()
        {
            ApplicationConfiguration.Initialize();
            Application.Run(MainForm);
        }
    }
}