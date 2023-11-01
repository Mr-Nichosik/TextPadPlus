
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
                MTextBox mtb = Program.MainUI.cTabControl.TabPages[Program.MainUI.cTabControl.SelectedIndex].Controls.OfType<MTextBox>().First();
                if (mtb.Text.Contains(FindTextBox.Text))
                {
                    int index = mtb.Text.IndexOf(FindTextBox.Text);
                    string str1, str2;

                    str1 = mtb.Text.Substring(0, index);
                    str2 = mtb.Text.Substring((index + FindTextBox.TextLength), (mtb.TextLength - (index + FindTextBox.TextLength)));
                    string result = str1 + ReplaceTextBox.Text + str2;
                    mtb.Clear();
                    mtb.AppendText(result);
                    mtb.Select(index + findCutLength, ReplaceTextBox.TextLength);
                    mtb.ScrollToCaret();
                    mtb.Focus();

                    Program.MainUI.textLengthLabel.Text = mtb.Text.Length.ToString();
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
                MTextBox mtb = Program.MainUI.cTabControl.TabPages[Program.MainUI.cTabControl.SelectedIndex].Controls.OfType<MTextBox>().First();
                if (mtb.Text.Contains(FindTextBox.Text))
                {
                    while (mtb.Text.Contains(FindTextBox.Text))
                    {
                        int index = mtb.Text.IndexOf(FindTextBox.Text);
                        string str1, str2;

                        str1 = mtb.Text.Substring(0, index);
                        str2 = mtb.Text.Substring((index + FindTextBox.TextLength), (mtb.TextLength - (index + FindTextBox.TextLength)));
                        string result = str1 + ReplaceTextBox.Text + str2;
                        mtb.Clear();
                        mtb.AppendText(result);

                        Program.MainUI.textLengthLabel.Text = mtb.Text.Length.ToString();
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
                MTextBox mtb = Program.MainUI.cTabControl.TabPages[Program.MainUI.cTabControl.SelectedIndex].Controls.OfType<MTextBox>().First();

                int lineNumber = Convert.ToInt32(numericLineNumber.Text);
                if (lineNumber > 0 && lineNumber <= mtb.Lines.Count())
                {
                    mtb.SelectionStart = mtb.GetFirstCharIndexFromLine(Convert.ToInt32(numericLineNumber.Text) - 1);
                    mtb.ScrollToCaret();
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
                MTextBox mtb = Program.MainUI.cTabControl.TabPages[Program.MainUI.cTabControl.SelectedIndex].Controls.OfType<MTextBox>().First();

                if (mtb.Text.ToLower().Contains(FindTextBox.Text.ToLower()))
                {
                    string text = mtb.Text.ToLower();
                    string nextText = text.Remove(0, findCutLength);
                    int resultPosition = nextText.IndexOf(FindTextBox.Text.ToLower());

                    if (resultPosition != -1)
                    {
                        mtb.Select(resultPosition + findCutLength, FindTextBox.Text.Length);
                        mtb.ScrollToCaret();
                        mtb.Focus();
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
