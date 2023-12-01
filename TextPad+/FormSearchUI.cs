
namespace TextPad_
{
    /// <summary>
    /// Класс окна с инструментами для перехода к строкам, поиска и замены слов в тексте.
    /// </summary>

    public partial class SearchUI : Form
    {
        private readonly LogSystem Logger = new LogSystem("logs");
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

        private void Replace(object sender, EventArgs e)
        {
            try
            {
                MTextBox mtb = Program.MainForm.cTabControl.TabPages[Program.MainForm.cTabControl.SelectedIndex].Controls.OfType<MTextBox>().First();
                if (mtb.Text.Contains(FindTextBox.Text))
                {
                    int index = mtb.Text.IndexOf(FindTextBox.Text);
                    string TextBeforeIndex, TextAfterIndex;

                    TextBeforeIndex = mtb.Text.Substring(0, index);
                    TextAfterIndex = mtb.Text.Substring(index + FindTextBox.TextLength, mtb.TextLength - (index + FindTextBox.TextLength));
                    string result = TextBeforeIndex + ReplaceTextBox.Text + TextAfterIndex;
                    mtb.Clear();
                    mtb.AppendText(result);
                    mtb.Select(index + findCutLength, ReplaceTextBox.TextLength);
                    mtb.ScrollToCaret();
                    mtb.Focus();

                    Program.MainForm.TextLengthLabel.Text = mtb.Text.Length.ToString();
                }
                else
                {
                    MessageBox.Show(Resources.Localization.SearchUINoFoundWord, "TextPad+", MessageBoxButtons.OK, MessageBoxIcon.Information); // в противном случае сообщаем о не найденном слове 
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(Resources.Localization.MSGErrorNoTabsOpen, "TextPad+", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Logger.Error($"{ex} Replace text error.");
                return;
            }
        }

        private void ReplaceAll(object sender, EventArgs e)
        {
            try
            {
                MTextBox mtb = Program.MainForm.cTabControl.TabPages[Program.MainForm.cTabControl.SelectedIndex].Controls.OfType<MTextBox>().First();
                if (mtb.Text.Contains(FindTextBox.Text))
                {
                    while (mtb.Text.Contains(FindTextBox.Text))
                    {
                        int index = mtb.Text.IndexOf(FindTextBox.Text);
                        string TextBeforeIndex, TextAfterIndex;

                        TextBeforeIndex = mtb.Text.Substring(0, index);
                        TextAfterIndex = mtb.Text.Substring(index + FindTextBox.TextLength, (mtb.TextLength - (index + FindTextBox.TextLength)));
                        string result = TextBeforeIndex + ReplaceTextBox.Text + TextAfterIndex;
                        mtb.Clear();
                        mtb.AppendText(result);

                        Program.MainForm.TextLengthLabel.Text = mtb.Text.Length.ToString();
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
                Logger.Error($"{ex} Replace all text error.");
                return;
            }
        }

        private void GoToLine(object sender, EventArgs e)
        {
            try
            {
                MTextBox mtb = Program.MainForm.cTabControl.TabPages[Program.MainForm.cTabControl.SelectedIndex].Controls.OfType<MTextBox>().First();

                int lineNumber = Convert.ToInt32(numericLineNumber.Text);
                if (lineNumber > 0 && lineNumber <= mtb.Lines.Length)
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
                Logger.Error($"{ex} Go to line error.");
                return;
            }
        }

        private void Search(object sender, EventArgs e)
        {
            Search(ref findCutLength);
        }

        private void Search(ref int findCutLength)
        {
            try
            {
                MTextBox mtb = Program.MainForm.cTabControl.TabPages[Program.MainForm.cTabControl.SelectedIndex].Controls.OfType<MTextBox>().First();

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
                Logger.Error($"{ex} Find text error.");
                return;
            }
        }
    }
}
