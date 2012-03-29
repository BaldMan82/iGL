namespace iGL.Designer
{
    partial class OrhogonalCameraComponentDlg
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
            // txtOrthogonalFarPlane
            // 
            this.txtOrthogonalFarPlane.Location = new System.Drawing.Point(118, 100);
            this.txtOrthogonalFarPlane.Name = "txtOrthogonalFarPlane";
            this.txtOrthogonalFarPlane.Size = new System.Drawing.Size(100, 20);
            this.txtOrthogonalFarPlane.TabIndex = 13;
            // 
            // txtOrthogonalNearPlane
            // 
            this.txtOrthogonalNearPlane.Location = new System.Drawing.Point(118, 71);
            this.txtOrthogonalNearPlane.Name = "txtOrthogonalNearPlane";
            this.txtOrthogonalNearPlane.Size = new System.Drawing.Size(100, 20);
            this.txtOrthogonalNearPlane.TabIndex = 12;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(16, 103);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(52, 13);
            this.label5.TabIndex = 11;
            this.label5.Text = "Far Plane";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(16, 74);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(60, 13);
            this.label6.TabIndex = 10;
            this.label6.Text = "Near Plane";
            // 
            // txtOrthogonalHeight
            // 
            this.txtOrthogonalHeight.Location = new System.Drawing.Point(118, 42);
            this.txtOrthogonalHeight.Name = "txtOrthogonalHeight";
            this.txtOrthogonalHeight.Size = new System.Drawing.Size(100, 20);
            this.txtOrthogonalHeight.TabIndex = 17;
            // 
            // txtOrthogonalWidth
            // 
            this.txtOrthogonalWidth.Location = new System.Drawing.Point(118, 13);
            this.txtOrthogonalWidth.Name = "txtOrthogonalWidth";
            this.txtOrthogonalWidth.Size = new System.Drawing.Size(100, 20);
            this.txtOrthogonalWidth.TabIndex = 16;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(16, 45);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(38, 13);
            this.label7.TabIndex = 15;
            this.label7.Text = "Height";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(16, 16);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(35, 13);
            this.label8.TabIndex = 14;
            this.label8.Text = "Width";
            // 
            // OrhogonalCameraComponentDlg
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
            this.Name = "OrhogonalCameraComponentDlg";
            this.Size = new System.Drawing.Size(290, 140);
            this.Load += new System.EventHandler(this.OrhogonalCameraComponentDlg_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

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
