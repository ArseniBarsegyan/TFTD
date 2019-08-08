using UnityEngine;

public class NewBaseController : MonoBehaviour
{
    [SerializeField] private GameObject messagePanel;
    [SerializeField] private GameObject baseLocationPanel;
    [SerializeField] private GameObject speedButtonsPanel;

    public void ShowNewBasePanel()
    {
        messagePanel.SetActive(true);
        baseLocationPanel.SetActive(true);
        speedButtonsPanel.SetActive(false);
    }

    public void HideNewBasePanel()
    {
        messagePanel.SetActive(false);
        baseLocationPanel.SetActive(false);
        speedButtonsPanel.SetActive(true);
    }
}
