using UnityEngine;
using UnityEngine.UI;

public class DifficultyMenu : MonoBehaviour
{
    [SerializeField] private Button beginnerButton;
    [SerializeField] private Button experiencedButton;
    [SerializeField] private Button veteranButton;
    [SerializeField] private Button geniusButton;
    [SerializeField] private Button superhumanButton;
    [SerializeField] private Button backToMainButton;

    public void SelectDifficulty(Button menuButton)
    {
        if (menuButton == beginnerButton)
        {
            Debug.Log("beginner");
        }
        else if (menuButton == experiencedButton)
        {
            Debug.Log("experienced");
        }
        else if (menuButton == veteranButton)
        {
            Debug.Log("veteran");
        }
        else if (menuButton == geniusButton)
        {
            Debug.Log("genius");
        }
        else if (menuButton == superhumanButton)
        {
            Debug.Log("superhuman");
        }
        else if (menuButton == backToMainButton)
        {
            Managers.Menu.BackToMain();
        }
    }
}
