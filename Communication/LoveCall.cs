using System;
using Mogre;
using MASProject.Objects;

namespace MASProject.Communication
{
    class LoveCall : Message
    {
        private Vector3 src;

        public LoveCall(Objects.OgreAgent o) : base(MessageType.LoveCall)
        {
            this.src = o.Position;
        }

        public Vector3 Source
        {
            get { return src; }
        }
    }
}
