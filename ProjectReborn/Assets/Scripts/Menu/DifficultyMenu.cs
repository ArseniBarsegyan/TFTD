using UnityEngine;

public class DifficultyMenu : MonoBehaviour
{
    public void SetDifficulty(int level)
    {
        DifficultyLevel difficultyLevel = (DifficultyLevel) level;
        Managers.Data.SaveCurrentDifficulty(difficultyLevel);
        Debug.Log(Managers.Data.LoadCurrentDifficulty());

        // Start new game after select difficulty
        Managers.Mission.StartNewGame();
    }

    public void BackToMain()
    {
        Managers.Menu.BackToMain();
    }
}
