using System.Collections.Generic;
using GameEvents;
using JetBrains.Annotations;
//using NUnit.Framework;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Utilities
{
    public class InputManager : Singleton<InputManager>
    {
        #region Variables

        #region PublicVariables 

        [Range(0,1)]
        public float joystickScreenPercentage = 1;

        #endregion

        #region PrivateVariables

        private bool _isFingerDown;
        private bool _isFingerMoving;
        private bool _inputJustEnded;

        //  Finger variables

        private Vector2 _startFingerPosition;
        private Vector2 _currentFingerPosition;
        private Vector2 _currentMovementVector;

        //  Time variables

        private float _touchDownTime;
        float _timeSinceLastInput;

        private const float TapTime = 0.2f;
        private const float HoldTime = 0.5f;
        private const float MinimumFingerMovement = 0.1f;

        //  Accelerometer

        private Vector3 _wantedDeadZone;
        private Matrix4x4 _calibrationMatrix;
        
        List<Vector3> pastAcceleration;

        #endregion

        #endregion

        #region Properties

        public static Vector2 StartFingerPosition => Instance._startFingerPosition;

        public static Vector2 StartWorldFingerPosition => Camera.main.ScreenToWorldPoint(StartFingerPosition);

        public static Vector2 CurrentWorldFingerPosition => Camera.main.ScreenToWorldPoint(CurrentFingerPosition);

        public static Vector2 CurrentFingerPosition => Instance._currentFingerPosition;

        public static Vector2 CurrentFingerPositionScreenCentered =>
            CurrentFingerPosition - new Vector2(Screen.width / 2, Screen.height / 2);

        public static Vector2 CurrentMovementVector
        {
            get => Instance._currentMovementVector;
            set => Instance._currentMovementVector = value;
        }

        public static bool IsFingerDown => Instance._isFingerDown;

        public static bool IsFingerMoving => Instance._isFingerMoving;

        public static bool InputJustEnded => Instance._inputJustEnded;

        public static float FingerDownTime => Instance._touchDownTime;

        public static Vector3 acceleration
        {
            get
            {
                var mean = Vector3.zero;

                if (Instance.pastAcceleration == null)
                {
                    Instance.pastAcceleration = new List<Vector3>()
                    {
                        Instance._calibrationMatrix.MultiplyVector(Input.acceleration),
                        Instance._calibrationMatrix.MultiplyVector(Input.acceleration),
                        Instance._calibrationMatrix.MultiplyVector(Input.acceleration),
                        Instance._calibrationMatrix.MultiplyVector(Input.acceleration),
                        Instance._calibrationMatrix.MultiplyVector(Input.acceleration)
                    };
                }
                else
                {
                    Instance.pastAcceleration.RemoveAt(0);
                    Instance.pastAcceleration.Add(Instance._calibrationMatrix.MultiplyVector(Input.acceleration));
                }


                for (int i = 0; i < Instance.pastAcceleration.Count; i++)
                {
                    mean += Instance.pastAcceleration[0];
                }

                return (mean / Instance.pastAcceleration.Count) / 5;
            }
        }

        #endregion

        #region MonoBehabiourMethods

        private void Update()
        {
            if (!GetTouchInputDown() && !GetTouchInput() && !GetTouchInputUp() && _isFingerDown)
            {
                _startFingerPosition = Vector2.zero;
                _currentFingerPosition = Vector2.zero;
                _isFingerDown = false;
                _isFingerMoving = false;
            }

            _inputJustEnded = false;

            _timeSinceLastInput += Time.deltaTime;

            //  Input started
            if (GetTouchInputDown())
            {
                _isFingerDown = true;
                _startFingerPosition = _currentFingerPosition = GetScreenTouchPosition();
                _touchDownTime = 0;
            }
            //  Input happening
            else if (GetTouchInput())
            {
                _isFingerMoving = (Vector2.Distance(_currentFingerPosition, GetScreenTouchPosition()) > 0.0001f);
                _currentMovementVector = GetScreenTouchPosition() - _currentFingerPosition;
                _currentFingerPosition = GetScreenTouchPosition();
                _touchDownTime += Time.deltaTime;
            }
            //  Input just ended
            else if (GetTouchInputUp())
            {
                _currentFingerPosition = GetScreenTouchPosition();
                _isFingerDown = false;
                _isFingerMoving = false;
                _inputJustEnded = true;
                _timeSinceLastInput = 0;

                Swipe();
            }

            if (Input.GetButton("Horizontal"))
            {
                EventManager.TriggerEvent(Input.GetAxis("Horizontal") > 0
                    ? GameEvent.INPUT_SWIPE_RIGHT
                    : GameEvent.INPUT_SWIPE_LEFT);
            }
            if (Input.GetButton("Vertical"))
            {
                EventManager.TriggerEvent(Input.GetAxis("Vertical") > 0
                    ? GameEvent.INPUT_SWIPE_UP
                    : GameEvent.INPUT_SWIPE_DOWN);
            }

            if (Input.GetKeyDown(KeyCode.P)) EventManager.TriggerEvent(GameEvent.INPUT_SCREENSHOT);

        }

        #endregion

        #region Methods

        #region PublicMethods

        /// <summary>
        /// Checks is a Tap has happened in the current frame;
        /// </summary>
        /// <returns>The tap.</returns>
        public static bool Tap()
        {
            return !Instance._isFingerDown && Instance._touchDownTime <= TapTime && Instance._inputJustEnded;
        }

        public static float ScreenLeftRightJoystick()
        {
            return Instance._isFingerDown?Mathf.Clamp((CurrentFingerPosition.x - Screen.width/2)/(Screen.width * Instance.joystickScreenPercentage)* 2,-1,1) : 0;
        }

        /// <summary>
        /// Checks if a Hold has happened in the current frame;
        /// </summary>
        /// <returns>The hold.</returns>
        public static bool Hold()
        {
            return !Instance._isFingerDown && Instance._touchDownTime >= HoldTime && Instance._inputJustEnded;
        }

        /// <summary>
        /// Checks if a holding event is currently happening;
        /// </summary>
        /// <returns>The holding.</returns>
        public static bool Holding()
        {
            return Instance._isFingerDown && Instance._touchDownTime >= HoldTime;
        }

        /// <summary>
        /// Calculates the vector between the touch starting position and the touch end position;
        /// </summary>
        /// <returns>The movement vector.</returns>
        public static Vector2 FingerMovementVector()
        {
            return Instance._currentFingerPosition - Instance._startFingerPosition;
        }

        public static bool FingerMoved()
        {
            return CurrentMovementVector.sqrMagnitude > Mathf.Pow(MinimumFingerMovement, 2);
        }

        public static Vector2 Direction()
        {
            var direction = Vector2.zero;
#if UNITY_EDITOR || UNITY_STANDALONE
            direction = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
#elif UNITY_IOS || UNITY_ANDROID
        direction = acceleration;
#endif
            return direction;
        }

        public static void CalibrateAccelerometer()
        {
            Instance._wantedDeadZone = Input.acceleration;
            var rotateQuaternion = Quaternion.FromToRotation(new Vector3(0f, 0f, -1f), Instance._wantedDeadZone);

            //create identity matrix ... rotate our matrix to match up with down vec
            var matrix = Matrix4x4.TRS(Vector3.zero, rotateQuaternion, new Vector3(1f, 1f, 1f));

            //get the inverse of the matrix
            Instance._calibrationMatrix = matrix.inverse;
        }

        #endregion

        #region PrivateMethods

        /// <summary>
        /// Check if the touch input has started
        /// </summary>
        /// <returns><c>true</c>, if touch input has just started, <c>false</c> otherwise.</returns>
        private static bool GetTouchInputDown()
        {
            var isTouchInputDown = false;
#if UNITY_EDITOR || UNITY_STANDALONE
            isTouchInputDown = Input.GetMouseButtonDown(0);
#elif UNITY_IOS || UNITY_ANDROID
        if (Input.touchCount > 0)
        {
            isTouchInputDown = Input.touches[0].phase == TouchPhase.Began;
        }
#endif
            return isTouchInputDown;
        }

        private static bool GetTouchInputUp()
        {
            var isTouchInputUp = false;
#if UNITY_EDITOR || UNITY_STANDALONE
            isTouchInputUp = Input.GetMouseButtonUp(0);
#elif UNITY_IOS || UNITY_ANDROID
            if (Input.touchCount > 0)
            {
                isTouchInputUp = Input.touches[0].phase == TouchPhase.Ended ||
                                 Input.touches[0].phase == TouchPhase.Canceled;
            }
#endif
            return isTouchInputUp;
        }

        /// <summary>
        /// Check if the touch input
        /// </summary>
        /// <returns><c>true</c>, if touch input is in progress, <c>false</c> otherwise.</returns>
        private static bool GetTouchInput()
        {
            var isTouchInput = false;
#if UNITY_EDITOR || UNITY_STANDALONE
            isTouchInput = Input.GetMouseButton(0);
#elif UNITY_IOS || UNITY_ANDROID
        if (Input.touchCount > 0)
        {
            isTouchInput =
 Input.touches[0].phase == TouchPhase.Moved || Input.touches[0].phase == TouchPhase.Stationary;
        }
#endif
            return isTouchInput;
        }

        /// <summary>
        /// Gets the screen touch position.
        /// </summary>
        /// <returns>The screen touch position.</returns>
        private static Vector2 GetScreenTouchPosition()
        {
            var screenTouchPosition = Vector2.negativeInfinity;
#if UNITY_EDITOR || UNITY_STANDALONE
            screenTouchPosition = Input.mousePosition;
#elif UNITY_IOS || UNITY_ANDROID
        if(Input.touchCount > 0){
            screenTouchPosition = Input.touches[0].position;
        }
#endif
            return screenTouchPosition;
        }

        private void Swipe()
        {
            if (!(FingerMovementVector().SqrMagnitude() > MinimumFingerMovement * MinimumFingerMovement)) return;
            var angle = Vector3.SignedAngle(FingerMovementVector(), Vector2.up, Vector3.forward);
            angle += (angle < 0 ? 360 : 0) + 45;
            if (angle < 0) angle = 360 - angle;
            else if (angle > 360) angle -= 360;
            if (angle <= 90)
            {
                EventManager.TriggerEvent(GameEvent.INPUT_SWIPE_UP);
            }
            else if (angle <= 180)
            {
                EventManager.TriggerEvent(GameEvent.INPUT_SWIPE_RIGHT);
            }
            else if (angle <= 270)
            {
                EventManager.TriggerEvent(GameEvent.INPUT_SWIPE_DOWN);
            }
            else if (angle <= 360)
            {
                EventManager.TriggerEvent(GameEvent.INPUT_SWIPE_LEFT);
            }
        }

        #endregion

        #endregion
    }
}