using System.Collections.Generic;
using System;

#if UNITY_EDITOR
using UnityEngine;
#endif

namespace FTKingdom.Utils
{
    public static class EventsManager
    {
#if UNITY_EDITOR
        private const bool SHOW_DEBUG = false;
#endif
        #region Listerners - UI

        #endregion

        #region Listerners -  Game States

        #endregion

        #region Listerners -  Gameplay

        #endregion

        #region Listerners -  Structure

        #endregion Structure

        private static Dictionary<string, List<Action<IGameEvent>>> _eventDictionary = new Dictionary<string, List<Action<IGameEvent>>>();

        public static void AddListener(string eventName, Action<IGameEvent> callbackToAdd)
        {
            if (!_eventDictionary.TryGetValue(eventName, out List<Action<IGameEvent>> callbackList))
            {
                callbackList = new List<Action<IGameEvent>>();
                _eventDictionary.Add(eventName, callbackList);
#if UNITY_EDITOR
                if (SHOW_DEBUG)
                {
#pragma warning disable CS0162 // Unreachable code detected
                    Debug.LogWarning($"There is no event called: {eventName}. Creating a new one.");
#pragma warning restore CS0162 // Unreachable code detected
                }
#endif
            }
#if UNITY_EDITOR
            else if (SHOW_DEBUG)
            {
#pragma warning disable CS0162 // Unreachable code detected
                Debug.Log($"Event: {eventName}. Adding {callbackToAdd.Method} from {callbackToAdd.Target}.");
#pragma warning restore CS0162 // Unreachable code detected
            }
#endif

            callbackList.Add(callbackToAdd);
        }

        public static void RemoveListener(string eventName, Action<IGameEvent> callbackToRemove)
        {
            if (_eventDictionary.TryGetValue(eventName, out List<Action<IGameEvent>> callbackList))
            {
                callbackList?.Remove(callbackToRemove);

#if UNITY_EDITOR
                if (SHOW_DEBUG)
                {
#pragma warning disable CS0162 // Unreachable code detected
                    Debug.Log($"Event: {eventName}. Removing {callbackToRemove.Method} from {callbackToRemove.Target}.");
#pragma warning restore CS0162 // Unreachable code detected
                }
#endif
            }
#if UNITY_EDITOR
            else if (SHOW_DEBUG)
            {
#pragma warning disable CS0162 // Unreachable code detected
                Debug.LogWarning($"There is no event called: {eventName}.");
#pragma warning restore CS0162 // Unreachable code detected
            }
#endif
        }


        public static void Publish(string eventName, IGameEvent eventInfos = null)
        {
            if (_eventDictionary.TryGetValue(eventName, out List<Action<IGameEvent>> callbackList))
            {
                foreach (var callback in callbackList)
                {
                    callback?.Invoke(eventInfos);

                    if (callback != null)
                    {
#if UNITY_EDITOR
                        if (SHOW_DEBUG)
                        {
#pragma warning disable CS0162 // Unreachable code detected
                            Debug.Log($"Event: {eventName}. Calling {callback.Method} from {callback.Target}.");
#pragma warning restore CS0162 // Unreachable code detected
                        }
#endif
                    }
#if UNITY_EDITOR
                    else if (SHOW_DEBUG)
                    {
#pragma warning disable CS0162 // Unreachable code detected
                        Debug.LogWarning($"Event: {eventName}. Some callback is null.");
#pragma warning restore CS0162 // Unreachable code detected
                    }
#endif
                }
            }
        }
    }
}