﻿using Mogre;
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
                case MOIS.KeyCode.KC_H:
                    Overlays.Helper.Toggle();
                    break;
                case MOIS.KeyCode.KC_X:
                    mode = InputMode.None;
                    break;
                case MOIS.KeyCode.KC_SPACE:
                    TimeProperties.TogglePause();
                    break;
                case MOIS.KeyCode.KC_ESCAPE:
                    shutdownAsked = true;
                    break;
                case MOIS.KeyCode.KC_UP:
                    CameraManager.ForwardMove = 1;
                    break;
                case MOIS.KeyCode.KC_DOWN:
                    CameraManager.ForwardMove = -1;
                    break;
                case MOIS.KeyCode.KC_LEFT:
                    CameraManager.LateralMove = -1;
                    break;
                case MOIS.KeyCode.KC_RIGHT:
                    CameraManager.LateralMove = 1;
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
            Overlays.Helper.Update(mode);
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
                case MOIS.KeyCode.KC_UP:
                case MOIS.KeyCode.KC_DOWN:
                    CameraManager.ForwardMove = 0f;
                    break;
                case MOIS.KeyCode.KC_LEFT:
                case MOIS.KeyCode.KC_RIGHT:
                    CameraManager.LateralMove = 0f;
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
