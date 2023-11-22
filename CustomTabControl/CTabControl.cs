using System.Reflection;

namespace CustomTabControl
{
    public partial class CTabControl : TabControl
    {
        public CTabControl()
        {
            InitializeComponent();
        }

        public static string GetAssemblyVersion()
        {
            string assembly = Assembly.GetExecutingAssembly().GetName().Version.ToString();
            assembly = assembly.Remove(assembly.Length - 2);
            return assembly;
        }
    }
}