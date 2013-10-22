using System;
using System.Collections.Generic;
using Mogre;
using MASProject.Utils;

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


        private float highestStoneDensity;
        private Vector3 highestStoneDensityPos;
        private Stone carriedStone;

        private float gripRadius;

        /* After a certain age, ogreHeads stop growing [s] */
        private static float fullSizeAge = 30f;
        /* The time an ogre is expected to live [s] */
        public static float longevity = 50;
        private static float minSize = 60f;
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
            carriedStone = null;
            ent = sm.CreateEntity(entityName, "ogrehead.mesh");
            node = sm.RootSceneNode.CreateChildSceneNode(nodeName, initialLocation);
            node.AttachObject(ent);
            updateGoal();
            this.visionRadius = visionRadius;
            highestStoneDensity = 0;
            highestStoneDensityPos = Vector3.ZERO;
            gripRadius = visionRadius / 3;
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
            float dist = (node.Position - highestStoneDensityPos).Length;
            if (dist > 1500 && carriedStone != null)
            {
                goal = highestStoneDensityPos;
            }
            else
            {
                goal = WorldUtils.RandomLocation;
            }
        }

        private void moveMutation(float elapsedTime)
        {
            float speed = 5f;
            float minDist = 100;
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

        public override void mutate(float elapsedTime, World w)
        {
            // Age Mutation
            age += elapsedTime;
            updateSize();
            DateTime startLoop = DateTime.Now;
            List<Ogre> nearbyOgres = w.nearbyOgres(this, this.visionRadius);
            List<Stone> nearbyStones = w.nearbyStones(this, this.visionRadius);
            TimeSpan loopDuration = DateTime.Now - startLoop;
            Utils.DebugUtils.writeMessage("\t\tloopDuration : " + loopDuration.ToString());
            if (carriedStone != null)
            {
                DateTime start = DateTime.Now;
                dropMutation(w, nearbyStones);
                TimeSpan duration = DateTime.Now - start;
                DebugUtils.writeMessage("\t\tdropMutation : " + duration.ToString());
            }
            else if (nearbyStones.Count > 0)
            {
                DateTime start = DateTime.Now;
                captureMutation(w, nearbyStones);
                TimeSpan duration = DateTime.Now - start;
                DebugUtils.writeMessage("\t\tcaptureMutation : " + duration.ToString());
            }
            //TODO avoid collision
            DateTime start2 = DateTime.Now;
            moveMutation(elapsedTime);
            TimeSpan duration2 = DateTime.Now - start2;
            DebugUtils.writeMessage("\t\tMoveMutation : " + duration2.ToString());
        }

        private void updateStoneDensity(List<Stone> nearbyStone)
        {
            float density = (float)(nearbyStone.Count / System.Math.Pow(visionRadius, 2));
            if (density > highestStoneDensity)
            {
                highestStoneDensity = density;
                highestStoneDensityPos = node.Position;
            }
        }

        /* An idea found on this page: http://liris.cnrs.fr/simon.gay/index.php?page=sma&lang=en
         * is that we can use a probability inversely proportional to the amount of resources for
         * taking an object and a probability proportional to the amount of resources for dropping
         * the resource
         */
        
        /* This methods capture a stone with a probability inversely
         * proportional to the number of available stones.
         * nearbyStones must 
         */
        private void captureMutation(World w, List<Stone> nearbyStones)
        {
            if (nearbyStones.Count == 0) return;
            double neededScore = 1f - System.Math.Pow(0.9f, nearbyStones.Count);
            if (WorldUtils.RndGen.NextDouble() > neededScore)
            {
                foreach (Stone s in nearbyStones)
                {
                    if ((s.Position - Position).Length < gripRadius)
                    {
                        captureStone(w, s);
                        updateGoal();
                        break;
                    }
                }
            }
        }

        private void dropMutation(World w, List<Stone> nearbyStones)
        {
            double neededScore = System.Math.Pow(0.9f, nearbyStones.Count);
            float totalX = 0;
            float totalZ = 0;
            //TODO use a barycenter function
            foreach (Stone s in nearbyStones)
            {
                totalX += s.Position.x;
                totalZ += s.Position.z;
            }
            //TODO use parameters
            float avgX = totalX / nearbyStones.Count;
            float avgZ = totalZ / nearbyStones.Count;
            Vector3 center = new Vector3(avgX, carriedStone.BoundingBox.Minimum.y, avgZ);
            if (WorldUtils.RndGen.NextDouble() > neededScore)
            {
                releaseStone(w, center);
                updateGoal();
            }
        }

        private void captureStone(World w, Stone s)
        {
            w.release(s);
            node.AddChild(s.Node);
            s.Node.SetPosition(0, BoundingBox.Maximum.y,0);
            carriedStone = s;
        }

        private void releaseStone(World w, Vector3 droppingPosition)
        {
            node.RemoveChild(carriedStone.Node);
            float randomDist = 50;
            WorldUtils.placeRandomly(carriedStone, droppingPosition, randomDist, randomDist, w.neighborhood(droppingPosition, randomDist));
            w.acquire(carriedStone);
            carriedStone = null;
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
