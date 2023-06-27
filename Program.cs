global using System;
global using System.IO;
global using System.Drawing;
global using System.Linq;
global using System.Windows.Forms;
global using System.Xml;
global using System.Diagnostics;
global using LoggingSystem;

namespace TextPad_
{
    /// <summary>
    /// ������� ����� ���������
    /// </summary>
    internal static class Program
    {
        public static FormMainUI mainUI = new FormMainUI();
        /// <summary>
        ///  ����� ����� � ���������.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            ApplicationConfiguration.Initialize();
            Application.Run(new FormMainUI());
        }
    }
}