﻿using Mogre;
using Mogre.TutorialFramework;
using MogreFramework;
using System;
using System.Windows.Forms;
using MASProject.Input;

/* Our project is entirely based on the tutorial that can be found at
 * http://www.ogre3d.org/tikiwiki/Mogre+Wiki+Tutorial+Framework
 */

namespace MASProject
{

    class MASProject : BaseApplication
    {
        private static int NB_OGREHEADS = 20;
        private static int NB_STONES = 200;
        private static int NB_ROBOTS = 15;

        private World environment;
        private InputManager inputMgr;

        public static void Main()
        {
            Utils.DebugUtils.writeMessage("Starting");

            new MASProject().Go();
        }


        public MASProject() : base()
        {
            inputMgr = new InputManager();
        }

        private bool updateContent(FrameEvent evt)
        {
            float realElapsedTime = evt.timeSinceLastFrame;
            float worldElapsedTime = realElapsedTime * TimeManager.Speed;
            inputMgr.processBufferedInput(evt);
            if (worldElapsedTime > 0)
            {
                environment.mutate(worldElapsedTime);
            }
            inputMgr.finalUpdate(worldElapsedTime);
            mSceneMgr.AmbientLight = inputMgr.AmbientLight;
            mSceneMgr.SetFog(FogManager.Mode, FogManager.Color, FogManager.Strength);
            CameraManager.UpdateCamera(mSceneMgr, realElapsedTime);
            Overlays.StatusOverlay.Update(environment);
            // Hiding this overlay at start only is not enough
            OverlayManager.Singleton.GetByName("Core/DebugOverlay").Hide();
            return !inputMgr.ShutdownAsked;
        }

        protected override void InitializeInput()
        {
            //base.InitializeInput();

            inputMgr.initializeInput(mWindow);
            CameraManager.Init(mSceneMgr, environment);
        }

        protected override void CreateFrameListeners()
        {
            //base.CreateFrameListeners();
            mRoot.FrameRenderingQueued += new FrameListener.FrameRenderingQueuedHandler(updateContent);
        }

        protected override void CreateScene()
        {
            environment = new World(mSceneMgr, NB_OGREHEADS, NB_ROBOTS, NB_STONES);
            // Initializing lights
            mSceneMgr.AmbientLight = ColourValue.Black;
            mSceneMgr.ShadowTechnique = ShadowTechnique.SHADOWTYPE_STENCIL_MODULATIVE;
            LightManager.Init(environment,mSceneMgr);
            // Adding overlays
            Overlays.HelperOverlay.Init(mWindow);
            Overlays.DebugOverlay.Init(mWindow);
            Overlays.StatusOverlay.Init(mWindow);
            mSceneMgr.SetSkyDome(true, "Examples/CloudySky", 5, 8);
        }
        protected override void CreateCamera()
        {
            mCamera = mSceneMgr.CreateCamera(Input.CameraManager.CameraName);
        }
    }
}