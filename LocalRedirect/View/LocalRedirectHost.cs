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
        Panel pnl;
        public LocalRedirectHost()
        {
            InitializeComponent();
            pnl = new Panel();
            Controls.Add(pnl);
            pnl.Dock = DockStyle.Fill;
            pnl.AllowDrop = true;            
            pnl.DragEnter += OnDragEnter;
            pnl.DragDrop += OnDragDRop;
            pnl.DragLeave += OnDragLeave;
            pnl.Visible = false;
            pnl.BackColor = Color.Red;
            this.elementHost1.Child.PreviewDragEnter += OnPreviewDragEnter;
        }

        private void OnDragEnter(object sender, DragEventArgs e)
        {
            // pnl.DoDragDrop(e.Data, e.AllowedEffect);
        }                

        private void OnPreviewDragEnter(object sender, System.Windows.DragEventArgs e)
        {            
            this.elementHost1.Visible = false;
            pnl.Visible = true;
            e.Handled = true;
        }

        private void OnDragLeave(object sender, EventArgs e)
        {
            this.elementHost1.Visible = true;
            pnl.Visible = false;
        }        
        
        void OnDragDRop(object sender, DragEventArgs e)
        {
            if (e.Data.GetFormats().Any(f => f == "Fiddler.Session[]"))
            {
                var fiddlerSessions = (Fiddler.Session[])e.Data.GetData("Fiddler.Session[]");
                foreach (var fiddlerSession in fiddlerSessions)
                {
                    ViewModel.Redirects.Add(new Model.Redirect() { FromUrl = fiddlerSession.fullUrl, IsEnabled = true });
                }
            }
            this.elementHost1.Visible = true;
            pnl.Visible = false;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);                        
            elementHost1.AllowDrop = true;
            PropertyInfo pi = elementHost1.HostContainer.GetType().GetProperty("Sink");

            if (pi == null)
                return;

            RegisterDropTarget((HwndSource)pi.GetValue(elementHost1.HostContainer, null));
        }

        //void LocalRedirectHost_DragEnter(object sender, DragEventArgs e)
        //{
        //    if (e.Data.GetFormats().Any(f => f == "Fiddler.Session[]"))
        //    {
        //        var fiddlerSessions = (Fiddler.Session[])e.Data.GetData("Fiddler.Session[]");
        //        foreach (var fiddlerSession in fiddlerSessions)
        //        {
        //            int i = 0;
        //            // ViewModel.Redirects.Add(new Model.Redirect() { FromUrl = fiddlerSession.fullUrl, IsEnabled = true });
        //        }
        //    }            
        //}

        void Child_Drop(object sender, System.Windows.DragEventArgs e)
        {
            if (e.Data.GetFormats().Any(f => f == "Fiddler.Session[]"))
            {
                var fiddlerSessions = (Fiddler.Session[])e.Data.GetData("Fiddler.Session[]");
                foreach (var fiddlerSession in fiddlerSessions)
                {
                    ViewModel.Redirects.Add(new Model.Redirect() { FromUrl = fiddlerSession.fullUrl, IsEnabled = true });
                }
            }
        }
        
        
        public RedirectViewModel ViewModel 
        {
            get { return localRedirect1.DataContext as RedirectViewModel; }
            set { localRedirect1.DataContext = value; }
        }

        private static void RegisterDropTarget(HwndSource source)
        {

            try
            {

                typeof(System.Windows.DragDrop).GetMethod("RegisterDropTarget", BindingFlags.Static | BindingFlags.NonPublic).

                    Invoke(null, new object[] { source.Handle });

                var fieldInfo = typeof(HwndSource).GetField("_registeredDropTargetCount",

                    BindingFlags.NonPublic | BindingFlags.Instance);

                var currentValue = (int)fieldInfo.GetValue(source);

                fieldInfo.SetValue(source, currentValue + 1);

            }

            catch { }

        }
    }
}
