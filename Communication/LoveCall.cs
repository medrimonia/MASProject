using System;
using Mogre;

namespace MASProject.Communication
{
    class LoveCall : Message
    {
        private Vector3 src;

        public LoveCall(Ogre o) : base(MessageType.LoveCall)
        {
            this.src = o.Position;
        }

        public Vector3 Source
        {
            get { return src; }
        }
    }
}
