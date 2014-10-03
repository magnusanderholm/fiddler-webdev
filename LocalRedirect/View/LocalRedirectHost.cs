using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Fiddler.LocalRedirect.ViewModel;
using System.Reflection;
using System.Windows.Interop;

namespace Fiddler.LocalRedirect.View
{
    public partial class LocalRedirectHost : UserControl
    {        

        private readonly Color dropZoneColorInactive;
        private readonly Color dropZoneColorActive;
        public LocalRedirectHost()
        {
            InitializeComponent();            
            
            
            this.wpfHost.Child.AllowDrop = true;         
            this.AllowDrop = true;

            this.wpfHost.Child.DragEnter += (s,e) => wpfHost.Visible = false; // Disable wpf control so dnd events go to winforms instead.
            this.DragEnter += OnWinFormsDragEnter;
            this.DragDrop += OnWinFormsDragDrop;
            this.DragLeave += OnWinFormsDragLeave;

            //dropZoneColorInactive = this.wpfHost.BackColor;
            //dropZoneColorActive = ControlPaint.LightLight(dropZoneColorInactive);
        }

        public RedirectViewModel ViewModel
        {
            get { return localRedirect1.DataContext as RedirectViewModel; }
            set { localRedirect1.DataContext = value; }
        } 

        private void OnWinFormsDragEnter(object sender, DragEventArgs e)
        {
            if (e.AllowedEffect == DragDropEffects.Copy)
            {
                e.Effect = DragDropEffects.Copy;
                //this.wpfHost.BackColor = dropZoneColorActive;
                // Change background color to something better.
            }
            else
            {
                wpfHost.Visible = true;
                this.wpfHost.BackColor = dropZoneColorInactive;
            }
        }

        private void OnWinFormsDragLeave(object sender, EventArgs e)
        {
            wpfHost.Visible = true;
            //this.wpfHost.BackColor = dropZoneColorInactive;
        }

        private void OnWinFormsDragDrop(object sender, DragEventArgs e)
        {
            //this.pnlDropZone.Visible = false;
            wpfHost.Visible = true;
            //this.wpfHost.BackColor = dropZoneColorInactive;
            if (e.Data.GetFormats().Any(f => f == "Fiddler.Session[]"))
            {
                var fiddlerSessions = (Fiddler.Session[])e.Data.GetData("Fiddler.Session[]") ?? new Fiddler.Session[] { };
                foreach (var session in fiddlerSessions)
                    ViewModel.Redirects.Add(new Model.Redirect() { FromUrl = session.fullUrl, ToPort = session.port });
            }
        }

                                                        
   
    }
}
