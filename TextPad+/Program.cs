
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
    /// ������� ����� ���������
    /// </summary>
    internal static class Program
    {
        // �������� ��� ������ � ����� � ���������
        public static readonly string Name = Assembly.GetExecutingAssembly().GetName().Name!.ToString();
        public static readonly string Version = Assembly.GetExecutingAssembly().GetName().Version!.ToString(3);
        public static readonly string DateOfRelease = "Dev";
        public static readonly string Path = Application.StartupPath;
        public static readonly string WebSite = "https://mr-nichosik.github.io/Main_Page/";

        // ���� ���������
        internal static readonly MainUI MainForm = new();
        internal static readonly Updater.UpdaterUI UpdaterForm = new();
        internal static readonly PythonInterpreterUI PythonInterpreterForm = new();
        // ������ ���������� ���������
        // 0 - ��������� �� �����������; 1 - ��������� ����������� � ��������� ������; 2 - ��������� ����������� � ���������� � �������
        public static byte UpdateStatus { get; set; } = 0;

        /// <summary>
        /// ����� ����� � ���������
        /// </summary>
        [STAThread]
        private static void Main()
        {
            ApplicationConfiguration.Initialize();
            Application.Run(MainForm);
        }
    }
}