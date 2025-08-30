using System.IO;
using UnityEngine;

namespace Internal.Core.DataModel
{
    public static class SaveSystem
    {
        private static readonly string SavePath = Path.Combine(Application.persistentDataPath, "save.json");

        public static void Save(PlayerData playerData)
        {
            var dataInJson = JsonUtility.ToJson(playerData, prettyPrint: true);
            File.WriteAllText(SavePath, dataInJson);
        }

        public static PlayerData Load()
        {
            if (!File.Exists(SavePath))
            {
                Debug.unityLogger.Log($"[{nameof(SaveSystem)}] No Save Found!");
                return new();
            }

            var jsonText = File.ReadAllText(SavePath);
            return JsonUtility.FromJson<PlayerData>(jsonText);
        }
    }
}