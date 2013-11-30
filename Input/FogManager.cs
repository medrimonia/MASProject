using Mogre;
using Mogre.TutorialFramework;
using MogreFramework;

namespace MASProject.Input
{
    class FogManager

    {
        static float fogColorIntensity = 0.9f;
        static FogMode mode = FogMode.FOG_NONE;


        public static ColourValue Color
        {
            get { return new ColourValue(fogColorIntensity, fogColorIntensity, fogColorIntensity); }
        }

        public static float Strength
        {
            get { return 0.001f; }
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

        public static bool treatKeyPressed(MOIS.KeyEvent arg)
        {
            switch (arg.key)
            {
                case MOIS.KeyCode.KC_T:
                    Toggle(); break;
            }
            return true;
        }
    }
}
