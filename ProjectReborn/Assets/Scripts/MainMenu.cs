using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public void ToDifficultyMenu()
    {
        Managers.Menu.SelectDifficulty();
    }

    public void ShowLoadingMenu()
    {
        Managers.Menu.ShowLoadMenu();
    }

    public void ShopOptionsMenu()
    {
        Managers.Menu.ShowOptions();
    }

    public void ConfirmQuit()
    {
        Managers.Menu.ConfirmQuit();
    }
}
