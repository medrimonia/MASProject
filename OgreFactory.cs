using System;
using Mogre;

namespace MASProject
{
    class OgreFactory
    {
        private static Random posGenerator = new Random();
        private static int nbOgresCreated = 0;

        private static Int32 minX = -1000;
        private static Int32 maxX =  1000;
        private static Int32 minZ = -1000;
        private static Int32 maxZ =  1000;


        public static Ogre createOgre(SceneManager sm)
        {
            int xPos = posGenerator.Next(minX, maxX);
            int zPos = posGenerator.Next(minZ, maxZ);
            return new Ogre(sm, nbOgresCreated++, xPos, zPos);
        }
    }
}
