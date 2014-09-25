using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Fiddler.VSAutoResponder.View
{
    public partial class LocalRedirectSettings : UserControl
    {
        private Model.Settings settings;
        
        public LocalRedirectSettings()
        {
            InitializeComponent();
            dgvRedirects.AutoGenerateColumns = false;
        }

        public Model.Settings Settings 
        {
            get { return settings; }
            set
            {
                settings = value;
                dgvRedirects.DataSource = settings.Redirects;
            }
        }

        private void dgvRedirects_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            if (dgvRedirects.IsCurrentCellDirty && dgvRedirects.CurrentCell is DataGridViewCheckBoxCell)
            {
                dgvRedirects.CommitEdit(DataGridViewDataErrorContexts.Commit);
            }
        }
    }
}
