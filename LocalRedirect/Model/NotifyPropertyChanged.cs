using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

namespace System.ComponentModel
{
    public class NotifyPropertyChanged
    {
        private readonly Action<PropertyChangedEventArgs> onPropertyChanged;
        
        public NotifyPropertyChanged(Action<PropertyChangedEventArgs> onPropertyChanged)
        {
            this.onPropertyChanged = onPropertyChanged;
            IsChanged = false;
            Enabled = true;            
        }

        public bool Enabled { get; set; }

        public bool IsChanged { get; private set; }
        

        public NotifyPropertyChanged Update<T>(ref T field, T value, NotifyPropertyChanged parent = null, [CallerMemberName]string name="")
        {
            if ((IsChanged = !object.Equals(field, value)))
            {
                field = value;
                if (Enabled)                
                    onPropertyChanged(new PropertyChangedEventArgs(name));                                    
            }
            return this;
        }

        public void Extra(string name, params string[] extraNames)
        {
            if (Enabled && IsChanged && extraNames.Length > 0)
            {
                foreach (var n in extraNames)                
                    onPropertyChanged(new PropertyChangedEventArgs(n));                                 
            }
        }
        
    }
}
