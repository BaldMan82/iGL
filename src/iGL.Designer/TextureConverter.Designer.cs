namespace iGL.Designer
{
    partial class TextureConverter
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.btnSelect = new System.Windows.Forms.Button();
            this.lblTexture = new System.Windows.Forms.Label();
            this.btnConvert = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.saveFileDialog = new System.Windows.Forms.SaveFileDialog();
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.SuspendLayout();
            // 
            // btnSelect
            // 
            this.btnSelect.Location = new System.Drawing.Point(145, 27);
            this.btnSelect.Name = "btnSelect";
            this.btnSelect.Size = new System.Drawing.Size(75, 23);
            this.btnSelect.TabIndex = 0;
            this.btnSelect.Text = "Select";
            this.btnSelect.UseVisualStyleBackColor = true;
            this.btnSelect.Click += new System.EventHandler(this.btnSelect_Click);
            // 
            // lblTexture
            // 
            this.lblTexture.AutoSize = true;
            this.lblTexture.Location = new System.Drawing.Point(12, 32);
            this.lblTexture.Name = "lblTexture";
            this.lblTexture.Size = new System.Drawing.Size(46, 13);
            this.lblTexture.TabIndex = 1;
            this.lblTexture.Text = "Select...";
            // 
            // btnConvert
            // 
            this.btnConvert.Location = new System.Drawing.Point(145, 75);
            this.btnConvert.Name = "btnConvert";
            this.btnConvert.Size = new System.Drawing.Size(75, 23);
            this.btnConvert.TabIndex = 2;
            this.btnConvert.Text = "Convert";
            this.btnConvert.UseVisualStyleBackColor = true;
            this.btnConvert.Click += new System.EventHandler(this.btnConvert_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(64, 75);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 3;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // saveFileDialog
            // 
            this.saveFileDialog.Filter = "Texture|*.text";
            // 
            // openFileDialog
            // 
            this.openFileDialog.Filter = "Bmp|*.bmp|Jpeg|*.jpg|Png|*.png";
            // 
            // TextureConverter
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(239, 110);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnConvert);
            this.Controls.Add(this.lblTexture);
            this.Controls.Add(this.btnSelect);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "TextureConverter";
            this.Text = "TextureConverter";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnSelect;
        private System.Windows.Forms.Label lblTexture;
        private System.Windows.Forms.Button btnConvert;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.SaveFileDialog saveFileDialog;
        private System.Windows.Forms.OpenFileDialog openFileDialog;
    }
}