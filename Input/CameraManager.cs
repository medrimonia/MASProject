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

        private static float pitchMove = 0f;
        private static float yawMove = 0f;

        private static float translateSpeed = 100f;
        private static float rotateSpeed = 0.5f;

        public static string CameraName
        {
            get { return "MainCamera"; }
        }

        private static void Move(float elapsedTime)
        {
            Vector3 move = Vector3.ZERO;
            move += -mainCameraOrientation.ZAxis * forwardMove;
            move += mainCameraOrientation.XAxis * lateralMove;
            move *= elapsedTime * translateSpeed;
            mainCameraPosition += move;
        }

        private static void Rotate(float elapsedTime)
        {
            Quaternion pitchRotation = new Quaternion(new Degree(PitchMove * rotateSpeed), Vector3.UNIT_X);
            Quaternion yawRotation = new Quaternion(new Degree(-yawMove * rotateSpeed), Vector3.UNIT_Y);
            mainCameraOrientation = mainCameraOrientation * yawRotation * pitchRotation;
        }

        public static void UpdateCamera(SceneManager sm, float elapsedTime)
        {
            Move(elapsedTime);
            Rotate(elapsedTime);
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

        public static float PitchMove
        {
            get { return pitchMove; }
            set { pitchMove = value; }
        }

        public static float YawMove
        {
            get { return yawMove; }
            set { yawMove = value; }
        }

    }
}
