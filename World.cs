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

        private OgreFactory oFactory;
        private StoneFactory sFactory;

        private List<Ogre> ogres;
        private List<Stone> stones;
        private SceneManager sm;

        public World(SceneManager sm, int nbOgre, int nbRobots, int nbStones)
        {
            this.sm = sm;
            oFactory = new OgreFactory();
            sFactory = new StoneFactory();
            // Creating the ground
            addPlane();

            ogres = new List<Ogre>();
            // Creating the ogres
            for (int i = 0; i < nbOgre; i++)
            {
                ogres.Add((Ogre)oFactory.create(sm));
            }
            stones = new List<Stone>();
            // Creating the stones
            for (int i = 0; i < nbStones; i++)
            {
                stones.Add((Stone)sFactory.create(sm));
            }
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

        public List<Ogre> nearbyOgres(GraphicalAgent a, float maxDist)
        {
            List<Ogre> neighbors = new List<Ogre>();
            foreach (Ogre o in ogres)
            {
                if (!a.Equals(o) && a.canSee(o))
                {
                    neighbors.Add(o);
                }
            }
            return neighbors;
        }

        public List<Stone> nearbyStones(GraphicalAgent a, float maxDist)
        {
            List<Stone> neighbors = new List<Stone>();
            foreach (Stone s in stones)
            {
                if (!a.Equals(s) && a.canSee(s))
                {
                    neighbors.Add(s);
                }
            }
            return neighbors;
        }

        public void mutate(float elapsedTime)
        {
            //TODO shuffle objects at each mutation
            /* using ToArray because some objects might be removed during the
             * loop
             */
            foreach (Ogre o in ogres.ToArray())
            {
                DateTime agentStart = DateTime.Now;
                o.mutate(elapsedTime, this);
                TimeSpan agentDuration = DateTime.Now - agentStart;
                Utils.DebugUtils.writeMessage("\tAgent time :" + agentDuration.ToString());
            }
        }

        public void release(Stone s)
        {
            sm.RootSceneNode.RemoveChild(s.Node);
            stones.Remove(s);
        }

        public void acquire(Stone s)
        {
            sm.RootSceneNode.AddChild(s.Node);
            stones.Add(s);
        }

    }
}
