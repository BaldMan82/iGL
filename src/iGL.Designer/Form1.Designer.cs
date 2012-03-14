namespace iGL.Designer
{
    partial class Form1
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.glControl1 = new OpenTK.GLControl();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this._gameObjectsTree = new System.Windows.Forms.TreeView();
            this.propertiesTab = new System.Windows.Forms.TabPage();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.renderTimer = new System.Windows.Forms.Timer(this.components);
            this.tickTimer = new System.Windows.Forms.Timer(this.components);
            this.sceneTree = new System.Windows.Forms.TreeView();
            this.label1 = new System.Windows.Forms.Label();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolTranslate = new System.Windows.Forms.ToolStripButton();
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolRotate = new System.Windows.Forms.ToolStripButton();
            this.toolScale = new System.Windows.Forms.ToolStripButton();
            this.menuStrip1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.propertiesTab.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.statusStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // glControl1
            // 
            this.glControl1.AllowDrop = true;
            this.glControl1.BackColor = System.Drawing.Color.Black;
            this.glControl1.Location = new System.Drawing.Point(12, 58);
            this.glControl1.Name = "glControl1";
            this.glControl1.Size = new System.Drawing.Size(960, 640);
            this.glControl1.TabIndex = 0;
            this.glControl1.VSync = false;
            this.glControl1.Load += new System.EventHandler(this.glControl1_Load);
            this.glControl1.DragDrop += new System.Windows.Forms.DragEventHandler(this.glControl1_DragDrop);
            this.glControl1.DragEnter += new System.Windows.Forms.DragEventHandler(this.glControl1_DragEnter);
            this.glControl1.Paint += new System.Windows.Forms.PaintEventHandler(this.glControl1_Paint);
            this.glControl1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.glControl1_MouseDown);
            this.glControl1.MouseMove += new System.Windows.Forms.MouseEventHandler(this.glControl1_MouseMove);
            this.glControl1.MouseUp += new System.Windows.Forms.MouseEventHandler(this.glControl1_MouseUp);
            this.glControl1.Resize += new System.EventHandler(this.glControl1_Resize);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1352, 24);
            this.menuStrip1.TabIndex = 1;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "&File";
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.propertiesTab);
            this.tabControl1.Location = new System.Drawing.Point(988, 58);
            this.tabControl1.Margin = new System.Windows.Forms.Padding(0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(341, 403);
            this.tabControl1.TabIndex = 2;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this._gameObjectsTree);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(333, 377);
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
            this._gameObjectsTree.Size = new System.Drawing.Size(327, 371);
            this._gameObjectsTree.TabIndex = 0;
            this._gameObjectsTree.ItemDrag += new System.Windows.Forms.ItemDragEventHandler(this._gameObjectsTree_ItemDrag);
            // 
            // propertiesTab
            // 
            this.propertiesTab.AutoScroll = true;
            this.propertiesTab.Controls.Add(this.flowLayoutPanel1);
            this.propertiesTab.Location = new System.Drawing.Point(4, 22);
            this.propertiesTab.Name = "propertiesTab";
            this.propertiesTab.Size = new System.Drawing.Size(333, 377);
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
            this.flowLayoutPanel1.Size = new System.Drawing.Size(333, 377);
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
            this.sceneTree.Location = new System.Drawing.Point(988, 486);
            this.sceneTree.Name = "sceneTree";
            this.sceneTree.Size = new System.Drawing.Size(341, 212);
            this.sceneTree.TabIndex = 3;
            this.sceneTree.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.sceneTree_AfterSelect);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(985, 470);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(77, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "Scene Objects";
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolTranslate,
            this.toolRotate,
            this.toolScale});
            this.toolStrip1.Location = new System.Drawing.Point(0, 24);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(1352, 25);
            this.toolStrip1.TabIndex = 5;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // toolTranslate
            // 
            this.toolTranslate.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolTranslate.Image = ((System.Drawing.Image)(resources.GetObject("toolTranslate.Image")));
            this.toolTranslate.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolTranslate.Name = "toolTranslate";
            this.toolTranslate.Size = new System.Drawing.Size(23, 22);
            this.toolTranslate.Text = "toolStripButton1";
            this.toolTranslate.Click += new System.EventHandler(this.toolTranslate_Click);
            // 
            // statusStrip
            // 
            this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel});
            this.statusStrip.Location = new System.Drawing.Point(0, 701);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.Size = new System.Drawing.Size(1352, 22);
            this.statusStrip.TabIndex = 6;
            this.statusStrip.Text = "statusStrip1";
            // 
            // toolStripStatusLabel
            // 
            this.toolStripStatusLabel.Name = "toolStripStatusLabel";
            this.toolStripStatusLabel.Size = new System.Drawing.Size(0, 17);
            // 
            // toolRotate
            // 
            this.toolRotate.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolRotate.Image = ((System.Drawing.Image)(resources.GetObject("toolRotate.Image")));
            this.toolRotate.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolRotate.Name = "toolRotate";
            this.toolRotate.Size = new System.Drawing.Size(23, 22);
            this.toolRotate.Text = "toolStripButton1";
            this.toolRotate.Click += new System.EventHandler(this.toolRotate_Click);
            // 
            // toolScale
            // 
            this.toolScale.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolScale.Image = ((System.Drawing.Image)(resources.GetObject("toolScale.Image")));
            this.toolScale.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolScale.Name = "toolScale";
            this.toolScale.Size = new System.Drawing.Size(23, 22);
            this.toolScale.Text = "toolStripButton1";
            this.toolScale.Click += new System.EventHandler(this.toolScale_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1352, 723);
            this.Controls.Add(this.statusStrip);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.sceneTree);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.glControl1);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.MaximizeBox = false;
            this.Name = "Form1";
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

        private OpenTK.GLControl glControl1;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TreeView _gameObjectsTree;
        private System.Windows.Forms.TabPage propertiesTab;
        private System.Windows.Forms.Timer renderTimer;
        private System.Windows.Forms.Timer tickTimer;
        private System.Windows.Forms.TreeView sceneTree;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton toolTranslate;
        private System.Windows.Forms.StatusStrip statusStrip;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel;
        private System.Windows.Forms.ToolStripButton toolRotate;
        private System.Windows.Forms.ToolStripButton toolScale;
    }
}

