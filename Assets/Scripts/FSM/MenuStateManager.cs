using Entities;
using GameField;
using UnityEngine;
using UnityEngine.UI;
using Utilities;
using Debug = UnityEngine.Debug;

namespace FSM
{
    public class MenuStateManager : StateManager<MenuStateManager>
    {
        #region Variables

        #region PublicVariables

        [Header("GamePlay")] public Player player;
        public Ball ball;
        [Range(0, 1)] public float acceleration = 0.5f;

        public Text uiText;

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
            state = new MenuState(this);
            ball.ThrowBall(new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized * 2);
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

    public class MenuState : FSMState
    {
        private readonly MenuStateManager _stateManager;

        private float currentSpeed;

        public MenuState(MenuStateManager stateManager)
        {
            _stateManager = stateManager;
            stateID = StateID.MenuStateID;
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
            var hitPosition = _stateManager.ball.HitPosition();
            var position = GameFieldManager.Instance.ClosestPercentage(hitPosition);
            var playerPosition = _stateManager.player.FloatPosition;

            var positionDir = playerPosition - position;

            if (Mathf.Abs(playerPosition - position) > 0.1f)
            {
                currentSpeed = Mathf.Lerp(currentSpeed, positionDir < 0 ? 1.5f : -1.5f,
                    0.05f);
                _stateManager.player.Move(currentSpeed);
            }
            else if (Mathf.Abs(playerPosition - position) > 0.01f)
            {
                currentSpeed = Mathf.Lerp(currentSpeed, positionDir < 0 ? 0.5f : -0.5f,
                    0.1f);
                _stateManager.player.Move(currentSpeed);
            }else
            {
                currentSpeed = Mathf.Lerp(currentSpeed, 0, 0.1f);
            }

//            currentSpeed = Mathf.Lerp(currentSpeed, InputManager.ScreenLeftRightJoystick(), _stateManager.acceleration);
//            _stateManager.player.Move(currentSpeed);
        }
    }
}