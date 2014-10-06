using System;
using System.Linq;
using System.Windows.Forms;

namespace Fiddler.LocalRedirect.View
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

        private void dgvRedirects_DragEnter(object sender, DragEventArgs e)
        {
            //((Fiddler.Session[])(e.Data.GetData("Fiddler.Session[]")))[0]
            if (e.Data.GetFormats().Any(f => f == "Fiddler.Session[]"))
                e.Effect = DragDropEffects.Copy;
        }

        private void dgvRedirects_DragDrop(object sender, DragEventArgs e)
        {
            if (e.Data.GetFormats().Any(f => f == "Fiddler.Session[]"))
            {
                var fiddlerSessions = (Fiddler.Session[])e.Data.GetData("Fiddler.Session[]");
                foreach(var fiddlerSession in fiddlerSessions)
                {
                    Settings.Redirects.Add(new Model.Redirect() { FromUrl = fiddlerSession.fullUrl, IsEnabled = true, ForceUnminified = false});
                }
            }
        }

        private void dgvRedirects_DragOver(object sender, DragEventArgs e)
        {
            int i = 0;
        }
    }
}
