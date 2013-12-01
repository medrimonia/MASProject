﻿using Mogre;
using Mogre.TutorialFramework;
using MogreFramework;
using MASProject.Input;


namespace MASProject.Overlays
{
    abstract class HelperOverlay
    {
        private static string OverlayName{
            get {return "Helper";}
        }

        private static string MessageBoxName
        {
            get { return OverlayName + "/MessageBox"; }
        }

        private static string BodyName
        {
            get { return MessageBoxName + "/Body"; }
        }

        public static void Init(RenderWindow w)
        {
            var messageBox = OverlayManager.Singleton.GetOverlayElement(MessageBoxName);
            messageBox.Left = (w.Width - messageBox.Width) / 2;
            messageBox.Top = (w.Height - messageBox.Height) / 2;

            var messageBody = OverlayManager.Singleton.GetOverlayElement(BodyName);
            messageBody.Caption = "Helper";
        }

        public static void Update(InputManager.InputMode mode)
        {
            var messageBody = OverlayManager.Singleton.GetOverlayElement(BodyName);
            messageBody.Caption = "";
            foreach (CommandHelper c in CommandDatabase.getCommands(mode)){
                messageBody.Caption += c.ToString() + '\n';
            }
        }

        public static void Toggle()
        {
            var overlay = OverlayManager.Singleton.GetByName(OverlayName);
            if (overlay.IsVisible)
                Hide();
            else
                Show();
        }

        public static void Show()
        {
            OverlayManager.Singleton.GetByName(OverlayName).Show();
        }

        public static void Hide()
        {
            OverlayManager.Singleton.GetByName(OverlayName).Hide();
        }
    }
}
