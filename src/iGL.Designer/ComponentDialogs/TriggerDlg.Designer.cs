namespace iGL.Designer.ComponentDialogs
{
    partial class TriggerDlg
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
            this.ddTriggerType = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.ddTarget = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.ddTargetComponent = new System.Windows.Forms.ComboBox();
            this.ddTriggerAction = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.ddTriggerSource = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.btnOk = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(15, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(70, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Trigger Type:";
            // 
            // ddTriggerType
            // 
            this.ddTriggerType.FormattingEnabled = true;
            this.ddTriggerType.Location = new System.Drawing.Point(133, 12);
            this.ddTriggerType.Name = "ddTriggerType";
            this.ddTriggerType.Size = new System.Drawing.Size(121, 21);
            this.ddTriggerType.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(15, 107);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(41, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Target:";
            // 
            // ddTarget
            // 
            this.ddTarget.FormattingEnabled = true;
            this.ddTarget.Location = new System.Drawing.Point(133, 104);
            this.ddTarget.Name = "ddTarget";
            this.ddTarget.Size = new System.Drawing.Size(121, 21);
            this.ddTarget.TabIndex = 3;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(15, 133);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(98, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "Target Component:";
            // 
            // ddTargetComponent
            // 
            this.ddTargetComponent.FormattingEnabled = true;
            this.ddTargetComponent.Location = new System.Drawing.Point(133, 130);
            this.ddTargetComponent.Name = "ddTargetComponent";
            this.ddTargetComponent.Size = new System.Drawing.Size(121, 21);
            this.ddTargetComponent.TabIndex = 5;
            // 
            // ddTriggerAction
            // 
            this.ddTriggerAction.FormattingEnabled = true;
            this.ddTriggerAction.Location = new System.Drawing.Point(133, 61);
            this.ddTriggerAction.Name = "ddTriggerAction";
            this.ddTriggerAction.Size = new System.Drawing.Size(121, 21);
            this.ddTriggerAction.TabIndex = 7;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(15, 64);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(76, 13);
            this.label4.TabIndex = 6;
            this.label4.Text = "Trigger Action:";
            // 
            // ddTriggerSource
            // 
            this.ddTriggerSource.FormattingEnabled = true;
            this.ddTriggerSource.Location = new System.Drawing.Point(133, 36);
            this.ddTriggerSource.Name = "ddTriggerSource";
            this.ddTriggerSource.Size = new System.Drawing.Size(121, 21);
            this.ddTriggerSource.TabIndex = 9;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(15, 39);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(80, 13);
            this.label5.TabIndex = 8;
            this.label5.Text = "Trigger Source:";
            // 
            // btnOk
            // 
            this.btnOk.Location = new System.Drawing.Point(181, 177);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(75, 23);
            this.btnOk.TabIndex = 10;
            this.btnOk.Text = "Ok";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(100, 177);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 11;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // TriggerDlg
            // 
            this.AcceptButton = this.btnOk;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(268, 212);
            this.ControlBox = false;
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.ddTriggerSource);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.ddTriggerAction);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.ddTargetComponent);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.ddTarget);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.ddTriggerType);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "TriggerDlg";
            this.Text = "Trigger";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox ddTriggerType;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox ddTarget;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox ddTargetComponent;
        private System.Windows.Forms.ComboBox ddTriggerAction;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox ddTriggerSource;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Button btnCancel;

    }
}
