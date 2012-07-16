namespace iGL.Designer.ComponentDialogs
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
            this.cbStatic = new System.Windows.Forms.CheckBox();
            this.cbGravitySource = new System.Windows.Forms.CheckBox();
            this.txtMass = new System.Windows.Forms.TextBox();
            this.txtKineticFriction = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtRestitution = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.cbSensor = new System.Windows.Forms.CheckBox();
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
            // cbStatic
            // 
            this.cbStatic.AutoSize = true;
            this.cbStatic.Location = new System.Drawing.Point(6, 99);
            this.cbStatic.Name = "cbStatic";
            this.cbStatic.Size = new System.Drawing.Size(53, 17);
            this.cbStatic.TabIndex = 1;
            this.cbStatic.Text = "Static";
            this.cbStatic.UseVisualStyleBackColor = true;
            // 
            // cbGravitySource
            // 
            this.cbGravitySource.AutoSize = true;
            this.cbGravitySource.Location = new System.Drawing.Point(63, 99);
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
            this.cbSensor.Location = new System.Drawing.Point(164, 99);
            this.cbSensor.Name = "cbSensor";
            this.cbSensor.Size = new System.Drawing.Size(59, 17);
            this.cbSensor.TabIndex = 10;
            this.cbSensor.Text = "Sensor";
            this.cbSensor.UseVisualStyleBackColor = true;
            // 
            // RigidBodyFarseerComponentDlg
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Controls.Add(this.cbSensor);
            this.Controls.Add(this.txtRestitution);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.txtKineticFriction);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtMass);
            this.Controls.Add(this.cbGravitySource);
            this.Controls.Add(this.cbStatic);
            this.Controls.Add(this.label1);
            this.Name = "RigidBodyFarseerComponentDlg";
            this.Size = new System.Drawing.Size(290, 130);
            this.Load += new System.EventHandler(this.RigidBodyComponentDlg_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox cbStatic;
        private System.Windows.Forms.CheckBox cbGravitySource;
        private System.Windows.Forms.TextBox txtMass;
        private System.Windows.Forms.TextBox txtKineticFriction;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtRestitution;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.CheckBox cbSensor;
    }
}
