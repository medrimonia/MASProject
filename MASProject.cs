using Mogre;
using Mogre.TutorialFramework;
using System;

/* Our project is entirely based on the tutorial that can be found at
 * http://www.ogre3d.org/tikiwiki/Mogre+Wiki+Tutorial+Framework
 */

namespace MASProject
{
    class MASProject : BaseApplication
    {
        protected World environment;

        public static void Main()
        {
            new MASProject().Go();
        }

        protected override void CreateScene()
        {
            environment = new World(mSceneMgr, 5, 0);
        }
    }
}