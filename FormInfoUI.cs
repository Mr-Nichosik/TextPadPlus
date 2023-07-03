
namespace TextPad_
{
    /// <summary>
    ///  Класс окна с информацией о программе. из функционала есть только кнопка для перехода на сайт проекта.
    /// </summary>
    public partial class FormInfoUI : Form
    {
        private readonly ILogger Ls = new LogSystem("logs");

        public FormInfoUI()
        {
            if (Properties.Settings.Default.Language == "English")
            {
                System.Threading.Thread.CurrentThread.CurrentUICulture = System.Globalization.CultureInfo.GetCultureInfo("en-US");
                System.Threading.Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.GetCultureInfo("en-US");
            }
            else if (Properties.Settings.Default.Language == "Russian")
            {
                System.Threading.Thread.CurrentThread.CurrentUICulture = System.Globalization.CultureInfo.GetCultureInfo("ru-RU");
                System.Threading.Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.GetCultureInfo("ru-RU");
            }

            InitializeComponent();
        }

        private void OpenWebSite(object sender, EventArgs e)
        {
            try
            {
                Process websiteProcess = new Process();
                websiteProcess.StartInfo.UseShellExecute = true;
                websiteProcess.StartInfo.FileName = Program.mainUI.WebSite;
                websiteProcess.Start();
            }
            catch (Exception ex)
            {
                MessageBox.Show(Resources.Localization.MSGErrorWhenCheckingInternet, "TextPad+", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Ls.Error($"{ex} when starting web site from InfoUI window.");
            }
        }

        private void _infoUI_Load(object sender, EventArgs e)
        {

            textBoxWebSiteUrl.Text = Program.mainUI.WebSite;

            ProgramNameLabel.Text = Program.mainUI.GetAssemblyName().ToString();
            VersionLabel.Text = Program.mainUI.GetAssemblyVersion().ToString();
            DateOfReleaseLabel.Text = Program.mainUI.DateOfRelease;
            DeveloperLabel.Text = Program.mainUI.GetAssemblyCompany().ToString();

            labelVersion.Text += Program.mainUI.GetAssemblyVersion();
            LSVersionLabel.Text = LogSystem.GetAssemblyVersion();
            CTCLabelVersion.Text = CTabControl.GetAssemblyVersion();
        }
    }
}
