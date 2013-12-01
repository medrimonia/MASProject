using Mogre;
using Mogre.TutorialFramework;
using MogreFramework;
using System;
using System.Windows.Forms;
using MASProject.Utils;
using System.Collections.Generic;

namespace MASProject.Input
{
    class InputManager
    {
        public enum InputMode
        {
            None,
            Light,
            Fog
        }

        //Input handling
        private MOIS.InputManager inputMgr;
        private MOIS.Keyboard keyboardMgr;

        // Specific managers
        private LightManager lightMgr;
        private InputMode mode;

        private bool ctrlModifier;

        private bool shutdownAsked;

        public bool ShutdownAsked
        {
            get { return shutdownAsked; }
        }

        public InputManager()
        {
            lightMgr = new LightManager();
            mode = InputMode.None;
            shutdownAsked = false;
        }

        public void initializeInput(RenderWindow w)
        {
            int windowHandle;
            w.GetCustomAttribute("WINDOW", out windowHandle);
            inputMgr = MOIS.InputManager.CreateInputSystem((uint)windowHandle);
            keyboardMgr = (MOIS.Keyboard)inputMgr.CreateInputObject(MOIS.Type.OISKeyboard, true);
            keyboardMgr.KeyPressed += new MOIS.KeyListener.KeyPressedHandler(OnKeyPressed);
            keyboardMgr.KeyReleased += new MOIS.KeyListener.KeyReleasedHandler(OnKeyReleased);
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
        public void finalUpdate(double timeElapsed)
        {
            lightMgr.updateLights(timeElapsed);
        }

        protected bool treatKeyPressed(MOIS.KeyEvent arg)
        {
            #region Treating keys always activated
            switch (arg.key)
            {
                case MOIS.KeyCode.KC_LCONTROL:
                    DebugUtils.writeMessage("Ctrl Modifier : on");
                    ctrlModifier = true;
                    break;
                case MOIS.KeyCode.KC_F1:
                    Overlays.DebugOverlay.Toggle();
                    break;
                case MOIS.KeyCode.KC_H:
                    Overlays.HelperOverlay.Toggle();
                    break;
                case MOIS.KeyCode.KC_SPACE:
                    TimeProperties.TogglePause();
                    break;
                case MOIS.KeyCode.KC_ESCAPE:
                    if (ctrlModifier) shutdownAsked = true;
                    else mode = InputMode.None;
                    break;
                case MOIS.KeyCode.KC_UP:
                    if (!ctrlModifier) CameraManager.ForwardMove = 1;
                    else CameraManager.PitchMove = 1;
                    break;
                case MOIS.KeyCode.KC_DOWN:
                    if (!ctrlModifier) CameraManager.ForwardMove = -1;
                    else CameraManager.PitchMove = -1;
                    break;
                case MOIS.KeyCode.KC_LEFT:
                    if (!ctrlModifier) CameraManager.LateralMove = -1;
                    else CameraManager.YawMove = -1;
                    break;
                case MOIS.KeyCode.KC_RIGHT:
                    if (!ctrlModifier) CameraManager.LateralMove = 1;
                    else CameraManager.YawMove = 1;
                    break;
            }
            #endregion

            #region Treating keys on no mode
            if (mode == InputMode.None)
            {
                switch (arg.key)
                {
                    case MOIS.KeyCode.KC_L:
                        mode = InputMode.Light;
                        break;
                    case MOIS.KeyCode.KC_F:
                        mode = InputMode.Fog;
                        break;
                }
            }
            #endregion
            Overlays.HelperOverlay.Update(mode);
            return true;
        }

        protected bool OnKeyPressed(MOIS.KeyEvent arg)
        {
            treatKeyPressed(arg);
            switch (mode)
            {
                case InputMode.Light:
                    return lightMgr.treatKeyPressed(arg);
                case InputMode.Fog:
                    return FogManager.treatKeyPressed(arg);
            }
            return true;
        }

        protected bool treatKeyReleased(MOIS.KeyEvent arg)
        {
            switch (arg.key)
            {
                case MOIS.KeyCode.KC_LCONTROL:
                    DebugUtils.writeMessage("Ctrl modifier : off");
                    ctrlModifier = false;
                    break;
                case MOIS.KeyCode.KC_UP:
                case MOIS.KeyCode.KC_DOWN:
                    CameraManager.ForwardMove = 0f;
                    CameraManager.PitchMove = 0f;
                    break;
                case MOIS.KeyCode.KC_LEFT:
                case MOIS.KeyCode.KC_RIGHT:
                    CameraManager.LateralMove = 0f;
                    CameraManager.YawMove = 0f;
                    break;
            }
            return true;
        }

        protected bool OnKeyReleased(MOIS.KeyEvent arg)
        {
            treatKeyReleased(arg);
            return true;
        }

        public ColourValue AmbientLight
        {
            get { return lightMgr.AmbientLight; }
        }
    }
}
