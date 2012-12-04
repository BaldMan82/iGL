namespace iGL.Designer.ComponentDialogs
{
    partial class TextAnimatorComponentDlg
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
            this.txtCharacterDelay = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtText = new System.Windows.Forms.TextBox();
            this.txtLineDelay = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.baseAnimationComponentDlg = new iGL.Designer.ComponentDialogs.Animations.BaseAnimationComponentDlg();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(14, 11);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(86, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Character Delay:";
            // 
            // txtCharacterDelay
            // 
            this.txtCharacterDelay.Location = new System.Drawing.Point(106, 8);
            this.txtCharacterDelay.Name = "txtCharacterDelay";
            this.txtCharacterDelay.Size = new System.Drawing.Size(100, 20);
            this.txtCharacterDelay.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(14, 75);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(31, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Text:";
            // 
            // txtText
            // 
            this.txtText.Location = new System.Drawing.Point(106, 75);
            this.txtText.Multiline = true;
            this.txtText.Name = "txtText";
            this.txtText.Size = new System.Drawing.Size(172, 91);
            this.txtText.TabIndex = 3;
            this.txtText.WordWrap = false;
            // 
            // txtLineDelay
            // 
            this.txtLineDelay.Location = new System.Drawing.Point(106, 39);
            this.txtLineDelay.Name = "txtLineDelay";
            this.txtLineDelay.Size = new System.Drawing.Size(100, 20);
            this.txtLineDelay.TabIndex = 5;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(14, 42);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(60, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "Line Delay:";
            // 
            // baseAnimationComponentDlg
            // 
            this.baseAnimationComponentDlg.AnimationComponent = null;
            this.baseAnimationComponentDlg.Location = new System.Drawing.Point(-1, 186);
            this.baseAnimationComponentDlg.Name = "baseAnimationComponentDlg";
            this.baseAnimationComponentDlg.Size = new System.Drawing.Size(286, 103);
            this.baseAnimationComponentDlg.TabIndex = 67;
            // 
            // TextAnimatorComponentDlg
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Controls.Add(this.baseAnimationComponentDlg);
            this.Controls.Add(this.txtLineDelay);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtText);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtCharacterDelay);
            this.Controls.Add(this.label1);
            this.Name = "TextAnimatorComponentDlg";
            this.Size = new System.Drawing.Size(290, 314);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtCharacterDelay;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtText;
        private System.Windows.Forms.TextBox txtLineDelay;
        private System.Windows.Forms.Label label3;
        private Animations.BaseAnimationComponentDlg baseAnimationComponentDlg;

    }
}
