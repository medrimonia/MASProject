using Mogre;
using Mogre.TutorialFramework;
using MogreFramework;
using MASProject.Input;
using System.Collections.Generic;


namespace MASProject.Overlays
{
    abstract class StatusOverlay
    {
        private static string OverlayName{
            get {return "Status";}
        }

        private static string MessageBoxName
        {
            get { return OverlayName + "/MessageBox"; }
        }

        private static string BodyName
        {
            get { return MessageBoxName + "/Body"; }
        }

        private static string TitleName
        {
            get { return MessageBoxName + "/Title"; }
        }

        public static void Init(RenderWindow w)
        {
            var messageBox = OverlayManager.Singleton.GetOverlayElement(MessageBoxName);
            messageBox.Left = (w.Width - messageBox.Width) / 2;
            messageBox.Top = (w.Height - messageBox.Height) / 2;

            var title = OverlayManager.Singleton.GetOverlayElement(TitleName);
            title.Left = messageBox.Width / 2;
            title.Caption = "World Status";

            var messageBody = OverlayManager.Singleton.GetOverlayElement(BodyName);
            messageBody.Caption = "";
        }

        private static string ogresLine(World w)
        {
            string msg = "Ogres : " + w.OgresCount + "\n";
            msg += "\tMale : " + w.MaleOgresCount + "\n";
            msg += "\tFemale : " + w.FemaleOgresCount + "\n";
            return msg;
        }

        private static string robotsLine(World w)
        {
            return "Robots : " + w.Robot.Count + "\n";
        }

        private static string stonesLine(World w)
        {
            return "Stones : " + w.Stones.Count + "\n";
        }

        public static void Update(World w)
        {
            var messageBody = OverlayManager.Singleton.GetOverlayElement(BodyName);
            messageBody.Caption = "";
            messageBody.Caption += ogresLine(w);
            messageBody.Caption += robotsLine(w);
            messageBody.Caption += stonesLine(w);
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
