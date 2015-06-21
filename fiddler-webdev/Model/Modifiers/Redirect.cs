namespace Fiddler.Webdev.Model
{
    using System;
    using System.ComponentModel;
    using System.Runtime.Serialization;
    using System.Xml.Serialization;

    [DataContract(Name="redirect", Namespace="")]
    [Modifier(Order = 4, IsEnabled = true)] 
    [Serializable()]
    [XmlRoot(Namespace = "", ElementName = "redirect")]
    public class Redirect : Modifier
    {
        private string toUrl;        

        private Redirect()
            : this(null)
        {           
        }
        
        public Redirect(UrlRule parent)
            : base(parent)
        {
            this.toUrl = "http://localhost";
        }        
               
        [DataMember(Name="tourl", IsRequired=false), DefaultValue("")]
        [XmlAttribute(AttributeName = "toport")]
        public string ToUrl
        {
            get { return this.toUrl; }
            set 
            {
                var val = value;
                if (val != null)
                    val = new Uri(val, UriKind.Absolute).ToString();
                pC.Update(ref toUrl, val); 
            }
        }
                        

        public override void RequestBefore(Session session)
        {
            base.RequestBefore(session);
            if (IsEnabled)            
                session.fullUrl = new Uri(new Uri(ToUrl), session.PathAndQuery).ToString();
        }
                
    }
}
