using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MASProject.Objects;

namespace MASProject.Behavior
{
    class MaleSexualBehavior : SexualBehavior
    {

        private static float loveDistance = 50;

        public static float LoveDist
        {
            get { return loveDistance; }
        }

        public override bool readyToInseminate(double age)
        {
            return true;
        }

        public override void apply(World w, OgreAgent o)
        {
            foreach (OgreAgent n in w.nearbyOgres(o, loveDistance))
            {
                if (n.isFertile())
                {
                    n.inseminate();
                    //After having inseminate a female, ogre loose attraction until next lovecall
                    Activity = false;
                }
            }
        }
    }
}
