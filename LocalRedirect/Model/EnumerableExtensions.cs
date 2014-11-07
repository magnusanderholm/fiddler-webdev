using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System.Linq
{
    public static class EnumerableExtensions
    {
        public static IEnumerable<TChild> Children<TParent, TChild>(this IEnumerable<TParent> parents, Func<TParent, IEnumerable<TChild>> childSelector)
        {
            return parents.SelectMany(p => childSelector(p));            
        }        
    }
}
