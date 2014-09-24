using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Fiddler.VSAutoResponder.Model
{
    public class Redirect : INotifyPropertyChanged
    {
        public Uri FromUrl { get; set; }

        public Uri ToUrl { get; set; }

        public bool IsEnabled { get; set; }

        public bool IsLocalHostRedirect { get; set; }

        // TODO How do we tell the engine to use replace minified js/css with unminified versions?
                
        public event PropertyChangedEventHandler PropertyChanged;
    }
}
