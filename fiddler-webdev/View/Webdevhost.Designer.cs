namespace Fiddler.Webdev.View
{
    partial class WebdevHost
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
            this.wpfHost = new System.Windows.Forms.Integration.ElementHost();
            this.webDev = new Fiddler.Webdev.View.Webdev();
            this.lblDropZone = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.lblDropZone)).BeginInit();
            this.SuspendLayout();
            // 
            // wpfHost
            // 
            this.wpfHost.Dock = System.Windows.Forms.DockStyle.Fill;
            this.wpfHost.Location = new System.Drawing.Point(0, 0);
            this.wpfHost.Name = "wpfHost";
            this.wpfHost.Size = new System.Drawing.Size(372, 276);
            this.wpfHost.TabIndex = 0;
            this.wpfHost.Text = "elementHost1";
            this.wpfHost.Child = this.webDev;
            // 
            // lblDropZone
            // 
            this.lblDropZone.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblDropZone.Location = new System.Drawing.Point(0, 0);
            this.lblDropZone.Name = "lblDropZone";
            this.lblDropZone.Size = new System.Drawing.Size(372, 276);
            this.lblDropZone.TabIndex = 1;
            this.lblDropZone.TabStop = false;
            // 
            // LocalRedirectHost
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.wpfHost);
            this.Controls.Add(this.lblDropZone);
            this.Name = "WebdevHost";
            this.Size = new System.Drawing.Size(372, 276);
            ((System.ComponentModel.ISupportInitialize)(this.lblDropZone)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Integration.ElementHost wpfHost;
        private Webdev webDev;
        private System.Windows.Forms.PictureBox lblDropZone;
    }
}
