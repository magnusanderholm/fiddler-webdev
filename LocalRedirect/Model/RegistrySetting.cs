namespace Fiddler.LocalRedirect.Model
{
    using Microsoft.Win32;
    using System;
    using System.ComponentModel;

    public class RegistrySetting<TItem>
    {        
        private readonly RegistryKey registryKey;
        private readonly string name;
        private readonly TItem defaultValue;
        private readonly Func<TItem, string> toString;
        private readonly Func<string, TItem> fromString;
        private readonly StringConverter stringConverter = new StringConverter();
        private static readonly ILogger log = LogManager.CreateCurrentClassLogger();
        public RegistrySetting(string registryKey, string name, TItem defaultValue, Func<string, TItem> fromString = null, Func<TItem, string> toString = null)            
        {
            this.toString = toString ?? ((TItem i) => stringConverter.ConvertToInvariantString(i));
            this.fromString = fromString ?? ((string str) => (TItem)stringConverter.ConvertFromInvariantString(str));
            this.registryKey =  Registry.CurrentUser.CreateSubKey(registryKey);
            this.defaultValue = defaultValue;            
            this.name = name;
        }
        
        public TItem Get()
        {
            TItem ret = defaultValue;
            var str = (string)registryKey.GetValue(name, null);
            if (str != null)
            {
                try
                {
                    ret = fromString(str);
                }
                catch (Exception e)
                {
                    log.Error(() => "Failed to convert registry setting to object", e);
                }                
            }
            return ret;
        }

        public void Set(TItem item)
        {
            registryKey.SetValue(name, toString(item));
        }         
    }
}
