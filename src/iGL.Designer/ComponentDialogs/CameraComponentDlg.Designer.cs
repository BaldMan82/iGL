namespace iGL.Designer
{
    partial class CameraComponentDlg
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
            this.radioPerspective = new System.Windows.Forms.RadioButton();
            this.radioOrthogonal = new System.Windows.Forms.RadioButton();
            this.txtPerspectiveFOV = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.txtPerspectiveAspectRatio = new System.Windows.Forms.TextBox();
            this.txtPerspectiveNearPlane = new System.Windows.Forms.TextBox();
            this.txtPerspectiveFarPlane = new System.Windows.Forms.TextBox();
            this.txtOrthogonalFarPlane = new System.Windows.Forms.TextBox();
            this.txtOrthogonalNearPlane = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.txtOrthogonalHeight = new System.Windows.Forms.TextBox();
            this.txtOrthogonalWidth = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // radioPerspective
            // 
            this.radioPerspective.AutoSize = true;
            this.radioPerspective.Location = new System.Drawing.Point(13, 15);
            this.radioPerspective.Name = "radioPerspective";
            this.radioPerspective.Size = new System.Drawing.Size(81, 17);
            this.radioPerspective.TabIndex = 0;
            this.radioPerspective.TabStop = true;
            this.radioPerspective.Text = "Perspective";
            this.radioPerspective.UseVisualStyleBackColor = true;
            this.radioPerspective.CheckedChanged += new System.EventHandler(this.radioPerspective_CheckedChanged);
            // 
            // radioOrthogonal
            // 
            this.radioOrthogonal.AutoSize = true;
            this.radioOrthogonal.Location = new System.Drawing.Point(13, 167);
            this.radioOrthogonal.Name = "radioOrthogonal";
            this.radioOrthogonal.Size = new System.Drawing.Size(77, 17);
            this.radioOrthogonal.TabIndex = 1;
            this.radioOrthogonal.TabStop = true;
            this.radioOrthogonal.Text = "Orthogonal";
            this.radioOrthogonal.UseVisualStyleBackColor = true;
            this.radioOrthogonal.CheckedChanged += new System.EventHandler(this.radioOrthogonal_CheckedChanged);
            // 
            // txtPerspectiveFOV
            // 
            this.txtPerspectiveFOV.Location = new System.Drawing.Point(143, 43);
            this.txtPerspectiveFOV.Name = "txtPerspectiveFOV";
            this.txtPerspectiveFOV.Size = new System.Drawing.Size(100, 20);
            this.txtPerspectiveFOV.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(41, 46);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(69, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Field Of View";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(41, 75);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(68, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Aspect Ratio";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(41, 104);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(60, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "Near Plane";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(41, 133);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(52, 13);
            this.label4.TabIndex = 6;
            this.label4.Text = "Far Plane";
            // 
            // txtPerspectiveAspectRatio
            // 
            this.txtPerspectiveAspectRatio.Location = new System.Drawing.Point(143, 72);
            this.txtPerspectiveAspectRatio.Name = "txtPerspectiveAspectRatio";
            this.txtPerspectiveAspectRatio.Size = new System.Drawing.Size(100, 20);
            this.txtPerspectiveAspectRatio.TabIndex = 7;
            // 
            // txtPerspectiveNearPlane
            // 
            this.txtPerspectiveNearPlane.Location = new System.Drawing.Point(143, 101);
            this.txtPerspectiveNearPlane.Name = "txtPerspectiveNearPlane";
            this.txtPerspectiveNearPlane.Size = new System.Drawing.Size(100, 20);
            this.txtPerspectiveNearPlane.TabIndex = 8;
            // 
            // txtPerspectiveFarPlane
            // 
            this.txtPerspectiveFarPlane.Location = new System.Drawing.Point(143, 130);
            this.txtPerspectiveFarPlane.Name = "txtPerspectiveFarPlane";
            this.txtPerspectiveFarPlane.Size = new System.Drawing.Size(100, 20);
            this.txtPerspectiveFarPlane.TabIndex = 9;
            // 
            // txtOrthogonalFarPlane
            // 
            this.txtOrthogonalFarPlane.Location = new System.Drawing.Point(143, 274);
            this.txtOrthogonalFarPlane.Name = "txtOrthogonalFarPlane";
            this.txtOrthogonalFarPlane.Size = new System.Drawing.Size(100, 20);
            this.txtOrthogonalFarPlane.TabIndex = 13;
            // 
            // txtOrthogonalNearPlane
            // 
            this.txtOrthogonalNearPlane.Location = new System.Drawing.Point(143, 245);
            this.txtOrthogonalNearPlane.Name = "txtOrthogonalNearPlane";
            this.txtOrthogonalNearPlane.Size = new System.Drawing.Size(100, 20);
            this.txtOrthogonalNearPlane.TabIndex = 12;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(41, 277);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(52, 13);
            this.label5.TabIndex = 11;
            this.label5.Text = "Far Plane";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(41, 248);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(60, 13);
            this.label6.TabIndex = 10;
            this.label6.Text = "Near Plane";
            // 
            // txtOrthogonalHeight
            // 
            this.txtOrthogonalHeight.Location = new System.Drawing.Point(143, 216);
            this.txtOrthogonalHeight.Name = "txtOrthogonalHeight";
            this.txtOrthogonalHeight.Size = new System.Drawing.Size(100, 20);
            this.txtOrthogonalHeight.TabIndex = 17;
            // 
            // txtOrthogonalWidth
            // 
            this.txtOrthogonalWidth.Location = new System.Drawing.Point(143, 187);
            this.txtOrthogonalWidth.Name = "txtOrthogonalWidth";
            this.txtOrthogonalWidth.Size = new System.Drawing.Size(100, 20);
            this.txtOrthogonalWidth.TabIndex = 16;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(41, 219);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(38, 13);
            this.label7.TabIndex = 15;
            this.label7.Text = "Height";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(41, 190);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(35, 13);
            this.label8.TabIndex = 14;
            this.label8.Text = "Width";
            // 
            // CameraComponentDlg
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Controls.Add(this.txtOrthogonalHeight);
            this.Controls.Add(this.txtOrthogonalWidth);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.txtOrthogonalFarPlane);
            this.Controls.Add(this.txtOrthogonalNearPlane);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.txtPerspectiveFarPlane);
            this.Controls.Add(this.txtPerspectiveNearPlane);
            this.Controls.Add(this.txtPerspectiveAspectRatio);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtPerspectiveFOV);
            this.Controls.Add(this.radioOrthogonal);
            this.Controls.Add(this.radioPerspective);
            this.Name = "CameraComponentDlg";
            this.Size = new System.Drawing.Size(290, 347);
            this.Load += new System.EventHandler(this.CameraComponentDlg_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RadioButton radioPerspective;
        private System.Windows.Forms.RadioButton radioOrthogonal;
        private System.Windows.Forms.TextBox txtPerspectiveFOV;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtPerspectiveAspectRatio;
        private System.Windows.Forms.TextBox txtPerspectiveNearPlane;
        private System.Windows.Forms.TextBox txtPerspectiveFarPlane;
        private System.Windows.Forms.TextBox txtOrthogonalFarPlane;
        private System.Windows.Forms.TextBox txtOrthogonalNearPlane;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtOrthogonalHeight;
        private System.Windows.Forms.TextBox txtOrthogonalWidth;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;


    }
}
