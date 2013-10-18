using System;
using System.Collections.Generic;
using Mogre;

/* This class will contain every object existing in the world and
 * will allow to access them easily.
 */

namespace MASProject
{
    class World
    {
        private int worldMinX = -2000;
        private int worldMaxX =  2000;
        private int worldMinZ = -2000;
        private int worldMaxZ = 2000;

        private List<GraphicalAgent> agents;


        public int WorldMinX
        {
            get { return worldMinX; }
        }
        public int WorldMaxX
        {
            get { return worldMaxX; }
        }
        public int WorldMinZ
        {
            get { return worldMinZ; }
        }
        public int WorldMaxZ
        {
            get { return worldMaxZ; }
        }
        public int WorldWidth
        {
            get { return WorldMaxX - WorldMinX; }
        }
        public int WorldDepth
        {
            get { return WorldMaxZ - WorldMinZ; }
        }

        public World(SceneManager sm, int nbOgre, int nbRobots)
        {
            // Creating the ground
            addPlane(sm);

            agents = new List<GraphicalAgent>();
            // Creating the ogres
            for (int i = 0; i < nbOgre; i++)
            {
                agents.Add(OgreFactory.createOgre(sm));
            }
        }

        private void addPlane(SceneManager sm)
        {
            Plane plane = new Plane(Vector3.UNIT_Y, 0);
            //TODO size of plane should be related to max locations of objects
            MeshManager.Singleton.CreatePlane("ground",
    ResourceGroupManager.DEFAULT_RESOURCE_GROUP_NAME, plane,
    WorldWidth, WorldDepth, 20, 20, true, 1, 5, 5, Vector3.UNIT_Z);

            Entity groundEnt = sm.CreateEntity("GroundEntity", "ground");
            sm.RootSceneNode.CreateChildSceneNode().AttachObject(groundEnt);
        }

        public void mutate(float elapsedTime)
        {
            foreach (GraphicalAgent a in agents)
            {
                a.mutate(elapsedTime);
            }
        }
    }
}
