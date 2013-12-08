using Mogre;
using Mogre.TutorialFramework;
using MogreFramework;
using MASProject.Objects;

namespace MASProject.Input
{
    class CameraManager
    {
        #region Backup Values
        private static Vector3 mainCameraPosition;
        private static Quaternion mainCameraOrientation;
        #endregion

        private static SceneManager sm;
        private static World environment;
        private static GraphicalObject tracked;

        #region Camera current properties
        private static float nearClippingDistance;
        private static Vector3 xAxis;
        private static Vector3 zAxis;
        private static Vector3 pos;

        private static Vector3 Position
        {
            get { return pos; }
        }

        private static Vector3 XAxis
        {
            get { return xAxis; }
            set { value.Normalise(); xAxis = value; }
        }
        private static Vector3 YAxis
        {
            get { return ZAxis.CrossProduct(XAxis).NormalisedCopy; }
        }
        private static Vector3 ZAxis
        {
            get { return zAxis; }
            set { value.Normalise(); zAxis = value; }
        }
        #endregion

        #region HighSpeed Properties
        private static bool highSpeed;
        private static float highSpeedFactor;
        #endregion

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

        static CameraManager()
        {
            sm = null;
            ResetConfig();
        }

        public static string CameraName
        {
            get { return "MainCamera"; }
        }

        public static Camera Camera
        {
            get { return sm.GetCamera(CameraName); }
        }

        public static void Init(SceneManager sManager, World w)
        {
            sm = sManager;
            environment = w;
        }

        public static Quaternion Orientation
        {
            get { return new Quaternion(XAxis, YAxis, ZAxis); }
            set
            {
                XAxis = value.XAxis;
                ZAxis = value.ZAxis;
            }
        }

        private static void ResetConfig()
        {
            tracked = null;
            pos = new Vector3(0, 1000f, 0);
            XAxis = Vector3.UNIT_Z;
            ZAxis = Vector3.UNIT_Y;
            mainCameraPosition = pos;
            mainCameraOrientation = Orientation;
            nearClippingDistance = 40f;
            HighSpeed = false;
            highSpeedFactor = 10f;
        }

        private static void Move(float elapsedTime)
        {
            Vector3 move = Vector3.ZERO;
            move += -Camera.Orientation.ZAxis * forwardMove;
            move += Camera.Orientation.XAxis * lateralMove;
            move *= elapsedTime * translateSpeed;
            if (HighSpeed) move *= highSpeedFactor;
            Move(move);
        }

        private static void Move(Vector3 move)
        {
            pos += move;
        }

        private static void Rotate(float elapsedTime)
        {
            float pitchDelta = PitchMove * rotateSpeed * elapsedTime;
            float yawDelta = YawMove * rotateSpeed * elapsedTime;
            XAxis = XAxis + ZAxis * yawDelta;
            // If ZAxis is not update here, a problem will happen
            ZAxis = xAxis.CrossProduct(YAxis);
            ZAxis = ZAxis - YAxis * pitchDelta;
        }

        /// <summary>
        /// Try to reach the destination with a small steps
        /// </summary>
        private static void smoothTransition(Vector3 goalPos, Quaternion goalOrientation)
        {
            float alpha = 0.02f;
            pos = Position * (1 - alpha) + goalPos * alpha;
            XAxis = XAxis * (1 - alpha) + goalOrientation.XAxis * alpha;
            ZAxis = ZAxis * (1 - alpha) + goalOrientation.ZAxis * alpha;
        }

        public static void UpdateCamera(SceneManager sm, float elapsedTime)
        {
            if (tracked == null || !tracked.Useable)
            {
                Orientation = mainCameraOrientation;
                pos = mainCameraPosition;
                //Compute
                Move(elapsedTime);
                Rotate(elapsedTime);
                mainCameraPosition = pos;
                mainCameraOrientation = Orientation;
            }
            else
            {
                smoothTransition(tracked.Position, tracked.CameraOrientation);
            }
            // apply
            Camera.NearClipDistance = nearClippingDistance;
            Camera.Position = Position;
            Camera.Orientation = Orientation;
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

        public static bool HighSpeed
        {
            get { return highSpeed; }
            set { highSpeed = value; }
        }

        public static bool treatKeyPressed(MOIS.KeyEvent arg)
        {
            switch (arg.key)
            {
                case MOIS.KeyCode.KC_R:
                    ResetConfig(); break;
                case MOIS.KeyCode.KC_TAB:
                    if (environment != null)
                    {
                        tracked = environment.getNextOgre(tracked);
                    }
                    break;
            }
            return true;
        }
    }
}
