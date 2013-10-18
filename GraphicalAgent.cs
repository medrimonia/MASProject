using System;
using System.Collections.Generic;
using Mogre;

namespace MASProject
{
    abstract class GraphicalAgent
    {
        protected SceneNode node;
        protected Entity ent;
        protected float visionRadius;

        public Vector3 Position
        {
            get {return node.Position; }
        }

        public AxisAlignedBox BoundingBox
        {
            get { return ent.BoundingBox;}
        }

        /* At each step, an agent will perform a mutation according to the
         * elapsed time.
         */
        public abstract void mutate(float elapsedTime, List<GraphicalAgent> neighbors);

        public bool canSee(GraphicalAgent a)
        {
            return (Position - a.Position).Length < visionRadius;
        }
    }
}
