using Mogre;
using Mogre.TutorialFramework;
using MogreFramework;

namespace MASProject.Input
{
    class FogManager

    {
        private static float fogColorIntensity;
        private static FogMode mode;
        private static float strength;


        static FogManager()
        {
            Reset();
        }

        private static void Reset()
        {
            fogColorIntensity = 0.9f;
            mode = FogMode.FOG_NONE;
            strength = 0.001f;
        }

        public static ColourValue Color
        {
            get { return new ColourValue(fogColorIntensity, fogColorIntensity, fogColorIntensity); }
        }

        public static float Strength
        {
            get { return strength; }
        }

        public static FogMode Mode
        {
            get { return mode; }
        }

        public static void Toggle()
        {
            if (mode == FogMode.FOG_NONE)
            {
                Enable();
            }
            else
            {
                Disable();
            }
        }

        public static void Disable()
        {
            mode = FogMode.FOG_NONE;
        }

        public static void Enable()
        {
            mode = FogMode.FOG_EXP2;
        }

        public static void Darker()
        {
            fogColorIntensity -= 0.1f;
            if (fogColorIntensity < 0f) fogColorIntensity = 0f;
        }

        public static void Brighter()
        {
            fogColorIntensity += 0.1f;
            if (fogColorIntensity > 1f) fogColorIntensity = 1f;
        }

        public static void Increase()
        {
            strength *= 1.1f;
        }

        public static void Decrease()
        {
            strength /= 1.1f;
        }

        public static bool treatKeyPressed(MOIS.KeyEvent arg)
        {
            switch (arg.key)
            {
                case MOIS.KeyCode.KC_T:
                    Toggle(); break;
                case MOIS.KeyCode.KC_ADD:
                    Increase(); break;
                case MOIS.KeyCode.KC_SUBTRACT:
                    Decrease(); break;
                case MOIS.KeyCode.KC_R:
                    Reset(); break;
                case MOIS.KeyCode.KC_D:
                    Darker(); break;
                case MOIS.KeyCode.KC_B:
                    Brighter(); break;
            }
            return true;
        }
    }
}
