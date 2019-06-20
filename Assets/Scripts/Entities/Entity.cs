using UnityEngine;

namespace Entities
{
    public abstract class Entity : MonoBehaviour
    {
        #region Variables

        #region PublicVariables

        #endregion

        #region PrivateVariables

        protected float _position;

        #endregion

        #endregion

        #region Properties

        #endregion

        #region MonoBehaviourMethods

        void Start()
        {

        }

        void OnEnable()
        {

        }

        void OnDisable()
        {

        }

        void OnDestroy()
        {

        }

        #endregion

        #region Methods

        #region

        public abstract void Move(float direction);

        public abstract void SetPosition(float position);

        #endregion

        #region PrivateMethods

        #endregion

        #endregion

        #region Coroutines

        #endregion
    }
}