using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fiddler.LocalRedirect.Model
{
    [AttributeUsageAttribute(AttributeTargets.Class, Inherited = true)]
    public class ModifierAttribute : Attribute
    {
        public ModifierAttribute()
        {
            Order = 0;
            IsEnabled = true;
        }

        public int Order { get; set; }

        public bool IsEnabled { get; set; }
    }
}
