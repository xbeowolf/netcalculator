using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using Expression;

namespace NetCalculator
{

	public struct Highlight
	{
		public Highlight(string d, Color fg, Color bg, bool b, bool i)
		{
			description = d;
			foregraund = fg;
			background = bg;
			bold = b;
			italic = i;
		}

		public string description;
		public Color foregraund, background;
		public bool bold, italic;
	}

	public partial class NetCalculator : Form
	{
		/// <summary>
		/// Expression object representation
		/// </summary>
		public Expression.Expression express;

		/// <summary>
		/// Font facename for expression and result window
		/// </summary>
		private string font;

		/// <summary>
		/// Opacity for active form state
		/// </summary>
		private float activeopacity = 0.90f;
		/// <summary>
		/// Opacity for inactive form state
		/// </summary>
		private float inactiveopacity = 0.75f;

		private Highlight[] syntax =
		{
			new Highlight("bracket input", Color.Black, Color.LightGreen, false, false),
			new Highlight("bracket", Color.DarkSeaGreen, Color.White, false, false)
		};

		private int prevpos = 0, prevlen = 0, bracket1 = -1, bracket2 = -1;
		private bool processsel = true;

		/// <summary>
		/// Popup window with context term list
		/// </summary>
		private NetTerm TermForm;

		/// <summary>
		/// Allow escape popup window for current term
		/// </summary>
		private bool bKeepTermformInvisibleNow = false;

		private void EvaluateContent(bool show, bool sel)
		{
			try
			{
				string res = express.Evaluate(
					richExpression.Text,
					hexadecimalToolStripMenuItem.Checked ? 16 : 10);
				if (show) richResult.Text = res;
				if (netcalcErrorProvider.GetError(richExpression).Length > 0)
					netcalcErrorProvider.SetError(richExpression, "");
			}
			catch (ExpressException except)
			{
				if (show) richResult.Text = "error";
				if (sel && except.endpos - except.lexeme.Length >= 0 && except.endpos < richExpression.TextLength)
				{
					richExpression.SelectionStart = except.endpos - except.lexeme.Length;
					richExpression.SelectionLength = except.lexeme.Length;
				}
				netcalcErrorProvider.SetError(richExpression, except.Message);
			}
			catch (Exception except)
			{
				if (show) richResult.Text = "error";
				netcalcErrorProvider.SetError(richExpression, except.Message);
			}
		}

		public NetCalculator()
		{
			InitializeComponent();

			express = new Expression.Expression();
			fontToolStripComboBox.Text = font = "Arial";

			TermForm = new NetTerm(richExpression, express);
			this.AddOwnedForm(TermForm);
		}

		private void EvTextChangedExpress(object sender, EventArgs e)
		{
			//int start, end;
			string content = richExpression.Text;

			int curpos = richExpression.SelectionStart, cursel = richExpression.SelectionLength, curlen = richExpression.TextLength;

			if (curpos < bracket1) bracket1++;
			if (curpos < bracket2) bracket2++;

			prevpos = curpos; prevlen = curlen;

			if (content.Length == 0)
			{
				netcalcErrorProvider.SetError(richExpression, "");
				return;
			}

			if (hotCheckingToolStripMenuItem.Checked)
			{
				EvaluateContent(hotcalculatingToolStripMenuItem.Checked, false);
			}
			else
				if (netcalcErrorProvider.GetError(richExpression).Length > 0)
					netcalcErrorProvider.SetError(richExpression, "");
		}

