﻿using System;
using System.Collections.Generic;
using Mogre;
using MASProject.Communication;
using MASProject.Utils;

namespace MASProject
{
    public enum OgreGender
    {
        Male,
        Female
    }

    class Ogre : GraphicalAgent
    {

        /* The robot will often have a goal to which he can't move in one
         * frame. It will be stored and used for the further moves in order
         * to spare computation time.
         */
        private Vector3 goal;

        /* Each ogre head has it's own age [s] */
        private float age;

        private OgreGender gender;

        private float highestStoneDensity;
        private Vector3 highestStoneDensityPos;
        private Stone carriedStone;

        private float gripRadius;

        /* After a certain age, ogreHeads stop growing [s] */
        private static float fullSizeAge = 30f;
        private static float fertilityStart = 20f;
        private static float menopauseStart = 40f;//[s]
        /* The time an ogre is expected to live [s] */
        public static float longevity = 50;
        private static float minSize = 60f;
        private static float maxSize = 60f;
        private float pregnancyStart;
        private bool isPregnant = false;

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

        public Ogre(SceneManager sm, int ogreId, Vector3 initialLocation, float visionRadius, OgreGender gender, float originalAge=0f)
            : base()
        {
            age = originalAge;
            this.gender = gender;
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

        private void treatMessage(DensityMessage m)
        {
            if (m.Density > highestStoneDensity)
            {
                highestStoneDensity = m.Density;
                highestStoneDensityPos = m.Position;
            }
        }

        private void treatMessage(Message m)
        {
            switch (m.Type)
            {
                case MessageType.DensityMessage:
                    treatMessage((DensityMessage)m);
                    break;
                default:
                    break;
            }
        }

        private void treatReceivedMessages()
        {
            while (messagesBuffer.Count > 0)
            {
                treatMessage(messagesBuffer.Dequeue());
            }
        }

        private void communicationMutation(List<Ogre> nearbyOgres)
        {
            Utils.DebugUtils.writeMessage("CommunicationMutation");
            foreach (Ogre o in nearbyOgres)
            {
                if (!o.Equals(this))
                {
                    o.receiveMessage(new DensityMessage(highestStoneDensityPos, highestStoneDensity));
                }
            }
        }

        private bool Fertile
        {
            get { return !isPregnant && age > fertilityStart && age < menopauseStart; }
        }

        private void loveMutation()
        {
            switch (gender)
            {
                case OgreGender.Female:
                    if (Fertile)
                    {
                        //TODO send message each half second as example
                    }
                    break;
                case OgreGender.Male:
                    //TODO try to copulate with every women of the neighborhood
                    break;
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

        private void deathMutation(World w, float elapsedTime)
        {
            // probability of dying during the elapsed time is :
            // integral{from : t=age - elapsedTime, to : t = age} p(t)
            // We know that
            // integral{from : t=0, to : t=maxAge} p(t) = 1
            // We have to choose a function p(t) respecting that condition
            // if we choose p(t) to be in a form as : p(t) = a * t^3,
            // We got P(t) = a /4 * t^4 + K
            // P(maxAge) - P(0) = 1
            // maxAge ^4 * a/4 + K - K = 1
            // -> a = 4 / maxAge ^4
            double lastAge = age - elapsedTime;
            double PNow = System.Math.Pow(age / longevity, 4);
            double PBefore = System.Math.Pow(lastAge / longevity, 4);
            double pDie = PNow - PBefore;
            double dice = WorldUtils.RndGen.NextDouble();
            if (dice < pDie)
            {
                die(w);
            }
        }

        private void die(World w)
        {
            if (carriedStone != null)
            {
                releaseStone(w, Position);
            }
            w.release(this);
        }

        public override void mutate(float elapsedTime, World w)
        {
            // Age Mutation
            age += elapsedTime;
            updateSize();
            List<Ogre> nearbyOgres = w.nearbyOgres(this, this.visionRadius);
            List<Stone> nearbyStones = w.nearbyStones(this, this.visionRadius);
            if (carriedStone != null)
            {
                dropMutation(w, nearbyStones);
            }
            else if (nearbyStones.Count > 0)
            {
                captureMutation(w, nearbyStones);
            }
            //TODO avoid collision
            moveMutation(elapsedTime);
            communicationMutation(nearbyOgres);
            deathMutation(w, elapsedTime);
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
            double neededScore = 1f - System.Math.Pow(0.95f, nearbyStones.Count);
            double toHighestDensity = (Position - highestStoneDensity).Length;
            if (WorldUtils.RndGen.NextDouble() > neededScore || toHighestDensity > 500)
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
            double neededScore = System.Math.Pow(0.95f, nearbyStones.Count);
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