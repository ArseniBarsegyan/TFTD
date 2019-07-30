using UnityEngine;
using UnityEngine.SceneManagement;

public class MissionManager : MonoBehaviour
{
    private const string IsNewGame = "IsNewGame";

    public void StartNewGame()
    {
        PlayerPrefs.SetInt(IsNewGame, 1);
        SceneManager.LoadScene("Geoscape");
    }
}
