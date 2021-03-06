﻿using System;
using System.Collections.Generic;
using Mogre;
using MASProject.Factories;
using MASProject.Utils;
using MASProject.Objects;

/* This class will contain every object existing in the world and
 * will allow to access them easily.
 */

namespace MASProject
{
    class World
    {
        private OgreFactory oFactory;
        private StoneFactory sFactory;
        private RobotFactory rFactory;

        private List<GraphicalObject> objects;
        private SceneManager sm;

        public World(SceneManager sm, int nbOgre, int nbRobots, int nbStones)
        {
            DebugUtils.writeMessage("Creating world");
            this.sm = sm;
            oFactory = new OgreFactory();
            sFactory = new StoneFactory();
            rFactory = new RobotFactory();
            // Creating the ground
            addPlane();
            

            objects = new List<GraphicalObject>();
            // Creating the ogres
            for (int i = 0; i < nbOgre; i++)
            {
                GraphicalObject o = oFactory.create(sm);
                objects.Add(o);
                o.Useable = true;
                WorldUtils.placeRandomly(o, Vector3.ZERO, WorldUtils.Width, WorldUtils.Depth, objects);
            }
            // Creating the stones
            for (int i = 0; i < nbStones; i++)
            {
                GraphicalObject o = sFactory.create(sm);
                objects.Add(o);
                o.Useable = true;
                WorldUtils.placeRandomly(o, Vector3.ZERO, WorldUtils.Width, WorldUtils.Depth, objects);
                o.placeOnGround();
            }
            for (int i = 0; i < nbRobots; i++)
            {
                GraphicalObject r = rFactory.create(sm);
                objects.Add(r);
                r.Useable = true;
                WorldUtils.placeRandomly(r, Vector3.ZERO, WorldUtils.Width, WorldUtils.Depth, objects);
            }
            DebugUtils.writeMessage("World created");
        }


        public List<GraphicalObject> RandomizedObjectsCopy
        {
            get
            {
                List<GraphicalObject> tmpCopy = new List<GraphicalObject>();
                List<GraphicalObject> randomized = new List<GraphicalObject>();
                tmpCopy.AddRange(objects);
                while (tmpCopy.Count > 0)
                {
                    int index = Utils.WorldUtils.RndGen.Next(tmpCopy.Count);
                    GraphicalObject o = tmpCopy[index];
                    randomized.Add(o);
                    tmpCopy.Remove(o);
                }
                return randomized;
            }
        }

        /// <summary>
        /// Get the next ogre which is not dead and useable
        /// </summary>
        /// <param name="current"></param>
        /// <returns></returns>
        public GraphicalObject getNextOgre(GraphicalObject current)
        {
            List<GraphicalObject> ogres = Ogres;
            if (ogres.Count == 0) return null;
            int index = 0;
            if (current != null)
            {
                index = (ogres.IndexOf(current ) + 1) % ogres.Count;
                // If given values can't be found, start at zero
                if (index == -1) index = 0;
            }
            int startingIndex = index;
            while (!ogres[index].Useable)
            {
                index = (index + 1) % ogres.Count;
                if (index == startingIndex) return null;
            }
            return ogres[index];
        }

        public List<GraphicalObject> Ogres
        {
            get
            {
                List<GraphicalObject> result = new List<GraphicalObject>();
                foreach (GraphicalObject o in objects)
                {
                    if (o is OgreAgent)
                    {
                        result.Add(o);
                    }
                }
                return result;
            }
        }

        public List<GraphicalObject> Stones
        {
            get
            {
                List<GraphicalObject> result = new List<GraphicalObject>();
                foreach (GraphicalObject o in objects)
                {
                    if (o is Stone)
                    {
                        result.Add(o);
                    }
                }
                return result;
            }
        }

