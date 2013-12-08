using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MASProject.Objects;

namespace MASProject.Behavior
{
    abstract class SexualBehavior
    {
        /* The behavior is active, if a male is going toward a female or
         * if a female is waiting for male to come.
         */
        private bool sexualActivity;

        public bool Activity
        {
            get { return sexualActivity; }
            set { sexualActivity = value; }
        }
        public virtual bool readyForPregnancy(OgreAgent o)
        {
            return false;
        }

        public virtual bool readyToInseminate(double age)
        {
            return false;
        }

        public virtual void inseminate(double age)
        {
        }

        public abstract void apply(World w, OgreAgent o);
    }
}
