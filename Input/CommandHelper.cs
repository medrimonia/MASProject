using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MASProject.Input
{
    class CommandHelper
    {
        private char shortcut;
        private string description;

        public CommandHelper(char shortcut, string description)
        {
            this.shortcut = shortcut;
            this.description = description;
        }

        public override string ToString()
        {
            return shortcut + " : " + description;
        }
    }
}
