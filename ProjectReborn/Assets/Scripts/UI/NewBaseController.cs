using UnityEngine;

public class NewBaseController : MonoBehaviour
{
    [SerializeField] private GameObject messagePanel;

    public void ShowNewBasePanel()
    {
        messagePanel.SetActive(true);
    }

    public void HideNewBasePanel()
    {
        messagePanel.SetActive(false);
    }
}
