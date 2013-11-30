using System;
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
            Utils.DebugUtils.writeMessage("adding global command");
            foreach (CommandHelper c in globalCommands)
            {
                commands.Add(c);
            }
            Utils.DebugUtils.writeMessage("adding internal command");
            foreach (CommandHelper c in internalCommands(mode))
            {
                commands.Add(c);
            }
            Utils.DebugUtils.writeMessage("internal commands added");
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
            globalCommands.Add(new CommandHelper('h', "toggle the helper on and off"));
            globalCommands.Add(new CommandHelper('x', "return to global mode"));
            #endregion
            specificCommands = new Dictionary<InputManager.InputMode, List<CommandHelper>>();
            InputManager.InputMode m;
            #region No mode commands
            m = InputManager.InputMode.None;
            specificCommands.Add(m, new List<CommandHelper>());
            addCommand(m, new CommandHelper('l', "activating light control"));
            addCommand(m, new CommandHelper('f', "activating fog control"));
            #endregion
            #region Light commands
            m = InputManager.InputMode.Light;
            specificCommands.Add(m, new List<CommandHelper>());
            addCommand(m, new CommandHelper('d', "activate day mode"));
            addCommand(m, new CommandHelper('n', "activate night mode"));
            addCommand(m, new CommandHelper('c', "activating cycle mode (day and night alterning)"));
            #endregion
            #region Fog commands
            m = InputManager.InputMode.Fog;
            specificCommands.Add(m, new List<CommandHelper>());
            addCommand(m, new CommandHelper('t', "toggle fog on/off"));
            #endregion
        }
    }
}