        public List<GraphicalObject> Robot
        {
            get
            {
                List<GraphicalObject> result = new List<GraphicalObject>();
                foreach (GraphicalObject o in objects)
                {
                    if (o is Robot)
                    {
                        result.Add(o);
                    }
                }
                return result;
            }
        }
        
        public int OgresCount
        {
            get { return Ogres.Count; }
        }

        public int FemaleOgresCount
        {
            get
            {
                int n = 0;
                foreach (OgreAgent o in Ogres)
                {
                    if (o.IsFemale)
                    {
                        n++;
                    }
                }
                return n;
            }
        }

        public int MaleOgresCount
        {
            get
            {
                int n = 0;
                foreach (OgreAgent o in Ogres)
                {
                    if (o.IsMale)
                    {
                        n++;
                    }
                }
                return n;
            }
        }

        public void createBabyOgre(Vector3 pos)
        {
            GraphicalObject baby = oFactory.createBaby(sm);
            objects.Add(baby);
            WorldUtils.placeRandomly(baby, pos, 25, 25, objects);
        }

        private void addPlane()
        {
            Plane plane = new Plane(Vector3.UNIT_Y, 0);
            MeshManager.Singleton.CreatePlane("ground",
    ResourceGroupManager.DEFAULT_RESOURCE_GROUP_NAME, plane,
    WorldUtils.Width + WorldUtils.Margin, WorldUtils.Depth + WorldUtils.Margin, 20, 20, true, 1, 5, 5, Vector3.UNIT_Z);

            Entity groundEnt = sm.CreateEntity("GroundEntity", "ground");
            sm.RootSceneNode.CreateChildSceneNode().AttachObject(groundEnt);
            groundEnt.SetMaterialName("Examples/Rockwall");
            
            groundEnt.CastShadows = false;
        }

        public List<GraphicalObject> neighborhood(Vector3 position, float maxDist)
        {
            List<GraphicalObject> neighbors = new List<GraphicalObject>();
            foreach (GraphicalObject o in objects)
            {
                if (o.Useable && (o.Position - position).Length < maxDist)
                {
                    neighbors.Add(o);
                }
            }
            return neighbors;
        }

        public List<OgreAgent> nearbyOgres(GraphicalAgent a, float maxDist)
        {
            List<OgreAgent> neighbors = new List<OgreAgent>();
            foreach (GraphicalObject obj in objects)
            {
                OgreAgent o = obj as OgreAgent;
                if (o != null && o.Useable && !a.Equals(o) && (o.Position - a.Position).Length < maxDist && o is OgreAgent)
                {
                    neighbors.Add(o);
                }
            }
            return neighbors;
        }

        public List<Stone> nearbyStones(GraphicalAgent a, float maxDist)
        {
            List<Stone> neighbors = new List<Stone>();
            foreach (GraphicalObject o in objects)
            {
                Stone s = o as Stone;
                if (s != null && s.Useable && !a.Equals(s) && (s.Position - a.Position).Length < maxDist)
                {
                    neighbors.Add(s);
                }
            }
            return neighbors;
        }

        public void mutate(float elapsedTime)
        {
            foreach (GraphicalObject o in RandomizedObjectsCopy)
            {
                GraphicalAgent a = o as GraphicalAgent;
                if (a != null)
                {
                    DateTime agentStart = DateTime.Now;
                    a.mutate(elapsedTime, this);
                    TimeSpan agentDuration = DateTime.Now - agentStart;
                    //Utils.DebugUtils.writeMessage("\tAgent time :" + agentDuration.ToString());
                }
            }
        }

        public void removeNode(Node n)
        {
            sm.RootSceneNode.RemoveChild(n);
        }

        public void addNode(Node n)
        {
            sm.RootSceneNode.AddChild(n);
        }

        public void release(GraphicalObject o)
        {
            removeNode(o.Node);
            objects.Remove(o);
        }

        public void acquire(GraphicalObject o)
        {
            addNode(o.Node);
            objects.Add(o);
            o.placeOnGround();
        }

    }
}
