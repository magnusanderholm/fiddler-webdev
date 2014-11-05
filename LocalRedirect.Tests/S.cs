using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VSAutoResponder.Tests
{
    [Serializable()]
    public class S
    {
        public static readonly int Integer = 10;

        public S()
        {
        }

        public string Value { get; set; }
    }
}
