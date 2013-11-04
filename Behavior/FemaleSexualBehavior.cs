using System;
using MASProject.Communication;

namespace MASProject.Behavior
{
    class FemaleSexualBehavior : SexualBehavior
    {
        private double pregnancyStart;
        private bool isPregnant;
        private double lastPregnancy;
        private static double pregnancyDuration = 3;
        private double lastLoveCall = 0;
        private double loveCallFrequency = 2;
        private static float fertilityStart = 10f;//[s]
        private static float menopauseStart = 40f;//[s]
        private static float loveCallRange = 1500f;

        public FemaleSexualBehavior()
        {
            lastLoveCall = 0;
            isPregnant = false;
        }

        public override bool readyForPregnancy(double age)
        {
            return !isPregnant && age > fertilityStart && age < menopauseStart;
        }

        private void giveBirth(World w, Ogre o)
        {
            lastPregnancy = o.Age;
            isPregnant = false;
            Utils.DebugUtils.writeMessage("An ogre is trying to give birth");
            w.createBabyOgre(o.Position);
        }

        public override void apply(World w, Ogre o)
        {
            if (readyForPregnancy(o.Age) && (o.Age - lastLoveCall) > 1.0 / loveCallFrequency)
            {
                Utils.DebugUtils.writeMessage("Calling for Love");
                Message m = new LoveCall(o);
                foreach (Ogre n in w.nearbyOgres(o, loveCallRange))
                {
                    n.receiveMessage(m);
                }
                lastLoveCall = o.Age;
            }
            if (isPregnant)
            {
                Utils.DebugUtils.writeMessage("Pregnancy time : " + (o.Age - pregnancyStart));
            }
            if (isPregnant && o.Age - pregnancyStart > pregnancyDuration)
            {
                giveBirth(w, o);
            }
        }

        public override void inseminate(double age)
        {
            pregnancyStart = age;
            isPregnant = true;
        }
    }
}
