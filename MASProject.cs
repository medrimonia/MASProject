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
        private static int NB_OGREHEADS = 0;
        private static int NB_STONES = 100;
        private static int NB_ROBOTS = 10;

        // lights
        private Light overallLight;
        private Light mainSpot;
        //Input handling
        private MOIS.InputManager inputMgr;
        private MOIS.Keyboard lightKeyboard;

        private World environment;

        //TODO add a frame displaying the number of each object

        public static void Main()
        {
            Utils.DebugUtils.writeMessage("Starting");

            new MASProject().Go();
        }


        private void updateAdditionalInfo()
        {
            mDebugOverlay.AdditionalInfo = "nbOgres : " + environment.OgresCount + " ";
            mDebugOverlay.AdditionalInfo += "[M : " + environment.MaleOgresCount + " ";
            mDebugOverlay.AdditionalInfo += "F : " + environment.FemaleOgresCount + "]";
        }

        private void updateLights()
        {
            GraphicalObject tracked = environment.TrackedObject;
            if (tracked == null)
            {
                mainSpot.Visible = false;
                return;
            }
            mainSpot.Visible = true;
            Vector3 direction = tracked.Position - mainSpot.Position;
            direction.Normalise();
            mainSpot.Direction = direction;
        }

        protected bool OnLightKeyPressed(MOIS.KeyEvent arg)
        {

            switch (arg.key)
            {
                case MOIS.KeyCode.KC_TAB://next ogre
                    environment.trackNext();
                    break;
            }
            return true;
        }

        protected bool processBufferedInput(FrameEvent evt)
        {
            lightKeyboard.Capture();
            return true;
        }

        private bool updateContent(FrameEvent evt)
        {    
            processBufferedInput(evt);
            updateAdditionalInfo();
            DateTime start = DateTime.Now;
            environment.mutate(evt.timeSinceLastFrame);
            DateTime end = DateTime.Now;
            TimeSpan duration = end - start;
            //Utils.DebugUtils.writeMessage("WorldMutation : " + duration.ToString());
            updateLights();
            return true;
        }

        protected override void InitializeInput()
        {
            base.InitializeInput();

            int windowHandle;
            mWindow.GetCustomAttribute("WINDOW", out windowHandle);
            inputMgr = MOIS.InputManager.CreateInputSystem((uint)windowHandle);
            lightKeyboard = (MOIS.Keyboard)inputMgr.CreateInputObject(MOIS.Type.OISKeyboard, true);
            lightKeyboard.KeyPressed += new MOIS.KeyListener.KeyPressedHandler(OnLightKeyPressed);
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
            // Adding a global light
            overallLight = mSceneMgr.CreateLight("overallLight");
            overallLight.Type = Light.LightTypes.LT_DIRECTIONAL;
            overallLight.DiffuseColour = new ColourValue(.25f, .25f, .25f);
            overallLight.SpecularColour = new ColourValue(.25f, .25f, .25f);
            overallLight.Direction = new Vector3(0, -1, 1);
            // Spot Light
            mainSpot = mSceneMgr.CreateLight("mainSpot");
            mainSpot.Type = Light.LightTypes.LT_SPOTLIGHT;
            mainSpot.DiffuseColour = new ColourValue(1f, 1f, 1f);
            mainSpot.SpecularColour = new ColourValue(1f, 1f, 1f);
            mainSpot.Direction = new Vector3(0, -1, 0);
            mainSpot.Position = new Vector3(0, 600, 0);
            mainSpot.SetSpotlightRange(new Degree(10), new Degree(20));
        }
    }
}