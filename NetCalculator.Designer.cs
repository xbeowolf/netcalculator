namespace NetCalculator
{
    partial class NetCalculator
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
					this.components = new System.ComponentModel.Container();
					System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(NetCalculator));
					this.richExpression = new System.Windows.Forms.RichTextBox();
					this.expressContextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
					this.evaluateToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
					this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
					this.cutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
					this.copyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
					this.pasteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
					this.clearToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
					this.selectallToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
					this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
					this.interfaceToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
					this.topmostToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
					this.activeopacityToolStripComboBox = new System.Windows.Forms.ToolStripComboBox();
					this.inactiveopacityToolStripComboBox = new System.Windows.Forms.ToolStripComboBox();
					this.transparencyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
					this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
					this.scalebyzoomingToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
					this.fontToolStripComboBox = new System.Windows.Forms.ToolStripComboBox();
					this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
					this.hotCheckingToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
					this.hotcalculatingToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
					this.termToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
					this.richResult = new System.Windows.Forms.RichTextBox();
					this.resultContextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
					this.copyresToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
					this.selectallresToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
					this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
					this.hexadecimalToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
					this.toolTipContext = new System.Windows.Forms.ToolTip(this.components);
					this.calcSplitContainer = new System.Windows.Forms.SplitContainer();
					this.netcalcErrorProvider = new System.Windows.Forms.ErrorProvider(this.components);
					this.expressContextMenuStrip.SuspendLayout();
					this.resultContextMenuStrip.SuspendLayout();
					this.calcSplitContainer.Panel1.SuspendLayout();
					this.calcSplitContainer.Panel2.SuspendLayout();
					this.calcSplitContainer.SuspendLayout();
					((System.ComponentModel.ISupportInitialize)(this.netcalcErrorProvider)).BeginInit();
					this.SuspendLayout();
					// 
					// richExpression
					// 
					this.richExpression.ContextMenuStrip = this.expressContextMenuStrip;
					this.richExpression.DetectUrls = false;
					this.richExpression.Dock = System.Windows.Forms.DockStyle.Fill;
					this.richExpression.EnableAutoDragDrop = true;
					this.richExpression.Font = new System.Drawing.Font("Arial", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
					this.richExpression.HideSelection = false;
					this.netcalcErrorProvider.SetIconPadding(this.richExpression, -16);
					this.richExpression.Location = new System.Drawing.Point(0, 0);
					this.richExpression.Multiline = false;
					this.richExpression.Name = "richExpression";
					this.richExpression.Size = new System.Drawing.Size(259, 28);
					this.richExpression.TabIndex = 1;
					this.richExpression.Text = "";
					this.toolTipContext.SetToolTip(this.richExpression, "Type here any mathematical expression and press \"Enter\" key to calculate.\r\nExpres" +
									"sion can be with all mathematical and logical operators, brackets,\r\nfunctions an" +
									"d common constants.");
					this.richExpression.SelectionChanged += new System.EventHandler(this.richExpression_SelectionChanged);
					this.richExpression.SizeChanged += new System.EventHandler(this.richExpression_SizeChanged);
					this.richExpression.Enter += new System.EventHandler(this.EvTextChangedExpress);
					this.richExpression.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.EvKeyPressExpress);
					this.richExpression.TextChanged += new System.EventHandler(this.EvTextChangedExpress);
					// 
					// expressContextMenuStrip
					// 
					this.expressContextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.evaluateToolStripMenuItem,
            this.toolStripSeparator1,
            this.cutToolStripMenuItem,
            this.copyToolStripMenuItem,
            this.pasteToolStripMenuItem,
            this.clearToolStripMenuItem,
            this.selectallToolStripMenuItem,
            this.toolStripSeparator2,
            this.interfaceToolStripMenuItem,
            this.termToolStripMenuItem});
					this.expressContextMenuStrip.Name = "expressContextMenuStrip";
					this.expressContextMenuStrip.Size = new System.Drawing.Size(186, 192);
					this.expressContextMenuStrip.Opening += new System.ComponentModel.CancelEventHandler(this.contextMenuStripExpress_Opening);
					// 
					// evaluateToolStripMenuItem
					// 
					this.evaluateToolStripMenuItem.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
					this.evaluateToolStripMenuItem.ForeColor = System.Drawing.SystemColors.HotTrack;
					this.evaluateToolStripMenuItem.Name = "evaluateToolStripMenuItem";
					this.evaluateToolStripMenuItem.ShortcutKeyDisplayString = "";
					this.evaluateToolStripMenuItem.Size = new System.Drawing.Size(185, 22);
					this.evaluateToolStripMenuItem.Text = "&Evaluate";
					this.evaluateToolStripMenuItem.ToolTipText = "Calculate expression at left part and put result at right";
					this.evaluateToolStripMenuItem.Click += new System.EventHandler(this.evaluateToolStripMenuItem_Click);
					// 
					// toolStripSeparator1
					// 
					this.toolStripSeparator1.Name = "toolStripSeparator1";
					this.toolStripSeparator1.Size = new System.Drawing.Size(182, 6);
					// 
					// cutToolStripMenuItem
					// 
					this.cutToolStripMenuItem.ForeColor = System.Drawing.SystemColors.ControlText;
					this.cutToolStripMenuItem.Name = "cutToolStripMenuItem";
					this.cutToolStripMenuItem.Size = new System.Drawing.Size(185, 22);
					this.cutToolStripMenuItem.Text = "Cu&t";
					this.cutToolStripMenuItem.ToolTipText = "Cut selection into Windows clipboard";
					this.cutToolStripMenuItem.Click += new System.EventHandler(this.cutToolStripMenuItem_Click);
					// 
					// copyToolStripMenuItem
					// 
					this.copyToolStripMenuItem.Name = "copyToolStripMenuItem";
					this.copyToolStripMenuItem.Size = new System.Drawing.Size(185, 22);
					this.copyToolStripMenuItem.Text = "&Copy";
					this.copyToolStripMenuItem.ToolTipText = "Copy selection into Windows clipboard";
					this.copyToolStripMenuItem.Click += new System.EventHandler(this.copyToolStripMenuItem_Click);
					// 
					// pasteToolStripMenuItem
					// 
					this.pasteToolStripMenuItem.Name = "pasteToolStripMenuItem";
					this.pasteToolStripMenuItem.Size = new System.Drawing.Size(185, 22);
					this.pasteToolStripMenuItem.Text = "&Paste";
					this.pasteToolStripMenuItem.ToolTipText = "Paste text from Windows clipboard";
					this.pasteToolStripMenuItem.Click += new System.EventHandler(this.pasteToolStripMenuItem_Click);
					// 
					// clearToolStripMenuItem
					// 
					this.clearToolStripMenuItem.Name = "clearToolStripMenuItem";
					this.clearToolStripMenuItem.Size = new System.Drawing.Size(185, 22);
					this.clearToolStripMenuItem.Text = "C&lear";
					this.clearToolStripMenuItem.ToolTipText = "Erase content of left part";
					// 
					// selectallToolStripMenuItem
					// 
					this.selectallToolStripMenuItem.Name = "selectallToolStripMenuItem";
					this.selectallToolStripMenuItem.Size = new System.Drawing.Size(185, 22);
					this.selectallToolStripMenuItem.Text = "Select &All";
					this.selectallToolStripMenuItem.ToolTipText = "Select all content of left part";
					this.selectallToolStripMenuItem.Click += new System.EventHandler(this.selectallToolStripMenuItem_Click);
					// 
					// toolStripSeparator2
					// 
					this.toolStripSeparator2.Name = "toolStripSeparator2";
					this.toolStripSeparator2.Size = new System.Drawing.Size(182, 6);
					// 
					// interfaceToolStripMenuItem
					// 
					this.interfaceToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.topmostToolStripMenuItem,
            this.activeopacityToolStripComboBox,
            this.inactiveopacityToolStripComboBox,
            this.transparencyToolStripMenuItem,
            this.toolStripSeparator3,
            this.scalebyzoomingToolStripMenuItem,
            this.fontToolStripComboBox,
            this.toolStripSeparator4,
            this.hotCheckingToolStripMenuItem,
            this.hotcalculatingToolStripMenuItem});
					this.interfaceToolStripMenuItem.Name = "interfaceToolStripMenuItem";
					this.interfaceToolStripMenuItem.Size = new System.Drawing.Size(185, 22);
					this.interfaceToolStripMenuItem.Text = "Interface";
					this.interfaceToolStripMenuItem.ToolTipText = "Interface setting";
					// 
					// topmostToolStripMenuItem
					// 
					this.topmostToolStripMenuItem.Checked = true;
					this.topmostToolStripMenuItem.CheckOnClick = true;
					this.topmostToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
					this.topmostToolStripMenuItem.Name = "topmostToolStripMenuItem";
					this.topmostToolStripMenuItem.Size = new System.Drawing.Size(181, 22);
					this.topmostToolStripMenuItem.Text = "Always on &top";
					this.topmostToolStripMenuItem.ToolTipText = "Puts window always on top if it\'s checked";
					this.topmostToolStripMenuItem.CheckedChanged += new System.EventHandler(this.topmostToolStripMenuItem_CheckedChanged);
					// 
					// activeopacityToolStripComboBox
					// 
					this.activeopacityToolStripComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
					this.activeopacityToolStripComboBox.Items.AddRange(new object[] {
            "100% active opacity",
            "95% active opacity",
            "90% active opacity",
            "85% active opacity",
            "80% active opacity",
            "75% active opacity",
            "70% active opacity",
            "65% active opacity",
            "60% active opacity",
            "55% active opacity",
            "50% active opacity",
            "45% active opacity",
            "40% active opacity",
            "35% active opacity",
            "30% active opacity",
            "25% active opacity",
            "20% active opacity"});
					this.activeopacityToolStripComboBox.Name = "activeopacityToolStripComboBox";
					this.activeopacityToolStripComboBox.Size = new System.Drawing.Size(121, 21);
					this.activeopacityToolStripComboBox.ToolTipText = "Opacity for active state";
					this.activeopacityToolStripComboBox.SelectedIndexChanged += new System.EventHandler(this.activeopacityToolStripComboBox_SelectedIndexChanged);
					// 
					// inactiveopacityToolStripComboBox
					// 
					this.inactiveopacityToolStripComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
					this.inactiveopacityToolStripComboBox.Items.AddRange(new object[] {
            "100% inactive opacity",
            "95% inactive opacity",
            "90% inactive opacity",
            "85% inactive opacity",
            "80% inactive opacity",
            "75% inactive opacity",
            "70% inactive opacity",
            "65% inactive opacity",
            "60% inactive opacity",
            "55% inactive opacity",
            "50% inactive opacity",
            "45% inactive opacity",
            "40% inactive opacity",
            "35% inactive opacity",
            "30% inactive opacity",
            "25% inactive opacity",
            "20% inactive opacity"});
					this.inactiveopacityToolStripComboBox.Name = "inactiveopacityToolStripComboBox";
					this.inactiveopacityToolStripComboBox.Size = new System.Drawing.Size(121, 21);
					this.inactiveopacityToolStripComboBox.ToolTipText = "Opacity for inactive state";
					this.inactiveopacityToolStripComboBox.SelectedIndexChanged += new System.EventHandler(this.inactiveopacityToolStripComboBox_SelectedIndexChanged);
					// 
					// transparencyToolStripMenuItem
					// 
					this.transparencyToolStripMenuItem.Checked = true;
					this.transparencyToolStripMenuItem.CheckOnClick = true;
					this.transparencyToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
					this.transparencyToolStripMenuItem.Name = "transparencyToolStripMenuItem";
					this.transparencyToolStripMenuItem.Size = new System.Drawing.Size(181, 22);
					this.transparencyToolStripMenuItem.Text = "Inactive transparency";
					this.transparencyToolStripMenuItem.ToolTipText = "Make transparant controls background at inactive state";
					// 
					// toolStripSeparator3
					// 
					this.toolStripSeparator3.Name = "toolStripSeparator3";
					this.toolStripSeparator3.Size = new System.Drawing.Size(178, 6);
					// 
					// scalebyzoomingToolStripMenuItem
					// 
					this.scalebyzoomingToolStripMenuItem.Checked = true;
					this.scalebyzoomingToolStripMenuItem.CheckOnClick = true;
					this.scalebyzoomingToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
					this.scalebyzoomingToolStripMenuItem.Name = "scalebyzoomingToolStripMenuItem";
					this.scalebyzoomingToolStripMenuItem.Size = new System.Drawing.Size(181, 22);
					this.scalebyzoomingToolStripMenuItem.Text = "Scale by &zooming";
					this.scalebyzoomingToolStripMenuItem.ToolTipText = "On checked state indicates for scaling controls content by zooming,\r\non unchecked" +
							" indicates for scaling content by font resizing.\r\nIt\'s has a mean for rich text " +
							"copy-paste operations.";
					this.scalebyzoomingToolStripMenuItem.CheckedChanged += new System.EventHandler(this.scalebyzoomingToolStripMenuItem_CheckedChanged);
					// 
					// fontToolStripComboBox
					// 
					this.fontToolStripComboBox.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
					this.fontToolStripComboBox.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
					this.fontToolStripComboBox.BackColor = System.Drawing.SystemColors.Window;
					this.fontToolStripComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
					this.fontToolStripComboBox.Items.AddRange(new object[] {
            "Arial",
            "Arial Black",
            "Arial Narrow",
            "Book Antiqua",
            "Bookman Old Style",
            "Century Gothic",
            "Colonna MT",
            "Comic Sans MS",
            "Courier",
            "Courier New",
            "Fixedsys",
            "Garamond",
            "Georgia",
            "Haettenschweiler",
            "Impact",
            "Lucida Console",
            "Lucida Sans",
            "Modern",
            "MS Dialog",
            "MS Dialog Light",
            "MS Sans Serif",
            "MS Serif",
            "System",
            "Tahoma",
            "Terminal",
            "Times New Roman"});
					this.fontToolStripComboBox.MaxLength = 32;
					this.fontToolStripComboBox.Name = "fontToolStripComboBox";
					this.fontToolStripComboBox.Size = new System.Drawing.Size(121, 21);
					this.fontToolStripComboBox.Sorted = true;
					this.fontToolStripComboBox.ToolTipText = "Font facename";
					this.fontToolStripComboBox.SelectedIndexChanged += new System.EventHandler(this.fontToolStripComboBox_SelectedIndexChanged);
					// 
					// toolStripSeparator4
					// 
					this.toolStripSeparator4.Name = "toolStripSeparator4";
					this.toolStripSeparator4.Size = new System.Drawing.Size(178, 6);
					// 
					// hotCheckingToolStripMenuItem
					// 
					this.hotCheckingToolStripMenuItem.Checked = true;
					this.hotCheckingToolStripMenuItem.CheckOnClick = true;
					this.hotCheckingToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
					this.hotCheckingToolStripMenuItem.Name = "hotCheckingToolStripMenuItem";
					this.hotCheckingToolStripMenuItem.Size = new System.Drawing.Size(181, 22);
					this.hotCheckingToolStripMenuItem.Text = "Hot c&hecking";
					this.hotCheckingToolStripMenuItem.ToolTipText = "Expression checking on text changes";
					this.hotCheckingToolStripMenuItem.CheckedChanged += new System.EventHandler(this.hotCheckingToolStripMenuItem_CheckedChanged);
					// 
					// hotcalculatingToolStripMenuItem
					// 
					this.hotcalculatingToolStripMenuItem.CheckOnClick = true;
					this.hotcalculatingToolStripMenuItem.Name = "hotcalculatingToolStripMenuItem";
					this.hotcalculatingToolStripMenuItem.Size = new System.Drawing.Size(181, 22);
					this.hotcalculatingToolStripMenuItem.Text = "Hot &calculating";
					this.hotcalculatingToolStripMenuItem.ToolTipText = "Expression calculating on text changes";
					this.hotcalculatingToolStripMenuItem.CheckedChanged += new System.EventHandler(this.hotcalculatingToolStripMenuItem_CheckedChanged);
					// 
					// termToolStripMenuItem
					// 
					this.termToolStripMenuItem.Name = "termToolStripMenuItem";
					this.termToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.J)));
					this.termToolStripMenuItem.Size = new System.Drawing.Size(185, 22);
					this.termToolStripMenuItem.Text = "&Built-in lexemes";
					this.termToolStripMenuItem.ToolTipText = "Show or hide pop up window with builtin names of\r\nconstants and functions with co" +
							"mpact description";
					this.termToolStripMenuItem.Click += new System.EventHandler(this.termToolStripMenuItem_Click);
					// 
					// richResult
					// 
					this.richResult.BackColor = System.Drawing.SystemColors.Window;
					this.richResult.ContextMenuStrip = this.resultContextMenuStrip;
					this.richResult.DetectUrls = false;
					this.richResult.Dock = System.Windows.Forms.DockStyle.Fill;
					this.richResult.EnableAutoDragDrop = true;
					this.richResult.Font = new System.Drawing.Font("Arial", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
					this.richResult.HideSelection = false;
					this.netcalcErrorProvider.SetIconPadding(this.richResult, -16);
					this.richResult.Location = new System.Drawing.Point(0, 0);
					this.richResult.MaxLength = 32;
					this.richResult.Multiline = false;
					this.richResult.Name = "richResult";
					this.richResult.ReadOnly = true;
					this.richResult.Size = new System.Drawing.Size(129, 28);
					this.richResult.TabIndex = 2;
					this.richResult.Text = "";
					this.toolTipContext.SetToolTip(this.richResult, "Result of calculation. You can copy text from this window for your use.");
					this.richResult.SizeChanged += new System.EventHandler(this.richResult_SizeChanged);
					// 
					// resultContextMenuStrip
					// 
					this.resultContextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.copyresToolStripMenuItem,
            this.selectallresToolStripMenuItem,
            this.toolStripSeparator5,
            this.hexadecimalToolStripMenuItem});
					this.resultContextMenuStrip.Name = "resultContextMenuStrip";
					this.resultContextMenuStrip.Size = new System.Drawing.Size(135, 76);
					this.resultContextMenuStrip.Opening += new System.ComponentModel.CancelEventHandler(this.resultContextMenuStrip_Opening);
					// 
					// copyresToolStripMenuItem
					// 
					this.copyresToolStripMenuItem.Name = "copyresToolStripMenuItem";
					this.copyresToolStripMenuItem.Size = new System.Drawing.Size(134, 22);
					this.copyresToolStripMenuItem.Text = "&Copy";
					this.copyresToolStripMenuItem.ToolTipText = "Copy selection into Windows clipboard";
					this.copyresToolStripMenuItem.Click += new System.EventHandler(this.copyresToolStripMenuItem_Click);
					// 
					// selectallresToolStripMenuItem
					// 
					this.selectallresToolStripMenuItem.Name = "selectallresToolStripMenuItem";
					this.selectallresToolStripMenuItem.Size = new System.Drawing.Size(134, 22);
					this.selectallresToolStripMenuItem.Text = "Select &All";
					this.selectallresToolStripMenuItem.ToolTipText = "Select all content of right part";
					this.selectallresToolStripMenuItem.Click += new System.EventHandler(this.selectallresToolStripMenuItem_Click);
					// 
					// toolStripSeparator5
					// 
					this.toolStripSeparator5.Name = "toolStripSeparator5";
					this.toolStripSeparator5.Size = new System.Drawing.Size(131, 6);
					// 
					// hexadecimalToolStripMenuItem
					// 
					this.hexadecimalToolStripMenuItem.CheckOnClick = true;
					this.hexadecimalToolStripMenuItem.Name = "hexadecimalToolStripMenuItem";
					this.hexadecimalToolStripMenuItem.Size = new System.Drawing.Size(134, 22);
					this.hexadecimalToolStripMenuItem.Text = "&Hexadecimal";
					this.hexadecimalToolStripMenuItem.ToolTipText = "Displays result in hexadecimal format if checked";
					this.hexadecimalToolStripMenuItem.CheckedChanged += new System.EventHandler(this.hexadecimalToolStripMenuItem_CheckedChanged);
					// 
					// toolTipContext
					// 
					this.toolTipContext.AutoPopDelay = 12000;
					this.toolTipContext.InitialDelay = 500;
					this.toolTipContext.IsBalloon = true;
					this.toolTipContext.ReshowDelay = 100;
					this.toolTipContext.ShowAlways = true;
					this.toolTipContext.ToolTipIcon = System.Windows.Forms.ToolTipIcon.Info;
					this.toolTipContext.ToolTipTitle = "This control is for...";
					// 
					// calcSplitContainer
					// 
					this.calcSplitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
					this.calcSplitContainer.Location = new System.Drawing.Point(0, 0);
					this.calcSplitContainer.Name = "calcSplitContainer";
					// 
					// calcSplitContainer.Panel1
					// 
					this.calcSplitContainer.Panel1.Controls.Add(this.richExpression);
					this.calcSplitContainer.Panel1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
					this.calcSplitContainer.Panel1MinSize = 60;
					// 
					// calcSplitContainer.Panel2
					// 
					this.calcSplitContainer.Panel2.Controls.Add(this.richResult);
					this.calcSplitContainer.Panel2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
					this.calcSplitContainer.Panel2MinSize = 60;
					this.calcSplitContainer.Size = new System.Drawing.Size(392, 28);
					this.calcSplitContainer.SplitterDistance = 259;
					this.calcSplitContainer.TabIndex = 3;
					this.toolTipContext.SetToolTip(this.calcSplitContainer, "Drag this bar to resize controls");
					// 
					// netcalcErrorProvider
					// 
					this.netcalcErrorProvider.ContainerControl = this.calcSplitContainer;
					// 
					// NetCalculator
					// 
					this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
					this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
					this.ClientSize = new System.Drawing.Size(392, 28);
					this.Controls.Add(this.calcSplitContainer);
					this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
					this.HelpButton = true;
					this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
					this.MaximizeBox = false;
					this.MaximumSize = new System.Drawing.Size(1024, 128);
					this.MinimumSize = new System.Drawing.Size(256, 44);
					this.Name = "NetCalculator";
					this.Opacity = 0.5;
					this.Text = "Calculator#";
					this.TopMost = true;
					this.TransparencyKey = System.Drawing.SystemColors.Window;
					this.Deactivate += new System.EventHandler(this.NetCalculator_Deactivate);
					this.Activated += new System.EventHandler(this.NetCalculator_Activated);
					this.Move += new System.EventHandler(this.NetCalculator_Move);
					this.Resize += new System.EventHandler(this.NetCalculator_Resize);
					this.expressContextMenuStrip.ResumeLayout(false);
					this.resultContextMenuStrip.ResumeLayout(false);
					this.calcSplitContainer.Panel1.ResumeLayout(false);
					this.calcSplitContainer.Panel2.ResumeLayout(false);
					this.calcSplitContainer.ResumeLayout(false);
					((System.ComponentModel.ISupportInitialize)(this.netcalcErrorProvider)).EndInit();
					this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.RichTextBox richExpression;
        private System.Windows.Forms.RichTextBox richResult;
        private System.Windows.Forms.ToolTip toolTipContext;
        private System.Windows.Forms.ContextMenuStrip expressContextMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem cutToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem copyToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem pasteToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem clearToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem selectallToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem evaluateToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.SplitContainer calcSplitContainer;
        private System.Windows.Forms.ContextMenuStrip resultContextMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem copyresToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem selectallresToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem termToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem interfaceToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem topmostToolStripMenuItem;
        private System.Windows.Forms.ToolStripComboBox fontToolStripComboBox;
        private System.Windows.Forms.ToolStripComboBox activeopacityToolStripComboBox;
        private System.Windows.Forms.ToolStripComboBox inactiveopacityToolStripComboBox;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripMenuItem scalebyzoomingToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem transparencyToolStripMenuItem;
        private System.Windows.Forms.ErrorProvider netcalcErrorProvider;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripMenuItem hotCheckingToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem hotcalculatingToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
        private System.Windows.Forms.ToolStripMenuItem hexadecimalToolStripMenuItem;
    }
}

