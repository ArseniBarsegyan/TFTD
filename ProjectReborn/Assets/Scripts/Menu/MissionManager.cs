using UnityEngine;
using UnityEngine.SceneManagement;

public class MissionManager : MonoBehaviour
{
    public void StartNewGame()
    {
        SceneManager.LoadScene("Geoscape");
    }
}
