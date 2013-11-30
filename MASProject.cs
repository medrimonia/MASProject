using Mogre;
using Mogre.TutorialFramework;
using MogreFramework;
using System;
using System.Windows.Forms;
using MASProject.Input;

/* Our project is entirely based on the tutorial that can be found at
 * http://www.ogre3d.org/tikiwiki/Mogre+Wiki+Tutorial+Framework
 */

namespace MASProject
{

    class MASProject : BaseApplication
    {
        private static int NB_OGREHEADS = 10;
        private static int NB_STONES = 100;
        private static int NB_ROBOTS = 10;

        private World environment;
        private InputManager inputMgr;

        //TODO add a frame displaying the number of each object

        public static void Main()
        {
            Utils.DebugUtils.writeMessage("Starting");

            new MASProject().Go();
        }


        public MASProject() : base()
        {
            inputMgr = new InputManager();
        }

        private void updateAdditionalInfo()
        {
            mDebugOverlay.AdditionalInfo = "nbOgres : " + environment.OgresCount + " ";
            mDebugOverlay.AdditionalInfo += "[M : " + environment.MaleOgresCount + " ";
            mDebugOverlay.AdditionalInfo += "F : " + environment.FemaleOgresCount + "]";
        }

        private bool updateContent(FrameEvent evt)
        {
            inputMgr.processBufferedInput(evt);
            updateAdditionalInfo();
            DateTime start = DateTime.Now;
            environment.mutate(evt.timeSinceLastFrame);
            DateTime end = DateTime.Now;
            TimeSpan duration = end - start;
            //Utils.DebugUtils.writeMessage("WorldMutation : " + duration.ToString());
            inputMgr.finalUpdate(evt.timeSinceLastFrame);
            return true;
        }

        protected override void InitializeInput()
        {
            base.InitializeInput();

            inputMgr.initializeInput(mWindow);
        }

        protected override void CreateFrameListeners()
        {
            base.CreateFrameListeners();
            mRoot.FrameRenderingQueued += new FrameListener.FrameRenderingQueuedHandler(updateContent);
        }

        protected override void CreateScene()
        {
            environment = new World(mSceneMgr, NB_OGREHEADS, NB_ROBOTS, NB_STONES);
            // Initializing lights
            mSceneMgr.AmbientLight = ColourValue.Black;
            mSceneMgr.ShadowTechnique = ShadowTechnique.SHADOWTYPE_STENCIL_MODULATIVE;
            inputMgr.initializeScene(environment, mSceneMgr);
        }
    }
}