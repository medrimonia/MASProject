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

        /* Each ogre head has it's own age [s] */
        private float age;

        /* After a certain age, ogreHeads stop growing [s] */
        private static float fullSizeAge = 30f;
        /* The time an ogre is expected to live [s] */
        public static float longevity = 50;
        private static float minSize = 5f;
        private static float maxSize = 60f;

        public static float Longevity
        {
            get { return longevity; }
        }

        private float WishedHeight
        {
            get
            {
                if (age < fullSizeAge)
                {
                    return minSize + (maxSize - minSize) * age / fullSizeAge;
                }
                return maxSize;
            }
        }

        public Ogre(SceneManager sm, int ogreId, Vector3 initialLocation, float visionRadius, float originalAge=0f)
        {
            age = originalAge;
            string entityName = "OgreHead" + ogreId;
            string nodeName = "OgreHeadNode" + ogreId;
            ent = sm.CreateEntity(entityName, "ogrehead.mesh");
            node = sm.RootSceneNode.CreateChildSceneNode(nodeName, initialLocation);
            node.AttachObject(ent);
            updateGoal();
            this.visionRadius = visionRadius;
        }

        private void updateSize()
        {
            float actualHeight = ent.BoundingBox.Size.y;
            float ratio = WishedHeight / actualHeight;
            node.SetScale(ratio, ratio, ratio);
            // Fixing the head on the ground whatever the height might be
            float wishedY = -ent.BoundingBox.Minimum.y * ratio;
            node.SetPosition(node.Position.x, wishedY, node.Position.z);

        }

        private void updateGoal()
        {
            goal = OgreFactory.randomLocation();
        }

        private void moveMutation(float elapsedTime)
        {
            float speed = 1.5f;
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

        public override void mutate(float elapsedTime, List<GraphicalAgent> neighbors)
        {
            // Age Mutation
            age += elapsedTime;
            updateSize();
            // If close to another ogre, stop
            if (neighbors.Count == 0)
            {
                moveMutation(elapsedTime);
            }
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
