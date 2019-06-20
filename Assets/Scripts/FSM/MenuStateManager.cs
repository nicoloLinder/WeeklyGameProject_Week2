using Entities;
using UnityEngine;
using Utilities;

namespace FSM
{
    public class MenuStateManager : StateManager<MenuStateManager>
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
        
        public MenuState(MenuStateManager stateManager)
        {
            _stateManager = stateManager;
            stateID = StateID.MenuStateID;
        }
        
        public override void Reason(){
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
            _stateManager.player.Move(InputManager.ScreenLeftRightJoystick());
        }
    }
}