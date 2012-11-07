namespace iGL.Designer.ComponentDialogs
{
    partial class DistanceJointFarseerComponentDlg
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
            this.ddObject = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(67, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Other Object";
            // 
            // ddObject
            // 
            this.ddObject.FormattingEnabled = true;
            this.ddObject.Location = new System.Drawing.Point(88, 10);
            this.ddObject.Name = "ddObject";
            this.ddObject.Size = new System.Drawing.Size(121, 21);
            this.ddObject.TabIndex = 2;
            // 
            // DistanceJointFarseerComponentDlg
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Controls.Add(this.ddObject);
            this.Controls.Add(this.label1);
            this.Name = "DistanceJointFarseerComponentDlg";
            this.Size = new System.Drawing.Size(290, 45);
            this.Load += new System.EventHandler(this.DistanceJointFarseerComponentDlg_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox ddObject;
    }
}
