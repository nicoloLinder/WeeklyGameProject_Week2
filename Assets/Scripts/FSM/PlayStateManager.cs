using System.Diagnostics;
using System.Numerics;
using Entities;
using GameEvents;
using GameField;
using Social;
using UnityEngine;
using Utilities;
using Debug = UnityEngine.Debug;
using Vector2 = UnityEngine.Vector2;

namespace FSM
{

    public class PlayStateManager : StateManager<PlayStateManager>
    {
        #region Variables

        #region PublicVariables

        #endregion

        #region PrivateVariables

        #endregion

        #endregion

        #region Properties

        #endregion

        #region MonoBehaviourMethods

        protected override void Awake()
        {
            base.Awake();
            state = new PlayState(this);
        }

        #endregion

        #region Methods

        #region PublicMethods

        #endregion

        #region PrivateMethods

        #endregion

        #endregion

        #region Coroutines

        #endregion
    }


    public class PlayState : FSMState
    {
        private readonly PlayStateManager _stateManager;
        private float currentSpeed;

        public PlayState(PlayStateManager stateManager)
        {
            _stateManager = stateManager;
            stateID = StateID.PlayStateID;

            StartListeningToEvents();
        }

        public override void Reason()
        {
        }

        public override void DoBeforeEntering()
        {
            _stateManager.onStateEnterTransitionEvent.Invoke();
            base.DoBeforeEntering();
        }

        public override void DoBeforeLeaving()
        {
            _stateManager.onStateExitTransitionEvent.Invoke();
            base.DoBeforeLeaving();
        }

        public override void Act()
        {
            if (!InputManager.IsFingerDown) return;
//            currentSpeed = Mathf.Lerp(currentSpeed, InputManager.ScreenLeftRightJoystick(), 0.5f);
            var currentFingerPosition = InputManager.CurrentFingerPositionScreenCentered;
            var percentage = Vector2.SignedAngle(Vector2.down, currentFingerPosition.normalized);

            percentage += (percentage < 0) ? 360 : 0;
            percentage /= 360;
            
            var currentPercentage = GameManager.Player.FloatPosition;

            if (Mathf.Abs(currentPercentage - percentage) > 0.5f)
            {
                percentage += (currentPercentage > percentage)? 1:-1;
            }
            
            if(Mathf.Abs(currentPercentage - percentage) > 0)
                GameManager.Player.SetPosition(Mathf.Lerp(currentPercentage, percentage, GameManager.Ball.squishing? 0.001f : 0.1f));
//            GameManager.Player.SetPosition(percentage);
//            var currentPlayerPosition = GameManager.Player.transform.position.normalized;
            
//            var sign = Mathf.Sign(currentFingerPosition.x * currentPlayerPosition.y - currentFingerPosition.y * currentPlayerPosition.x);
//            percentage = Vector2.Angle(currentFingerPosition, currentPlayerPosition) * sign;
        }

        private void StartListeningToEvents()
        {
            EventManager.StartListening(GameEvent.BALL_PLAYER_HIT, OnBallPlayerHit);
            EventManager.StartListening(GameEvent.BALL_WALL_HIT, OnBallWallHit);
        }
        
        private void OnBallPlayerHit()
        {
            if (GameManager.CurrentState == stateID)
            {
                ScoreManager.Instance.Score++;
            }
        }

        private void OnBallWallHit()
        {
            if (GameManager.CurrentState == stateID)
            {
                ScoreManager.Instance.Score--;
                EventManager.TriggerEvent(GameEvent.WEAK_SCREEN_SHAKE);
            }
        }
    }
}