using System;
using Mogre;

namespace MASProject
{
    class OgreFactory
    {
        private static Random posGenerator = new Random();
        private static int nbOgresCreated = 0;

        /* This value is used to see the heads as "on" the plane and not
         * in the middle of it.
         */
        private static float defaultY = 23;

        //TODO: add "blind" ogres?
        private static int defaultVisionRadius = 100;

        private static Int32 minX = -1000;
        private static Int32 maxX =  1000;
        private static Int32 minZ = -1000;
        private static Int32 maxZ =  1000;

        public static Vector3 randomLocation()
        {
            int xPos = posGenerator.Next(minX, maxX);
            int zPos = posGenerator.Next(minZ, maxZ);
            return new Vector3(xPos, defaultY, zPos);
        }


        public static Ogre createOgre(SceneManager sm)
        {
            return new Ogre(sm, nbOgresCreated++, randomLocation(), defaultVisionRadius);
        }
    }
}
