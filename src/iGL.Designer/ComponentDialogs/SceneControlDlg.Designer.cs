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
            this.label1 = new System.Windows.Forms.Label();
            this.ddCameras = new System.Windows.Forms.ComboBox();
            this.gameObjectBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.ddLights = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.txtAmbientAlpha = new System.Windows.Forms.TextBox();
            this.btnAmbient = new System.Windows.Forms.Button();
            this.pnlAmbient = new System.Windows.Forms.Panel();
            this.label3 = new System.Windows.Forms.Label();
            this.colorDialog = new System.Windows.Forms.ColorDialog();
            ((System.ComponentModel.ISupportInitialize)(this.gameObjectBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(4, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(80, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Current Camera";
            // 
            // ddCameras
            // 
            this.ddCameras.FormattingEnabled = true;
            this.ddCameras.Location = new System.Drawing.Point(112, 16);
            this.ddCameras.Name = "ddCameras";
            this.ddCameras.Size = new System.Drawing.Size(158, 21);
            this.ddCameras.TabIndex = 1;
            this.ddCameras.SelectedValueChanged += new System.EventHandler(this.ddCameras_SelectedValueChanged);
            // 
            // gameObjectBindingSource
            // 
            this.gameObjectBindingSource.DataSource = typeof(iGL.Engine.GameObject);
            // 
            // ddLights
            // 
            this.ddLights.FormattingEnabled = true;
            this.ddLights.Location = new System.Drawing.Point(111, 52);
            this.ddLights.Name = "ddLights";
            this.ddLights.Size = new System.Drawing.Size(158, 21);
            this.ddLights.TabIndex = 3;
            this.ddLights.SelectedValueChanged += new System.EventHandler(this.ddLights_SelectedValueChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(3, 52);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(67, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Current Light";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(218, 84);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(14, 13);
            this.label4.TabIndex = 14;
            this.label4.Text = "A";
            // 
            // txtAmbientAlpha
            // 
            this.txtAmbientAlpha.Location = new System.Drawing.Point(238, 81);
            this.txtAmbientAlpha.Name = "txtAmbientAlpha";
            this.txtAmbientAlpha.Size = new System.Drawing.Size(42, 20);
            this.txtAmbientAlpha.TabIndex = 13;
            // 
            // btnAmbient
            // 
            this.btnAmbient.Location = new System.Drawing.Point(127, 79);
            this.btnAmbient.Name = "btnAmbient";
            this.btnAmbient.Size = new System.Drawing.Size(75, 23);
            this.btnAmbient.TabIndex = 12;
            this.btnAmbient.Text = "Select";
            this.btnAmbient.UseVisualStyleBackColor = true;
            this.btnAmbient.Click += new System.EventHandler(this.btnAmbient_Click);
            // 
            // pnlAmbient
            // 
            this.pnlAmbient.Location = new System.Drawing.Point(72, 79);
            this.pnlAmbient.Name = "pnlAmbient";
            this.pnlAmbient.Size = new System.Drawing.Size(49, 23);
            this.pnlAmbient.TabIndex = 11;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(8, 84);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(45, 13);
            this.label3.TabIndex = 10;
            this.label3.Text = "Ambient";
            // 
            // SceneControlDlg
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Controls.Add(this.label4);
            this.Controls.Add(this.txtAmbientAlpha);
            this.Controls.Add(this.btnAmbient);
            this.Controls.Add(this.pnlAmbient);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.ddLights);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.ddCameras);
            this.Controls.Add(this.label1);
            this.Name = "SceneControlDlg";
            this.Size = new System.Drawing.Size(290, 128);
            this.Load += new System.EventHandler(this.SceneControlDlg_Load);
            ((System.ComponentModel.ISupportInitialize)(this.gameObjectBindingSource)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox ddCameras;
        private System.Windows.Forms.BindingSource gameObjectBindingSource;
        private System.Windows.Forms.ComboBox ddLights;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtAmbientAlpha;
        private System.Windows.Forms.Button btnAmbient;
        private System.Windows.Forms.Panel pnlAmbient;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ColorDialog colorDialog;
    }
}
