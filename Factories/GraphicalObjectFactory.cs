using System;
using Mogre;
using MASProject.Utils;
using MASProject.Objects;

/* Only a factory by tuple {class, SceneManager} */
namespace MASProject.Factories
{
    abstract class GraphicalObjectFactory
    {
        protected int nbObjectsCreated;

        protected GraphicalObjectFactory()
        {
            nbObjectsCreated = 0;
        }

        public abstract GraphicalObject create(SceneManager sm);
    }
}
