using Mogre;
using Mogre.TutorialFramework;
using MogreFramework;
using MASProject.Input;
using System.Collections.Generic;


namespace MASProject.Overlays
{
    abstract class DebugOverlay
    {
        private static Queue<string> lastLines = new Queue<string>();

        private static int maxLines = 15;

        private static bool initialized = false;

        private static string OverlayName{
            get {return "Debug";}
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
            messageBox.Left = Margins.BorderMargin;
            messageBox.Top = w.Height - messageBox.Height - Margins.BorderMargin;

            var title = OverlayManager.Singleton.GetOverlayElement(TitleName);
            title.Left = messageBox.Width / 2;
            title.Caption = "Debugging Informations";

            var messageBody = OverlayManager.Singleton.GetOverlayElement(BodyName);
            messageBody.Caption = "";
            initialized = true;
        }

        private static void Update()
        {
            var messageBody = OverlayManager.Singleton.GetOverlayElement(BodyName);
            messageBody.Caption = "";
            foreach (string line in lastLines){
                messageBody.Caption += line + '\n';
            }
        }

        public static void WriteLine(string line)
        {
            if (!initialized) return;
            if (lastLines.Count == maxLines)
                lastLines.Dequeue();
            lastLines.Enqueue(line);
            Update();
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
