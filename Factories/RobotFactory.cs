using System;
using Mogre;
using MASProject.Utils;

namespace MASProject.Factories
{
    class RobotFactory : GraphicalObjectFactory
    {

        public RobotFactory() : base()
        {
        }

        public override GraphicalObject create(SceneManager sm)
        {
            return new Robot(sm, nbObjectsCreated++, WorldUtils.RandomLocation, WorldUtils.RandomLocation);
        }
    }
}
