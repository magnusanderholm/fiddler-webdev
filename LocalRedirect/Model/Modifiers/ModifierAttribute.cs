namespace Fiddler.LocalRedirect.Model
{
    using System;

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
