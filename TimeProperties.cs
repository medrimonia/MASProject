namespace MASProject
{
    class TimeProperties
    {

        private static bool paused = false;
        private static float speed = 1.0f;

        public static float Speed
        {
            get {return paused ? 0.0f : speed; }
        }

        public static void TogglePause()
        {
            paused = !paused;
        }
    }
}
