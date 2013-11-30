using Mogre;
using Mogre.TutorialFramework;
using MogreFramework;

namespace MASProject.Overlays
{
    abstract class Helper
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
            messageBody.Caption = "Hello World!";
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
