using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Singleton<T> : MonoBehaviour where T : Component
{

    #region Variables

    /// <summary>
    /// The instance.
    /// </summary>
    private static T instance;

    #region PublicVariables

    public static bool initialized;

    #endregion

    #region PrivateVariables

    #endregion

    #endregion

    #region Properties

    /// <summary>
    /// Gets the instance.
    /// </summary>
    /// <value>The instance.</value>
    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<T>();
                if (instance == null)
                {
                    GameObject obj = new GameObject
                    {
                        name = typeof(T).Name
                    };
                    instance = obj.AddComponent<T>();
                }
            }

            initialized = true;
            return instance;
        }
    }

    #endregion

    #region MonoBehaviourMethods

    /// <summary>
    /// Use this for initialization.
    /// </summary>
    protected virtual void Awake()
    {
        if (instance == null)
        {
            instance = this as T;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
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