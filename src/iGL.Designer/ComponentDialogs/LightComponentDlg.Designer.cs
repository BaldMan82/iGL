namespace iGL.Designer.ComponentDialogs
{
    partial class LightComponentDlg
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
            iGL.Engine.PointLight pointLight1 = new iGL.Engine.PointLight();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LightComponentDlg));
            this.pointLightDlg1 = new iGL.Designer.ComponentDialogs.PointLightDlg();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // pointLightDlg1
            // 
            this.pointLightDlg1.Location = new System.Drawing.Point(-1, 37);
            this.pointLightDlg1.Name = "pointLightDlg1";
            pointLight1.Ambient = ((iGL.Engine.Math.Vector4)(resources.GetObject("pointLight1.Ambient")));
            pointLight1.Diffuse = ((iGL.Engine.Math.Vector4)(resources.GetObject("pointLight1.Diffuse")));
            pointLight1.Specular = ((iGL.Engine.Math.Vector4)(resources.GetObject("pointLight1.Specular")));
            this.pointLightDlg1.PointLight = pointLight1;
            this.pointLightDlg1.Size = new System.Drawing.Size(290, 150);
            this.pointLightDlg1.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label1.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.label1.Location = new System.Drawing.Point(-1, 12);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(290, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Point light";
            // 
            // LightComponentDlg
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Controls.Add(this.label1);
            this.Controls.Add(this.pointLightDlg1);
            this.Name = "LightComponentDlg";
            this.Size = new System.Drawing.Size(288, 204);
            this.ResumeLayout(false);

        }

        #endregion

        private PointLightDlg pointLightDlg1;
        private System.Windows.Forms.Label label1;

    }
}
