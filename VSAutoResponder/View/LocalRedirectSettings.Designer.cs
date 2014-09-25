namespace Fiddler.VSAutoResponder.View
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
            this.colFrom = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colUseMinified = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.colEnabled = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dgvRedirects)).BeginInit();
            this.SuspendLayout();
            // 
            // dgvRedirects
            // 
            this.dgvRedirects.AllowUserToOrderColumns = true;
            this.dgvRedirects.BackgroundColor = System.Drawing.SystemColors.Window;
            this.dgvRedirects.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvRedirects.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvRedirects.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colFrom,
            this.colUseMinified,
            this.colEnabled});
            this.dgvRedirects.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvRedirects.Location = new System.Drawing.Point(0, 0);
            this.dgvRedirects.Margin = new System.Windows.Forms.Padding(0);
            this.dgvRedirects.Name = "dgvRedirects";
            this.dgvRedirects.RowHeadersVisible = false;
            this.dgvRedirects.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvRedirects.Size = new System.Drawing.Size(359, 274);
            this.dgvRedirects.TabIndex = 2;
            // 
            // colFrom
            // 
            this.colFrom.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colFrom.DataPropertyName = "FromUrl";
            this.colFrom.HeaderText = "Url";
            this.colFrom.Name = "colFrom";
            // 
            // colUseMinified
            // 
            this.colUseMinified.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.colUseMinified.HeaderText = "M";
            this.colUseMinified.Name = "colUseMinified";
            this.colUseMinified.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.colUseMinified.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.colUseMinified.Width = 41;
            // 
            // colEnabled
            // 
            this.colEnabled.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.colEnabled.HeaderText = "E";
            this.colEnabled.Name = "colEnabled";
            this.colEnabled.Width = 21;
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
        private System.Windows.Forms.DataGridViewTextBoxColumn colFrom;
        private System.Windows.Forms.DataGridViewCheckBoxColumn colUseMinified;
        private System.Windows.Forms.DataGridViewCheckBoxColumn colEnabled;
    }
}
