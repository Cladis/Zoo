﻿using System;
using System.Collections.Generic;

namespace Zoo
{
    /// <summary>
    /// A simple class for parsing console input of this application
    /// I do realise that I can just use one of the innumerable libraries for this,
    /// it just feels like an overkill for this limited scale project
    /// </summary>
    public class ConsoleCommand
    {
        public string Key { get; set; }
        public List<string> Flags { get; set; }
        public Dictionary<string, string> FlagValues { get; set; } = new Dictionary<string, string>();
        public string Description { get; set; }
        public List<string> Example { get; set; }
        public Action<Dictionary<string, string>> Action { get; set; }

        public ConsoleCommand(string key, IEnumerable<string> flags, string description,
            Action<Dictionary<string, string>> action, IEnumerable<string> example = null)
        {
            Key = key;
            Flags = new List<string>(flags);
            Description = description;
            Action = action;
            Example = new List<string>(example);
        }

        public override string ToString()
        {
            return Key + "\t" + Description;
        }

        public void Print()
        {
            Console.WriteLine(ToString());
        }

        public void Describe()
        {
            Print();
            Example?.ForEach(
                Console.WriteLine
            );
        }

    }
}
