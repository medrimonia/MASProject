﻿using System;
using System.Collections.Generic;
using Mogre;
using MogreFramework;
using MASProject.Communication;
using MASProject.Utils;




namespace MASProject.Objects
{
    class Robot : GraphicalAgent
    {   
        private static float GRIP_RADIUS = 100;

        private LinkedList<Vector3> mWalkList = null; // A doubly linked containing the waypoints -> une par robot
        private AnimationState mAnimationState = null; //The AnimationState the moving object
        private float mDistance = 0.0f;              //The distance the object has left to travel
        private Vector3 mDirection = Vector3.ZERO;   // The direction the object is moving
        private Vector3 mDestination = Vector3.ZERO; // The destination the object is moving towards

        private float age;

        private float highestStoneDensity;
        private Vector3 highestStoneDensityPos;
        private Vector3 LastDropPosition;


        float mWalkSpeed = 75.0f;  // The speed at which the object is moving


        public Robot(SceneManager sm, int robotId, Vector3 initialLocation, Vector3 initialGoal)
        {

            string nodeName = "RobotNode" + robotId;
            string entityName = "Robot" + robotId;
            mWalkList = new LinkedList<Vector3>();

            age = 0f;
            visionRadius = 100;

            carriedStone = null;
            ent = sm.CreateEntity(entityName, "robot.mesh");

            // Create the Robot's SceneNode
            node = sm.RootSceneNode.CreateChildSceneNode(nodeName,initialLocation);
            node.AttachObject(ent);

            mAnimationState = ent.GetAnimationState("Idle");

            addGoal(initialGoal);
            updateGoal(WorldUtils.RandomLocation);

        }

        public override void mutate(float elapsedTime, World w)
        {
            // Age Mutation
            age += elapsedTime;

            List<Stone> nearbyStones = w.nearbyStones(this, this.visionRadius);

            
            if (carriedStone == null & nearbyStones.Count > 0)
            {
                captureMutation(w, nearbyStones);
            }
            if (mDistance <= 0.0f)
            {
                if (carriedStone != null)
                {
                dropMutation(w, nearbyStones);
                }
                if (nextLocation())
                {
                    
                    addGoal(WorldUtils.RandomLocation);
                    TurnNextLocation();
                    
                }
                else
                {
                    mAnimationState = ent.GetAnimationState("Idle");
                }
            }
            else
            {
                moveMutation(elapsedTime);
            }
            
            mAnimationState.AddTime(elapsedTime * mWalkSpeed / 20);
            
        }

        private void dropMutation(World w, List<Stone> nearbyStones)
        {
            double neededScore = 1f - System.Math.Pow(0.95f, nearbyStones.Count);
            float density = (float)(nearbyStones.Count / System.Math.Pow(visionRadius, 2));
            float totalX = 0;
            float totalZ = 0;
            foreach (Stone s in nearbyStones)
            {
                totalX += s.Position.x;
                totalZ += s.Position.z;
            }

            float avgX =  nearbyStones.Count>0?totalX / nearbyStones.Count : this.Position.x;
            float avgY = this.Position.y + carriedStone.BoundingBox.HalfSize.y;//carriedStone.BoundingBox.Minimum.y;
            float avgZ = nearbyStones.Count > 0 ? totalZ / nearbyStones.Count : this.Position.z;
            double tohighestDensity = (Position - highestStoneDensityPos).Length;

            Vector3 center = new Vector3(avgX, avgY, avgZ);
            if (WorldUtils.RndGen.NextDouble() > neededScore || density < 200 )
            {
                releaseStone(w, center);
                LastDropPosition = this.Position;
                updateGoal(highestStoneDensityPos);
            }
        }

        private void captureMutation(World w, List<Stone> nearbyStones)
        {
            if (nearbyStones.Count == 0) return;
            double neededScore = System.Math.Pow(0.95f, nearbyStones.Count);
            double tohighestDensity = (Position - highestStoneDensity).Length;
            double dtoLDP = (Position - LastDropPosition).Length;
            if (WorldUtils.RndGen.NextDouble() > neededScore || tohighestDensity < 300 )
            {
                foreach (Stone s in nearbyStones)
                {
                    if ((s.Position - Position).Length < GRIP_RADIUS)
                    {
                        captureStone(w, s);
                        updateGoal(WorldUtils.RandomLocation);
                        break;
                    }
                }
            }
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
        public void moveMutation(float elapsedTime)
        {
            float move = mWalkSpeed * elapsedTime;
            mDistance -= move;

            node.Translate(mDirection * move);
        }
        private void addGoal(Vector3 goal)
        {
            this.mWalkList.AddLast(goal);
        }

        private void updateGoal(Vector3 NextLocation)
        {
            mWalkList.RemoveFirst();
            addGoal(NextLocation);
            TurnNextLocation();

        }

        protected bool nextLocation()
        {
            if (mWalkList.Count == 0)
                return false;
            return true;
        }
        private void TurnNextLocation()
        {
                //Start the walk animation
                mAnimationState = ent.GetAnimationState("Walk");
                mAnimationState.Loop = true;
                mAnimationState.Enabled = true;

                LinkedListNode<Vector3> tmp;  //temporary listNode

                mDestination = mWalkList.First.Value; //get the next destination.

                tmp = mWalkList.First; //save the node that held it
                mWalkList.RemoveFirst(); //remove that node from the front of the list
                mWalkList.AddLast(tmp);  //add it to the back of the list.

                //update the direction and the distance
                mDirection = mDestination - node.Position;
                mDistance = mDirection.Normalise();

                orientateToDestination(mDestination);

        }

    }
}
