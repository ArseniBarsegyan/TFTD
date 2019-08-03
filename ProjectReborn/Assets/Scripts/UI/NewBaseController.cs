using UnityEngine;

public class NewBaseController : MonoBehaviour
{
    [SerializeField] private GameObject messagePanel;
    [SerializeField] private GameObject baseLocationPanel;

    public void ShowNewBasePanel()
    {
        messagePanel.SetActive(true);
    }

    public void HideNewBasePanel()
    {
        messagePanel.SetActive(false);
    }
}
