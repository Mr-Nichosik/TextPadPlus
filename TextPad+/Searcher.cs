
namespace TextPad_
{
    /// <summary>
    /// Класс, занимающийся поиском, заменой в тексте и т.п.
    /// </summary>
    internal static class Searcher
    {
        private static readonly LogSystem Logger = new() { UserFolderName = $"{Application.StartupPath}\\logs" };
        private static int findCutLength = 0;

        internal static void Replace(ref TextBox FindTextBox, ref TextBox ReplaceTextBox)
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
                    MessageBox.Show(Resources.Localization.SearchUINoFoundWord, "TextPad+", MessageBoxButtons.OK, MessageBoxIcon.Information); // в противном случае сообщаем о не найденном слове 
            }
            catch (Exception ex)
            {
                MessageBox.Show(Resources.Localization.MSGErrorNoTabsOpen, "TextPad+", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Logger.Error($"{ex} Replace text error.");
                return;
            }
        }

        internal static void ReplaceAll(ref TextBox FindTextBox, ref TextBox ReplaceTextBox)
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
                    MessageBox.Show(Resources.Localization.SearchUINoFoundWord, "TextPad+", MessageBoxButtons.OK, MessageBoxIcon.Information); // в противном случае сообщаем о не найденном слове 
            }
            catch (Exception ex)
            {
                MessageBox.Show(Resources.Localization.MSGErrorNoTabsOpen, "TextPad+", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Logger.Error($"{ex} Replace all text error.");
                return;
            }
        }

        internal static void GoToLine(ref NumericUpDown GoToLineNumericUpDown)
        {
            try
            {
                MTextBox mtb = Program.MainForm.cTabControl.TabPages[Program.MainForm.cTabControl.SelectedIndex].Controls.OfType<MTextBox>().First();

                int lineNumber = Convert.ToInt32(GoToLineNumericUpDown.Text);
                if (lineNumber > 0 && lineNumber <= mtb.Lines.Length)
                {
                    mtb.SelectionStart = mtb.GetFirstCharIndexFromLine(Convert.ToInt32(GoToLineNumericUpDown.Text) - 1);
                    mtb.ScrollToCaret();
                }
                else
                    MessageBox.Show(Resources.Localization.LineNotFound, "TextPad+", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(Resources.Localization.MSGErrorNoTabsOpen, "TextPad+", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Logger.Error($"{ex} Go to line error.");
                return;
            }
        }

        internal static void Search(ref TextBox FindTextBox) => Search(ref FindTextBox, ref findCutLength);

        internal static void Search(ref TextBox FindTextBox, ref int findCutLength)
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
                        findCutLength = 0;

                    FindTextBox.BackColor = SystemColors.Window;
                }
                else
                {
                    findCutLength = 0;
                    FindTextBox.BackColor = Color.LavenderBlush;
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
