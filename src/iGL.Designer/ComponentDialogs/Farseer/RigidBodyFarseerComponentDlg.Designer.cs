﻿namespace iGL.Designer.ComponentDialogs
{
    partial class RigidBodyFarseerComponentDlg
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
            this.cbGravitySource = new System.Windows.Forms.CheckBox();
            this.txtMass = new System.Windows.Forms.TextBox();
            this.txtKineticFriction = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtRestitution = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.cbSensor = new System.Windows.Forms.CheckBox();
            this.txtGravityRange = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.ddBodyType = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 14);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(35, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Mass:";
            // 
            // cbGravitySource
            // 
            this.cbGravitySource.AutoSize = true;
            this.cbGravitySource.Location = new System.Drawing.Point(6, 122);
            this.cbGravitySource.Name = "cbGravitySource";
            this.cbGravitySource.Size = new System.Drawing.Size(96, 17);
            this.cbGravitySource.TabIndex = 2;
            this.cbGravitySource.Text = "Gravity Source";
            this.cbGravitySource.UseVisualStyleBackColor = true;
            // 
            // txtMass
            // 
            this.txtMass.Location = new System.Drawing.Point(123, 11);
            this.txtMass.Name = "txtMass";
            this.txtMass.Size = new System.Drawing.Size(98, 20);
            this.txtMass.TabIndex = 3;
            // 
            // txtKineticFriction
            // 
            this.txtKineticFriction.Location = new System.Drawing.Point(123, 37);
            this.txtKineticFriction.Name = "txtKineticFriction";
            this.txtKineticFriction.Size = new System.Drawing.Size(98, 20);
            this.txtKineticFriction.TabIndex = 5;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(3, 40);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(79, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Kinetic Friction:";
            // 
            // txtRestitution
            // 
            this.txtRestitution.Location = new System.Drawing.Point(123, 63);
            this.txtRestitution.Name = "txtRestitution";
            this.txtRestitution.Size = new System.Drawing.Size(98, 20);
            this.txtRestitution.TabIndex = 9;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(3, 66);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(60, 13);
            this.label4.TabIndex = 8;
            this.label4.Text = "Restitution:";
            // 
            // cbSensor
            // 
            this.cbSensor.AutoSize = true;
            this.cbSensor.Location = new System.Drawing.Point(108, 122);
            this.cbSensor.Name = "cbSensor";
            this.cbSensor.Size = new System.Drawing.Size(59, 17);
            this.cbSensor.TabIndex = 10;
            this.cbSensor.Text = "Sensor";
            this.cbSensor.UseVisualStyleBackColor = true;
            // 
            // txtGravityRange
            // 
            this.txtGravityRange.Location = new System.Drawing.Point(123, 154);
            this.txtGravityRange.Name = "txtGravityRange";
            this.txtGravityRange.Size = new System.Drawing.Size(98, 20);
            this.txtGravityRange.TabIndex = 12;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(3, 154);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(73, 13);
            this.label3.TabIndex = 11;
            this.label3.Text = "Gravity range:";
            // 
            // ddBodyType
            // 
            this.ddBodyType.FormattingEnabled = true;
            this.ddBodyType.Items.AddRange(new object[] {
            "Static",
            "Kinematic",
            "Dynamic"});
            this.ddBodyType.Location = new System.Drawing.Point(123, 90);
            this.ddBodyType.Name = "ddBodyType";
            this.ddBodyType.Size = new System.Drawing.Size(121, 21);
            this.ddBodyType.TabIndex = 13;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(3, 93);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(57, 13);
            this.label5.TabIndex = 14;
            this.label5.Text = "Body type:";
            // 
            // RigidBodyFarseerComponentDlg
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Controls.Add(this.label5);
            this.Controls.Add(this.ddBodyType);
            this.Controls.Add(this.txtGravityRange);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.cbSensor);
            this.Controls.Add(this.txtRestitution);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.txtKineticFriction);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtMass);
            this.Controls.Add(this.cbGravitySource);
            this.Controls.Add(this.label1);
            this.Name = "RigidBodyFarseerComponentDlg";
            this.Size = new System.Drawing.Size(290, 186);
            this.Load += new System.EventHandler(this.RigidBodyComponentDlg_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox cbGravitySource;
        private System.Windows.Forms.TextBox txtMass;
        private System.Windows.Forms.TextBox txtKineticFriction;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtRestitution;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.CheckBox cbSensor;
        private System.Windows.Forms.TextBox txtGravityRange;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox ddBodyType;
        private System.Windows.Forms.Label label5;
    }
}
