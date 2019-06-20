using UnityEngine.Events;

namespace FSM
{
    public class GameOverStateManager : StateManager<GameOverStateManager>
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
            state = new GameOverState(this);
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
    
    public class GameOverState : FSMState
    {
        private readonly GameOverStateManager _stateManager;
        public GameOverState(GameOverStateManager stateManager)
        {
            _stateManager = stateManager;
            stateID = StateID.GameOverStateID;
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

        public override void Act(){
        }
    }
}