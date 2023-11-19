
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
        // ������� ����
        public static FormMainUI MainUI = new();
        // ���� ����������� ����������
        public static Updater.FormUpdaterUI UpdaterUI = new();
        // ������ ���������� ���������
        // 0 - ��������� �� �����������; 1 - ��������� ����������� � ��������� ������; 2 - ��������� ����������� � ���������� � �������
        public static int UpdateStatus = 0;

        /// <summary>
        /// ����� ����� � ���������
        /// </summary>
        [STAThread]
        static void Main()
        {
            ApplicationConfiguration.Initialize();
            Application.Run(new FormMainUI());
        }
    }
}