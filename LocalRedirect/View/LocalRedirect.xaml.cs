using Fiddler.LocalRedirect.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Fiddler.LocalRedirect.View
{
    /// <summary>
    /// Interaction logic for LocalRedirect.xaml
    /// </summary>
    public partial class LocalRedirect : UserControl
    {
        public LocalRedirect()
        {
            InitializeComponent();
        }

        public RedirectViewModel ViewModel { get { return DataContext as RedirectViewModel; } }

        private void OnSetHeaderScriptClick(object sender, RoutedEventArgs e)
        {
            var dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.DefaultExt = ".html";
            dlg.Filter = "Html (*.html)|*.htm";
            bool? result = dlg.ShowDialog();
            if (result != null && result.Value)
            {
                var headerScript = (Config.HeaderScript)((System.Windows.FrameworkContentElement)(e.Source)).DataContext;
                headerScript.Path = dlg.FileName;
                headerScript.IsEnabled = true;
            }
        }        
    }
}
