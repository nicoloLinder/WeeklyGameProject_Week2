using UnityEngine;
using UnityEngine.Events;

namespace FSM
{
    public class StateManager<T> : Singleton<T> where T : Component
    {
        
        #region Variables

        #region PublicVariables
        
        [Tooltip("Unity Event to be called when the FSM transition into the state")]
        public UnityEvent onStateEnterTransitionEvent;
        [Tooltip("Unity Event to be called when the FSM transition out of the state")]
        public UnityEvent onStateExitTransitionEvent;

        #endregion

        #region PrivateVariables

        protected FSMState state;

        #endregion

        #endregion

        #region Properties
        
        public FSMState State => state;

        #endregion

        #region MonoBehaviourMethods

        #endregion

        #region Methods

        #region PublicMethods

        #endregion

        #region PrivateMethods

        #endregion

        #endregion
    }
}