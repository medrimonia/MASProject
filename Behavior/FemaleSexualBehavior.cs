using System;
using MASProject.Communication;
using MASProject.Objects;

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

        public override bool readyForPregnancy(OgreAgent o)
        {
            return !isPregnant && o.Age > fertilityStart && o.Age < menopauseStart && o.SmoothedDensity < densityThreshold;
        }

        private void giveBirth(World w, OgreAgent o)
        {
            lastPregnancy = o.Age;
            isPregnant = false;
            w.createBabyOgre(o.Position);
        }

        public override void apply(World w, OgreAgent o)
        {
            if (readyForPregnancy(o) && (o.Age - lastLoveCall) > 1.0 / loveCallFrequency)
            {
                Activity = true;
                Message m = new LoveCall(o);
                foreach (OgreAgent n in w.nearbyOgres(o, loveCallRange))
                {
                    n.receiveMessage(m);
                }
                lastLoveCall = o.Age;
            }
            if (isPregnant){
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
