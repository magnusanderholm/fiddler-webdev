namespace Fiddler.LocalRedirect.View
{
    partial class LocalRedirectSettings
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
            this.dgvRedirects = new System.Windows.Forms.DataGridView();
            this.colEnabled = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.colFrom = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colUseMinified = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dgvRedirects)).BeginInit();
            this.SuspendLayout();
            // 
            // dgvRedirects
            // 
            this.dgvRedirects.AllowDrop = true;
            this.dgvRedirects.AllowUserToOrderColumns = true;
            this.dgvRedirects.BackgroundColor = System.Drawing.SystemColors.Window;
            this.dgvRedirects.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvRedirects.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            this.dgvRedirects.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvRedirects.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colEnabled,
            this.colFrom,
            this.colUseMinified});
            this.dgvRedirects.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvRedirects.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.dgvRedirects.GridColor = System.Drawing.SystemColors.Control;
            this.dgvRedirects.Location = new System.Drawing.Point(0, 0);
            this.dgvRedirects.Margin = new System.Windows.Forms.Padding(0);
            this.dgvRedirects.Name = "dgvRedirects";
            this.dgvRedirects.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            this.dgvRedirects.RowHeadersWidth = 20;
            this.dgvRedirects.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.dgvRedirects.Size = new System.Drawing.Size(359, 274);
            this.dgvRedirects.TabIndex = 2;
            this.dgvRedirects.CurrentCellDirtyStateChanged += new System.EventHandler(this.dgvRedirects_CurrentCellDirtyStateChanged);
            this.dgvRedirects.DragDrop += new System.Windows.Forms.DragEventHandler(this.dgvRedirects_DragDrop);
            this.dgvRedirects.DragEnter += new System.Windows.Forms.DragEventHandler(this.dgvRedirects_DragEnter);
            this.dgvRedirects.DragOver += new System.Windows.Forms.DragEventHandler(this.dgvRedirects_DragOver);
            // 
            // colEnabled
            // 
            this.colEnabled.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.colEnabled.DataPropertyName = "IsEnabled";
            this.colEnabled.HeaderText = "E";
            this.colEnabled.Name = "colEnabled";
            this.colEnabled.ToolTipText = "If checked this redirect is enabled. Otherwise it is not enabled.";
            this.colEnabled.Width = 21;
            // 
            // colFrom
            // 
            this.colFrom.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colFrom.DataPropertyName = "Url";
            this.colFrom.HeaderText = "Url";
            this.colFrom.Name = "colFrom";
            this.colFrom.ToolTipText = "If a request url matches this it will be redirect to localhost. A website must ex" +
    "ist with same hostname as original request. Url must start with http:// or https" +
    ":// and have a valid host name.";
            // 
            // colUseMinified
            // 
            this.colUseMinified.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.colUseMinified.DataPropertyName = "UseMinified";
            this.colUseMinified.HeaderText = "Min";
            this.colUseMinified.Name = "colUseMinified";
            this.colUseMinified.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.colUseMinified.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.colUseMinified.ToolTipText = "If unchecked *.min.(css|js) will be replaced with .(css|js) requests instead.";
            this.colUseMinified.Width = 49;
            // 
            // LocalRedirectSettings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.dgvRedirects);
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "LocalRedirectSettings";
            this.Size = new System.Drawing.Size(359, 274);
            ((System.ComponentModel.ISupportInitialize)(this.dgvRedirects)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dgvRedirects;
        private System.Windows.Forms.DataGridViewCheckBoxColumn colEnabled;
        private System.Windows.Forms.DataGridViewTextBoxColumn colFrom;
        private System.Windows.Forms.DataGridViewCheckBoxColumn colUseMinified;
    }
}
