namespace iGL.Designer.ComponentDialogs
{
    partial class FixedRevoluteJointComponentDlg
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
            this.txtMotorSpeed = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txtMaxTorque = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.cbMotorEnabled = new System.Windows.Forms.CheckBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtTorque = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // txtMotorSpeed
            // 
            this.txtMotorSpeed.Location = new System.Drawing.Point(107, 42);
            this.txtMotorSpeed.Name = "txtMotorSpeed";
            this.txtMotorSpeed.Size = new System.Drawing.Size(100, 20);
            this.txtMotorSpeed.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(14, 45);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(71, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Motor Speed:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(14, 97);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(67, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Max Torque:";
            // 
            // txtMaxTorque
            // 
            this.txtMaxTorque.Location = new System.Drawing.Point(107, 94);
            this.txtMaxTorque.Name = "txtMaxTorque";
            this.txtMaxTorque.Size = new System.Drawing.Size(100, 20);
            this.txtMaxTorque.TabIndex = 2;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(14, 20);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(79, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "Motor Enabled:";
            // 
            // cbMotorEnabled
            // 
            this.cbMotorEnabled.AutoSize = true;
            this.cbMotorEnabled.Location = new System.Drawing.Point(107, 19);
            this.cbMotorEnabled.Name = "cbMotorEnabled";
            this.cbMotorEnabled.Size = new System.Drawing.Size(15, 14);
            this.cbMotorEnabled.TabIndex = 5;
            this.cbMotorEnabled.UseVisualStyleBackColor = true;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(14, 71);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(44, 13);
            this.label4.TabIndex = 7;
            this.label4.Text = "Torque:";
            // 
            // txtTorque
            // 
            this.txtTorque.Location = new System.Drawing.Point(107, 68);
            this.txtTorque.Name = "txtTorque";
            this.txtTorque.Size = new System.Drawing.Size(100, 20);
            this.txtTorque.TabIndex = 6;
            // 
            // FixedRevoluteJointComponentDlg
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Controls.Add(this.label4);
            this.Controls.Add(this.txtTorque);
            this.Controls.Add(this.cbMotorEnabled);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtMaxTorque);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtMotorSpeed);
            this.Name = "FixedRevoluteJointComponentDlg";
            this.Size = new System.Drawing.Size(290, 160);
            this.Load += new System.EventHandler(this.FixedRevoluteJointComponentDlg_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtMotorSpeed;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtMaxTorque;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.CheckBox cbMotorEnabled;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtTorque;
    }
}
