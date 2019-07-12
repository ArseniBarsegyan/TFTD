using UnityEngine;
using Button = UnityEngine.UI.Button;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private Button newGameButton;
    [SerializeField] private Button loadGameButton;
    [SerializeField] private Button optionsButton;
    [SerializeField] private Button quitButton;

    public void SelectMenu(Button menuButton)
    {
        if (menuButton == newGameButton)
        {
            Managers.Menu.SelectDifficulty();
        }
        else if (menuButton == loadGameButton)
        {
            Managers.Menu.ShowLoadMenu();
        }
        else if (menuButton == optionsButton)
        {
            Managers.Menu.ShowOptions();
        }
        else if (menuButton == quitButton)
        {
            Managers.Menu.ConfirmQuit();
        }
    }
}
