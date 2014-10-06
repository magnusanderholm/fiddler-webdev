namespace Fiddler.LocalRedirect.Config
{
    using System;
    using System.Xml.Serialization;
    
    public partial class UrlRule
    {        
        // private ObservableItemCollection<ChildSetting> settings;
        
        //[DataMember(Name = "settings", IsRequired = false, EmitDefaultValue = false)]
        //public ICollection<ChildSetting> Settings
        //{
        //    get 
        //    { 
        //        // Setup default child settings collection.
        //        return (settings = settings ?? new ObservableItemCollection<Settings>()); 
        //    }
        //    set
        //    {

        //    }
        //}

        [XmlIgnore()]
        public string Scheme
        {
            get
            {                
                return !string.IsNullOrWhiteSpace(Url)
                  ? new Uri(Url).Scheme.ToUpper()
                  : "NONE";
            }
        }  
    }
}
