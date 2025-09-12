using System;
using System.IO;
using UnityEditor;

#if UNITY_EDITOR
using UnityEngine;
#endif

namespace Internal.Core.DataModel
{
    public static class SaveSystem
    {
        public static readonly string SavePath = Path.Combine(Application.persistentDataPath, "save.json");

        public static void Save(PlayerData playerData)
        {
            var dataInJson = JsonUtility.ToJson(playerData, prettyPrint: true);
            File.WriteAllText(SavePath, dataInJson);
        }

        public static PlayerData Load()
        {
            if (!File.Exists(SavePath))
            {
                Debug.unityLogger.Log($"[{nameof(SaveSystem)}] No Save Found! Creating new...");
                return new();
            }

            var jsonText = File.ReadAllText(SavePath);
            return JsonUtility.FromJson<PlayerData>(jsonText);
        }
        
        // ReSharper disable Unity.PerformanceAnalysis
        public static bool TryDeleteSave()
        {
            if (!File.Exists(SavePath))
            {
                Debug.Log($"<color=green>[{nameof(SaveSystem)}] Save doesn't exist!</color>");
                return true;
            }

            try
            {
                File.Delete(SavePath);
                
                Debug.Log($"<color=green>[{nameof(SaveSystem)}] Successfully deleted local save!</color>");
                return true;
            }
            catch (Exception exception)
            {
                Debug.LogError($"[{nameof(SaveSystem)}] Failed to delete save file: {exception}");
                return false;
            }
        }

#if UNITY_EDITOR
        [MenuItem(StaticKeys.PROJECT_NAME + "/Fast Utilities/Delete Save")]
        public static void DeleteSave_Editor()
        {
            TryDeleteSave();
        }
#endif
    }
}