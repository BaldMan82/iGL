namespace iGL.Designer.ComponentDialogs.Animations
{
    partial class BaseAnimationComponentDlg
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
            this.cbAutoStart = new System.Windows.Forms.CheckBox();
            this.ddAnimMode = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.txtDuration = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txtInterval = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // cbAutoStart
            // 
            this.cbAutoStart.AutoSize = true;
            this.cbAutoStart.Location = new System.Drawing.Point(15, 89);
            this.cbAutoStart.Name = "cbAutoStart";
            this.cbAutoStart.Size = new System.Drawing.Size(71, 17);
            this.cbAutoStart.TabIndex = 15;
            this.cbAutoStart.Text = "Auto start";
            this.cbAutoStart.UseVisualStyleBackColor = true;
            // 
            // ddAnimMode
            // 
            this.ddAnimMode.FormattingEnabled = true;
            this.ddAnimMode.Location = new System.Drawing.Point(125, 63);
            this.ddAnimMode.Name = "ddAnimMode";
            this.ddAnimMode.Size = new System.Drawing.Size(121, 21);
            this.ddAnimMode.TabIndex = 14;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(12, 66);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(60, 13);
            this.label5.TabIndex = 13;
            this.label5.Text = "Anim Mode";
            // 
            // txtDuration
            // 
            this.txtDuration.Location = new System.Drawing.Point(146, 11);
            this.txtDuration.Name = "txtDuration";
            this.txtDuration.Size = new System.Drawing.Size(100, 20);
            this.txtDuration.TabIndex = 12;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 14);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(50, 13);
            this.label1.TabIndex = 11;
            this.label1.Text = "Duration:";
            // 
            // txtInterval
            // 
            this.txtInterval.Location = new System.Drawing.Point(146, 37);
            this.txtInterval.Name = "txtInterval";
            this.txtInterval.Size = new System.Drawing.Size(100, 20);
            this.txtInterval.TabIndex = 17;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 40);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(45, 13);
            this.label2.TabIndex = 16;
            this.label2.Text = "Interval:";
            // 
            // BaseAnimationComponentDlg
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.txtInterval);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.cbAutoStart);
            this.Controls.Add(this.ddAnimMode);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.txtDuration);
            this.Controls.Add(this.label1);
            this.Name = "BaseAnimationComponentDlg";
            this.Size = new System.Drawing.Size(288, 125);
            this.Load += new System.EventHandler(this.BaseAnimationComponentDlg_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox cbAutoStart;
        private System.Windows.Forms.ComboBox ddAnimMode;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtDuration;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtInterval;
        private System.Windows.Forms.Label label2;
    }
}
