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
        public LocalRedirectHost()
        {
            InitializeComponent();
            DragNDropFiddlerSessions = new Fiddler.Session[] { };
            this.wpfHost.Child.Drop += OnWpfDrop;
            this.wpfHost.Child.DragLeave += OnWpfDragLeave;
            
            this.AllowDrop = true;
            this.DragEnter += OnWinFormsDragEnter;
        }

        private IEnumerable<Fiddler.Session> DragNDropFiddlerSessions { get; set; }
                        
        private void OnWinFormsDragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetFormats().Any(f => f == "Fiddler.Session[]"))
            {
                DragNDropFiddlerSessions = (Fiddler.Session[])e.Data.GetData("Fiddler.Session[]") ?? new Fiddler.Session[] { };
                this.wpfHost.Visible = false;
            }
        }
         
        private void OnWpfDrop(object sender, System.Windows.DragEventArgs e)
        {
            IDataObject iData = Clipboard.GetDataObject();            
            foreach (var session in DragNDropFiddlerSessions)
                ViewModel.Redirects.Add(new Model.Redirect() { FromUrl = session.fullUrl, ToPort = session.port });            
        }

        private void OnWpfDragLeave(object sender, System.Windows.DragEventArgs e)
        {
            DragNDropFiddlerSessions = new Fiddler.Session[] { };
        }

        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);
        }
        
        public RedirectViewModel ViewModel 
        {
            get { return localRedirect1.DataContext as RedirectViewModel; }
            set { localRedirect1.DataContext = value; }
        }  
    }
}
