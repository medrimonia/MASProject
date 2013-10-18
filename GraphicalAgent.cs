using System;
using Mogre;

namespace MASProject
{
    abstract class GraphicalAgent
    {
        protected SceneNode node;
        protected Entity ent;

        /* At each step, an agent will perform a mutation according to the
         * elapsed time.
         */
        public abstract void mutate(double elapsedTime);
    }
}
