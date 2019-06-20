using System.Collections;
using UnityEngine;
#if UNITY_EDITOR
using UnityEngine.Windows;

#endif

namespace GameEvents
{
    public class BaseEvents : MonoBehaviour
    {
        #region Variables

        #region PublicVariables

        private static Transform _mainCamera;

        #endregion

        #region PrivateVariables

        #endregion

        #endregion

        #region Properties

        #endregion

        #region Methods

        #region MonoBehaviourMethods

        void Awake()
        {
            _mainCamera = Camera.main.transform;

            EventManager.StartListening(GameEvent.STRONG_SCREEN_SHAKE,
                () => { StartCoroutine(ScreenShakeCoroutine(0.15f, 0.5f, 0.75f)); });
            EventManager.StartListening(GameEvent.MEDIUM_SCREEN_SHAKE,
                () => { StartCoroutine(ScreenShakeCoroutine(0.15f, 0.25f, 0.5f)); });
            EventManager.StartListening(GameEvent.WEAK_SCREEN_SHAKE,
                () => { StartCoroutine(ScreenShakeCoroutine(0.15f, 0.1f, 0.25f)); });

#if UNITY_EDITOR
            EventManager.StartListening(GameEvent.INPUT_SCREENSHOT, Screenshot);
#endif
        }

        #endregion

        #region PublicMethods

        #endregion

        #region PrivateMethods

        private IEnumerator ScreenShakeCoroutine(float shakeTime, float shakeDecay, float shakeIntensity)
        {
#if UNITY_IOS || UNITY_ANDROID
            Handheld.Vibrate();
#endif
            while (shakeTime > 0)
            {
                _mainCamera.transform.localPosition = Vector3.zero;
                _mainCamera.transform.localPosition = Random.insideUnitSphere * shakeIntensity;
                shakeIntensity -= shakeDecay;
                shakeTime -= Time.deltaTime;
                yield return null;
            }

            _mainCamera.transform.localPosition = Vector3.zero;
        }

#if UNITY_EDITOR
        private void Screenshot()
        {
            if (!Directory.Exists("./ScreenShots/"))
            {
                Directory.CreateDirectory("./ScreenShots");
            }

            ScreenCapture.CaptureScreenshot($"./ScreenShots/ScreenShot_{System.DateTime.Now.Ticks}.png");
        }

#endif

        #endregion

        #endregion

        #region Coroutines

        #endregion
    }
}