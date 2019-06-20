using UnityEngine;
using UnityEngine.UI;

namespace Option
{
    public class OptionManager : Singleton<OptionManager>
    {
        #region Variables

        #region PublicVariables

        public Toggle musicToggle;
        public Toggle sfxToggle;
        public Toggle increaseOnMoveToggle;

        #endregion

        #region PrivateVariables

        private OptionData _localCopyOfOptionData;

        #endregion

        #endregion

        #region Properties

        public bool IncreaseOnMove
        {
            get => _localCopyOfOptionData.increaseOnMove;
            set => _localCopyOfOptionData.increaseOnMove = value;
        }

        public bool Sfx
        {
            get => _localCopyOfOptionData.sfx;
            set => _localCopyOfOptionData.sfx = value;
        }

        public bool Music
        {
            get => _localCopyOfOptionData.music;
            set => _localCopyOfOptionData.music = value;
        }

        #endregion

        #region Methods

        #region MonoBehaviour Methods

        protected override void Awake()
        {
            base.Awake();

            LoadOptionData();
        }

        #endregion

        #region PublicMethods

        public void ToggleMusic()
        {
            SetMusic(!Music);
        }

        public void ToggleSFX()
        {
            SetSFX(!Sfx);
        }

        public void ToggleIncreaseOnMove()
        {
            SetIncreaseOnMove(!IncreaseOnMove);
        }
        
        public void SetMusic(bool value)
        {
            Music = value;
            musicToggle.isOn = Music;
            SaveOptionData();
        }

        public void SetSFX(bool value)
        {
            Sfx = value;
            sfxToggle.isOn = Sfx;
            SaveOptionData();
        }

        public void SetIncreaseOnMove(bool value)
        {
            IncreaseOnMove = value;
            increaseOnMoveToggle.isOn = IncreaseOnMove;
            SaveOptionData();
        }

        #endregion

        #region PrivateMethods
            
        private  void LoadOptionData()
        {
            _localCopyOfOptionData = PlayerPrefs.HasKey("optionData") ? JsonUtility.FromJson<OptionData>(PlayerPrefs.GetString("optionData")) : new OptionData();

            SetMusic(_localCopyOfOptionData.music);
            SetSFX(_localCopyOfOptionData.sfx);
            SetIncreaseOnMove((_localCopyOfOptionData.increaseOnMove));
        }

        private void SaveOptionData()
        {
            PlayerPrefs.SetString("optionData", JsonUtility.ToJson(_localCopyOfOptionData));
        }

        #endregion

        #endregion

        #region Coroutines

        #endregion
    }
}