using System;
using Mogre;
using MASProject.Utils;

namespace MASProject.Factories
{
    class OgreFactory : GraphicalObjectFactory
    {
        //TODO: add "blind" ogres?
        private float defaultVisionRadius ;

        public OgreFactory(float visionRadius = 200) : base()
        {
            defaultVisionRadius = visionRadius;
        }

        public override GraphicalObject create(SceneManager sm)
        {
            float age = Ogre.Longevity * (float)WorldUtils.RndGen.NextDouble();
            return new Ogre(sm, nbObjectsCreated++, WorldUtils.RandomLocation, defaultVisionRadius, age);
        }
    }
}
