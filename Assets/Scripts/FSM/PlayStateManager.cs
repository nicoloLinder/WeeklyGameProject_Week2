using System.Diagnostics;
using Utilities;

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

        public PlayState(PlayStateManager stateManager)
        {
            _stateManager = stateManager;
            stateID = StateID.PlayStateID;
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
           
        }
    }
}