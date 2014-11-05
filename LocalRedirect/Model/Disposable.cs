using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fiddler.LocalRedirect.Model
{
    public static class Disposable
    {                     
        public static IDisposable Create<T>(T obj, Action<T> action)
        {
            return new DisposableInt<T>(obj, action);
        }
        
        class DisposableInt<T> : IDisposable
        {
            private Action<T> action;
            private T obj;

            public DisposableInt(T obj, Action<T> action)
            {
                this.obj = obj;
                this.action = action;
            }
            public void Dispose()
            {
                if (action != null)
                {
                    action(obj);
                    obj = default(T);
                    action = null;
                }
            }
        }
    }
}
