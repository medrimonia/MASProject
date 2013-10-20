﻿using System;
using Mogre;

namespace MASProject.Utils
{
    class WorldUtils
    {
        private static Random rndGen = new Random();
        private static Vector3 min = new Vector3(-500, 0, -500);
        private static Vector3 max = new Vector3( 500, 0,  500);

        public static Vector3 Min
        {
            get { return min; }
        }

        public static Vector3 Max
        {
            get { return max; }
        }

        public static float Width
        {
            get { return max.x - min.x; }
        }

        public static float Depth
        {
            get { return max.x - min.x; }
        }

        public static Random RndGen
        {
            get {return rndGen;}
        }

        public static Vector3 RandomLocation
        {
            get
            {
                float xPos = (float)rndGen.NextDouble() * Width + min.x;
                float zPos = (float)rndGen.NextDouble() * Depth + min.z;
                return new Vector3(xPos, 0, zPos);
            }
        }

        public static Vector3 getRandomPosition(Vector3 center, float deltaX, float deltaZ)
        {
            Vector3 copy = new Vector3(center.x, center.y, center.z);
            copy.x += (float)(rndGen.NextDouble() - 0.5) * deltaX;
            copy.z += (float)(rndGen.NextDouble() - 0.5) * deltaZ;
            return copy;
        }
    }
}
