namespace MASProject.Input
{
    class TimeManager
    {

        private static bool paused;
        private static float speed;

        static TimeManager()
        {
            Reset();
        }

        private static void Reset()
        {
            paused = false;
            speed = 1.0f;
        }

        public static float Speed
        {
            get {return paused ? 0.0f : speed; }
        }

        public static void Toggle()
        {
            paused = !paused;
        }

        public static void IncreaseSpeed()
        {
            speed *= SpeedGain;
        }

        public static void DecreaseSpeed()
        {
            speed /= SpeedGain;
        }

        private static float SpeedGain
        {
            get { return 1.5f; }
        }

        public static bool treatKeyPressed(MOIS.KeyEvent arg)
        {
            switch (arg.key)
            {
                case MOIS.KeyCode.KC_ADD:
                    IncreaseSpeed(); break;
                case MOIS.KeyCode.KC_SUBTRACT:
                    DecreaseSpeed(); break;
                case MOIS.KeyCode.KC_R:
                    Reset(); break;
            }
            return true;
        }
    }
}
