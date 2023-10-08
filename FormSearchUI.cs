
namespace TextPad_
{
    /// <summary>
    /// Класс окна с инструментами для перехода к строкам, поиска и замены слов в тексте.
    /// </summary>

    public partial class SearchUI : Form
    {
        private readonly ILogger LS = new LogSystem("logs");
        private int findCutLength = 0;

        public SearchUI()
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

        private void replaceClick(object sender, EventArgs e)
        {
            try
            {
                MTextBox rtb = Program.mainUI.cTabControl.TabPages[Program.mainUI.cTabControl.SelectedIndex].Controls.OfType<MTextBox>().First();
                if (rtb.Text.Contains(FindTextBox.Text))
                {
                    int index = rtb.Text.IndexOf(FindTextBox.Text);
                    string str1, str2;

                    str1 = rtb.Text.Substring(0, index);
                    str2 = rtb.Text.Substring((index + FindTextBox.TextLength), (rtb.TextLength - (index + FindTextBox.TextLength)));
                    string result = str1 + ReplaceTextBox.Text + str2;
                    rtb.Clear();
                    rtb.AppendText(result);
                    rtb.Select(index + findCutLength, ReplaceTextBox.TextLength);
                    rtb.ScrollToCaret();
                    rtb.Focus();

                    Program.mainUI.textLengthLabel.Text = rtb.Text.Length.ToString();
                }
                else
                {
                    MessageBox.Show(Resources.Localization.SearchUINoFoundWord, "TextPad+", MessageBoxButtons.OK, MessageBoxIcon.Information); // в противном случае сообщаем о не найденном слове 
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(Resources.Localization.MSGErrorNoTabsOpen, "TextPad+", MessageBoxButtons.OK, MessageBoxIcon.Error);
                LS.Error($"{ex} Replace text error.");
                return;
            }
        }

        private void ReplaceAllButton_Click(object sender, EventArgs e)
        {
            try
            {
                MTextBox rtb = Program.mainUI.cTabControl.TabPages[Program.mainUI.cTabControl.SelectedIndex].Controls.OfType<MTextBox>().First();
                if (rtb.Text.Contains(FindTextBox.Text))
                {
                    while (rtb.Text.Contains(FindTextBox.Text))
                    {
                        int index = rtb.Text.IndexOf(FindTextBox.Text);
                        string str1, str2;

                        str1 = rtb.Text.Substring(0, index);
                        str2 = rtb.Text.Substring((index + FindTextBox.TextLength), (rtb.TextLength - (index + FindTextBox.TextLength)));
                        string result = str1 + ReplaceTextBox.Text + str2;
                        rtb.Clear();
                        rtb.AppendText(result);

                        Program.mainUI.textLengthLabel.Text = rtb.Text.Length.ToString();
                    }
                }
                else
                {
                    MessageBox.Show(Resources.Localization.SearchUINoFoundWord, "TextPad+", MessageBoxButtons.OK, MessageBoxIcon.Information); // в противном случае сообщаем о не найденном слове 
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(Resources.Localization.MSGErrorNoTabsOpen, "TextPad+", MessageBoxButtons.OK, MessageBoxIcon.Error);
                LS.Error($"{ex} Replace all text error.");
                return;
            }
        }

        private void gotoBtnClick(object sender, EventArgs e)
        {
            try
            {
                MTextBox rtb = Program.mainUI.cTabControl.TabPages[Program.mainUI.cTabControl.SelectedIndex].Controls.OfType<MTextBox>().First();

                int lineNumber = Convert.ToInt32(numericLineNumber.Text);
                if (lineNumber > 0 && lineNumber <= rtb.Lines.Count())
                {
                    rtb.SelectionStart = rtb.GetFirstCharIndexFromLine(Convert.ToInt32(numericLineNumber.Text) - 1);
                    rtb.ScrollToCaret();
                    this.Close();
                }
                else
                {
                    MessageBox.Show(Resources.Localization.LineNotFound, "TextPad+", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(Resources.Localization.MSGErrorNoTabsOpen, "TextPad+", MessageBoxButtons.OK, MessageBoxIcon.Error);
                LS.Error($"{ex} Go to line error.");
                return;
            }
        }

        private void SearchButton_Click(object sender, EventArgs e)
        {
            findText(ref findCutLength);
        }

        private void findText(ref int findCutLength)
        {
            try
            {
                MTextBox rtb = Program.mainUI.cTabControl.TabPages[Program.mainUI.cTabControl.SelectedIndex].Controls.OfType<MTextBox>().First();

                if (rtb.Text.ToLower().Contains(FindTextBox.Text.ToLower()))
                {
                    string text = rtb.Text.ToLower();
                    string nextText = text.Remove(0, findCutLength);
                    int resultPosition = nextText.IndexOf(FindTextBox.Text.ToLower());

                    if (resultPosition != -1)
                    {
                        rtb.Select(resultPosition + findCutLength, FindTextBox.Text.Length);
                        rtb.ScrollToCaret();
                        rtb.Focus();
                        findCutLength += FindTextBox.Text.Length + resultPosition;
                    }
                    else if (resultPosition == -1 && findCutLength != 0)
                    {
                        findCutLength = 0;
                    }
                }
                else
                {
                    findCutLength = 0;
                    MessageBox.Show(Resources.Localization.SearchUINoFoundWord, "TextPad+", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(Resources.Localization.MSGErrorNoTabsOpen, "TextPad+", MessageBoxButtons.OK, MessageBoxIcon.Error);
                LS.Error($"{ex} Find text error.");
                return;
            }
        }
    }
}
