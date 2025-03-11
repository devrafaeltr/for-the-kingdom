using System.IO;
using UnityEngine;
using Newtonsoft.Json;

namespace FTKingdom
{
    public class SaveManager
    {
        #region SaveInfos
        public static readonly (string name, string format) kingdomData = ("kingdom", ".dat");
        #endregion

#if UNITY_EDITOR
        private static readonly bool SHOW_DEBUG = false;
#endif

        public static T LoadData<T>((string name, string format) saveInfo, System.Action<bool> resultCallback = null) where T : new()
        {
            T dataToLoad = new();
            bool success = false;
            string path = GetPath(saveInfo);

            if (File.Exists(path))
            {
                string dataText = File.ReadAllText(path);
                dataToLoad = JsonConvert.DeserializeObject<T>(dataText);
                success = true;

#if UNITY_EDITOR
                if (SHOW_DEBUG)
                {
                    Debug.Log($"Loaded {typeof(T)} from {path}.");
                }
#endif
            }
#if UNITY_EDITOR
            else if (SHOW_DEBUG)
            {
                Debug.Log($"No file found. Returning new {typeof(T)}");
            }
#endif

            resultCallback?.Invoke(success);

            return dataToLoad;
        }

        public static void SaveData<T>((string name, string format) saveInfo, T data, System.Action<bool> resultCallback = null)
        {
            bool success = false;
            string path = GetPath(saveInfo);

            if (data != null)
            {
                File.WriteAllText(path, JsonConvert.SerializeObject(data));
                success = true;

#if UNITY_EDITOR
                if (SHOW_DEBUG)
                {
                    Debug.Log($"Saved {typeof(T)} at {path}.");
                }
#endif
            }
#if UNITY_EDITOR
            else if (SHOW_DEBUG)
            {
                Debug.LogError($"data param is null. Nothing to save.");
            }
#endif

            resultCallback?.Invoke(success);
        }

        public static void DeleteData((string name, string format) saveInfo, System.Action<bool> resultCallback = null)
        {
            bool success = false;
            string tempPath = GetPath(saveInfo);
            if (File.Exists(tempPath))
            {
#if UNITY_EDITOR
                if (SHOW_DEBUG)
                {
                    Debug.Log($"Deleted {saveInfo.name}{saveInfo.format} at {Application.persistentDataPath}");
                }
#endif
                File.Delete(tempPath);
                success = true;
            }
#if UNITY_EDITOR
            else if (SHOW_DEBUG)
            {
                Debug.Log($"No file named {saveInfo.name}{saveInfo.format} found at {Application.persistentDataPath}");
            }
#endif

            resultCallback?.Invoke(success);
        }

        private static string GetPath((string name, string format) saveData)
        {
            return $"{Application.persistentDataPath}/{saveData.name}{saveData.format}";
        }
    }
}