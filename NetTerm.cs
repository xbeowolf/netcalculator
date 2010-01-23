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
	public partial class NetTerm : Form
	{
		/// <summary>
		/// Rich edit control to wich attached term list
		/// </summary>
		private RichTextBox rich;

		/// <summary>
		/// Expression object representation
		/// </summary>
		private Expression.Expression express;

		public NetTerm(RichTextBox r, Expression.Expression e)
		{
			InitializeComponent();
			rich = r;
			express = e;

			// Show given variables
			foreach (Expression.KeyValuePair<string, double> kvp in express.Variables)
			{
				string[][] row = { new string[] { kvp.Key, kvp.Value.ToString() } };
				dataGridViewVars.Rows.Add(row);
			}

			// Add some sample rows
			string[][] rows = {
				new string[] { "c", "299792458" },
				new string[] { "Gconst", "6.67428e-11" },
				new string[] { "phi", "(1 + sqrt 5) / 2" },
			};
			foreach (string[] row in rows)
			{
				express.Variables.Add(new Expression.KeyValuePair<string, double>(row[0], express.Evaluate(row[1])));
				dataGridViewVars.Rows.Add(row);
			}
		}

		public bool SelectTerm(string term)
		{
			ListViewItem lvi = listViewTerm.FindItemWithText(term, false, 0);
			if (lvi != null)
			{
				if (tabControlTerm.SelectedIndex != 0) tabControlTerm.SelectedIndex = 0;
				lvi.Focused = true;
				lvi.Selected = true;
				listViewTerm.EnsureVisible(lvi.Index);
				return true;
			}
			else return false;
		}

		public void InsertSelectedTerm()
		{
			ListView.SelectedListViewItemCollection selected = listViewTerm.SelectedItems;
			foreach (ListViewItem item in selected)
			{
				string str = rich.Text;
				int pos = rich.SelectionStart, start = pos, end = pos;
				for (start = pos; start > 0 && Expression.Expression.isAlphaChar(str[start - 1]); start--) ;
				for (end = pos; end < str.Length && Expression.Expression.isAlphaChar(str[end]); end++) ;
				str = str.Remove(start, end - start);
				str = str.Insert(start, item.Text);
				rich.Text = str;
				rich.SelectionStart = start + item.Text.Length;
				rich.SelectionLength = 0;
			}
		}

		private void detailsToolStripMenuItem_Click(object sender, EventArgs e)
		{
			listViewTerm.View = View.Details;
			smalliconsToolStripMenuItem.Checked = false;
			listToolStripMenuItem.Checked = false;
			tileToolStripMenuItem.Checked = false;
		}

		private void smalliconsToolStripMenuItem_Click(object sender, EventArgs e)
		{
			listViewTerm.View = View.SmallIcon;
			detailsToolStripMenuItem.Checked = false;
			listToolStripMenuItem.Checked = false;
			tileToolStripMenuItem.Checked = false;
		}

		private void listToolStripMenuItem_Click(object sender, EventArgs e)
		{
			listViewTerm.View = View.List;
			detailsToolStripMenuItem.Checked = false;
			smalliconsToolStripMenuItem.Checked = false;
			tileToolStripMenuItem.Checked = false;
		}

		private void tileToolStripMenuItem_Click(object sender, EventArgs e)
		{
			listViewTerm.View = View.Tile;
			detailsToolStripMenuItem.Checked = false;
			smalliconsToolStripMenuItem.Checked = false;
			listToolStripMenuItem.Checked = false;
		}

		private void listViewTerm_MouseDoubleClick(object sender, MouseEventArgs e)
		{
			InsertSelectedTerm();
		}

		private void dataGridViewVars_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
		{
		}

		private void dataGridViewVars_RowsRemoved(object sender, DataGridViewRowsRemovedEventArgs e)
		{
			for (int i = 0; i < e.RowCount; i++)
				express.Variables.RemoveAt(e.RowIndex);
		}

		private void dataGridViewVars_UserAddedRow(object sender, DataGridViewRowEventArgs e)
		{
			express.Variables.Add(new Expression.KeyValuePair<string, double>(
				e.Row.Cells[0].Value != null ? e.Row.Cells[0].Value.ToString() : "",
				express.Evaluate(e.Row.Cells[1].Value != null ? e.Row.Cells[1].Value.ToString() : "0")));
		}

		private void dataGridViewVars_UserDeletedRow(object sender, DataGridViewRowEventArgs e)
		{
		}

		private void dataGridViewVars_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
		{
			// Do not check new line content
			if (e.RowIndex == dataGridViewVars.NewRowIndex) return;

			// Check table entries
			string content = e.FormattedValue.ToString();
			if (e.ColumnIndex == 0) //check identifier name
			{
				if (content.Length > 0 && Expression.Expression.isAlphaChar(content[0]))
				{
					int i;
					for (i = 1; i < content.Length && Expression.Expression.isAlphaNumericChar(content[i]); i++)
					{
					}
					e.Cancel = i < content.Length;
				}
				else e.Cancel = true;
				if (e.Cancel)
				{
					termErrorProvider.SetError(dataGridViewVars, "Invalid identifier name");
				}
				else
				{
					express.Variables[e.RowIndex].Key = content;
					termErrorProvider.SetError(dataGridViewVars, "");
				}
			}
			else // check expression
			{
				try
				{
					express.Variables[e.RowIndex].Value = express.Evaluate(content);
					termErrorProvider.SetError(dataGridViewVars, "");
					e.Cancel = false;
				}
				catch (Exception except)
				{
					termErrorProvider.SetError(dataGridViewVars, except.Message);
					e.Cancel = true;
				}
			}
		}
	}
}