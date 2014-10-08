using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

namespace System.ComponentModel
{
    public class NotifyPropertyChanged
    {
        private readonly Action<PropertyChangedEventArgs> onPropertyChanged;
        private bool isChanged;

        public NotifyPropertyChanged(Action<PropertyChangedEventArgs> onPropertyChanged)
        {
            this.onPropertyChanged = onPropertyChanged;
            isChanged = false;
        }        

        public NotifyPropertyChanged Update<T>(ref T field, T value, [CallerMemberName]string name="")
        {            
            if ((isChanged = !object.Equals(field, value)))
            {
                field = value;
                onPropertyChanged(new PropertyChangedEventArgs(name));                
            }
            return this;
        }

        public void Extra(string name, params string[] extraNames)
        {
            if (isChanged)
            {
                onPropertyChanged(new PropertyChangedEventArgs(name));
                foreach (var n in extraNames)
                    onPropertyChanged(new PropertyChangedEventArgs(n));
            }
        }        
    }
}
