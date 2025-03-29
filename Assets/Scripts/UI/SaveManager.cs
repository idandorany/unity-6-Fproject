using System.IO;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    private string savePath;

    void Awake()
    {
        savePath = Application.persistentDataPath + "/save.json";

        if (Object.FindObjectsByType<SaveManager>(FindObjectsSortMode.None).Length > 1)
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
    }

    public void SaveGame(int wave, float volume)
    {
        SaveData data = new SaveData
        {
            wave = wave,
            volume = volume
        };

        string json = JsonUtility.ToJson(data);
        File.WriteAllText(savePath, json);
        Debug.Log("Game Saved: " + json);
    }

    public SaveData LoadGame()
    {
        if (File.Exists(savePath))
        {
            string json = File.ReadAllText(savePath);
            SaveData data = JsonUtility.FromJson<SaveData>(json);
            Debug.Log("Game Loaded: " + json);
            return data;
        }

        Debug.LogWarning("Save file not found.");
        return new SaveData();
    }
}
