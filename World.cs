using System;
using System.Collections.Generic;
using Mogre;
using MASProject.Factories;
using MASProject.Utils;

/* This class will contain every object existing in the world and
 * will allow to access them easily.
 */

namespace MASProject
{
    class World
    {
        private GraphicalObject trackedObject;

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
            trackedObject = null;
            // Creating the ground
            addPlane();

            objects = new List<GraphicalObject>();
            // Creating the ogres
            for (int i = 0; i < nbOgre; i++)
            {
                GraphicalObject o = oFactory.create(sm);
                objects.Add(o);
                WorldUtils.placeRandomly(o, Vector3.ZERO, WorldUtils.Width, WorldUtils.Depth, objects);
            }
            // Creating the stones
            for (int i = 0; i < nbStones; i++)
            {
                GraphicalObject o = sFactory.create(sm);
                objects.Add(o);
                WorldUtils.placeRandomly(o, Vector3.ZERO, WorldUtils.Width, WorldUtils.Depth, objects);
                o.placeOnGround();
            }
            for (int i = 0; i < nbRobots; i++)
            {
                GraphicalObject r = rFactory.create(sm);
                objects.Add(r);
                WorldUtils.placeRandomly(r, Vector3.ZERO, WorldUtils.Width, WorldUtils.Depth, objects);
            }
            DebugUtils.writeMessage("World created");
        }

        public GraphicalObject TrackedObject
        {
            get {return trackedObject;}
        }

        public void trackNext()
        {
            List<GraphicalObject> ogres = Ogres;
            int index = 0;
            if (trackedObject != null)
            {
                index = (ogres.IndexOf(trackedObject) + 1) % ogres.Count;
            }
            trackedObject = ogres[index];
        }

        public List<GraphicalObject> Ogres
        {
            get
            {
                List<GraphicalObject> result = new List<GraphicalObject>();
                foreach (GraphicalObject o in objects)
                {
                    if (o is Ogre){
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
                foreach (Ogre o in Ogres)
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
                foreach (Ogre o in Ogres)
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
            //TODO parameters to choose spawning dist
            WorldUtils.placeRandomly(baby, pos, 25, 25, objects);
        }

        private void addPlane()
        {
            Plane plane = new Plane(Vector3.UNIT_Y, 0);
            MeshManager.Singleton.CreatePlane("ground",
    ResourceGroupManager.DEFAULT_RESOURCE_GROUP_NAME, plane,
    WorldUtils.Width, WorldUtils.Depth, 20, 20, true, 1, 5, 5, Vector3.UNIT_Z);

            Entity groundEnt = sm.CreateEntity("GroundEntity", "ground");
            sm.RootSceneNode.CreateChildSceneNode().AttachObject(groundEnt);
        }

        public List<GraphicalObject> neighborhood(Vector3 position, float maxDist)
        {
            List<GraphicalObject> neighbors = new List<GraphicalObject>();
            foreach (GraphicalObject o in objects)
            {
                if ((o.Position - position).Length < maxDist)
                {
                    neighbors.Add(o);
                }
            }
            return neighbors;
        }

        public List<Ogre> nearbyOgres(GraphicalAgent a, float maxDist)
        {
            List<Ogre> neighbors = new List<Ogre>();
            foreach (GraphicalObject obj in objects)
            {
                Ogre o = obj as Ogre;
                if (o != null && !a.Equals(o) && (o.Position - a.Position).Length < maxDist && o is Ogre)
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
                if (s != null && !a.Equals(s) && (s.Position - a.Position).Length < maxDist)
                {
                    neighbors.Add(s);
                }
            }
            return neighbors;
        }

        public void mutate(float elapsedTime)
        {
            elapsedTime /= 10;// TODO doesn't seem to work
            //TODO shuffle objects at each mutation
            /* using ToArray because some objects might be removed during the
             * loop
             */
            foreach (GraphicalObject o in objects.ToArray())
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

        public void release(GraphicalObject o)
        {
            sm.RootSceneNode.RemoveChild(o.Node);
            objects.Remove(o);
        }

        public void acquire(GraphicalObject o)
        {
            sm.RootSceneNode.AddChild(o.Node);
            objects.Add(o);
            o.placeOnGround();
        }

    }
}
