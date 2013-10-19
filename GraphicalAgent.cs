using System;
using System.Collections.Generic;
using Mogre;

namespace MASProject
{
    abstract class GraphicalAgent : GraphicalObject
    {
        protected float visionRadius;

        /* At each step, an agent will perform a mutation according to the
         * elapsed time.
         */
        public abstract void mutate(float elapsedTime, List<GraphicalObject> neighbors);

        public bool canSee(GraphicalObject a)
        {
            return (Position - a.Position).Length < visionRadius;
        }
    }
}
