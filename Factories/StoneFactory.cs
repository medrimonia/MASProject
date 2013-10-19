using System;
using Mogre;
using MASProject.Utils;

namespace MASProject.Factories
{
    class StoneFactory : GraphicalObjectFactory
    {
        public override GraphicalObject create(SceneManager sm)
        {
            return new Stone(sm, nbObjectsCreated++, WorldUtils.RandomLocation);
        }
    }
}
