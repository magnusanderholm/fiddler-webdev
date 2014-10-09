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
        private bool isChanged;

        public NotifyPropertyChanged(Action<PropertyChangedEventArgs> onPropertyChanged)
        {
            this.onPropertyChanged = onPropertyChanged;
            isChanged = false;
            Enabled = true;
        }

        public void Register<T>(ObservableCollection<T> collection)
        {
            collection.CollectionChanged += (s,e) => OnChanged(EventArgs.Empty);
        }

        public bool Enabled { get; set; }

        // If set property changes events will be propagated to parent.
        public NotifyPropertyChanged Parent { get; set; }
        
        public event EventHandler Changed;

        protected virtual void OnChanged(EventArgs e)
        {
            var h = Changed;
            if (h != null)
                h(this, e);
            if (Parent != null)
                Parent.OnChanged(e);
        }

        public NotifyPropertyChanged Update<T>(ref T field, T value, NotifyPropertyChanged parent = null, [CallerMemberName]string name="")
        {            
            if ((isChanged = !object.Equals(field, value)))
            {
                field = value;
                if (Enabled)
                {
                    onPropertyChanged(new PropertyChangedEventArgs(name));
                    OnChanged(EventArgs.Empty);
                }
            }
            return this;
        }

        public void Extra(string name, params string[] extraNames)
        {
            if (Enabled && isChanged)
            {
                foreach (var n in extraNames)
                {
                    onPropertyChanged(new PropertyChangedEventArgs(n));
                    OnChanged(EventArgs.Empty);
                }
            }
        }
        
    }
}
