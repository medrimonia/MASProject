using Mogre;
using Mogre.TutorialFramework;
using MogreFramework;
using System;
using System.Windows.Forms;
using MASProject.Utils;

namespace MASProject.Input
{
    class InputManager
    {
        private enum InputMode
        {
            None,
            Light
        }

        //Input handling
        private MOIS.InputManager inputMgr;
        private MOIS.Keyboard keyboardMgr;

        // Specific managers
        private LightManager lightMgr;
        private InputMode mode;

        public InputManager()
        {
            lightMgr = new LightManager();
            mode = InputMode.None;
        }

        public void initializeInput(RenderWindow w)
        {
            int windowHandle;
            w.GetCustomAttribute("WINDOW", out windowHandle);
            inputMgr = MOIS.InputManager.CreateInputSystem((uint)windowHandle);
            keyboardMgr = (MOIS.Keyboard)inputMgr.CreateInputObject(MOIS.Type.OISKeyboard, true);
            keyboardMgr.KeyPressed += new MOIS.KeyListener.KeyPressedHandler(OnKeyPressed);
            DebugUtils.writeMessage("Input initialized");
        }

        public void initializeScene(World w, SceneManager sMgr)
        {
            lightMgr.initalizeScene(w, sMgr);
        }

        public bool processBufferedInput(FrameEvent evt)
        {
            keyboardMgr.Capture();
            return true;
        }

        /* This update needs to be performed once all objects have been moved */
        public void finalUpdate()
        {
            lightMgr.updateLights();
        }

        protected bool treatKeyPressed(MOIS.KeyEvent arg)
        {

            switch (arg.key)
            {
                case MOIS.KeyCode.KC_H:
                    //Show help
                    break;
                case MOIS.KeyCode.KC_L:
                    mode = InputMode.Light;
                    break;
            }
            return true;
        }

        protected bool OnKeyPressed(MOIS.KeyEvent arg)
        {
            DebugUtils.writeMessage("On key pressed");

            switch (mode)
            {
                case InputMode.None:
                    return treatKeyPressed(arg);
                case InputMode.Light:
                    return lightMgr.treatKeyPressed(arg);
            }
            return true;
        }
    }
}
