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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MeshComponentDlg));
            this.materialDlg = new iGL.Designer.ComponentDialogs.MaterialDlg();
            this.label1 = new System.Windows.Forms.Label();
            this.comboTexture = new System.Windows.Forms.ComboBox();
            this.lblTexture = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.comboMeshResource = new System.Windows.Forms.ComboBox();
            this.comboNormalMap = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // materialDlg
            // 
            this.materialDlg.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.materialDlg.Location = new System.Drawing.Point(0, 16);
            this.materialDlg.Material = ((iGL.Engine.Material)(resources.GetObject("materialDlg.Material")));
            this.materialDlg.Name = "materialDlg";
            this.materialDlg.Size = new System.Drawing.Size(290, 195);
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
            // comboTexture
            // 
            this.comboTexture.FormattingEnabled = true;
            this.comboTexture.Location = new System.Drawing.Point(105, 228);
            this.comboTexture.Name = "comboTexture";
            this.comboTexture.Size = new System.Drawing.Size(121, 21);
            this.comboTexture.TabIndex = 15;
            this.comboTexture.SelectedIndexChanged += new System.EventHandler(this.comboTexture_SelectedIndexChanged);
            // 
            // lblTexture
            // 
            this.lblTexture.AutoSize = true;
            this.lblTexture.Location = new System.Drawing.Point(5, 228);
            this.lblTexture.Name = "lblTexture";
            this.lblTexture.Size = new System.Drawing.Size(46, 13);
            this.lblTexture.TabIndex = 14;
            this.lblTexture.Text = "Texture:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(5, 285);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(85, 13);
            this.label2.TabIndex = 16;
            this.label2.Text = "Mesh Resource:";
            // 
            // comboMeshResource
            // 
            this.comboMeshResource.FormattingEnabled = true;
            this.comboMeshResource.Location = new System.Drawing.Point(105, 282);
            this.comboMeshResource.Name = "comboMeshResource";
            this.comboMeshResource.Size = new System.Drawing.Size(121, 21);
            this.comboMeshResource.TabIndex = 17;
            this.comboMeshResource.SelectedIndexChanged += new System.EventHandler(this.comboMeshResource_SelectedIndexChanged);
            // 
            // comboNormalMap
            // 
            this.comboNormalMap.FormattingEnabled = true;
            this.comboNormalMap.Location = new System.Drawing.Point(105, 255);
            this.comboNormalMap.Name = "comboNormalMap";
            this.comboNormalMap.Size = new System.Drawing.Size(121, 21);
            this.comboNormalMap.TabIndex = 19;
            this.comboNormalMap.SelectedIndexChanged += new System.EventHandler(this.comboNormalMap_SelectedIndexChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(5, 258);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(66, 13);
            this.label3.TabIndex = 18;
            this.label3.Text = "Normal map:";
            // 
            // MeshComponentDlg
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Controls.Add(this.comboNormalMap);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.comboMeshResource);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.comboTexture);
            this.Controls.Add(this.lblTexture);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.materialDlg);
            this.Name = "MeshComponentDlg";
            this.Size = new System.Drawing.Size(290, 319);
            this.Load += new System.EventHandler(this.MeshComponentDlg_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private MaterialDlg materialDlg;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox comboTexture;
        private System.Windows.Forms.Label lblTexture;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox comboMeshResource;
        private System.Windows.Forms.ComboBox comboNormalMap;
        private System.Windows.Forms.Label label3;
    }
}
