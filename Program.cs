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
        public static FormMainUI mainUI = new FormMainUI();
        public static bool isUpdating = false;

        /// <summary>
        /// Точка входа в программу
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            ApplicationConfiguration.Initialize();
            Application.Run(new FormMainUI());
        }
    }
}