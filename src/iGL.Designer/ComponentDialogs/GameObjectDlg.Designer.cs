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
            this.contentPanel = new System.Windows.Forms.Panel();
            this.cbDistanceSorting = new System.Windows.Forms.CheckBox();
            this.txtRenderQueue = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.btnResetScale = new System.Windows.Forms.Button();
            this.btnResetRotation = new System.Windows.Forms.Button();
            this.btnResetPosition = new System.Windows.Forms.Button();
            this.cbEnabled = new System.Windows.Forms.CheckBox();
            this.cbVisible = new System.Windows.Forms.CheckBox();
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
            this.lblComponentName = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.buttonAdd = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.contentPanel.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // contentPanel
            // 
            this.contentPanel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.contentPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.contentPanel.Controls.Add(this.cbDistanceSorting);
            this.contentPanel.Controls.Add(this.txtRenderQueue);
            this.contentPanel.Controls.Add(this.label2);
            this.contentPanel.Controls.Add(this.btnResetScale);
            this.contentPanel.Controls.Add(this.btnResetRotation);
            this.contentPanel.Controls.Add(this.btnResetPosition);
            this.contentPanel.Controls.Add(this.cbEnabled);
            this.contentPanel.Controls.Add(this.cbVisible);
            this.contentPanel.Controls.Add(this._txtScaleZ);
            this.contentPanel.Controls.Add(this._txtScaleY);
            this.contentPanel.Controls.Add(this._txtScaleX);
            this.contentPanel.Controls.Add(this.label12);
            this.contentPanel.Controls.Add(this._txtRotationZ);
            this.contentPanel.Controls.Add(this._txtRotationY);
            this.contentPanel.Controls.Add(this._txtRotationX);
            this.contentPanel.Controls.Add(this.label8);
            this.contentPanel.Controls.Add(this._txtPositionZ);
            this.contentPanel.Controls.Add(this._txtPositionY);
            this.contentPanel.Controls.Add(this._txtPositionX);
            this.contentPanel.Controls.Add(this.label1);
            this.contentPanel.Location = new System.Drawing.Point(3, 32);
            this.contentPanel.Name = "contentPanel";
            this.contentPanel.Size = new System.Drawing.Size(290, 244);
            this.contentPanel.TabIndex = 37;
            // 
            // cbDistanceSorting
            // 
            this.cbDistanceSorting.AutoSize = true;
            this.cbDistanceSorting.Location = new System.Drawing.Point(146, 187);
            this.cbDistanceSorting.Name = "cbDistanceSorting";
            this.cbDistanceSorting.Size = new System.Drawing.Size(104, 17);
            this.cbDistanceSorting.TabIndex = 56;
            this.cbDistanceSorting.Text = "Distance Sorting";
            this.cbDistanceSorting.UseVisualStyleBackColor = true;
            // 
            // txtRenderQueue
            // 
            this.txtRenderQueue.Location = new System.Drawing.Point(146, 211);
            this.txtRenderQueue.Name = "txtRenderQueue";
            this.txtRenderQueue.Size = new System.Drawing.Size(100, 20);
            this.txtRenderQueue.TabIndex = 55;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(9, 218);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(106, 13);
            this.label2.TabIndex = 54;
            this.label2.Text = "Renderqueue Priority";
            // 
            // btnResetScale
            // 
            this.btnResetScale.AutoSize = true;
            this.btnResetScale.Location = new System.Drawing.Point(252, 140);
            this.btnResetScale.Name = "btnResetScale";
            this.btnResetScale.Size = new System.Drawing.Size(22, 23);
            this.btnResetScale.TabIndex = 53;
            this.btnResetScale.Text = "x";
            this.btnResetScale.UseVisualStyleBackColor = true;
            this.btnResetScale.Click += new System.EventHandler(this.btnResetScale_Click);
            // 
            // btnResetRotation
            // 
            this.btnResetRotation.AutoSize = true;
            this.btnResetRotation.Location = new System.Drawing.Point(252, 85);
            this.btnResetRotation.Name = "btnResetRotation";
            this.btnResetRotation.Size = new System.Drawing.Size(22, 23);
            this.btnResetRotation.TabIndex = 52;
            this.btnResetRotation.Text = "x";
            this.btnResetRotation.UseVisualStyleBackColor = true;
            this.btnResetRotation.Click += new System.EventHandler(this.btnResetRotation_Click);
            // 
            // btnResetPosition
            // 
            this.btnResetPosition.AutoSize = true;
            this.btnResetPosition.Location = new System.Drawing.Point(252, 30);
            this.btnResetPosition.Name = "btnResetPosition";
            this.btnResetPosition.Size = new System.Drawing.Size(22, 23);
            this.btnResetPosition.TabIndex = 51;
            this.btnResetPosition.Text = "x";
            this.btnResetPosition.UseVisualStyleBackColor = true;
            this.btnResetPosition.Click += new System.EventHandler(this.btnResetPosition_Click);
            // 
            // cbEnabled
            // 
            this.cbEnabled.AutoSize = true;
            this.cbEnabled.Location = new System.Drawing.Point(72, 187);
            this.cbEnabled.Name = "cbEnabled";
            this.cbEnabled.Size = new System.Drawing.Size(65, 17);
            this.cbEnabled.TabIndex = 50;
            this.cbEnabled.Text = "Enabled";
            this.cbEnabled.UseVisualStyleBackColor = true;
            // 
            // cbVisible
            // 
            this.cbVisible.AutoSize = true;
            this.cbVisible.Location = new System.Drawing.Point(12, 187);
            this.cbVisible.Name = "cbVisible";
            this.cbVisible.Size = new System.Drawing.Size(54, 17);
            this.cbVisible.TabIndex = 49;
            this.cbVisible.Text = "Visble";
            this.cbVisible.UseVisualStyleBackColor = true;
            // 
            // _txtScaleZ
            // 
            this._txtScaleZ.Location = new System.Drawing.Point(172, 143);
            this._txtScaleZ.Name = "_txtScaleZ";
            this._txtScaleZ.Size = new System.Drawing.Size(74, 20);
            this._txtScaleZ.TabIndex = 48;
            // 
            // _txtScaleY
            // 
            this._txtScaleY.Location = new System.Drawing.Point(92, 143);
            this._txtScaleY.Name = "_txtScaleY";
            this._txtScaleY.Size = new System.Drawing.Size(74, 20);
            this._txtScaleY.TabIndex = 47;
            // 
            // _txtScaleX
            // 
            this._txtScaleX.Location = new System.Drawing.Point(12, 143);
            this._txtScaleX.Name = "_txtScaleX";
            this._txtScaleX.Size = new System.Drawing.Size(74, 20);
            this._txtScaleX.TabIndex = 46;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(9, 120);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(34, 13);
            this.label12.TabIndex = 45;
            this.label12.Text = "Scale";
            // 
            // _txtRotationZ
            // 
            this._txtRotationZ.Location = new System.Drawing.Point(172, 88);
            this._txtRotationZ.Name = "_txtRotationZ";
            this._txtRotationZ.Size = new System.Drawing.Size(74, 20);
            this._txtRotationZ.TabIndex = 44;
            // 
            // _txtRotationY
            // 
            this._txtRotationY.Location = new System.Drawing.Point(92, 88);
            this._txtRotationY.Name = "_txtRotationY";
            this._txtRotationY.Size = new System.Drawing.Size(74, 20);
            this._txtRotationY.TabIndex = 43;
            // 
            // _txtRotationX
            // 
            this._txtRotationX.Location = new System.Drawing.Point(12, 88);
            this._txtRotationX.Name = "_txtRotationX";
            this._txtRotationX.Size = new System.Drawing.Size(74, 20);
            this._txtRotationX.TabIndex = 42;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(9, 65);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(47, 13);
            this.label8.TabIndex = 41;
            this.label8.Text = "Rotation";
            // 
            // _txtPositionZ
            // 
            this._txtPositionZ.Location = new System.Drawing.Point(172, 33);
            this._txtPositionZ.Name = "_txtPositionZ";
            this._txtPositionZ.Size = new System.Drawing.Size(74, 20);
            this._txtPositionZ.TabIndex = 40;
            // 
            // _txtPositionY
            // 
            this._txtPositionY.Location = new System.Drawing.Point(92, 33);
            this._txtPositionY.Name = "_txtPositionY";
            this._txtPositionY.Size = new System.Drawing.Size(74, 20);
            this._txtPositionY.TabIndex = 39;
            // 
            // _txtPositionX
            // 
            this._txtPositionX.Location = new System.Drawing.Point(12, 33);
            this._txtPositionX.Name = "_txtPositionX";
            this._txtPositionX.Size = new System.Drawing.Size(74, 20);
            this._txtPositionX.TabIndex = 38;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 10);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(44, 13);
            this.label1.TabIndex = 37;
            this.label1.Text = "Position";
            // 
            // lblComponentName
            // 
            this.lblComponentName.AutoSize = true;
            this.lblComponentName.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.lblComponentName.Location = new System.Drawing.Point(3, 8);
            this.lblComponentName.Name = "lblComponentName";
            this.lblComponentName.Size = new System.Drawing.Size(31, 13);
            this.lblComponentName.TabIndex = 0;
            this.lblComponentName.Text = "Base";
            // 
            // panel1
            // 
            this.panel1.AutoSize = true;
            this.panel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.panel1.Location = new System.Drawing.Point(3, 32);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(0, 0);
            this.panel1.TabIndex = 38;
            // 
            // buttonAdd
            // 
            this.buttonAdd.Location = new System.Drawing.Point(228, 3);
            this.buttonAdd.Name = "buttonAdd";
            this.buttonAdd.Size = new System.Drawing.Size(47, 23);
            this.buttonAdd.TabIndex = 1;
            this.buttonAdd.Text = "Add";
            this.buttonAdd.UseVisualStyleBackColor = true;
            this.buttonAdd.Click += new System.EventHandler(this.buttonAdd_Click);
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.SystemColors.ControlDark;
            this.panel2.Controls.Add(this.lblComponentName);
            this.panel2.Controls.Add(this.buttonAdd);
            this.panel2.Location = new System.Drawing.Point(3, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(290, 31);
            this.panel2.TabIndex = 39;
            // 
            // GameObjectDlg
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.contentPanel);
            this.Name = "GameObjectDlg";
            this.Size = new System.Drawing.Size(298, 279);
            this.Load += new System.EventHandler(this.GameObjectDlg_Load);
            this.contentPanel.ResumeLayout(false);
            this.contentPanel.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel contentPanel;
        private System.Windows.Forms.Button btnResetScale;
        private System.Windows.Forms.Button btnResetRotation;
        private System.Windows.Forms.Button btnResetPosition;
        private System.Windows.Forms.CheckBox cbEnabled;
        private System.Windows.Forms.CheckBox cbVisible;
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
        private System.Windows.Forms.CheckBox cbDistanceSorting;
        private System.Windows.Forms.TextBox txtRenderQueue;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lblComponentName;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button buttonAdd;
        private System.Windows.Forms.Panel panel2;


    }
}
