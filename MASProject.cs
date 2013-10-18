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
        private static int NB_OGREHEADS = 25;

        protected World environment;

        public static void Main()
        {
            new MASProject().Go();
        }

        private bool updateContent(FrameEvent evt)
        {
            environment.mutate(evt.timeSinceLastFrame);
            return true;
        }

        protected override void CreateFrameListeners()
        {
            base.CreateFrameListeners();
            mRoot.FrameRenderingQueued += new FrameListener.FrameRenderingQueuedHandler(updateContent);
        }

        protected override void CreateScene()
        {
            environment = new World(mSceneMgr, NB_OGREHEADS, 0);
        }
    }
}