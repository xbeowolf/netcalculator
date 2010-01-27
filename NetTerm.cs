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

		/// <summary>
		/// Compose given array of arguments names to single string of arguments
		/// enumerated by comma.
		/// </summary>
		/// <param name="func">Given function array</param>
		/// <returns></returns>
		private string ComposeArgs(ref string[] func)
		{
			string args = "";
			for (int i = 1; i < func.Length; i++)
			{
				args += func[i];
				if (i < func.Length - 1) args += ", ";
			}
			return args;
		}

		/// <summary>
		/// Decompose single string with enumerated by comma arguments to array of
		/// arguments names.
		/// </summary>
		/// <param name="definition">Body of function, it's copy to 1st element of array.</param>
		/// <param name="args">Single string with enumerated arguments.</param>
		/// <param name="func">Result array.</param>
		private void DecomposeArgs(string definition, string args, out string[] func)
		{
			// Make a list of arguments
			List<string> list = new List<string>();
			int curpos = 0;
			Expression.Expression.skipSpace(ref args, ref curpos);
			while (curpos < args.Length)
			{
				if (Expression.Expression.isAlphaChar(args[curpos]))
				{
					int i;
					for (i = curpos + 1; i < args.Length && Expression.Expression.isAlphaNumericChar(args[i]); i++)
					{
					}
					list.Add(args.Substring(curpos, i - curpos));
					curpos = i;
				}
				else throw new ExpressException("Invalid symbol at argument name", curpos, args.Substring(curpos, 1));
				Expression.Expression.skipSpace(ref args, ref curpos);
				if (curpos < args.Length && args[curpos] == ',') curpos++;
				Expression.Expression.skipSpace(ref args, ref curpos);
			}
			// Format the result
			func = new string[1 + list.Count];
			func[0] = definition;
			for (int i = 1; i < func.Length; i++)
				func[i] = list[i - 1];
		}

		private void checkName(ref string name)
		{
			if (name.Length > 0 && Expression.Expression.isAlphaChar(name[0]))
			{
				int i;
				for (i = 1; i < name.Length && Expression.Expression.isAlphaNumericChar(name[i]); i++)
				{
				}
				if (i >= name.Length) return;
			}
			throw new Exception("Invalid identifier name");
		}

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
			string[][] rowsv = {
				new string[] { "c", "299792458" },
				new string[] { "Gconst", "6.67428e-11" },
				new string[] { "phi", "(1 + sqrt 5) / 2" },
			};
			foreach (string[] row in rowsv)
			{
				express.Variables.Add(new Expression.KeyValuePair<string, double>(row[0], express.Evaluate(row[1])));
				dataGridViewVars.Rows.Add(row);
			}

			// Show given functions
			foreach (Expression.KeyValuePair<string, string[]> kvp in express.Functions)
			{
				string[][] row = { new string[] { kvp.Key, ComposeArgs(ref kvp.Value), kvp.Value[0] } };
				dataGridViewVars.Rows.Add(row);
			}
			// Add some sample rows
			string[][] rowsf = {
				new string[] { "PlankMass", "", "sqrt( hbar*299792458/6.6742867e-11 )" },
				new string[] { "Fermat", "a, b, n", "a^n + b^n" },
				new string[] { "Binet", "n", "(phi^n - (-phi)^-n)/(phi^n - (-phi)^-1)" },
			};
			foreach (string[] row in rowsf)
			{
				string[] func;
				DecomposeArgs(row[2], row[1], out func); // No throw with valid predefined data!
				express.Functions.Add(new Expression.KeyValuePair<string, string[]>(row[0], func));
				dataGridViewFuncs.Rows.Add(row);
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

			try
			{
				// Check table entries
				string content = e.FormattedValue.ToString();
				if (e.ColumnIndex == 0) //check identifier name
				{
					checkName(ref content);
					express.Variables[e.RowIndex].Key = content;
				}
				else // check expression
				{
					express.Variables[e.RowIndex].Value = express.Evaluate(content);
				}
				termErrorProvider.SetError(dataGridViewVars, "");
				e.Cancel = false;
			}
			catch (Exception except)
			{
				termErrorProvider.SetError(dataGridViewVars, except.Message);
				e.Cancel = true;
			}
		}

		private void dataGridViewFuncs_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
		{
		}

		private void dataGridViewFuncs_RowsRemoved(object sender, DataGridViewRowsRemovedEventArgs e)
		{
			for (int i = 0; i < e.RowCount; i++)
				express.Functions.RemoveAt(e.RowIndex);
		}

		private void dataGridViewFuncs_UserAddedRow(object sender, DataGridViewRowEventArgs e)
		{
			string[] func;
			DecomposeArgs(
				e.Row.Cells[2].Value != null ? e.Row.Cells[2].Value.ToString() : "",
				e.Row.Cells[1].Value != null ? e.Row.Cells[1].Value.ToString() : "",
				out func);
			express.Functions.Add(new Expression.KeyValuePair<string, string[]>(
				e.Row.Cells[0].Value != null ? e.Row.Cells[0].Value.ToString() : "",
				func));
		}

		private void dataGridViewFuncs_UserDeletedRow(object sender, DataGridViewRowEventArgs e)
		{
		}

		private void dataGridViewFuncs_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
		{
			// Do not check new line content
			if (e.RowIndex == dataGridViewFuncs.NewRowIndex) return;

			try
			{
				// Check table entries
				string content = e.FormattedValue.ToString();
				if (e.ColumnIndex == 0) //check identifier name
				{
					checkName(ref content);
					express.Functions[e.RowIndex].Key = content;
				}
				else if (e.ColumnIndex == 1)
				{
					DecomposeArgs(express.Functions[e.RowIndex].Value[0], content, out express.Functions[e.RowIndex].Value);
				}
				else // check definition
				{
					express.Functions[e.RowIndex].Value[0] = content;
				}
				termErrorProvider.SetError(dataGridViewFuncs, "");
				e.Cancel = false;
			}
			catch (Exception except)
			{
				termErrorProvider.SetError(dataGridViewFuncs, except.Message);
				e.Cancel = true;
			}
		}
	}
}