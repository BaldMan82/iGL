namespace iGL.Designer
{
    partial class MainForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.newToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.loadToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.textureConverterToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this._gameObjectsTree = new System.Windows.Forms.TreeView();
            this.propertiesTab = new System.Windows.Forms.TabPage();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.renderTimer = new System.Windows.Forms.Timer(this.components);
            this.tickTimer = new System.Windows.Forms.Timer(this.components);
            this.sceneTree = new System.Windows.Forms.TreeView();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolTranslate = new System.Windows.Forms.ToolStripButton();
            this.toolRotate = new System.Windows.Forms.ToolStripButton();
            this.toolScale = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolPointer = new System.Windows.Forms.ToolStripButton();
            this.toolPan = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.toolPlay = new System.Windows.Forms.ToolStripButton();
            this.toolPause = new System.Windows.Forms.ToolStripButton();
            this.toolStop = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripSnap = new System.Windows.Forms.ToolStripButton();
            this.toolStripStabilize = new System.Windows.Forms.ToolStripButton();
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.saveFileDialog = new System.Windows.Forms.SaveFileDialog();
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.sceneControl = new iGL.Designer.SceneControl();
            this.openTKControl = new iGL.Designer.OpenTKControl();
            this.menuStrip1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.propertiesTab.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.statusStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.toolsToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1335, 24);
            this.menuStrip1.TabIndex = 1;
            this.menuStrip1.Text = "menuStrip1";
            this.menuStrip1.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.menuStrip1_ItemClicked);
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newToolStripMenuItem,
            this.saveToolStripMenuItem,
            this.saveToolStripMenuItem1,
            this.loadToolStripMenuItem,
            this.toolStripSeparator4,
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "&File";
            // 
            // newToolStripMenuItem
            // 
            this.newToolStripMenuItem.Name = "newToolStripMenuItem";
            this.newToolStripMenuItem.Size = new System.Drawing.Size(123, 22);
            this.newToolStripMenuItem.Text = "New";
            this.newToolStripMenuItem.Click += new System.EventHandler(this.newToolStripMenuItem_Click);
            // 
            // saveToolStripMenuItem
            // 
            this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            this.saveToolStripMenuItem.Size = new System.Drawing.Size(123, 22);
            this.saveToolStripMenuItem.Text = "Save";
            this.saveToolStripMenuItem.Click += new System.EventHandler(this.saveToolStripMenuItem_Click);
            // 
            // saveToolStripMenuItem1
            // 
            this.saveToolStripMenuItem1.Name = "saveToolStripMenuItem1";
            this.saveToolStripMenuItem1.Size = new System.Drawing.Size(123, 22);
            this.saveToolStripMenuItem1.Text = "Save As...";
            this.saveToolStripMenuItem1.Click += new System.EventHandler(this.saveToolStripMenuItem1_Click);
            // 
            // loadToolStripMenuItem
            // 
            this.loadToolStripMenuItem.Name = "loadToolStripMenuItem";
            this.loadToolStripMenuItem.Size = new System.Drawing.Size(123, 22);
            this.loadToolStripMenuItem.Text = "Load...";
            this.loadToolStripMenuItem.Click += new System.EventHandler(this.loadToolStripMenuItem_Click);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(120, 6);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(123, 22);
            this.exitToolStripMenuItem.Text = "Exit";
            // 
            // toolsToolStripMenuItem
            // 
            this.toolsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.textureConverterToolStripMenuItem});
            this.toolsToolStripMenuItem.Name = "toolsToolStripMenuItem";
            this.toolsToolStripMenuItem.Size = new System.Drawing.Size(48, 20);
            this.toolsToolStripMenuItem.Text = "Tools";
            // 
            // textureConverterToolStripMenuItem
            // 
            this.textureConverterToolStripMenuItem.Name = "textureConverterToolStripMenuItem";
            this.textureConverterToolStripMenuItem.Size = new System.Drawing.Size(168, 22);
            this.textureConverterToolStripMenuItem.Text = "Texture Converter";
            this.textureConverterToolStripMenuItem.Click += new System.EventHandler(this.textureConverterToolStripMenuItem_Click);
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.propertiesTab);
            this.tabControl1.Location = new System.Drawing.Point(988, 58);
            this.tabControl1.Margin = new System.Windows.Forms.Padding(0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(338, 807);
            this.tabControl1.TabIndex = 2;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this._gameObjectsTree);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(330, 781);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Objects";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // _gameObjectsTree
            // 
            this._gameObjectsTree.AllowDrop = true;
            this._gameObjectsTree.Dock = System.Windows.Forms.DockStyle.Fill;
            this._gameObjectsTree.Location = new System.Drawing.Point(3, 3);
            this._gameObjectsTree.Name = "_gameObjectsTree";
            this._gameObjectsTree.Size = new System.Drawing.Size(324, 775);
            this._gameObjectsTree.TabIndex = 0;
            this._gameObjectsTree.ItemDrag += new System.Windows.Forms.ItemDragEventHandler(this._gameObjectsTree_ItemDrag);
            // 
            // propertiesTab
            // 
            this.propertiesTab.AutoScroll = true;
            this.propertiesTab.Controls.Add(this.flowLayoutPanel1);
            this.propertiesTab.Location = new System.Drawing.Point(4, 22);
            this.propertiesTab.Name = "propertiesTab";
            this.propertiesTab.Size = new System.Drawing.Size(330, 781);
            this.propertiesTab.TabIndex = 1;
            this.propertiesTab.Text = "Properties";
            this.propertiesTab.UseVisualStyleBackColor = true;
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.AutoScroll = true;
            this.flowLayoutPanel1.BackColor = System.Drawing.SystemColors.Control;
            this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.flowLayoutPanel1.Margin = new System.Windows.Forms.Padding(0);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Padding = new System.Windows.Forms.Padding(0, 10, 0, 0);
            this.flowLayoutPanel1.Size = new System.Drawing.Size(330, 781);
            this.flowLayoutPanel1.TabIndex = 0;
            // 
            // renderTimer
            // 
            this.renderTimer.Interval = 1;
            this.renderTimer.Tick += new System.EventHandler(this.renderTimer_Tick);
            // 
            // tickTimer
            // 
            this.tickTimer.Interval = 10;
            this.tickTimer.Tick += new System.EventHandler(this.tickTimer_Tick);
            // 
            // sceneTree
            // 
            this.sceneTree.Location = new System.Drawing.Point(12, 704);
            this.sceneTree.Name = "sceneTree";
            this.sceneTree.Size = new System.Drawing.Size(357, 161);
            this.sceneTree.TabIndex = 3;
            this.sceneTree.BeforeSelect += new System.Windows.Forms.TreeViewCancelEventHandler(this.sceneTree_BeforeSelect);
            this.sceneTree.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.sceneTree_AfterSelect);
            // 
            // toolStrip1
            // 
            this.toolStrip1.AutoSize = false;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolTranslate,
            this.toolRotate,
            this.toolScale,
            this.toolStripSeparator1,
            this.toolPointer,
            this.toolPan,
            this.toolStripSeparator2,
            this.toolPlay,
            this.toolPause,
            this.toolStop,
            this.toolStripSeparator3,
            this.toolStripSnap,
            this.toolStripStabilize});
            this.toolStrip1.Location = new System.Drawing.Point(0, 24);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(1335, 34);
            this.toolStrip1.TabIndex = 5;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // toolTranslate
            // 
            this.toolTranslate.AutoSize = false;
            this.toolTranslate.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolTranslate.Image = ((System.Drawing.Image)(resources.GetObject("toolTranslate.Image")));
            this.toolTranslate.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolTranslate.Name = "toolTranslate";
            this.toolTranslate.Size = new System.Drawing.Size(32, 32);
            this.toolTranslate.Text = "toolStripButton1";
            this.toolTranslate.Click += new System.EventHandler(this.toolTranslate_Click);
            // 
            // toolRotate
            // 
            this.toolRotate.AutoSize = false;
            this.toolRotate.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolRotate.Image = ((System.Drawing.Image)(resources.GetObject("toolRotate.Image")));
            this.toolRotate.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolRotate.Name = "toolRotate";
            this.toolRotate.Size = new System.Drawing.Size(32, 32);
            this.toolRotate.Text = "toolStripButton1";
            this.toolRotate.Click += new System.EventHandler(this.toolRotate_Click);
            // 
            // toolScale
            // 
            this.toolScale.AutoSize = false;
            this.toolScale.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolScale.Image = ((System.Drawing.Image)(resources.GetObject("toolScale.Image")));
            this.toolScale.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolScale.Name = "toolScale";
            this.toolScale.Size = new System.Drawing.Size(32, 32);
            this.toolScale.Text = "toolStripButton1";
            this.toolScale.Click += new System.EventHandler(this.toolScale_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 34);
            // 
            // toolPointer
            // 
            this.toolPointer.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolPointer.Image = ((System.Drawing.Image)(resources.GetObject("toolPointer.Image")));
            this.toolPointer.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.toolPointer.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolPointer.Name = "toolPointer";
            this.toolPointer.Size = new System.Drawing.Size(23, 31);
            this.toolPointer.Text = "toolStripButton1";
            this.toolPointer.Click += new System.EventHandler(this.toolPointer_Click);
            // 
            // toolPan
            // 
            this.toolPan.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolPan.Image = ((System.Drawing.Image)(resources.GetObject("toolPan.Image")));
            this.toolPan.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolPan.Name = "toolPan";
            this.toolPan.Size = new System.Drawing.Size(23, 31);
            this.toolPan.Text = "toolStripButton1";
            this.toolPan.Click += new System.EventHandler(this.toolPan_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 34);
            // 
            // toolPlay
            // 
            this.toolPlay.AutoSize = false;
            this.toolPlay.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolPlay.Image = ((System.Drawing.Image)(resources.GetObject("toolPlay.Image")));
            this.toolPlay.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.toolPlay.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolPlay.Margin = new System.Windows.Forms.Padding(0);
            this.toolPlay.Name = "toolPlay";
            this.toolPlay.Size = new System.Drawing.Size(34, 34);
            this.toolPlay.Text = "toolStripButton1";
            this.toolPlay.Click += new System.EventHandler(this.toolPlay_Click);
            // 
            // toolPause
            // 
            this.toolPause.AutoSize = false;
            this.toolPause.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolPause.Image = ((System.Drawing.Image)(resources.GetObject("toolPause.Image")));
            this.toolPause.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.toolPause.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolPause.Margin = new System.Windows.Forms.Padding(0);
            this.toolPause.Name = "toolPause";
            this.toolPause.Size = new System.Drawing.Size(34, 34);
            this.toolPause.Text = "toolStripButton1";
            this.toolPause.Click += new System.EventHandler(this.toolPause_Click);
            // 
            // toolStop
            // 
            this.toolStop.AutoSize = false;
            this.toolStop.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStop.Image = ((System.Drawing.Image)(resources.GetObject("toolStop.Image")));
            this.toolStop.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.toolStop.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStop.Margin = new System.Windows.Forms.Padding(0);
            this.toolStop.Name = "toolStop";
            this.toolStop.Size = new System.Drawing.Size(34, 34);
            this.toolStop.Text = "toolStripButton1";
            this.toolStop.Click += new System.EventHandler(this.toolStop_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(6, 34);
            // 
            // toolStripSnap
            // 
            this.toolStripSnap.CheckOnClick = true;
            this.toolStripSnap.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripSnap.Image = ((System.Drawing.Image)(resources.GetObject("toolStripSnap.Image")));
            this.toolStripSnap.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripSnap.Name = "toolStripSnap";
            this.toolStripSnap.Size = new System.Drawing.Size(37, 31);
            this.toolStripSnap.Text = "Snap";
            this.toolStripSnap.Click += new System.EventHandler(this.toolStripSnap_Click);
            // 
            // toolStripStabilize
            // 
            this.toolStripStabilize.CheckOnClick = true;
            this.toolStripStabilize.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripStabilize.Image = ((System.Drawing.Image)(resources.GetObject("toolStripStabilize.Image")));
            this.toolStripStabilize.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripStabilize.Name = "toolStripStabilize";
            this.toolStripStabilize.Size = new System.Drawing.Size(76, 31);
            this.toolStripStabilize.Text = "Pre-Stabilize";
            this.toolStripStabilize.Click += new System.EventHandler(this.toolStripStabelize_Click);
            // 
            // statusStrip
            // 
            this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel});
            this.statusStrip.Location = new System.Drawing.Point(0, 871);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.Size = new System.Drawing.Size(1335, 22);
            this.statusStrip.TabIndex = 6;
            this.statusStrip.Text = "statusStrip1";
            // 
            // toolStripStatusLabel
            // 
            this.toolStripStatusLabel.Name = "toolStripStatusLabel";
            this.toolStripStatusLabel.Size = new System.Drawing.Size(0, 17);
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Location = new System.Drawing.Point(8, 8);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(80, 17);
            this.checkBox1.TabIndex = 0;
            this.checkBox1.Text = "checkBox1";
            this.checkBox1.UseVisualStyleBackColor = true;
            // 
            // saveFileDialog
            // 
            this.saveFileDialog.Filter = "Scene|*.igl";
            // 
            // openFileDialog
            // 
            this.openFileDialog.Filter = "Scene|*.igl";
            // 
            // sceneControl
            // 
            this.sceneControl.AutoSize = true;
            this.sceneControl.Location = new System.Drawing.Point(385, 704);
            this.sceneControl.Name = "sceneControl";
            this.sceneControl.Size = new System.Drawing.Size(574, 154);
            this.sceneControl.TabIndex = 8;
            // 
            // openTKControl
            // 
            this.openTKControl.AllowDrop = true;
            this.openTKControl.BackColor = System.Drawing.Color.Black;
            this.openTKControl.EditAxis = null;
            this.openTKControl.Location = new System.Drawing.Point(12, 58);
            this.openTKControl.Name = "openTKControl";
            this.openTKControl.Size = new System.Drawing.Size(960, 640);
            this.openTKControl.Snap = false;
            this.openTKControl.TabIndex = 7;
            this.openTKControl.VSync = false;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1335, 893);
            this.Controls.Add(this.sceneControl);
            this.Controls.Add(this.openTKControl);
            this.Controls.Add(this.statusStrip);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.sceneTree);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.Text = "iGL Scene Editor";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.propertiesTab.ResumeLayout(false);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.statusStrip.ResumeLayout(false);
            this.statusStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TreeView _gameObjectsTree;
        private System.Windows.Forms.TabPage propertiesTab;
        private System.Windows.Forms.Timer renderTimer;
        private System.Windows.Forms.Timer tickTimer;
        private System.Windows.Forms.TreeView sceneTree;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton toolTranslate;
        private System.Windows.Forms.StatusStrip statusStrip;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel;
        private System.Windows.Forms.ToolStripButton toolRotate;
        private System.Windows.Forms.ToolStripButton toolScale;
        private OpenTKControl openTKControl;
        private System.Windows.Forms.ToolStripButton toolPan;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton toolPointer;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripButton toolPlay;
        private System.Windows.Forms.ToolStripButton toolPause;
        private System.Windows.Forms.ToolStripButton toolStop; 
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripButton toolStripSnap;
        private System.Windows.Forms.ToolStripButton toolStripStabilize;
        private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem loadToolStripMenuItem;
        private System.Windows.Forms.SaveFileDialog saveFileDialog;
        private System.Windows.Forms.OpenFileDialog openFileDialog;
        private System.Windows.Forms.ToolStripMenuItem newToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem toolsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem textureConverterToolStripMenuItem;
        private SceneControl sceneControl;
    }
}

