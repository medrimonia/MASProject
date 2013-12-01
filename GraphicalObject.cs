using System;
using Mogre;

namespace MASProject
{
	abstract class GraphicalObject
    {
        protected bool useable;
        protected SceneNode node;
        protected Entity ent;

        public GraphicalObject()
        {
            useable = false;
            node = null;
            ent = null;
        }

        /// <summary>
        /// Some Objects can't be used or are already removed
        /// </summary>
        public bool Useable
        {
            get { return useable; }
            set { useable = value; }
        }

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

        public virtual Quaternion Orientation
        {
            get { return node.Orientation; }
        }

        public virtual Quaternion CameraOrientation
        {
            get { return Orientation; }
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

        public void orientateToDestination(Vector3 dst)
        {
            Vector3 direction = dst - node.Position;
            direction.y = 0f;
            direction.Normalise();

            Vector3 src = Orientation * Vector3.UNIT_X;

            if ((1.0f + src.DotProduct(direction)) < 0.0001f)
            {
                node.Yaw(new Angle(180.0f));
            }
            else
            {
                Quaternion quat = src.GetRotationTo(direction);
                node.Rotate(quat);
            }
        }
    }
}
