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
        public string Key { get; set; }
        public HashSet<string> Flags { get; set; }
        public string Description { get; set; }
        public string Example;

        public override string ToString()
        {
            return base.ToString();
        }

    }
}
