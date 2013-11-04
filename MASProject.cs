using Mogre;
using Mogre.TutorialFramework;
using MogreFramework;
using System;
using System.Windows.Forms;

/* Our project is entirely based on the tutorial that can be found at
 * http://www.ogre3d.org/tikiwiki/Mogre+Wiki+Tutorial+Framework
 */

namespace MASProject
{
    class MASProject : BaseApplication
    {
        private static int NB_OGREHEADS = 10;
        private static int NB_STONES = 200;
        private static int NB_ROBOTS = 10;

        protected World environment;

        //TODO add a frame displaying the number of each object

        public static void Main()
        {

            Utils.DebugUtils.writeMessage("Starting");

            new MASProject().Go();
        }

        private bool updateContent(FrameEvent evt)
        {
            DateTime start = DateTime.Now;
            environment.mutate(evt.timeSinceLastFrame);
            DateTime end = DateTime.Now;
            TimeSpan duration = end - start;
            //Utils.DebugUtils.writeMessage("WorldMutation : " + duration.ToString());
            return true;
        }

        protected override void CreateFrameListeners()
        {
            base.CreateFrameListeners();
            mRoot.FrameRenderingQueued += new FrameListener.FrameRenderingQueuedHandler(updateContent);
        }

        protected override void CreateScene()
        {
            environment = new World(mSceneMgr, NB_OGREHEADS, NB_ROBOTS, NB_STONES);
        }
    }
}