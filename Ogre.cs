using System;
using System.Collections.Generic;
using Mogre;

namespace MASProject
{
    class Ogre : GraphicalAgent
    {

        /* The robot will often have a goal to which he can't move in one
         * frame. It will be stored and used for the further moves in order
         * to spare computation time.
         */
        private Vector3 goal;

        public Ogre(SceneManager sm, int ogreId, Vector3 initialLocation, float visionRadius)
        {
            string entityName = "OgreHead" + ogreId;
            string nodeName = "OgreHeadNode" + ogreId;
            ent = sm.CreateEntity(entityName,"ogrehead.mesh");
            node = sm.RootSceneNode.CreateChildSceneNode(nodeName, initialLocation);
            node.AttachObject(ent);
            updateGoal();
            this.visionRadius = visionRadius;
        }

        private void updateGoal()
        {
            goal = OgreFactory.randomLocation();
        }

        public override void mutate(double elapsedTime, List<GraphicalAgent> neighbors)
        {
            float speed = 1.5f;
            float minDist = 10;
            // If close to another ogre, stop
            if (neighbors.Count != 0)
            {
                return;
            }
            // If we're close to the goal, modify the goal
            if ((goal - node.Position).Length < minDist)
            {
                updateGoal();
            }
            Vector3 toGoal = (goal - node.Position);
            toGoal.Normalise();            
            toGoal *= speed;
            node.Position += toGoal;
        }

        public override bool Equals(object obj)
        {
            Ogre o = obj as Ogre;
            if (o != null)
            {
                return o.ent.Name.Equals(ent.Name);
            }
            return false;
        }
    }
}
