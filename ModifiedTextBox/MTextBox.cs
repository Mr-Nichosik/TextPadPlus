using System.ComponentModel;
using System.Reflection;
using System.Windows.Forms;

namespace ModifiedTextBox
{
    public partial class MTextBox : RichTextBox
    {
        private bool _IsChanged = false;
        private string _FileName = "Missing";
        private string _Encoding = "UTF-8";

        [Category("File System"), Description("Own properties for storing information about the file located in this TextBox.")]

        public bool IsFileChanged
        {
            get { return _IsChanged; }
            set
            {
                _IsChanged = value;
            }
        }

        public string Encoding
        {
            get { return _Encoding; }
            set
            {
                _Encoding = value;
            }
        }

        public string FileName
        {
            get { return _FileName; }
            set
            {
                _FileName = value;
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