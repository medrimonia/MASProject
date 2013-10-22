using System;
using System.Collections.Generic;
using Mogre;
using MASProject.Communication;

namespace MASProject
{
    abstract class GraphicalAgent : GraphicalObject
    {
        protected float visionRadius;

        protected Queue<Message> messagesBuffer;

        protected GraphicalAgent()
        {
            messagesBuffer = new Queue<Message>();
        }

        /* At each step, an agent will perform a mutation according to the
         * elapsed time.
         */
        public abstract void mutate(float elapsedTime, World w);

        public bool canSee(GraphicalObject a)
        {
            return (Position - a.Position).Length < visionRadius;
        }

        public void receiveMessage(Message m)
        {
            messagesBuffer.Enqueue(m);
        }
    }
}
