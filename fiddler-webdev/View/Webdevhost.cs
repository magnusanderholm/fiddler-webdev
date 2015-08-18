namespace Fiddler.Webdev.View
{
    using Fiddler.Webdev.ViewModel;
    using System;
    using System.Drawing;
    using System.Drawing.Imaging;
    using System.Linq;
    using System.Windows.Forms;
    using System.Windows.Media;
    using System.Windows.Media.Imaging;

    public partial class WebdevHost : UserControl
    {                
        public WebdevHost()
        {
            InitializeComponent();                        
            this.wpfHost.Child.AllowDrop = true;         
            this.AllowDrop = true;
            
            // Below DnD code is a attempt to work around the fact that Fiddler.Session is not serializable 
            // so it cannot be dnd on top of a WPF control. We therefore briong out a winforms control on dragenter of wpf controls
            // so we can drop the item on that instead. When DnD operations is done we bring the wpf control to the front again.
            // The winforms control we show as drop target is simply a ImageBox with the WPF control rendered in the image.
            this.wpfHost.Child.DragEnter += OnWpfDragEnter; // Disable wpf control so dnd events go to winforms instead.
            this.DragEnter += OnWinFormsDragEnter;
            this.DragDrop += OnWinFormsDragDrop;
            this.DragLeave += OnWinFormsDragLeave;            
        }

        public WebdevViewModel ViewModel
        {
            get { return webDev.DataContext as WebdevViewModel; }
            set { webDev.DataContext = value; }
        }

        private void OnWpfDragEnter(object sender, System.Windows.DragEventArgs e)
        {            
            var rTb = new RenderTargetBitmap(
                (int)wpfHost.Child.RenderSize.Width, 
                (int)wpfHost.Child.RenderSize.Height, 
                96, 
                96, 
                PixelFormats.Pbgra32);                        
            rTb.Render(wpfHost.Child);
            wpfHost.Child.SnapsToDevicePixels = true;
            PngBitmapEncoder encoder = new PngBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(rTb));            
            using (var mS = new System.IO.MemoryStream())
            {
                encoder.Save(mS);
                mS.Position = 0;
                var bmp = new Bitmap(mS);
                ChangeColors(bmp);
                if (lblDropZone.Image != null)
                    lblDropZone.Image.Dispose();
                lblDropZone.Image = bmp;
            }
                                                
            wpfHost.SendToBack();
        }

        private void OnWinFormsDragEnter(object sender, DragEventArgs e)
        {
            if (e.AllowedEffect == DragDropEffects.Copy)            
                e.Effect = DragDropEffects.Copy;                            
            else           
                wpfHost.BringToFront();                
        }

        private void OnWinFormsDragLeave(object sender, EventArgs e)
        {
            wpfHost.BringToFront();            
        }

        private void OnWinFormsDragDrop(object sender, DragEventArgs e)
        {
            //this.pnlDropZone.Visible = false;
            wpfHost.BringToFront();  
            //this.wpfHost.BackColor = dropZoneColorInactive;
            if (e.Data.GetFormats().Any(f => f == "Fiddler.Session[]"))
            {
                var fiddlerSessions = (Fiddler.Session[])e.Data.GetData("Fiddler.Session[]") ?? new Fiddler.Session[] { };
                foreach (var session in fiddlerSessions)
                {
                    var urlRule = ViewModel.SettingsStorage.Settings.UrlRuleFactory.Create(); 
                    urlRule.UrlString = session.fullUrl;         
                    ViewModel.SettingsStorage.Settings.UrlRules.Add(urlRule);
                }                    
            }
        }

        private static unsafe void ChangeColors(Bitmap img)
        {
            //const int noOfChannels = 4;            
            BitmapData data = img.LockBits(new Rectangle(0, 0, img.Width, img.Height), ImageLockMode.ReadWrite, img.PixelFormat);
            byte* ptr = (byte*)data.Scan0;
            for (int j = 0; j < data.Height; j++)
            {
                byte* scanPtr = ptr + (j * data.Stride);
                for (int i = 0; i < data.Stride; i++, scanPtr++)                
                    *scanPtr = (byte)(*scanPtr *0.92);                
            }

            img.UnlockBits(data);            
        }

                                                        
   
    }
}
