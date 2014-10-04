using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Fiddler.LocalRedirect.View
{
    public class LabelEx : Label
    {
        public LabelEx()
        {
            SetStyle(ControlStyles.SupportsTransparentBackColor, true);            
        }
    }
}
