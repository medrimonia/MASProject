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

        private bool updateContent(FrameEvent evt)
        {
            environment.mutate(evt.timeSinceLastEvent);
            return true;
        }

        protected override void CreateFrameListeners()
        {
            base.CreateFrameListeners();
            mRoot.FrameRenderingQueued += new FrameListener.FrameRenderingQueuedHandler(updateContent);
        }

        protected override void CreateScene()
        {
            environment = new World(mSceneMgr, 5, 0);
        }
    }
}