namespace Fiddler.LocalRedirect.View
{
    using Fiddler.LocalRedirect.ViewModel;
    using System.IO;
    using System.Windows;
    using System.Windows.Controls;

    /// <summary>
    /// Interaction logic for LocalRedirect.xaml
    /// </summary>
    public partial class LocalRedirect : UserControl
    {
        private const string defaultSettingsExtension = ".config";
        private const string settingsFileFilter = "Redirect config (*.config)|*.conf, *.config|All Files (*.*)|*.*";

        public LocalRedirect()
        {
            InitializeComponent();
        }

        public IRedirectViewModel ViewModel { get { return DataContext as IRedirectViewModel; } }

        private void OnSetInjectFragmentClick(object sender, RoutedEventArgs e)
        {
            var dlg = new Microsoft.Win32.OpenFileDialog();            
            dlg.Filter = "All Files (*.*)|*.*";
            bool? result = dlg.ShowDialog();
            if (result != null && result.Value)
            {
                var injectFragment = (Model.InjectFragment)((System.Windows.Controls.Control)(e.Source)).DataContext;
                injectFragment.Path = dlg.FileName;
                injectFragment.IsEnabled = true;
            }
        }

        private void OnBtnOpenClick(object sender, RoutedEventArgs e)
        {
            // Open a file dialog and let us load a configuration 
            var dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.DefaultExt = defaultSettingsExtension;
            dlg.Filter = settingsFileFilter;
            bool? result = dlg.ShowDialog();
            if (result != null && result.Value)
                ViewModel.SettingsStorage.CurrentStorage = new FileInfo(dlg.FileName);            
        }

        private void OnBtnSaveClick(object sender, RoutedEventArgs e)
        {            
            // Show save dialog and set ViewModel.CurrentSettingsFile. Otherwise just save.
            var dlg = new Microsoft.Win32.SaveFileDialog();
            dlg.DefaultExt = defaultSettingsExtension;
            dlg.Filter = settingsFileFilter;            
            bool? result = dlg.ShowDialog();
            if (result != null && result.Value)            
                ViewModel.SettingsStorage.CurrentStorage =  new FileInfo(dlg.FileName);            
        }

        private void OnBtnAddClick(object sender, RoutedEventArgs e)
        {
            var urlRule = ViewModel.SettingsStorage.Settings.UrlRuleFactory.Create();
            ViewModel.SettingsStorage.Settings.UrlRules.Add(urlRule);
        }

        private void OnSetFileResponseDirectory(object sender, RoutedEventArgs e)
        {
            using (var dlg = new System.Windows.Forms.FolderBrowserDialog())
            {
                if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK && 
                    !string.IsNullOrEmpty(dlg.SelectedPath) && 
                    System.IO.Directory.Exists(dlg.SelectedPath))
                {
                    var fileResponse = (Model.FileResponse)((System.Windows.Controls.Control)(e.Source)).DataContext;
                    fileResponse.DirectoryPath = dlg.SelectedPath;                    
                }
            }                                    
        }

        private void OnSelectUrlRuleColor(object sender, RoutedEventArgs e)
        {
            using (var dlg = new System.Windows.Forms.ColorDialog())
            {
                dlg.AllowFullOpen = true;
                dlg.FullOpen = true;
                if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {                              
                    var urlRule = (Model.UrlRule)((System.Windows.FrameworkElement)(e.Source)).DataContext;
                    urlRule.Color = dlg.Color;                  
                }
            }      
        }

        private void OnBtnRecentFileClick(object sender, RoutedEventArgs e)
        {                        
            var menuItem = sender as MenuItem; 
            // get submenu clicked item constructed from MyMenuItems collection
            var myItemsMenuSubItem = e.OriginalSource as MenuItem; 

            // get underlying MyMenuItems collection item
            var o = menuItem
                .ItemContainerGenerator
                .ItemFromContainer(myItemsMenuSubItem);
            // convert to MyMenuItems type ... in our case string
            var file = o as FileInfo;
            ViewModel.SettingsStorage.CurrentStorage = file;
        }        
    }
}
