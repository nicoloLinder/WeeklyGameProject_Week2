using System.Diagnostics;
using Entities;
using GameEvents;
using GameField;
using Social;
using UnityEngine;
using Utilities;

namespace FSM
{

    public class PlayStateManager : StateManager<PlayStateManager>
    {
        #region Variables

        #region PublicVariables

        public Player player;

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
            currentSpeed = Mathf.Lerp(currentSpeed, InputManager.ScreenLeftRightJoystick(), 0.5f);
            _stateManager.player.Move(currentSpeed);
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