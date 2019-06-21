using Entities;
using UnityEngine;
using UnityEngine.UI;
using Utilities;

namespace FSM
{
    public class MenuStateManager : StateManager<MenuStateManager>
    {
        #region Variables

        #region PublicVariables

        [Header("GamePlay")] public Player player;
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
//            if (currentSpeed == 0 &&  InputManager.ScreenLeftRightJoystick() == 0) return;
            currentSpeed = Mathf.Lerp(currentSpeed, InputManager.ScreenLeftRightJoystick(), _stateManager.acceleration);
            _stateManager.player.Move(currentSpeed);
        }
    }
}