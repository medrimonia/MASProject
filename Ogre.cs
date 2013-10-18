using System;
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

        private string entityName;
        private string nodeName;

        public Ogre(SceneManager sm, int ogreId, Vector3 initialLocation)
        {
            entityName = "OgreHead" + ogreId;
            nodeName = "OgreHeadNode" + ogreId;
            ent = sm.CreateEntity(entityName,"ogrehead.mesh");
            node = sm.RootSceneNode.CreateChildSceneNode(nodeName, initialLocation);
            node.AttachObject(ent);
            updateGoal();
        }

        private void updateGoal()
        {
            goal = OgreFactory.randomLocation();
        }

        public override void mutate(double elapsedTime)
        {
            float speed = 0.5f;
            float minDist = 10;
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
    }
}
