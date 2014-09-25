using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Fiddler.VSAutoResponder.Model
{
    public class Redirect : INotifyPropertyChanged
    {
        public Uri Url { get; set; }        

        public bool IsEnabled { get; set; }

        public bool UseMinified { get; set; }
        
        // TODO How do we tell the engine to use replace minified js/css with unminified versions?ö                
        public event PropertyChangedEventHandler PropertyChanged;
    }
}
