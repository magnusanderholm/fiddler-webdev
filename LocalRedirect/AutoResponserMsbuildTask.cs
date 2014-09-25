namespace Fiddler.VSAutoResponder
{
    using Microsoft.Build.Utilities;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class AutoResponserMsbuildTask : Task
    {        
        public override bool Execute()
        {
            // TODO Load .proj file. Look at all included files and create
            // corresponding rules for them.
            // this.BuildEngine.ProjectFileOfTaskNode
        }
    }
}
