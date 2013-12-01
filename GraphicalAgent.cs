using System;
using System.Collections.Generic;
using Mogre;
using MASProject.Communication;
using MASProject.Utils;

namespace MASProject
{
    abstract class GraphicalAgent : GraphicalObject
    {
        protected float visionRadius;
        protected Stone carriedStone;

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

        protected void captureStone(World w, Stone s)
        {
            w.release(s);
            node.AddChild(s.Node);
            s.Node.SetPosition(0, BoundingBox.Maximum.y, 0);
            carriedStone = s;
        }

        protected void releaseStone(World w, Vector3 droppingPosition)
        {
            node.RemoveChild(carriedStone.Node);
            float randomDist = 50;
            WorldUtils.placeRandomly(carriedStone, droppingPosition, randomDist, randomDist, w.neighborhood(droppingPosition, randomDist));
            w.acquire(carriedStone);
            carriedStone = null;
        }
    }
}
