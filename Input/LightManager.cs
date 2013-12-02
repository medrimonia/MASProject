using Mogre;
using Mogre.TutorialFramework;
using MogreFramework;
using System;
using System.Windows.Forms;
using MASProject.Utils;

namespace MASProject.Input
{
    public enum LightningMode
    {
        Day,
        Night,
        Cycle
    }

    abstract class LightManager
    {
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
        private static Light mainSpot;
        private static LightningMode lightMode;
        private static float hour;

        // The world in which it evolves
        private static World environment;

        private static GraphicalObject tracked;

        /// <summary>
        /// The duration of a day in hours (LightManager time referential)
        /// </summary>
        private static float DayLength
        {
            get { return dayEnd - dayStart; }
        }

        /// <summary>
        /// Return the noon time in hours (In the LightManager time referential)
        /// </summary>
        private static float Noon
        {
            get { return DayLength / 2 + dayStart; }
        }

        private static float LightIntensity
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

        public static ColourValue AmbientLight
        {
            get { return new ColourValue(LightIntensity, LightIntensity, LightIntensity); }
        }

        /// <summary>
        /// Update the content of the light manager, including overall lights and spot
        /// </summary>
        /// <param name="elapsedTime">In seconds</param>
        public static void UpdateLights(double elapsedTime)
        {
            hour = (hour + 24 * (float)elapsedTime / secondsByDay) % 24;
            mainSpot.Visible = tracked != null && tracked.Useable;
            // Update Direction
            if (mainSpot.Visible)
            {
                Vector3 direction = tracked.Position - mainSpot.Position;
                direction.Normalise();
                mainSpot.Direction = direction;
            }
        }

        public static bool treatKeyPressed(MOIS.KeyEvent arg)
        {
            switch (arg.key)
            {
                case MOIS.KeyCode.KC_TAB://next ogre
                    tracked = environment.getNextOgre(tracked);
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

        public static void Init(World w, SceneManager mSceneMgr){
            environment = w;
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
