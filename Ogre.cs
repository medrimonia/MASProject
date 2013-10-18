using System;
using Mogre;

namespace MASProject
{
    class Ogre : GraphicalAgent
    {
        private static float defaultY = 23;

        public Ogre(SceneManager sm, int ogreId, int posX, int posZ)
        {
            Vector3 location = new Vector3(posX, defaultY, posZ);
            ent = sm.CreateEntity("OgreHead" + ogreId,"ogrehead.mesh");
            node = sm.RootSceneNode.CreateChildSceneNode("OgreHeadNode" + ogreId, location);
            node.AttachObject(ent);
        }
    }
}