		private void richExpression_SelectionChanged(object sender, EventArgs e)
		{
			if (!processsel) return;

			processsel = false;

			int curpos = richExpression.SelectionStart, cursel = richExpression.SelectionLength, curlen = richExpression.TextLength;

			string content = richExpression.Text;

			// Show context term in popup TermForm
			bool popup = false;
			if (curpos > 0 && curpos - 1 < content.Length && Expression.Expression.isAlphaChar(content[curpos - 1]))
			{
				int start;
				for (start = curpos;
						start > 0 && (Expression.Expression.isAlphaChar(content[start - 1]) ||
						(content[start - 1] >= '0' && content[start - 1] <= '9'));
						start--) ;
				popup = String.Compare(content, start, "0x", 0, 2, true) != 0;
			}
			if (popup)
			{
				if (!bKeepTermformInvisibleNow)
				{
					if (!TermForm.Visible)
					{
						Point p = new Point(this.Location.X, this.Location.Y + this.Height);
						TermForm.Location = p;
						TermForm.Show();
						this.Activate();
					}
					int termbegin;
					for (termbegin = curpos - 1; termbegin >= 0 && Expression.Expression.isAlphaChar(content[termbegin]); termbegin--) ;
					TermForm.SelectTerm(content.Substring(termbegin + 1, curpos - 1 - termbegin));
				}
			}
			else
			{
				bKeepTermformInvisibleNow = false;
				if (TermForm.Visible) TermForm.Hide();
			}

			// Hide previous brackets highlighting
			/*if (bracket1 >= 0)
			{
					richExpression.SelectionStart = bracket1;
					richExpression.SelectionLength = 1;
					richExpression.SelectionColor = syntax[1].foregraund;
					richExpression.SelectionBackColor = syntax[1].background;
			}
			if (bracket2 >= 0)
			{
					richExpression.SelectionStart = bracket2;
					richExpression.SelectionLength = 1;
					richExpression.SelectionColor = syntax[1].foregraund;
					richExpression.SelectionBackColor = syntax[1].background;
			}
			if (bracket1 >= 0 || bracket2 >= 0)
			{
					richExpression.SelectionStart = curpos;
					richExpression.SelectionLength = cursel;
					bracket1 = bracket2 = -1;
			}
			// Checkup on bracket highlighting
			if (curpos > 0 && (express.express[curpos - 1] == '(' || express.express[curpos - 1] == ')'))
			{
					int i, j;
					if (express.express[curpos - 1] == '(')
					{
							i = curpos; j = 1;
							while (i < express.express.Length && j > 0)
							{
									if (express.express[i] == '(') j++;
									if (express.express[i] == ')') j--;
									i++;
							}
							if (j == 0)
							{
									bracket1 = curpos - 1;
									bracket2 = i - 1;
							}
							else
									bracket1 = bracket2 = -1;
					}
					else
					{
							i = curpos - 2; j = 1;
							while (i >= 0 && j > 0)
							{
									if (express.express[i] == ')') j++;
									if (express.express[i] == '(') j--;
									i--;
							}
							if (j == 0)
							{
									bracket1 = i + 1;
									bracket2 = curpos - 1;
							}
							else
									bracket1 = bracket2 = -1;
					}
					if (bracket1 >= 0 && bracket2 >= 0)
					{
							richExpression.SelectionStart = bracket1;
							richExpression.SelectionLength = 1;
							richExpression.SelectionColor = syntax[0].foregraund;
							richExpression.SelectionBackColor = syntax[0].background;
							richExpression.SelectionStart = bracket2;
							richExpression.SelectionLength = 1;
							richExpression.SelectionColor = syntax[0].foregraund;
							richExpression.SelectionBackColor = syntax[0].background;
							richExpression.SelectionStart = curpos;
							richExpression.SelectionLength = cursel;
					}
			}*/
			processsel = true;
		}

		private void EvKeyPressExpress(object sender, KeyPressEventArgs e)
		{
			if (e.KeyChar == (char)Keys.Return)
			{
				if (TermForm.Visible)
				{
					TermForm.InsertSelectedTerm();
				}
				else
				{
					EvaluateContent(true, true);
				}
				e.Handled = true;
			}
			if (e.KeyChar == (char)Keys.Escape)
			{
				if (TermForm.Visible)
				{
					bKeepTermformInvisibleNow = true;
					TermForm.Hide();
					netcalcErrorProvider.SetError(richExpression, "");
					return;
				}
				bKeepTermformInvisibleNow = false;
				richExpression.Text = richResult.Text = "";
				e.Handled = true;
			}
		}

		private void NetCalculator_Activated(object sender, EventArgs e)
		{
			this.Opacity = activeopacity;
			this.TransparencyKey = Color.Empty;
		}

		private void NetCalculator_Deactivate(object sender, EventArgs e)
		{
			this.Opacity = inactiveopacity;
			this.TransparencyKey = transparencyToolStripMenuItem.Checked ? SystemColors.Window : Color.Empty;
		}

		private void NetCalculator_Move(object sender, EventArgs e)
		{
			if (TermForm.Visible)
			{
				Point p = new Point(this.Location.X, this.Location.Y + this.Height);
				TermForm.Location = p;
				this.Activate();
			}
		}

		private void NetCalculator_Resize(object sender, EventArgs e)
		{
			if (TermForm != null && TermForm.Visible)
			{
				Point p = new Point(this.Location.X, this.Location.Y + this.Height);
				TermForm.Location = p;
				this.Activate();
			}
		}

		private void evaluateToolStripMenuItem_Click(object sender, EventArgs e)
		{
			EvaluateContent(true, true);
		}

		private void cutToolStripMenuItem_Click(object sender, EventArgs e)
		{
			richExpression.Cut();
		}

		private void copyToolStripMenuItem_Click(object sender, EventArgs e)
		{
			richExpression.Copy();
		}

		private void pasteToolStripMenuItem_Click(object sender, EventArgs e)
		{
			richExpression.Paste();
		}

		private void clearToolStripMenuItem_Click(object sender, EventArgs e)
		{
			richExpression.Text = "";
		}

