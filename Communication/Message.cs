using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MASProject.Communication
{
    public enum MessageType
    {
        DensityMessage,
        LoveCall
    };

    abstract class Message
    {
        private MessageType type;

        protected Message(MessageType t)
        {
            type = t;
        }

        public MessageType Type
        {
            get { return type; }
        }
    }
}
