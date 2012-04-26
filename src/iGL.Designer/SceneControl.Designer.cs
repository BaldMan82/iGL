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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SceneControl));
            this.tabScene = new System.Windows.Forms.TabControl();
            this.ObjectPage = new System.Windows.Forms.TabPage();
            this.sceneTree = new System.Windows.Forms.TreeView();
            this.ResourcePage = new System.Windows.Forms.TabPage();
            this.resourceTree = new System.Windows.Forms.TreeView();
            this.resourceToolstrip = new System.Windows.Forms.ToolStrip();
            this.toolStripAdd = new System.Windows.Forms.ToolStripButton();
            this.toolStripAddFont = new System.Windows.Forms.ToolStripButton();
            this.tabScene.SuspendLayout();
            this.ObjectPage.SuspendLayout();
            this.ResourcePage.SuspendLayout();
            this.resourceToolstrip.SuspendLayout();
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
            // 
            // resourceToolstrip
            // 
            this.resourceToolstrip.GripMargin = new System.Windows.Forms.Padding(0);
            this.resourceToolstrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripAdd,
            this.toolStripAddFont});
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
            // toolStripAddFont
            // 
            this.toolStripAddFont.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripAddFont.Image = ((System.Drawing.Image)(resources.GetObject("toolStripAddFont.Image")));
            this.toolStripAddFont.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripAddFont.Name = "toolStripAddFont";
            this.toolStripAddFont.Size = new System.Drawing.Size(23, 22);
            this.toolStripAddFont.Text = "toolStripButton1";
            this.toolStripAddFont.Click += new System.EventHandler(this.toolStripAddFont_Click);
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
        private System.Windows.Forms.ToolStripButton toolStripAddFont;
    }
}
