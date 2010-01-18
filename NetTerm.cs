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
			for (int i = e.RowCount; i > 0; i--)
				express.Variables.Insert(e.RowIndex - 1, new Expression.Expression.KeyValuePair<string, double>("", 0));
		}

		private void dataGridViewVars_RowsRemoved(object sender, DataGridViewRowsRemovedEventArgs e)
		{
			for (int i = e.RowCount; i > 0; i--)
				express.Variables.RemoveAt(e.RowIndex);
		}

		private void dataGridViewVars_CellValueChanged(object sender, DataGridViewCellEventArgs e)
		{
			if (e.RowIndex < 0) return;
			if (e.ColumnIndex == 0)
				express.Variables[e.RowIndex].Key = dataGridViewVars[0, e.RowIndex].Value.ToString();
			else
				express.Variables[e.RowIndex].Value = Convert.ToDouble(dataGridViewVars[1, e.RowIndex].Value.ToString().Replace('.', ','));
		}
	}
}