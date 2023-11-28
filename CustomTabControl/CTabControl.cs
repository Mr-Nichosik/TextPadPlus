using System.Reflection;

namespace CustomTabControl
{
    public partial class CTabControl : TabControl
    {
        private bool TestFeatures = false;

        public CTabControl()
        {
            InitializeComponent();
        }

        public bool EnabneTestFeatures
        {
            get
            {
                return TestFeatures;
            }
            set
            {
                TestFeatures = value;
            }
        }

        public static string GetAssemblyVersion()
        {
            string assembly = Assembly.GetExecutingAssembly().GetName().Version!.ToString();
            assembly = assembly.Remove(assembly.Length - 2);
            return assembly;
        }
    }
}