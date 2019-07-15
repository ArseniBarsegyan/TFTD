using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    private string _fileName;
    private const string CurrentDifficulty = "CurrentDifficulty";
    
    public void SaveCurrentDifficulty(DifficultyLevel level)
    {
        PlayerPrefs.SetInt(CurrentDifficulty, (int)level);
    }

    public DifficultyLevel LoadCurrentDifficulty()
    {
        return (DifficultyLevel)PlayerPrefs.GetInt(CurrentDifficulty);
    }

    public void SaveGameState(string fileName)
    {
        _fileName = Path.Combine(Application.persistentDataPath, fileName);
        var gameState = new Dictionary<string, object>
        {
            { CurrentDifficulty, LoadCurrentDifficulty() }
        };
        var stream = File.Create(_fileName);
        var formatter = new BinaryFormatter();
        formatter.Serialize(stream, gameState);
        stream.Close();
    }

    public void LoadGameState()
    {
        if (string.IsNullOrEmpty(_fileName))
        {
            return;
        }
        if (!File.Exists(_fileName))
        {
            Debug.Log("No saved game");
            return;
        }

        Dictionary<string, object> gameState;
        var formatter = new BinaryFormatter();
        using (var stream = File.Open(_fileName, FileMode.Open))
        {
            gameState = formatter.Deserialize(stream) as Dictionary<string, object>;
        }

        if (gameState != null)
        {
            SaveCurrentDifficulty((DifficultyLevel)gameState[CurrentDifficulty]);
        }
    }
}
