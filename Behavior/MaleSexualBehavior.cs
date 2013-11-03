using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MASProject.Behavior
{
    class MaleSexualBehavior : SexualBehavior
    {

        private static float loveDistance = 150;

        public override void apply(World w, Ogre o)
        {
            foreach (Ogre n in w.nearbyOgres(o, loveDistance))
            {
                if (n.isFertile())
                {
                    Utils.DebugUtils.writeMessage("An ogre is trying to inseminate");
                    n.inseminate();
                }
            }
        }
    }
}
