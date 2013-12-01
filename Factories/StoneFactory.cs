using System;
using Mogre;
using MASProject.Utils;

namespace MASProject.Factories
{
    class StoneFactory : GraphicalObjectFactory
    {
        public override GraphicalObject create(SceneManager sm)
        {
            Stone s = new Stone(sm, nbObjectsCreated++, WorldUtils.RandomLocation);
            return s;
        }
    }
}
