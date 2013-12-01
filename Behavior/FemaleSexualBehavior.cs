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
        private static float densityThreshold = 10f;

        public FemaleSexualBehavior()
        {
            lastLoveCall = 0;
            isPregnant = false;
        }

        public override bool readyForPregnancy(Ogre o)
        {
            Utils.DebugUtils.writeMessage(o, "Smoothed density : " + o.SmoothedDensity);
            return !isPregnant && o.Age > fertilityStart && o.Age < menopauseStart && o.SmoothedDensity < densityThreshold;
        }

        private void giveBirth(World w, Ogre o)
        {
            lastPregnancy = o.Age;
            isPregnant = false;
            w.createBabyOgre(o.Position);
        }

        public override void apply(World w, Ogre o)
        {
            if (readyForPregnancy(o) && (o.Age - lastLoveCall) > 1.0 / loveCallFrequency)
            {
                Utils.DebugUtils.writeMessage(o, "Calling for love");
                Activity = true;
                Message m = new LoveCall(o);
                foreach (Ogre n in w.nearbyOgres(o, loveCallRange))
                {
                    n.receiveMessage(m);
                }
                lastLoveCall = o.Age;
            }
            if (isPregnant){
                Utils.DebugUtils.writeMessage(o, "Pregnancy time : " + (o.Age - pregnancyStart));
                if (o.Age - pregnancyStart > pregnancyDuration)
                {
                    giveBirth(w, o);
                }
            }
        }

        public override void inseminate(double age)
        {
            pregnancyStart = age;
            isPregnant = true;
            Activity = false;
        }
    }
}
