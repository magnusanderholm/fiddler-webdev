using Fiddler.LocalRedirect.ViewModel;
using System;
using System.Collections.Generic;
using System.IO;
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
                var headerScript = (Model.HeaderScript)((System.Windows.FrameworkContentElement)(e.Source)).DataContext;
                headerScript.HtmlFragmentPath = dlg.FileName;
                headerScript.IsEnabled = true;
            }
        }

        private void OnBtnOpenClick(object sender, RoutedEventArgs e)
        {
            // Open a file dialog and let us load a configuration 
            FileInfo fI = null;
            var dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.DefaultExt = ".config";
            dlg.Filter = "Config (*.config)|*.conf";
            bool? result = dlg.ShowDialog();
            if (result != null && result.Value)
            {
                fI = new FileInfo(dlg.FileName);
                ViewModel.SettingsRepository.Open(fI);
            }                        
        }

        private void OnBtnSaveClick(object sender, RoutedEventArgs e)
        {            
            // Show save dialog and set ViewModel.CurrentSettingsFile. Otherwise just save.
            var dlg = new Microsoft.Win32.SaveFileDialog();
            dlg.DefaultExt = ".config";
            dlg.Filter = "Config (*.config)|*.conf";
            bool? result = dlg.ShowDialog();
            if (result != null && result.Value)
            {
                ViewModel.SettingsRepository.Save(new FileInfo(dlg.FileName));
            }                        
        }

        private void OnBtnAddClick(object sender, RoutedEventArgs e)
        {
            var urlRule = ViewModel.SettingsRepository.Settings.UrlRuleFactory.Create();
            ViewModel.SettingsRepository.Settings.UrlRules.Add(urlRule);
        }        
    }
}
