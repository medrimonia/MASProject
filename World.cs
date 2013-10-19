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

        private List<GraphicalObject> objects;

        public World(SceneManager sm, int nbOgre, int nbRobots, int nbStones)
        {
            oFactory = new OgreFactory();
            sFactory = new StoneFactory();
            // Creating the ground
            addPlane(sm);

            objects = new List<GraphicalObject>();
            // Creating the ogres
            for (int i = 0; i < nbOgre; i++)
            {
                objects.Add(oFactory.create(sm));
            }
            // Creating the stones
            for (int i = 0; i < nbStones; i++)
            {
                objects.Add(sFactory.create(sm));
            }
        }

        private void addPlane(SceneManager sm)
        {
            Plane plane = new Plane(Vector3.UNIT_Y, 0);
            MeshManager.Singleton.CreatePlane("ground",
    ResourceGroupManager.DEFAULT_RESOURCE_GROUP_NAME, plane,
    WorldUtils.Width, WorldUtils.Depth, 20, 20, true, 1, 5, 5, Vector3.UNIT_Z);

            Entity groundEnt = sm.CreateEntity("GroundEntity", "ground");
            sm.RootSceneNode.CreateChildSceneNode().AttachObject(groundEnt);
        }

        public List<GraphicalObject> neighborHood(GraphicalAgent a)
        {
            List<GraphicalObject> neighbors = new List<GraphicalObject>();
            foreach (GraphicalObject o in objects)
            {
                if (!a.Equals(o) && a.canSee(o))
                {
                    neighbors.Add(o);
                }
            }
            return neighbors;
        }

        public void mutate(float elapsedTime)
        {
            foreach (GraphicalObject o in objects)
            {
                GraphicalAgent a = o as GraphicalAgent;
                if (a != null)
                {
                    a.mutate(elapsedTime, neighborHood(a));
                }
            }
        }


    }
}
