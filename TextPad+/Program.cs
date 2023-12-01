
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
    /// ������� ����� ���������
    /// </summary>
    internal static class Program
    {
        // ���� ���������
        internal static MainUI MainForm = new();
        internal static Updater.UpdaterUI UpdaterForm = new();
        internal static SearchUI SearchForm = new();
        internal static PythonInterpreterUI PythonInterpreterForm = new();
        // ������ ���������� ���������
        // 0 - ��������� �� �����������; 1 - ��������� ����������� � ��������� ������; 2 - ��������� ����������� � ���������� � �������
        public static byte UpdateStatus = 0;

        /// <summary>
        /// ����� ����� � ���������
        /// </summary>
        [STAThread]
        static void Main()
        {
            ApplicationConfiguration.Initialize();
            Application.Run(MainForm);
        }
    }
}