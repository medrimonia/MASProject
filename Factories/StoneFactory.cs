using System;
using Mogre;
using MASProject.Utils;
using MASProject.Objects;

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
