namespace Editor
{
    partial class frmMain
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMain));
            this.gbEditor = new System.Windows.Forms.GroupBox();
            this.lblError = new System.Windows.Forms.Label();
            this.gbEditor.SuspendLayout();
            this.SuspendLayout();
            // 
            // gbEditor
            // 
            this.gbEditor.Controls.Add(this.lblError);
            this.gbEditor.Location = new System.Drawing.Point(12, 12);
            this.gbEditor.Name = "gbEditor";
            this.gbEditor.Size = new System.Drawing.Size(268, 93);
            this.gbEditor.TabIndex = 0;
            this.gbEditor.TabStop = false;
            this.gbEditor.Text = "Editor";
            // 
            // lblError
            // 
            this.lblError.AutoSize = true;
            this.lblError.Location = new System.Drawing.Point(40, 46);
            this.lblError.Name = "lblError";
            this.lblError.Size = new System.Drawing.Size(191, 13);
            this.lblError.TabIndex = 0;
            this.lblError.Text = "ERROR: Editor disabled in this version.";
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(292, 117);
            this.Controls.Add(this.gbEditor);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmMain";
            this.Text = "Planetary Orbit - Editor";
            this.gbEditor.ResumeLayout(false);
            this.gbEditor.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox gbEditor;
        private System.Windows.Forms.Label lblError;
    }
}

