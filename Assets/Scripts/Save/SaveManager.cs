using System.IO;
using UnityEngine;

namespace SamGame.Save
{
    /// <summary>
    /// 세이브/로드 시스템
    /// </summary>
    public class SaveManager : MonoBehaviour
    {
        private static string SaveDirectory => Path.Combine(Application.persistentDataPath, "Saves");

        [System.Serializable]
        public class SaveData
        {
            public int CurrentTurn;
            public int CurrentYear;
            public string CurrentSeason;
            public string SaveDate;
            // TODO: 전체 게임 상태 직렬화
        }

        public void SaveGame(string slotName)
        {
            if (!Directory.Exists(SaveDirectory))
                Directory.CreateDirectory(SaveDirectory);

            var data = new SaveData
            {
                CurrentTurn = Core.GameManager.Instance.CurrentTurn,
                CurrentYear = Core.GameManager.Instance.CurrentYear,
                CurrentSeason = Core.GameManager.Instance.CurrentSeason.ToString(),
                SaveDate = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
            };

            string json = JsonUtility.ToJson(data, true);
            string path = Path.Combine(SaveDirectory, $"{slotName}.json");
            File.WriteAllText(path, json);

            Debug.Log($"Game saved to {path}");
        }

        public SaveData LoadGame(string slotName)
        {
            string path = Path.Combine(SaveDirectory, $"{slotName}.json");
            if (!File.Exists(path))
            {
                Debug.LogError($"Save file not found: {path}");
                return null;
            }

            string json = File.ReadAllText(path);
            return JsonUtility.FromJson<SaveData>(json);
        }

        public string[] GetSaveSlots()
        {
            if (!Directory.Exists(SaveDirectory))
                return System.Array.Empty<string>();

            var files = Directory.GetFiles(SaveDirectory, "*.json");
            var slots = new string[files.Length];
            for (int i = 0; i < files.Length; i++)
                slots[i] = Path.GetFileNameWithoutExtension(files[i]);
            return slots;
        }
    }
}
