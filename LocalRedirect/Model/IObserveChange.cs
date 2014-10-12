using System;
namespace Fiddler.LocalRedirect.Model
{
    public interface IChange
    {
        event EventHandler<EventArgs> Changed;
    }

    public interface IObserveChange : IChange
    {                
        void Observe(System.ComponentModel.INotifyPropertyChanged nPc);
        
        void Observe<T>(System.Collections.ObjectModel.ObservableCollection<T> coll);
    }
}
