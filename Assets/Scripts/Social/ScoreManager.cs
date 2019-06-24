using System;
using FSM;
using GameEvents;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;

namespace Social
{
    public class ScoreManager : Singleton<ScoreManager>
    {
        #region Variables

        #region PublicVariables

        public Text scoreText;

        #endregion

        #region PrivateVariables

        private int _score;
        private float _time;
        private int _highScore;
        private float _bestTime;

        #endregion

        #endregion

        #region Properties

        public int Score
        {
            get => _score;
            set
            {
                _score = Mathf.Clamp(value, 0, int.MaxValue);
                scoreText.text = _score.ToString();
            }
        }

        public float Time
        {
            get => _time;
            set
            {
                _time = value;
            }
        }

        private int HighScore
        {
            get => _highScore;
            set
            {
                _highScore = value;
            }
        }

        public float BestTime
        {
            get => _bestTime;
            set
            {
                _bestTime = value;
            }
        }

        #endregion

        #region MonoBehaviourMethods

        protected override void Awake()
        {
            base.Awake();
            
            SubscribeToEvents();
            LoadScore();
        }
        
        #endregion

        #region Methods

        #region PublicMethods

        #endregion

        #region PrivateMethods

        private void SubscribeToEvents()
        {
        }

        private void LoadScore()
        {
            HighScore = PlayerPrefs.GetInt("HighScore",0);
        }

        [UsedImplicitly]
        private void SaveScore()
        {
            PlayerPrefs.SetInt("HighScore", HighScore);
        }

        #endregion

        #endregion

        #region Coroutines

        #endregion
    }
}