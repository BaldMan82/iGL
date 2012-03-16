namespace iGL.Designer
{
    partial class GameObjectDlg
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
            this._txtScaleZ = new System.Windows.Forms.TextBox();
            this._txtScaleY = new System.Windows.Forms.TextBox();
            this._txtScaleX = new System.Windows.Forms.TextBox();
            this.label12 = new System.Windows.Forms.Label();
            this._txtRotationZ = new System.Windows.Forms.TextBox();
            this._txtRotationY = new System.Windows.Forms.TextBox();
            this._txtRotationX = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this._txtPositionZ = new System.Windows.Forms.TextBox();
            this._txtPositionY = new System.Windows.Forms.TextBox();
            this._txtPositionX = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.cbVisible = new System.Windows.Forms.CheckBox();
            this.cbEnabled = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // _txtScaleZ
            // 
            this._txtScaleZ.Location = new System.Drawing.Point(176, 143);
            this._txtScaleZ.Name = "_txtScaleZ";
            this._txtScaleZ.Size = new System.Drawing.Size(74, 20);
            this._txtScaleZ.TabIndex = 31;
            // 
            // _txtScaleY
            // 
            this._txtScaleY.Location = new System.Drawing.Point(96, 143);
            this._txtScaleY.Name = "_txtScaleY";
            this._txtScaleY.Size = new System.Drawing.Size(74, 20);
            this._txtScaleY.TabIndex = 30;
            // 
            // _txtScaleX
            // 
            this._txtScaleX.Location = new System.Drawing.Point(16, 143);
            this._txtScaleX.Name = "_txtScaleX";
            this._txtScaleX.Size = new System.Drawing.Size(74, 20);
            this._txtScaleX.TabIndex = 29;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(13, 120);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(34, 13);
            this.label12.TabIndex = 28;
            this.label12.Text = "Scale";
            // 
            // _txtRotationZ
            // 
            this._txtRotationZ.Location = new System.Drawing.Point(176, 88);
            this._txtRotationZ.Name = "_txtRotationZ";
            this._txtRotationZ.Size = new System.Drawing.Size(74, 20);
            this._txtRotationZ.TabIndex = 27;
            // 
            // _txtRotationY
            // 
            this._txtRotationY.Location = new System.Drawing.Point(96, 88);
            this._txtRotationY.Name = "_txtRotationY";
            this._txtRotationY.Size = new System.Drawing.Size(74, 20);
            this._txtRotationY.TabIndex = 26;
            // 
            // _txtRotationX
            // 
            this._txtRotationX.Location = new System.Drawing.Point(16, 88);
            this._txtRotationX.Name = "_txtRotationX";
            this._txtRotationX.Size = new System.Drawing.Size(74, 20);
            this._txtRotationX.TabIndex = 25;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(13, 65);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(47, 13);
            this.label8.TabIndex = 24;
            this.label8.Text = "Rotation";
            // 
            // _txtPositionZ
            // 
            this._txtPositionZ.Location = new System.Drawing.Point(176, 33);
            this._txtPositionZ.Name = "_txtPositionZ";
            this._txtPositionZ.Size = new System.Drawing.Size(74, 20);
            this._txtPositionZ.TabIndex = 23;
            // 
            // _txtPositionY
            // 
            this._txtPositionY.Location = new System.Drawing.Point(96, 33);
            this._txtPositionY.Name = "_txtPositionY";
            this._txtPositionY.Size = new System.Drawing.Size(74, 20);
            this._txtPositionY.TabIndex = 22;
            // 
            // _txtPositionX
            // 
            this._txtPositionX.Location = new System.Drawing.Point(16, 33);
            this._txtPositionX.Name = "_txtPositionX";
            this._txtPositionX.Size = new System.Drawing.Size(74, 20);
            this._txtPositionX.TabIndex = 21;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 10);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(44, 13);
            this.label1.TabIndex = 20;
            this.label1.Text = "Position";
            // 
            // cbVisible
            // 
            this.cbVisible.AutoSize = true;
            this.cbVisible.Location = new System.Drawing.Point(16, 187);
            this.cbVisible.Name = "cbVisible";
            this.cbVisible.Size = new System.Drawing.Size(54, 17);
            this.cbVisible.TabIndex = 32;
            this.cbVisible.Text = "Visble";
            this.cbVisible.UseVisualStyleBackColor = true;
            // 
            // cbEnabled
            // 
            this.cbEnabled.AutoSize = true;
            this.cbEnabled.Location = new System.Drawing.Point(76, 187);
            this.cbEnabled.Name = "cbEnabled";
            this.cbEnabled.Size = new System.Drawing.Size(65, 17);
            this.cbEnabled.TabIndex = 33;
            this.cbEnabled.Text = "Enabled";
            this.cbEnabled.UseVisualStyleBackColor = true;
            // 
            // GameObjectDlg
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Controls.Add(this.cbEnabled);
            this.Controls.Add(this.cbVisible);
            this.Controls.Add(this._txtScaleZ);
            this.Controls.Add(this._txtScaleY);
            this.Controls.Add(this._txtScaleX);
            this.Controls.Add(this.label12);
            this.Controls.Add(this._txtRotationZ);
            this.Controls.Add(this._txtRotationY);
            this.Controls.Add(this._txtRotationX);
            this.Controls.Add(this.label8);
            this.Controls.Add(this._txtPositionZ);
            this.Controls.Add(this._txtPositionY);
            this.Controls.Add(this._txtPositionX);
            this.Controls.Add(this.label1);
            this.Name = "GameObjectDlg";
            this.Size = new System.Drawing.Size(290, 222);
            this.Load += new System.EventHandler(this.GameObjectDlg_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox _txtScaleZ;
        private System.Windows.Forms.TextBox _txtScaleY;
        private System.Windows.Forms.TextBox _txtScaleX;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.TextBox _txtRotationZ;
        private System.Windows.Forms.TextBox _txtRotationY;
        private System.Windows.Forms.TextBox _txtRotationX;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox _txtPositionZ;
        private System.Windows.Forms.TextBox _txtPositionY;
        private System.Windows.Forms.TextBox _txtPositionX;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox cbVisible;
        private System.Windows.Forms.CheckBox cbEnabled;

    }
}
