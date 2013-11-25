using Mogre;
using Mogre.TutorialFramework;
using MogreFramework;
using System;
using System.Windows.Forms;
using MASProject.Utils;

namespace MASProject.Input
{
    class LightManager
    {
        public enum LightningMode
        {
            Day,
            Night,
            Cycle
        }
        
        // lights
        private Light overallLight;
        private Light mainSpot;
        private LightningMode lightMode;

        // The world in which it evolves
        private World environment;

        public LightManager()
        {
        }

        public void updateLights()
        {
            GraphicalObject tracked = environment.TrackedObject;
            mainSpot.Visible = tracked != null;
            // Update Direction
            if (tracked != null)
            {
                Vector3 direction = tracked.Position - mainSpot.Position;
                direction.Normalise();
                mainSpot.Direction = direction;
            }
            // Update Luminosity
            switch (lightMode)
            {
                case LightningMode.Day: overallLight.DiffuseColour = new ColourValue(1f, 1f, 1f); break;
                case LightningMode.Night: overallLight.DiffuseColour = new ColourValue(0.1f, 0.1f, 0.1f); break;
            }
        }

        public bool treatKeyPressed(MOIS.KeyEvent arg)
        {

            switch (arg.key)
            {
                case MOIS.KeyCode.KC_TAB://next ogre
                    environment.trackNext();
                    break;
                case MOIS.KeyCode.KC_D:
                    lightMode = LightningMode.Day; break;
                case MOIS.KeyCode.KC_N:
                    lightMode = LightningMode.Night; break;
            }
            return true;
        }

        public void initalizeScene(World w, SceneManager mSceneMgr){
            environment = w;
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
            lightMode = LightningMode.Day;
            DebugUtils.writeMessage("Lightning initialized");
        }
    }
}
