namespace iGL.Designer
{
    partial class SceneControl
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SceneControl));
            this.tabScene = new System.Windows.Forms.TabControl();
            this.ObjectPage = new System.Windows.Forms.TabPage();
            this.sceneTree = new System.Windows.Forms.TreeView();
            this.ResourcePage = new System.Windows.Forms.TabPage();
            this.resourceTree = new System.Windows.Forms.TreeView();
            this.resourceToolstrip = new System.Windows.Forms.ToolStrip();
            this.toolStripAdd = new System.Windows.Forms.ToolStripButton();
            this.contextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.dropMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.cloneMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.deleteMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.resourceContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.deleteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tabScene.SuspendLayout();
            this.ObjectPage.SuspendLayout();
            this.ResourcePage.SuspendLayout();
            this.resourceToolstrip.SuspendLayout();
            this.contextMenu.SuspendLayout();
            this.resourceContextMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabScene
            // 
            this.tabScene.Controls.Add(this.ObjectPage);
            this.tabScene.Controls.Add(this.ResourcePage);
            this.tabScene.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabScene.Location = new System.Drawing.Point(0, 0);
            this.tabScene.Name = "tabScene";
            this.tabScene.SelectedIndex = 0;
            this.tabScene.Size = new System.Drawing.Size(504, 148);
            this.tabScene.TabIndex = 0;
            // 
            // ObjectPage
            // 
            this.ObjectPage.Controls.Add(this.sceneTree);
            this.ObjectPage.Location = new System.Drawing.Point(4, 22);
            this.ObjectPage.Name = "ObjectPage";
            this.ObjectPage.Padding = new System.Windows.Forms.Padding(3);
            this.ObjectPage.Size = new System.Drawing.Size(496, 122);
            this.ObjectPage.TabIndex = 0;
            this.ObjectPage.Text = "Objects";
            this.ObjectPage.UseVisualStyleBackColor = true;
            // 
            // sceneTree
            // 
            this.sceneTree.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.sceneTree.Dock = System.Windows.Forms.DockStyle.Fill;
            this.sceneTree.Location = new System.Drawing.Point(3, 3);
            this.sceneTree.Name = "sceneTree";
            this.sceneTree.Size = new System.Drawing.Size(490, 116);
            this.sceneTree.TabIndex = 0;
            this.sceneTree.BeforeSelect += new System.Windows.Forms.TreeViewCancelEventHandler(this.sceneTree_BeforeSelect);
            this.sceneTree.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.sceneTree_AfterSelect);
            this.sceneTree.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.sceneTree_NodeMouseClick);
            this.sceneTree.KeyDown += new System.Windows.Forms.KeyEventHandler(this.sceneTree_KeyDown);
            this.sceneTree.MouseDown += new System.Windows.Forms.MouseEventHandler(this.sceneTree_MouseDown);
            // 
            // ResourcePage
            // 
            this.ResourcePage.Controls.Add(this.resourceTree);
            this.ResourcePage.Controls.Add(this.resourceToolstrip);
            this.ResourcePage.Location = new System.Drawing.Point(4, 22);
            this.ResourcePage.Name = "ResourcePage";
            this.ResourcePage.Size = new System.Drawing.Size(496, 122);
            this.ResourcePage.TabIndex = 1;
            this.ResourcePage.Text = "Resources";
            this.ResourcePage.UseVisualStyleBackColor = true;
            // 
            // resourceTree
            // 
            this.resourceTree.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.resourceTree.Dock = System.Windows.Forms.DockStyle.Fill;
            this.resourceTree.Location = new System.Drawing.Point(0, 25);
            this.resourceTree.Name = "resourceTree";
            this.resourceTree.Size = new System.Drawing.Size(496, 97);
            this.resourceTree.TabIndex = 2;
            this.resourceTree.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.resourceTree_NodeMouseClick);
            this.resourceTree.MouseDown += new System.Windows.Forms.MouseEventHandler(this.resourceTree_MouseDown);
            // 
            // resourceToolstrip
            // 
            this.resourceToolstrip.GripMargin = new System.Windows.Forms.Padding(0);
            this.resourceToolstrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripAdd});
            this.resourceToolstrip.Location = new System.Drawing.Point(0, 0);
            this.resourceToolstrip.Name = "resourceToolstrip";
            this.resourceToolstrip.Size = new System.Drawing.Size(496, 25);
            this.resourceToolstrip.TabIndex = 1;
            this.resourceToolstrip.Text = "toolStrip1";
            // 
            // toolStripAdd
            // 
            this.toolStripAdd.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripAdd.Image = ((System.Drawing.Image)(resources.GetObject("toolStripAdd.Image")));
            this.toolStripAdd.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripAdd.Name = "toolStripAdd";
            this.toolStripAdd.Size = new System.Drawing.Size(23, 22);
            this.toolStripAdd.Text = "toolStripButton1";
            this.toolStripAdd.Click += new System.EventHandler(this.toolStripAdd_Click);
            // 
            // contextMenu
            // 
            this.contextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.dropMenuItem,
            this.cloneMenuItem,
            this.deleteMenuItem});
            this.contextMenu.Name = "contextMenu";
            this.contextMenu.Size = new System.Drawing.Size(108, 70);
            // 
            // dropMenuItem
            // 
            this.dropMenuItem.Name = "dropMenuItem";
            this.dropMenuItem.Size = new System.Drawing.Size(107, 22);
            this.dropMenuItem.Text = "Drop";
            // 
            // cloneMenuItem
            // 
            this.cloneMenuItem.Name = "cloneMenuItem";
            this.cloneMenuItem.Size = new System.Drawing.Size(107, 22);
            this.cloneMenuItem.Text = "Clone";
            this.cloneMenuItem.Click += new System.EventHandler(this.cloneMenuItem_Click);
            // 
            // deleteMenuItem
            // 
            this.deleteMenuItem.Name = "deleteMenuItem";
            this.deleteMenuItem.Size = new System.Drawing.Size(107, 22);
            this.deleteMenuItem.Text = "Delete";
            this.deleteMenuItem.Click += new System.EventHandler(this.deleteMenuItem_Click);
            // 
            // resourceContextMenu
            // 
            this.resourceContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.deleteToolStripMenuItem});
            this.resourceContextMenu.Name = "resourceContextMenu";
            this.resourceContextMenu.Size = new System.Drawing.Size(108, 26);
            // 
            // deleteToolStripMenuItem
            // 
            this.deleteToolStripMenuItem.Name = "deleteToolStripMenuItem";
            this.deleteToolStripMenuItem.Size = new System.Drawing.Size(107, 22);
            this.deleteToolStripMenuItem.Text = "Delete";
            this.deleteToolStripMenuItem.Click += new System.EventHandler(this.deleteToolStripMenuItem_Click);
            // 
            // SceneControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.Controls.Add(this.tabScene);
            this.Name = "SceneControl";
            this.Size = new System.Drawing.Size(504, 148);
            this.tabScene.ResumeLayout(false);
            this.ObjectPage.ResumeLayout(false);
            this.ResourcePage.ResumeLayout(false);
            this.ResourcePage.PerformLayout();
            this.resourceToolstrip.ResumeLayout(false);
            this.resourceToolstrip.PerformLayout();
            this.contextMenu.ResumeLayout(false);
            this.resourceContextMenu.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabScene;
        private System.Windows.Forms.TabPage ObjectPage;
        private System.Windows.Forms.TreeView sceneTree;
        private System.Windows.Forms.TabPage ResourcePage;
        private System.Windows.Forms.ToolStrip resourceToolstrip;
        private System.Windows.Forms.ToolStripButton toolStripAdd;
        private System.Windows.Forms.TreeView resourceTree;
        private System.Windows.Forms.ContextMenuStrip contextMenu;
        private System.Windows.Forms.ToolStripMenuItem dropMenuItem;
        private System.Windows.Forms.ToolStripMenuItem cloneMenuItem;
        private System.Windows.Forms.ToolStripMenuItem deleteMenuItem;
        private System.Windows.Forms.ContextMenuStrip resourceContextMenu;
        private System.Windows.Forms.ToolStripMenuItem deleteToolStripMenuItem;
    }
}
