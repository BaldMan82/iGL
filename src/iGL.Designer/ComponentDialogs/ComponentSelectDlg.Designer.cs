namespace iGL.Designer
{
    partial class ComponentSelectDlg
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
            this.btnOk = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.listComponents = new System.Windows.Forms.ListBox();
            this.lblSelectComponent = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btnOk
            // 
            this.btnOk.Location = new System.Drawing.Point(12, 227);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(99, 23);
            this.btnOk.TabIndex = 0;
            this.btnOk.Text = "Ok";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(118, 227);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(99, 23);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // listComponents
            // 
            this.listComponents.FormattingEnabled = true;
            this.listComponents.Location = new System.Drawing.Point(12, 37);
            this.listComponents.Name = "listComponents";
            this.listComponents.Size = new System.Drawing.Size(205, 173);
            this.listComponents.TabIndex = 2;
            this.listComponents.DoubleClick += new System.EventHandler(this.listComponents_DoubleClick);
            // 
            // lblSelectComponent
            // 
            this.lblSelectComponent.AutoSize = true;
            this.lblSelectComponent.Location = new System.Drawing.Point(13, 13);
            this.lblSelectComponent.Name = "lblSelectComponent";
            this.lblSelectComponent.Size = new System.Drawing.Size(97, 13);
            this.lblSelectComponent.TabIndex = 3;
            this.lblSelectComponent.Text = "Select Component:";
            // 
            // ComponentSelectDlg
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(229, 262);
            this.ControlBox = false;
            this.Controls.Add(this.lblSelectComponent);
            this.Controls.Add(this.listComponents);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOk);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ComponentSelectDlg";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "Add Component";
            this.Load += new System.EventHandler(this.ComponentSelectDlg_Load);
            this.Shown += new System.EventHandler(this.ComponentSelectDlg_Shown);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.ListBox listComponents;
        private System.Windows.Forms.Label lblSelectComponent;
    }
}