namespace iGL.Designer.ComponentDialogs
{
    partial class MeshComponentDlg
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
            iGL.Engine.Material material1 = new iGL.Engine.Material();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MeshComponentDlg));
            this.materialDlg = new iGL.Designer.ComponentDialogs.MaterialDlg();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // materialDlg
            // 
            this.materialDlg.Location = new System.Drawing.Point(-1, 16);
            material1.Ambient = ((iGL.Engine.Math.Vector4)(resources.GetObject("material1.Ambient")));
            material1.Diffuse = ((iGL.Engine.Math.Vector4)(resources.GetObject("material1.Diffuse")));
            material1.Shininess = 0F;
            material1.Specular = ((iGL.Engine.Math.Vector4)(resources.GetObject("material1.Specular")));
            this.materialDlg.Material = material1;
            this.materialDlg.Name = "materialDlg";
            this.materialDlg.Size = new System.Drawing.Size(290, 120);
            this.materialDlg.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.label1.Dock = System.Windows.Forms.DockStyle.Top;
            this.label1.Location = new System.Drawing.Point(0, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(290, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Mesh material";
            // 
            // MeshComponentDlg
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Controls.Add(this.label1);
            this.Controls.Add(this.materialDlg);
            this.Name = "MeshComponentDlg";
            this.Size = new System.Drawing.Size(290, 131);
            this.Load += new System.EventHandler(this.MeshComponentDlg_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private MaterialDlg materialDlg;
        private System.Windows.Forms.Label label1;
    }
}
