using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Fiddler.VSAutoResponder
{        
    [DataContract()]
    public class AutoResponderRule
    {
        [DataMember()]
        public string Regex { get; set; }
        
        [DataMember()]
        public string FullFilePath { get; set; }

        public string Key { get { return string.Format("{0}:{1}", Regex, FullFilePath.ToLower()); } }
    }
}
