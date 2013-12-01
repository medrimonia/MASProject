using Mogre;
using Mogre.TutorialFramework;
using MogreFramework;

namespace MASProject.Input
{
    class CameraManager
    {
        private static Vector3 mainCameraPosition = new Vector3(0, 1000f, 0);
        private static Quaternion mainCameraOrientation = new Quaternion(Vector3.UNIT_Z, Vector3.UNIT_X, Vector3.UNIT_Y);
        private static Vector3 mainCameraDirection = Vector3.NEGATIVE_UNIT_Y;

        private static float nearClippingDistance = 5f;

        private static float forwardMove = 0f;
        /// <summary>
        /// + -> To the right of the camera
        /// - -> To the left of the camera
        /// </summary>
        private static float lateralMove = 0f;

        private static float cameraSpeed = 100f;

        public static string CameraName
        {
            get { return "TestCamera"; }
        }

        private static void Move(float elapsedTime)
        {
            Vector3 move = Vector3.ZERO;
            move += -mainCameraOrientation.ZAxis * forwardMove;
            move += mainCameraOrientation.XAxis * lateralMove;
            move *= elapsedTime * cameraSpeed;
            mainCameraPosition += move;
        }

        public static void UpdateCamera(SceneManager sm, float elapsedTime)
        {
            Move(elapsedTime);
            var mCamera = sm.GetCamera(CameraName);
            mCamera.Position = mainCameraPosition;
            mCamera.Orientation = mainCameraOrientation;
            mCamera.NearClipDistance = nearClippingDistance;
        }

        public static float ForwardMove
        {
            get { return forwardMove; }
            set { forwardMove = value; }
        }

        public static float LateralMove
        {
            get { return lateralMove; }
            set { lateralMove = value; }
        }

    }
}
