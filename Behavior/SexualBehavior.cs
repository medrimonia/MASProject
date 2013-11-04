using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MASProject.Behavior
{
    abstract class SexualBehavior
    {
        public virtual bool readyForPregnancy(double age){
            return false;
        }

        public virtual bool readyToInseminate(double age)
        {
            return false;
        }

        public virtual void inseminate(double age)
        {
        }

        public abstract void apply(World w, Ogre o);
    }
}
