﻿namespace iGL.Designer.ComponentDialogs
{
    partial class PropertyAnimationComponentDlg
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
            this.txtDuration = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.ddProperties = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtStartValue = new System.Windows.Forms.TextBox();
            this.txtStopValue = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.ddAnimMode = new System.Windows.Forms.ComboBox();
            this.cbAutoStart = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(14, 47);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(50, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Duration:";
            // 
            // txtDuration
            // 
            this.txtDuration.Location = new System.Drawing.Point(130, 44);
            this.txtDuration.Name = "txtDuration";
            this.txtDuration.Size = new System.Drawing.Size(100, 20);
            this.txtDuration.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(14, 14);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(49, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Property:";
            // 
            // ddProperties
            // 
            this.ddProperties.FormattingEnabled = true;
            this.ddProperties.Location = new System.Drawing.Point(109, 11);
            this.ddProperties.Name = "ddProperties";
            this.ddProperties.Size = new System.Drawing.Size(121, 21);
            this.ddProperties.TabIndex = 3;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(14, 73);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(62, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "Start Value:";
            // 
            // txtStartValue
            // 
            this.txtStartValue.Location = new System.Drawing.Point(130, 70);
            this.txtStartValue.Name = "txtStartValue";
            this.txtStartValue.Size = new System.Drawing.Size(100, 20);
            this.txtStartValue.TabIndex = 5;
            // 
            // txtStopValue
            // 
            this.txtStopValue.Location = new System.Drawing.Point(130, 96);
            this.txtStopValue.Name = "txtStopValue";
            this.txtStopValue.Size = new System.Drawing.Size(100, 20);
            this.txtStopValue.TabIndex = 7;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(14, 99);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(62, 13);
            this.label4.TabIndex = 6;
            this.label4.Text = "Stop Value:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(16, 131);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(60, 13);
            this.label5.TabIndex = 8;
            this.label5.Text = "Anim Mode";
            // 
            // ddAnimMode
            // 
            this.ddAnimMode.FormattingEnabled = true;
            this.ddAnimMode.Location = new System.Drawing.Point(109, 128);
            this.ddAnimMode.Name = "ddAnimMode";
            this.ddAnimMode.Size = new System.Drawing.Size(121, 21);
            this.ddAnimMode.TabIndex = 9;
            // 
            // cbAutoStart
            // 
            this.cbAutoStart.AutoSize = true;
            this.cbAutoStart.Location = new System.Drawing.Point(19, 161);
            this.cbAutoStart.Name = "cbAutoStart";
            this.cbAutoStart.Size = new System.Drawing.Size(71, 17);
            this.cbAutoStart.TabIndex = 10;
            this.cbAutoStart.Text = "Auto start";
            this.cbAutoStart.UseVisualStyleBackColor = true;
            // 
            // PropertyAnimationComponentDlg
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Controls.Add(this.cbAutoStart);
            this.Controls.Add(this.ddAnimMode);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.txtStopValue);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.txtStartValue);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.ddProperties);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtDuration);
            this.Controls.Add(this.label1);
            this.Name = "PropertyAnimationComponentDlg";
            this.Size = new System.Drawing.Size(290, 190);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtDuration;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox ddProperties;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtStartValue;
        private System.Windows.Forms.TextBox txtStopValue;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox ddAnimMode;
        private System.Windows.Forms.CheckBox cbAutoStart;

    }
}
