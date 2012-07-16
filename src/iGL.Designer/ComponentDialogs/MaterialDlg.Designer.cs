namespace iGL.Designer.ComponentDialogs
{
    partial class MaterialDlg
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
            this.label1 = new System.Windows.Forms.Label();
            this.colorDialog = new System.Windows.Forms.ColorDialog();
            this.pnlAmbient = new System.Windows.Forms.Panel();
            this.btnAmbient = new System.Windows.Forms.Button();
            this.btnDiffuse = new System.Windows.Forms.Button();
            this.pnlDiffuse = new System.Windows.Forms.Panel();
            this.label2 = new System.Windows.Forms.Label();
            this.btnSpecular = new System.Windows.Forms.Button();
            this.pnlSpecular = new System.Windows.Forms.Panel();
            this.label3 = new System.Windows.Forms.Label();
            this.txtAmbientAlpha = new System.Windows.Forms.TextBox();
            this.txtDiffuseAlpha = new System.Windows.Forms.TextBox();
            this.txtSpecularAlpha = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.cbTilingRepeatX = new System.Windows.Forms.CheckBox();
            this.cbTilingRepeatY = new System.Windows.Forms.CheckBox();
            this.txtTilingX = new System.Windows.Forms.TextBox();
            this.txtTilingY = new System.Windows.Forms.TextBox();
            this.txtShininess = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 18);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(45, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Ambient";
            // 
            // pnlAmbient
            // 
            this.pnlAmbient.Location = new System.Drawing.Point(67, 13);
            this.pnlAmbient.Name = "pnlAmbient";
            this.pnlAmbient.Size = new System.Drawing.Size(49, 23);
            this.pnlAmbient.TabIndex = 1;
            // 
            // btnAmbient
            // 
            this.btnAmbient.Location = new System.Drawing.Point(122, 13);
            this.btnAmbient.Name = "btnAmbient";
            this.btnAmbient.Size = new System.Drawing.Size(75, 23);
            this.btnAmbient.TabIndex = 2;
            this.btnAmbient.Text = "Select";
            this.btnAmbient.UseVisualStyleBackColor = true;
            this.btnAmbient.Click += new System.EventHandler(this.btnAmbient_Click);
            // 
            // btnDiffuse
            // 
            this.btnDiffuse.Location = new System.Drawing.Point(122, 42);
            this.btnDiffuse.Name = "btnDiffuse";
            this.btnDiffuse.Size = new System.Drawing.Size(75, 23);
            this.btnDiffuse.TabIndex = 5;
            this.btnDiffuse.Text = "Select";
            this.btnDiffuse.UseVisualStyleBackColor = true;
            this.btnDiffuse.Click += new System.EventHandler(this.btnDiffuse_Click);
            // 
            // pnlDiffuse
            // 
            this.pnlDiffuse.Location = new System.Drawing.Point(67, 42);
            this.pnlDiffuse.Name = "pnlDiffuse";
            this.pnlDiffuse.Size = new System.Drawing.Size(49, 23);
            this.pnlDiffuse.TabIndex = 4;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(3, 47);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(40, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Diffuse";
            // 
            // btnSpecular
            // 
            this.btnSpecular.Location = new System.Drawing.Point(122, 71);
            this.btnSpecular.Name = "btnSpecular";
            this.btnSpecular.Size = new System.Drawing.Size(75, 23);
            this.btnSpecular.TabIndex = 5;
            this.btnSpecular.Text = "Select";
            this.btnSpecular.UseVisualStyleBackColor = true;
            this.btnSpecular.Click += new System.EventHandler(this.btnSpecular_Click);
            // 
            // pnlSpecular
            // 
            this.pnlSpecular.Location = new System.Drawing.Point(67, 71);
            this.pnlSpecular.Name = "pnlSpecular";
            this.pnlSpecular.Size = new System.Drawing.Size(49, 23);
            this.pnlSpecular.TabIndex = 4;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(3, 76);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(49, 13);
            this.label3.TabIndex = 3;
            this.label3.Text = "Specular";
            // 
            // txtAmbientAlpha
            // 
            this.txtAmbientAlpha.Location = new System.Drawing.Point(233, 15);
            this.txtAmbientAlpha.Name = "txtAmbientAlpha";
            this.txtAmbientAlpha.Size = new System.Drawing.Size(42, 20);
            this.txtAmbientAlpha.TabIndex = 6;
            // 
            // txtDiffuseAlpha
            // 
            this.txtDiffuseAlpha.Location = new System.Drawing.Point(233, 44);
            this.txtDiffuseAlpha.Name = "txtDiffuseAlpha";
            this.txtDiffuseAlpha.Size = new System.Drawing.Size(42, 20);
            this.txtDiffuseAlpha.TabIndex = 7;
            // 
            // txtSpecularAlpha
            // 
            this.txtSpecularAlpha.Location = new System.Drawing.Point(233, 73);
            this.txtSpecularAlpha.Name = "txtSpecularAlpha";
            this.txtSpecularAlpha.Size = new System.Drawing.Size(42, 20);
            this.txtSpecularAlpha.TabIndex = 8;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(213, 18);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(14, 13);
            this.label4.TabIndex = 9;
            this.label4.Text = "A";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(213, 47);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(14, 13);
            this.label5.TabIndex = 10;
            this.label5.Text = "A";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(213, 76);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(14, 13);
            this.label6.TabIndex = 11;
            this.label6.Text = "A";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(4, 132);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(84, 13);
            this.label7.TabIndex = 12;
            this.label7.Text = "Texture Tiling X:";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(4, 158);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(84, 13);
            this.label8.TabIndex = 13;
            this.label8.Text = "Texture Tiling Y:";
            // 
            // cbTilingRepeatX
            // 
            this.cbTilingRepeatX.AutoSize = true;
            this.cbTilingRepeatX.Location = new System.Drawing.Point(200, 132);
            this.cbTilingRepeatX.Name = "cbTilingRepeatX";
            this.cbTilingRepeatX.Size = new System.Drawing.Size(61, 17);
            this.cbTilingRepeatX.TabIndex = 14;
            this.cbTilingRepeatX.Text = "Repeat";
            this.cbTilingRepeatX.UseVisualStyleBackColor = true;
            // 
            // cbTilingRepeatY
            // 
            this.cbTilingRepeatY.AutoSize = true;
            this.cbTilingRepeatY.Location = new System.Drawing.Point(200, 157);
            this.cbTilingRepeatY.Name = "cbTilingRepeatY";
            this.cbTilingRepeatY.Size = new System.Drawing.Size(61, 17);
            this.cbTilingRepeatY.TabIndex = 15;
            this.cbTilingRepeatY.Text = "Repeat";
            this.cbTilingRepeatY.UseVisualStyleBackColor = true;
            // 
            // txtTilingX
            // 
            this.txtTilingX.Location = new System.Drawing.Point(94, 129);
            this.txtTilingX.Name = "txtTilingX";
            this.txtTilingX.Size = new System.Drawing.Size(87, 20);
            this.txtTilingX.TabIndex = 16;
            // 
            // txtTilingY
            // 
            this.txtTilingY.Location = new System.Drawing.Point(94, 155);
            this.txtTilingY.Name = "txtTilingY";
            this.txtTilingY.Size = new System.Drawing.Size(87, 20);
            this.txtTilingY.TabIndex = 17;
            // 
            // txtShininess
            // 
            this.txtShininess.Location = new System.Drawing.Point(67, 103);
            this.txtShininess.Name = "txtShininess";
            this.txtShininess.Size = new System.Drawing.Size(100, 20);
            this.txtShininess.TabIndex = 18;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(3, 103);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(52, 13);
            this.label9.TabIndex = 19;
            this.label9.Text = "Shininess";
            // 
            // MaterialDlg
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Controls.Add(this.label9);
            this.Controls.Add(this.txtShininess);
            this.Controls.Add(this.txtTilingY);
            this.Controls.Add(this.txtTilingX);
            this.Controls.Add(this.cbTilingRepeatY);
            this.Controls.Add(this.cbTilingRepeatX);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.txtSpecularAlpha);
            this.Controls.Add(this.txtDiffuseAlpha);
            this.Controls.Add(this.txtAmbientAlpha);
            this.Controls.Add(this.btnSpecular);
            this.Controls.Add(this.btnDiffuse);
            this.Controls.Add(this.pnlSpecular);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.pnlDiffuse);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.btnAmbient);
            this.Controls.Add(this.pnlAmbient);
            this.Controls.Add(this.label1);
            this.Name = "MaterialDlg";
            this.Size = new System.Drawing.Size(288, 187);
            this.Load += new System.EventHandler(this.MaterialDlg_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ColorDialog colorDialog;
        private System.Windows.Forms.Panel pnlAmbient;
        private System.Windows.Forms.Button btnAmbient;
        private System.Windows.Forms.Button btnDiffuse;
        private System.Windows.Forms.Panel pnlDiffuse;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnSpecular;
        private System.Windows.Forms.Panel pnlSpecular;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtAmbientAlpha;
        private System.Windows.Forms.TextBox txtDiffuseAlpha;
        private System.Windows.Forms.TextBox txtSpecularAlpha;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.CheckBox cbTilingRepeatX;
        private System.Windows.Forms.CheckBox cbTilingRepeatY;
        private System.Windows.Forms.TextBox txtTilingX;
        private System.Windows.Forms.TextBox txtTilingY;
        private System.Windows.Forms.TextBox txtShininess;
        private System.Windows.Forms.Label label9;
    }
}
