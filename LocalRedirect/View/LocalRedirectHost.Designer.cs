namespace Fiddler.LocalRedirect.View
{
    partial class LocalRedirectHost
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
            this.lblDropZone = new System.Windows.Forms.Label();
            this.wpfHost = new System.Windows.Forms.Integration.ElementHost();
            this.localRedirect1 = new Fiddler.LocalRedirect.View.LocalRedirect();
            this.SuspendLayout();
            // 
            // lblDropZone
            // 
            this.lblDropZone.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.lblDropZone.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblDropZone.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDropZone.Location = new System.Drawing.Point(0, 0);
            this.lblDropZone.Name = "lblDropZone";
            this.lblDropZone.Size = new System.Drawing.Size(372, 276);
            this.lblDropZone.TabIndex = 1;
            this.lblDropZone.Text = "Drop sessions anywhere...";
            this.lblDropZone.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // wpfHost
            // 
            this.wpfHost.Dock = System.Windows.Forms.DockStyle.Fill;
            this.wpfHost.Location = new System.Drawing.Point(0, 0);
            this.wpfHost.Name = "wpfHost";
            this.wpfHost.Size = new System.Drawing.Size(372, 276);
            this.wpfHost.TabIndex = 0;
            this.wpfHost.Text = "elementHost1";
            this.wpfHost.Child = this.localRedirect1;
            // 
            // LocalRedirectHost
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.wpfHost);
            this.Controls.Add(this.lblDropZone);
            this.Name = "LocalRedirectHost";
            this.Size = new System.Drawing.Size(372, 276);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Integration.ElementHost wpfHost;
        private LocalRedirect localRedirect1;
        private System.Windows.Forms.Label lblDropZone;
    }
}
