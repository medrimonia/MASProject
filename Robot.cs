using System;
using System.Collections.Generic;
using Mogre;
using MogreFramework;
using MASProject.Communication;
using MASProject.Utils;

namespace MASProject
{
    class Robot : GraphicalAgent
    {
        private LinkedList<Vector3> mWalkList = null; // A doubly linked containing the waypoints -> une par robot
        private AnimationState mAnimationState = null; //The AnimationState the moving object
        private float mDistance = 0.0f;              //The distance the object has left to travel
        private Vector3 mDirection = Vector3.ZERO;   // The direction the object is moving
        private Vector3 mDestination = Vector3.ZERO; // The destination the object is moving towards
        private float age;

        float mWalkSpeed = 50.0f;  // The speed at which the object is moving

        public Robot(SceneManager sm, int robotId, Vector3 initialLocation, Vector3 initialGoal)
        {

            string nodeName = "RobotNode" + robotId;
            string entityName = "Robot" + robotId;
            mWalkList = new LinkedList<Vector3>();

            age = 0f;

            ent = sm.CreateEntity(entityName, "robot.mesh");

            // Create the Robot's SceneNode
            node = sm.RootSceneNode.CreateChildSceneNode(nodeName,initialLocation);
            node.AttachObject(ent);

            mAnimationState = ent.GetAnimationState("Idle");
//            mAnimationState.Loop = true;
//            mAnimationState.Enabled = true;

            addGoal(initialGoal);
            updateGoal();

        }

        public override void mutate(float elapsedTime, World w)
        {
            // Age Mutation
            age += elapsedTime;
            if (mDistance <= 0.0f)
            {
                if (nextLocation())
                {
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

        private void updateGoal()
        {
            

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

                Vector3 src = node.Orientation * Vector3.UNIT_X;

                if ((1.0f + src.DotProduct(mDirection)) < 0.0001f)
                {
                    node.Yaw(new Angle(180.0f));
                }
                else
                {
                    Quaternion quat = src.GetRotationTo(mDirection);
                    node.Rotate(quat);
                }

        }

    }
}
