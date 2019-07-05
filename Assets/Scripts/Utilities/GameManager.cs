using System;
using System.Collections;
using System.Collections.Generic;
using Entities;
using FSM;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    #region Variables

    #region PublicVariables

    [SerializeField] private Player _player;
    [SerializeField] private Ball _ball;
    
    #endregion

    #region PrivateVariables

    private FSMSystem fsmSystem;

    #endregion

    #endregion

    #region Properties

    public static StateID CurrentState => Instance.fsmSystem.CurrentStateID;

    public static Player Player => Instance._player;

    public static Ball Ball => Instance._ball;

    #endregion

    #region MonoBehaviourMethods

    private void Start()
    {
        MakeFSM();
    }

    private void Update()
    {
        fsmSystem.CurrentState.Reason();
        fsmSystem.CurrentState.Act();
    }

    #endregion

    #region Methods

    #region PublicMethods

    public void Play()
    {
        switch (fsmSystem.CurrentStateID)
        {
            case StateID.NullStateID:
                Debug.LogError($"No valid transition to the {StateID.PlayStateID} state from the {fsmSystem.CurrentStateID} state");
                break;
            case StateID.MenuStateID:
                fsmSystem.PerformTransition(Transition.MenuPlayTransition);
                break;
            case StateID.PlayStateID:
                Debug.LogError($"No valid transition to the {StateID.PlayStateID} state from the {fsmSystem.CurrentStateID} state");
                break;
            case StateID.GameOverStateID:
                fsmSystem.PerformTransition(Transition.GameOverPlayTransition);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    public void Quit()
    {
        switch (fsmSystem.CurrentStateID)
        {
            case StateID.NullStateID:
                Debug.LogError($"No valid transition to the {StateID.MenuStateID} state from the {fsmSystem.CurrentStateID} state");
                break;
            case StateID.MenuStateID:
                Debug.LogError($"No valid transition to the {StateID.MenuStateID} state from the {fsmSystem.CurrentStateID} state");
                break;
            case StateID.PlayStateID:
                fsmSystem.PerformTransition(Transition.PlayMenuTransition);
                break;
            case StateID.GameOverStateID:
                fsmSystem.PerformTransition(Transition.GameOverMenuTransition);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
    
    public void GameOver()
    {
        switch (fsmSystem.CurrentStateID)
        {
            case StateID.NullStateID:
                Debug.LogError($"No valid transition to the {StateID.GameOverStateID} state from the {fsmSystem.CurrentStateID} state");
                break;
            case StateID.MenuStateID:
                Debug.LogError($"No valid transition to the {StateID.GameOverStateID} state from the {fsmSystem.CurrentStateID} state");
                break;
            case StateID.PlayStateID:
                fsmSystem.PerformTransition(Transition.PlayGameOverTransition);
                break;
            case StateID.GameOverStateID:
                Debug.LogError($"No valid transition to the {StateID.GameOverStateID} state from the {fsmSystem.CurrentStateID} state");
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    #endregion

    #region PrivateMethods

    private void MakeFSM()
    {

        var menuState = (MenuState) MenuStateManager.Instance.State;
        menuState.AddTransition(Transition.MenuPlayTransition, StateID.PlayStateID);

        var playState = (PlayState)PlayStateManager.Instance.State;
        playState.AddTransition(Transition.PlayGameOverTransition, StateID.GameOverStateID);
        playState.AddTransition(Transition.PlayMenuTransition, StateID.MenuStateID);
        
        var gameOverState = (GameOverState)GameOverStateManager.Instance.State;
        gameOverState.AddTransition (Transition.GameOverMenuTransition, StateID.MenuStateID);
        gameOverState.AddTransition (Transition.GameOverPlayTransition, StateID.PlayStateID);

        fsmSystem = new FSMSystem();
        fsmSystem.AddState(menuState);
        fsmSystem.AddState(playState);
        fsmSystem.AddState (gameOverState);
    }

    #endregion

    #endregion

    #region Coroutines

    #endregion
}