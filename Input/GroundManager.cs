using Mogre;
using Mogre.TutorialFramework;
using MogreFramework;

namespace MASProject.Input
{
    class GroundManager
    {
        private static SceneManager scn;

        static GroundManager()
        {
            ResetConfig();
        }

        public static void Init(SceneManager scnManager)
        {
            scn = scnManager;
        }
        private static void ResetConfig()
        {
            if (scn == null)
                return;
            scn.SetWorldGeometry("terrain.cfg");
        }

        public bool treatKeyPressed(MOIS.KeyEvent arg)
        {
            switch (arg.key)
            {
                case MOIS.KeyCode.KC_L:
                    scn.SetWorldGeometry("terrain.cfg"); break;
                case MOIS.KeyCode.KC_E:
                    scn.SetWorldGeometry("terrain.cfg"); break;
                case MOIS.KeyCode.KC_R:
                    ResetConfig(); break;
            }
            return true;
        }
        public void updateGround()
        {

        }

    }
}
