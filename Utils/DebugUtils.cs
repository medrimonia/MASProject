using System;
using Mogre;

namespace MASProject.Utils
{
    class DebugUtils
    {
        private static System.IO.StreamWriter debugFile = new System.IO.StreamWriter("debugLogs.txt");

        public static void writeMessage(string msg)
        {
            debugFile.WriteLine(msg);
            debugFile.Flush();
            Overlays.DebugOverlay.WriteLine(msg);
        }

        public static void writeMessage(GraphicalObject o, string msg)
        {
            debugFile.WriteLine(o.Entity.Name + " : " + msg);
            Overlays.DebugOverlay.WriteLine(msg);
        }
    }
}
