using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Events;

namespace GameEvents
{

    public enum GameEvent
    {
        GAME_START,
        GAME_OVER,
        GAME_WIN,
        
        SCREEN_SIZE_CHANGED,
        
        INPUT_SWIPE_UP,
        INPUT_SWIPE_RIGHT,
        INPUT_SWIPE_DOWN,
        INPUT_SWIPE_LEFT,
        INPUT_SCREENSHOT,
        
        STRONG_SCREEN_SHAKE,
        MEDIUM_SCREEN_SHAKE,
        WEAK_SCREEN_SHAKE
        
    }

    public class EventManager : Singleton<EventManager>
    {

        #region Variables

        #region PublicVariables

        public bool debugLog = false;

        #endregion

        #region PrivateVariables

        private Dictionary<GameEvent, UnityEvent> eventDictionary;

        #endregion

        #endregion

        #region Properties

        #endregion

        #region MonoBehabiourMethods

        protected override void Awake()
        {
            base.Awake();
            Initialize();
        }

        #endregion

        #region Methods

        #region PublicMethods

        /// <summary>
        /// Starts the listening to an event.
        /// </summary>
        /// <param name="eventName">Event name.</param>
        /// <param name="listener">Listener.</param>
        public static void StartListening(GameEvent eventName, UnityAction listener)
        {
            if (Instance.eventDictionary.TryGetValue(eventName, out var thisEvent))
            {
                thisEvent.AddListener(listener);
            }
            else
            {
                thisEvent = new UnityEvent();
                thisEvent.AddListener(listener);
                Instance.eventDictionary.Add(eventName, thisEvent);
            }
        }

        /// <summary>
        /// Stops the listening to an event.
        /// </summary>
        /// <param name="eventName">Event name.</param>
        /// <param name="listener">Listener.</param>
        public static void StopListening(GameEvent eventName, UnityAction listener)
        {
            if (!initialized) return;
            if (Instance.eventDictionary.TryGetValue(eventName, out var thisEvent))
            {
                thisEvent.RemoveListener(listener);
            }
        }

        /// <summary>
        /// Removes an event completely.
        /// </summary>
        /// <param name="eventName">Event name.</param>
        public static void StopListening(GameEvent eventName)
        {
            if (!initialized) return;
            if (Instance.eventDictionary.TryGetValue(eventName, out _))
            {
                Instance.eventDictionary.Remove(eventName);
            }
        }

        /// <summary>
        /// Trigger an event.
        /// </summary>
        /// <param name="eventName">Event name.</param>
        public static void TriggerEvent(GameEvent eventName)
        {
            if (!initialized) return;
            if (!Instance.eventDictionary.TryGetValue(eventName, out var thisEvent)) return;
            thisEvent.Invoke();
        }

        /// <summary>
        /// Trigger an event from the scene.
        /// </summary>
        /// <param name="eventName">Event name.</param>
        [UsedImplicitly]
        public void TriggerEventFromInspector(GameEvent eventName)
        {
            if (eventDictionary.TryGetValue(eventName, out var thisEvent))
            {
                thisEvent.Invoke();
            }
        }

        #endregion

        #region PrivateMethods

        private void Initialize()
        {
            if (eventDictionary == null)
            {
                eventDictionary = new Dictionary<GameEvent, UnityEvent>();
            }
        }


        #endregion

        #endregion
    }

}