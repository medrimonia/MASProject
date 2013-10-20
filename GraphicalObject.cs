using System;
using Mogre;

namespace MASProject
{
	abstract class GraphicalObject
    {
        protected SceneNode node;
        protected Entity ent;

        public SceneNode Node
        {
            get { return node; }
        }

        public Entity Entity
        {
            get { return ent; }
        }

        public Vector3 Position
        {
            get { return node.Position; }
        }

        public AxisAlignedBox BoundingBox
        {
            get { return ent.BoundingBox; }
        }

        public void removeFromWorld(World w)
        {
            
        }

	}
}
