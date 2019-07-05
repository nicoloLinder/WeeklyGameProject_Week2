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
            GameManager.Ball.ThrowBall(new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized * 2);
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
            var hitPosition = GameManager.Ball.ExpectedHitPosition();

            var percentage = GameFieldManager.Instance.ClosestPercentage(hitPosition);
            var currentPercentage = GameManager.Player.FloatPosition;

            if (Mathf.Abs(currentPercentage - percentage) > 0.5f)
            {
                percentage += (currentPercentage > percentage)? 1:-1;
            }
            
            if(Mathf.Abs(currentPercentage - percentage) > 0)
                GameManager.Player.SetPosition(Mathf.Lerp(currentPercentage, percentage, 0.05f));
        }
    }
}