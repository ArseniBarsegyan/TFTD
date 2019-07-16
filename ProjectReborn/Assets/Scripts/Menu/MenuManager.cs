using UnityEngine;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private GameObject mainMenuPanel;
    [SerializeField] private GameObject diffucultyPanel;
    [SerializeField] private GameObject loadGamePanel;
    [SerializeField] private GameObject optionsPanel;
    [SerializeField] private GameObject quitConfirmPanel;

    void Start()
    {
        InitialHideMenu();
    }

    void InitialHideMenu()
    {
        diffucultyPanel.gameObject.SetActive(false);
        loadGamePanel.gameObject.SetActive(false);
        optionsPanel.gameObject.SetActive(false);
        quitConfirmPanel.gameObject.SetActive(false);
    }

    public void BackToMain()
    {
        Hide(diffucultyPanel);
        Hide(loadGamePanel);
        Hide(optionsPanel);
        Hide(quitConfirmPanel);
        Show(mainMenuPanel);
    }

    public void SelectDifficulty()
    {
        Hide(mainMenuPanel);
        Show(diffucultyPanel);
    }

    public void ShowLoadMenu()
    {
        Hide(mainMenuPanel);
        Show(loadGamePanel);
    }

    public void ShowOptions()
    {
        Hide(mainMenuPanel);
        Show(optionsPanel);
    }

    public void ConfirmQuit()
    {
        Hide(mainMenuPanel);
        Show(quitConfirmPanel);
    }

    void Show(GameObject obj)
    {
        obj.gameObject.SetActive(true);
    }

    void Hide(GameObject obj)
    {
        obj.gameObject.SetActive(false);
    }
}
