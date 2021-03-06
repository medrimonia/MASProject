﻿using System;
using Mogre;

namespace MASProject.Communication
{
    class DensityMessage : Message
    {
        private float density;
        private Vector3 position;

        public DensityMessage(Vector3 position, float density)
            : base(MessageType.DensityMessage)
        {
            this.position = position;
            this.density = density;
        }

        public Vector3 Position
        {
            get { return position; }
        }

        public float Density
        {
            get { return density; }
        }
    }
}
