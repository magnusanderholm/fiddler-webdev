namespace System.ComponentModel
{
    using System;
    using System.Runtime.CompilerServices;

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
        

        public NotifyPropertyChanged Update<T>(ref T field, T value, Func<T,T, bool> equals = null, [CallerMemberName]string name="")
        {
            equals = equals ?? ((T t0, T t1) => object.Equals(t0, t1));
            if ((IsChanged = !equals(field, value)))
            {
                field = value;
                if (Enabled)                
                    onPropertyChanged(new PropertyChangedEventArgs(name));                                    
            }
            return this;
        }

        public NotifyPropertyChanged Extra(params string[] extraNames)
        {
            if (Enabled && IsChanged && extraNames.Length > 0)
            {
                foreach (var n in extraNames)                
                    onPropertyChanged(new PropertyChangedEventArgs(n));                                 
            }
            return this;
        }
        
    }
}
