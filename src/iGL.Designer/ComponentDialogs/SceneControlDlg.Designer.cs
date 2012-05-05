namespace iGL.Designer
{
    partial class SceneControlDlg
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
            this.components = new System.ComponentModel.Container();
            this.colorDialog = new System.Windows.Forms.ColorDialog();
            this.panel2 = new System.Windows.Forms.Panel();
            this.lblComponentName = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label4 = new System.Windows.Forms.Label();
            this.txtAmbientAlpha = new System.Windows.Forms.TextBox();
            this.btnAmbient = new System.Windows.Forms.Button();
            this.pnlAmbient = new System.Windows.Forms.Panel();
            this.label3 = new System.Windows.Forms.Label();
            this.ddLights = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.ddPlayCameras = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.gameObjectBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.ddDesignCameras = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.panel2.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gameObjectBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.SystemColors.ControlDark;
            this.panel2.Controls.Add(this.lblComponentName);
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(290, 31);
            this.panel2.TabIndex = 40;
            // 
            // lblComponentName
            // 
            this.lblComponentName.AutoSize = true;
            this.lblComponentName.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.lblComponentName.Location = new System.Drawing.Point(3, 8);
            this.lblComponentName.Name = "lblComponentName";
            this.lblComponentName.Size = new System.Drawing.Size(38, 13);
            this.lblComponentName.TabIndex = 0;
            this.lblComponentName.Text = "Scene";
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.ddDesignCameras);
            this.panel1.Controls.Add(this.label5);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.txtAmbientAlpha);
            this.panel1.Controls.Add(this.btnAmbient);
            this.panel1.Controls.Add(this.pnlAmbient);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.ddLights);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.ddPlayCameras);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Location = new System.Drawing.Point(0, 32);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(290, 130);
            this.panel1.TabIndex = 41;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(218, 102);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(14, 13);
            this.label4.TabIndex = 23;
            this.label4.Text = "A";
            // 
            // txtAmbientAlpha
            // 
            this.txtAmbientAlpha.Location = new System.Drawing.Point(238, 99);
            this.txtAmbientAlpha.Name = "txtAmbientAlpha";
            this.txtAmbientAlpha.Size = new System.Drawing.Size(42, 20);
            this.txtAmbientAlpha.TabIndex = 22;
            this.txtAmbientAlpha.TextChanged += new System.EventHandler(this.txtAmbientAlpha_TextChanged);
            // 
            // btnAmbient
            // 
            this.btnAmbient.Location = new System.Drawing.Point(127, 97);
            this.btnAmbient.Name = "btnAmbient";
            this.btnAmbient.Size = new System.Drawing.Size(75, 23);
            this.btnAmbient.TabIndex = 21;
            this.btnAmbient.Text = "Select";
            this.btnAmbient.UseVisualStyleBackColor = true;
            this.btnAmbient.Click += new System.EventHandler(this.btnAmbient_Click);
            // 
            // pnlAmbient
            // 
            this.pnlAmbient.Location = new System.Drawing.Point(72, 97);
            this.pnlAmbient.Name = "pnlAmbient";
            this.pnlAmbient.Size = new System.Drawing.Size(49, 23);
            this.pnlAmbient.TabIndex = 20;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(8, 102);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(45, 13);
            this.label3.TabIndex = 19;
            this.label3.Text = "Ambient";
            // 
            // ddLights
            // 
            this.ddLights.FormattingEnabled = true;
            this.ddLights.Location = new System.Drawing.Point(112, 62);
            this.ddLights.Name = "ddLights";
            this.ddLights.Size = new System.Drawing.Size(158, 21);
            this.ddLights.TabIndex = 18;
            this.ddLights.SelectedValueChanged += new System.EventHandler(this.ddLights_SelectedValueChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(5, 65);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(67, 13);
            this.label2.TabIndex = 17;
            this.label2.Text = "Current Light";
            // 
            // ddPlayCameras
            // 
            this.ddPlayCameras.FormattingEnabled = true;
            this.ddPlayCameras.Location = new System.Drawing.Point(112, 33);
            this.ddPlayCameras.Name = "ddPlayCameras";
            this.ddPlayCameras.Size = new System.Drawing.Size(158, 21);
            this.ddPlayCameras.TabIndex = 16;
            this.ddPlayCameras.SelectedValueChanged += new System.EventHandler(this.ddPlayCameras_SelectedValueChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(5, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(79, 13);
            this.label1.TabIndex = 15;
            this.label1.Text = "Design Camera";
            // 
            // gameObjectBindingSource
            // 
            this.gameObjectBindingSource.DataSource = typeof(iGL.Engine.GameObject);
            // 
            // ddDesignCameras
            // 
            this.ddDesignCameras.FormattingEnabled = true;
            this.ddDesignCameras.Location = new System.Drawing.Point(112, 6);
            this.ddDesignCameras.Name = "ddDesignCameras";
            this.ddDesignCameras.Size = new System.Drawing.Size(158, 21);
            this.ddDesignCameras.TabIndex = 25;
            this.ddDesignCameras.SelectedIndexChanged += new System.EventHandler(this.ddDesignCameras_SelectedIndexChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(5, 36);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(66, 13);
            this.label5.TabIndex = 24;
            this.label5.Text = "Play Camera";
            // 
            // SceneControlDlg
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panel2);
            this.Name = "SceneControlDlg";
            this.Size = new System.Drawing.Size(292, 472);
            this.Load += new System.EventHandler(this.SceneControlDlg_Load);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gameObjectBindingSource)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.BindingSource gameObjectBindingSource;
        private System.Windows.Forms.ColorDialog colorDialog;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label lblComponentName;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtAmbientAlpha;
        private System.Windows.Forms.Button btnAmbient;
        private System.Windows.Forms.Panel pnlAmbient;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox ddLights;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox ddPlayCameras;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox ddDesignCameras;
        private System.Windows.Forms.Label label5;
    }
}
