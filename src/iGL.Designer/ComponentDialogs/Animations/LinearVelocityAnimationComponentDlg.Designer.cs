namespace iGL.Designer.ComponentDialogs
{
    partial class LinearVelocityAnimationComponentDlg
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
            this.btnResetEndPointA = new System.Windows.Forms.Button();
            this._txtEndPointAZ = new System.Windows.Forms.TextBox();
            this._txtEndPointAY = new System.Windows.Forms.TextBox();
            this._txtEndPointAX = new System.Windows.Forms.TextBox();
            this.btnResetEndPointB = new System.Windows.Forms.Button();
            this._txtEndPointBZ = new System.Windows.Forms.TextBox();
            this._txtEndPointBY = new System.Windows.Forms.TextBox();
            this._txtEndPointBX = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.linkCopyPosition = new System.Windows.Forms.LinkLabel();
            this.linkLabel1 = new System.Windows.Forms.LinkLabel();
            this.baseAnimationComponentDlg = new iGL.Designer.ComponentDialogs.Animations.BaseAnimationComponentDlg();
            this.SuspendLayout();
            // 
            // btnResetEndPointA
            // 
            this.btnResetEndPointA.AutoSize = true;
            this.btnResetEndPointA.Location = new System.Drawing.Point(248, 26);
            this.btnResetEndPointA.Name = "btnResetEndPointA";
            this.btnResetEndPointA.Size = new System.Drawing.Size(22, 23);
            this.btnResetEndPointA.TabIndex = 55;
            this.btnResetEndPointA.Text = "x";
            this.btnResetEndPointA.UseVisualStyleBackColor = true;
            this.btnResetEndPointA.Click += new System.EventHandler(this.btnResetEndPointA_Click);
            // 
            // _txtEndPointAZ
            // 
            this._txtEndPointAZ.Location = new System.Drawing.Point(168, 29);
            this._txtEndPointAZ.Name = "_txtEndPointAZ";
            this._txtEndPointAZ.Size = new System.Drawing.Size(74, 20);
            this._txtEndPointAZ.TabIndex = 54;
            // 
            // _txtEndPointAY
            // 
            this._txtEndPointAY.Location = new System.Drawing.Point(88, 29);
            this._txtEndPointAY.Name = "_txtEndPointAY";
            this._txtEndPointAY.Size = new System.Drawing.Size(74, 20);
            this._txtEndPointAY.TabIndex = 53;
            // 
            // _txtEndPointAX
            // 
            this._txtEndPointAX.Location = new System.Drawing.Point(8, 29);
            this._txtEndPointAX.Name = "_txtEndPointAX";
            this._txtEndPointAX.Size = new System.Drawing.Size(74, 20);
            this._txtEndPointAX.TabIndex = 52;
            // 
            // btnResetEndPointB
            // 
            this.btnResetEndPointB.AutoSize = true;
            this.btnResetEndPointB.Location = new System.Drawing.Point(248, 73);
            this.btnResetEndPointB.Name = "btnResetEndPointB";
            this.btnResetEndPointB.Size = new System.Drawing.Size(22, 23);
            this.btnResetEndPointB.TabIndex = 59;
            this.btnResetEndPointB.Text = "x";
            this.btnResetEndPointB.UseVisualStyleBackColor = true;
            this.btnResetEndPointB.Click += new System.EventHandler(this.btnResetEndPointB_Click);
            // 
            // _txtEndPointBZ
            // 
            this._txtEndPointBZ.Location = new System.Drawing.Point(168, 76);
            this._txtEndPointBZ.Name = "_txtEndPointBZ";
            this._txtEndPointBZ.Size = new System.Drawing.Size(74, 20);
            this._txtEndPointBZ.TabIndex = 58;
            // 
            // _txtEndPointBY
            // 
            this._txtEndPointBY.Location = new System.Drawing.Point(88, 76);
            this._txtEndPointBY.Name = "_txtEndPointBY";
            this._txtEndPointBY.Size = new System.Drawing.Size(74, 20);
            this._txtEndPointBY.TabIndex = 57;
            // 
            // _txtEndPointBX
            // 
            this._txtEndPointBX.Location = new System.Drawing.Point(8, 76);
            this._txtEndPointBX.Name = "_txtEndPointBX";
            this._txtEndPointBX.Size = new System.Drawing.Size(74, 20);
            this._txtEndPointBX.TabIndex = 56;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(8, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(60, 13);
            this.label1.TabIndex = 60;
            this.label1.Text = "EndPoint A";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(8, 60);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(60, 13);
            this.label2.TabIndex = 61;
            this.label2.Text = "EndPoint B";
            // 
            // linkCopyPosition
            // 
            this.linkCopyPosition.AutoSize = true;
            this.linkCopyPosition.Location = new System.Drawing.Point(149, 10);
            this.linkCopyPosition.Name = "linkCopyPosition";
            this.linkCopyPosition.Size = new System.Drawing.Size(121, 13);
            this.linkCopyPosition.TabIndex = 64;
            this.linkCopyPosition.TabStop = true;
            this.linkCopyPosition.Text = "Copy from world position";
            this.linkCopyPosition.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkCopyPosition_LinkClicked);
            // 
            // linkLabel1
            // 
            this.linkLabel1.AutoSize = true;
            this.linkLabel1.Location = new System.Drawing.Point(149, 60);
            this.linkLabel1.Name = "linkLabel1";
            this.linkLabel1.Size = new System.Drawing.Size(121, 13);
            this.linkLabel1.TabIndex = 65;
            this.linkLabel1.TabStop = true;
            this.linkLabel1.Text = "Copy from world position";
            this.linkLabel1.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel1_LinkClicked);
            // 
            // baseAnimationComponentDlg
            // 
            this.baseAnimationComponentDlg.AnimationComponent = null;
            this.baseAnimationComponentDlg.Location = new System.Drawing.Point(1, 102);
            this.baseAnimationComponentDlg.Name = "baseAnimationComponentDlg";
            this.baseAnimationComponentDlg.Size = new System.Drawing.Size(286, 103);
            this.baseAnimationComponentDlg.TabIndex = 66;
            // 
            // LinearVelocityAnimationComponentDlg
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Controls.Add(this.baseAnimationComponentDlg);
            this.Controls.Add(this.linkLabel1);
            this.Controls.Add(this.linkCopyPosition);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnResetEndPointB);
            this.Controls.Add(this._txtEndPointBZ);
            this.Controls.Add(this._txtEndPointBY);
            this.Controls.Add(this._txtEndPointBX);
            this.Controls.Add(this.btnResetEndPointA);
            this.Controls.Add(this._txtEndPointAZ);
            this.Controls.Add(this._txtEndPointAY);
            this.Controls.Add(this._txtEndPointAX);
            this.Name = "LinearVelocityAnimationComponentDlg";
            this.Size = new System.Drawing.Size(290, 211);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnResetEndPointA;
        private System.Windows.Forms.TextBox _txtEndPointAZ;
        private System.Windows.Forms.TextBox _txtEndPointAY;
        private System.Windows.Forms.TextBox _txtEndPointAX;
        private System.Windows.Forms.Button btnResetEndPointB;
        private System.Windows.Forms.TextBox _txtEndPointBZ;
        private System.Windows.Forms.TextBox _txtEndPointBY;
        private System.Windows.Forms.TextBox _txtEndPointBX;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.LinkLabel linkCopyPosition;
        private System.Windows.Forms.LinkLabel linkLabel1;
        private Animations.BaseAnimationComponentDlg baseAnimationComponentDlg;

    }
}
