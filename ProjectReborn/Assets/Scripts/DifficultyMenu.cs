using UnityEngine;

public class DifficultyMenu : MonoBehaviour
{
    public void SetDifficulty(int level)
    {
        DifficultyLevel difficultyLevel = (DifficultyLevel) level;
        Debug.Log(difficultyLevel);
    }

    public void BackToMain()
    {
        Managers.Menu.BackToMain();
    }
}
