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
        /// <summary>
        /// The number of seconds in the user referential that correspond to
        /// 24 hours in the LightManager referential
        /// </summary>
        private static float secondsByDay = 10;
        private static float dayStart = 5f;
        private static float dayEnd = 19f;
        private static float dayIntensity = 1f;
        private static float nightIntensity = 0.1f;

        // lights
        private Light overallLight;
        private Light mainSpot;
        private LightningMode lightMode;
        private float hour;

        // The world in which it evolves
        private World environment;

        public LightManager()
        {
        }

        /// <summary>
        /// The duration of a day in hours (LightManager time referential)
        /// </summary>
        private float DayLength
        {
            get { return dayEnd - dayStart; }
        }

        /// <summary>
        /// Return the noon time in hours (In the LightManager time referential)
        /// </summary>
        private float Noon
        {
            get { return DayLength / 2 + dayStart; }
        }

        private float LightIntensity
        {
            get
            {
                switch (lightMode)
                {
                    case LightningMode.Day: return dayIntensity;
                    case LightningMode.Night: return nightIntensity;
                    case LightningMode.Cycle:
                        if (hour < dayStart) return nightIntensity;
                        if (hour < Noon)
                        {
                            // [dayStart, Noon] -> [0,1]
                            float pos = (hour - dayStart) / (DayLength / 2);
                            return pos * (dayIntensity - nightIntensity) + nightIntensity;
                        }
                        if (hour < dayEnd)
                        {
                            // [Noon, dayEnd] -> [0,1]
                            float pos = (hour - Noon) / (DayLength / 2);
                            return dayIntensity - pos * (dayIntensity - nightIntensity);
                        }
                        return nightIntensity;
                }
                return dayIntensity;
            }
        }

        /// <summary>
        /// Update the content of the light manager, including overall lights and spot
        /// </summary>
        /// <param name="elapsedTime">In seconds</param>
        public void updateLights(double elapsedTime)
        {
            hour = (hour + 24 * (float)elapsedTime / secondsByDay) % 24;
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
            overallLight.DiffuseColour = new ColourValue(LightIntensity, LightIntensity, LightIntensity);
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
                case MOIS.KeyCode.KC_C:
                    lightMode = LightningMode.Cycle; break;
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
