
using System.Reflection;
using System.Drawing;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace CustomTabControl
{
    public partial class CTabControl : TabControl
    {
        private bool TestFeatures = false;

        public CTabControl()
        {
            InitializeComponent();
        }

        public static string Version
        {
            get { return Assembly.GetExecutingAssembly().GetName().Version!.ToString(3); }
        }

        public bool EnabneTestFeatures
        {
            get { return TestFeatures; }
            set { TestFeatures = value; }
        }

        public void OnDrawPage(object sender, PaintEventArgs e)
        {
            e.Graphics.DrawString("+", new Font("verdana", 10, FontStyle.Bold), Brushes.Black, e.ClipRectangle.X + 10, e.ClipRectangle.Y + 10);
        }
    }
}