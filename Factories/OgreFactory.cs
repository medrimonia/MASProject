using System;
using Mogre;
using MASProject.Utils;
using MASProject.Objects;

namespace MASProject.Factories
{
    class OgreFactory : GraphicalObjectFactory
    {
        //TODO: add "blind" ogres?
        private float defaultVisionRadius ;

        public OgreFactory(float visionRadius = 300) : base()
        {
            defaultVisionRadius = visionRadius;
        }

        public override GraphicalObject create(SceneManager sm)
        {
            float age = OgreAgent.Longevity * (float)WorldUtils.RndGen.NextDouble();
            OgreGender g = OgreGender.Female;
            if (Utils.WorldUtils.RndGen.NextDouble() < 0.5)
            {
                g = OgreGender.Male;
            }
            return new OgreAgent(sm, nbObjectsCreated++, WorldUtils.RandomLocation, defaultVisionRadius, g, age);
        }

        public GraphicalObject createBaby(SceneManager sm)
        {
            float age = 0;
            OgreGender g = OgreGender.Female;
            if (Utils.WorldUtils.RndGen.NextDouble() < 0.5)
            {
                g = OgreGender.Male;
            }
            return new OgreAgent(sm, nbObjectsCreated++, WorldUtils.RandomLocation, defaultVisionRadius, g, age);
        }
    }
}
