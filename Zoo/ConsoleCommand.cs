using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zoo
{
    /// <summary>
    /// A simple class for parsing console input of this application
    /// I do realise that I can just use one of the innumerable libraries for this,
    /// it just feels like an overkill for this limited scale project
    /// </summary>
    class ConsoleCommand
    {
        public String Key { get; set; }
        public HashSet<String> Flags { get; set; }
        public String Description { get; set; }
        public String Example;

        public override string ToString()
        {
            return base.ToString();
        }

    }
}
