﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MASProject.Input
{
    class CommandDatabase
    {

        /// <summary>
        /// Commands available in all modes
        /// </summary>
        private static List<CommandHelper> globalCommands;
        /// <summary>
        /// Commands available in any mode
        /// </summary>
        private static Dictionary<InputManager.InputMode,List<CommandHelper>> specificCommands;


        public static List<CommandHelper> getCommands(InputManager.InputMode mode)
        {
            List<CommandHelper> commands = new List<CommandHelper>();
            foreach (CommandHelper c in globalCommands)
            {
                commands.Add(c);
            }
            foreach (CommandHelper c in internalCommands(mode))
            {
                commands.Add(c);
            }
            return commands;
        }

        /// <summary>
        /// Return the real list of commands, it shouldn't be modifed!
        /// </summary>
        /// <param name="mode"></param>
        /// <returns></returns>
        private static List<CommandHelper> internalCommands(InputManager.InputMode mode)
        {
            List<CommandHelper> l;
            specificCommands.TryGetValue(mode, out l);
            return l;
        }

        private static void addCommand(InputManager.InputMode m, CommandHelper c){
            List<CommandHelper> l;
            specificCommands.TryGetValue(m, out l);
            l.Add(c);
        }

        static CommandDatabase()
        {
            #region Global Commands
            globalCommands = new List<CommandHelper>();
            globalCommands.Add(new CommandHelper("h", "toggle the helper overlay"));
            globalCommands.Add(new CommandHelper("F1", "toggle the debug overlay"));
            globalCommands.Add(new CommandHelper("F2", "toggle the status overlay"));
            globalCommands.Add(new CommandHelper("SPACE", "pause execution"));
            globalCommands.Add(new CommandHelper("esc", "return to global mode"));
            globalCommands.Add(new CommandHelper("ctrl + esc", "return to global mode"));
            globalCommands.Add(new CommandHelper("directional keys", "move camera source"));
            globalCommands.Add(new CommandHelper("ctrl + directional keys", "rotate camera direction"));
            #endregion
            specificCommands = new Dictionary<InputManager.InputMode, List<CommandHelper>>();
            InputManager.InputMode m;
            #region No mode commands
            m = InputManager.InputMode.None;
            specificCommands.Add(m, new List<CommandHelper>());
            addCommand(m, new CommandHelper("l", "activating light control"));
            addCommand(m, new CommandHelper("f", "activating fog control"));
            #endregion
            #region Light commands
            m = InputManager.InputMode.Light;
            specificCommands.Add(m, new List<CommandHelper>());
            addCommand(m, new CommandHelper("d", "activate day mode"));
            addCommand(m, new CommandHelper("n", "activate night mode"));
            addCommand(m, new CommandHelper("c", "activating cycle mode (day and night alterning)"));
            #endregion
            #region Fog commands
            m = InputManager.InputMode.Fog;
            specificCommands.Add(m, new List<CommandHelper>());
            addCommand(m, new CommandHelper("t", "toggle fog on/off"));
            #endregion
        }
    }
}