		private void selectallToolStripMenuItem_Click(object sender, EventArgs e)
		{
			richExpression.SelectAll();
		}

		private void copyresToolStripMenuItem_Click(object sender, EventArgs e)
		{
			richResult.Copy();
		}

		private void selectallresToolStripMenuItem_Click(object sender, EventArgs e)
		{
			richResult.SelectAll();
		}

		private void hexadecimalToolStripMenuItem_CheckedChanged(object sender, EventArgs e)
		{
			EvaluateContent(true, false);
		}

		private void contextMenuStripExpress_Opening(object sender, CancelEventArgs e)
		{
			evaluateToolStripMenuItem.Enabled = richExpression.TextLength > 0;
			cutToolStripMenuItem.Enabled = richExpression.SelectionLength > 0;
			copyToolStripMenuItem.Enabled = richExpression.SelectionLength > 0;
			pasteToolStripMenuItem.Enabled = richExpression.CanPaste(DataFormats.GetFormat(DataFormats.Text));
			clearToolStripMenuItem.Enabled = richExpression.TextLength > 0;
			selectallToolStripMenuItem.Enabled = richExpression.CanSelect && richExpression.TextLength > 0;
			topmostToolStripMenuItem.Checked = this.TopMost;
		}

		private void resultContextMenuStrip_Opening(object sender, CancelEventArgs e)
		{
			copyresToolStripMenuItem.Enabled = richResult.SelectionLength > 0;
			selectallresToolStripMenuItem.Enabled = richResult.CanSelect && richResult.TextLength > 0;
		}

		private void topmostToolStripMenuItem_CheckedChanged(object sender, EventArgs e)
		{
			this.TopMost = topmostToolStripMenuItem.Checked;
		}

		private void activeopacityToolStripComboBox_SelectedIndexChanged(object sender, EventArgs e)
		{
			activeopacity = 1 - activeopacityToolStripComboBox.SelectedIndex * 0.05f;
			this.Opacity = activeopacity;
		}

		private void inactiveopacityToolStripComboBox_SelectedIndexChanged(object sender, EventArgs e)
		{
			inactiveopacity = 1 - inactiveopacityToolStripComboBox.SelectedIndex * 0.05f;
		}

		private const float FontFactor = 0.75f;

		private void scalebyzoomingToolStripMenuItem_CheckedChanged(object sender, EventArgs e)
		{
			if (scalebyzoomingToolStripMenuItem.Checked)
			{
				richExpression.Font = richResult.Font = new Font(font, 28 * FontFactor, GraphicsUnit.Pixel);
				richExpression.ZoomFactor = richResult.ZoomFactor = (float)richExpression.Height / 28;
			}
			else
			{
				richExpression.Font = richResult.Font = new Font(font, richExpression.Height * FontFactor, GraphicsUnit.Pixel);
				richExpression.ZoomFactor = richResult.ZoomFactor = 1;
			}
		}

		private void fontToolStripComboBox_SelectedIndexChanged(object sender, EventArgs e)
		{
			font = fontToolStripComboBox.Text;
			if (scalebyzoomingToolStripMenuItem.Checked)
			{
				richExpression.Font = richResult.Font = new Font(font, 28 * FontFactor, GraphicsUnit.Pixel);
			}
			else
			{
				richExpression.Font = richResult.Font = new Font(font, richExpression.Height * FontFactor, GraphicsUnit.Pixel);
			}
		}

		private void richExpression_SizeChanged(object sender, EventArgs e)
		{
			if (scalebyzoomingToolStripMenuItem.Checked)
			{
				richExpression.ZoomFactor = (float)richExpression.Height / 28;
			}
			else
			{
				richExpression.Font = new Font(font, richExpression.Height * FontFactor, GraphicsUnit.Pixel);
			}
		}

		private void richResult_SizeChanged(object sender, EventArgs e)
		{
			if (scalebyzoomingToolStripMenuItem.Checked)
			{
				richResult.ZoomFactor = (float)richResult.Height / 28f;
			}
			else
			{
				richResult.Font = new Font(font, richResult.Height * FontFactor, GraphicsUnit.Pixel);
			}
		}

		private void termToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (TermForm.Visible)
			{
				TermForm.Hide();
			}
			else
			{
				Point p = new Point(this.Location.X, this.Location.Y + this.Height);
				TermForm.Location = p;
				TermForm.Show();
				this.Activate();
			}
		}

		private void hotCheckingToolStripMenuItem_CheckedChanged(object sender, EventArgs e)
		{
			if (!hotCheckingToolStripMenuItem.Checked) netcalcErrorProvider.SetError(richExpression, "");
		}

		private void hotcalculatingToolStripMenuItem_CheckedChanged(object sender, EventArgs e)
		{
		}
	}
}