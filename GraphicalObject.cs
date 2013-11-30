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
            get { return BoundingBox.Center; }
        }

        public AxisAlignedBox BoundingBox
        {
            get {
                AxisAlignedBox b =  new AxisAlignedBox(ent.BoundingBox);
                b.TransformAffine(Matrix4.GetScale(node.GetScale()));
                b.TransformAffine(Matrix4.GetTrans(node.Position));
                return b;
            }
       }

        public void placeOnGround()
        {
            float yWished = Position.y - BoundingBox.Minimum.y;
            Node.SetPosition(Position.x, yWished, Position.z);
        }

	}
}
