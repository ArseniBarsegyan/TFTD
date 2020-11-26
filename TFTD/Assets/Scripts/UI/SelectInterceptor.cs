using Assets.Scripts.Messaging;
using System;
using UnityEngine;
using UnityEngine.UI;

public class SelectInterceptor : MonoBehaviour
{
    [SerializeField] private GameObject interceptionPanel;
    [SerializeField] private GameObject interceptorListItemPrefab;

    private Guid alienTargetId;

    void Awake()
    {
        interceptionPanel.SetActive(false);
        MessagingCenter.Subscribe<ClickableAlienTarget, Guid>
            (this, GameEvent.AlienTargetClicked,
            (target, id) =>
            {
                ShowInterceptionPanel();
                alienTargetId = id;
            });

        MessagingCenter.Subscribe<SelectableInterceptorListItem, Guid>
            (this, GameEvent.InterceptorListItemSelected,
            (item, id) =>
            {
                SelectInterceptorConfirmed(id);
            });
    }

    void Destroy()
    {
        MessagingCenter.Unsubscribe<ClickableAlienTarget>
            (this, GameEvent.AlienTargetClicked);
        MessagingCenter.Unsubscribe<SelectableInterceptorListItem>
            (this, GameEvent.InterceptorListItemSelected);
    }

    public void ShowInterceptionPanel()
    {
        interceptionPanel.SetActive(true);

        var allInterceptors = XComObjectsController.Interceptors.InterceptorsList;

        for (int i = 0; i < allInterceptors.Count; i++)
        {
            var interceptorListItem = Instantiate(interceptorListItemPrefab);
            if (interceptorListItem != null)
            {
                DrawListItem(interceptorListItem, i);
                SetListItemText(interceptorListItem, $"Interceptor {i + 1}");
                SetupListItemSelectableInterceptorComponent(interceptorListItem, allInterceptors[i].Id);
            }
        }
    }

    private void SetListItemText(GameObject item, string text)
    {
        var button = item.GetComponent<Button>();
        var textComponent = button.GetComponentInChildren<Text>();
        textComponent.text = text;
    }

    private void DrawListItem(GameObject listItem, int index)
    {
        listItem.transform.position = GetListItemMaxTopPosition();
        listItem.transform.position = GetListItemActualPosition(listItem, index);
        listItem.transform.SetParent(interceptionPanel.transform, true);
    }

    private void SetupListItemSelectableInterceptorComponent(GameObject listItem, Guid id)
    {
        var selectableListItem = listItem.GetComponent<SelectableInterceptorListItem>();
        if (selectableListItem != null)
        {
            selectableListItem.InterceptorId = id;
        }
    }

    private Vector3 GetListItemMaxTopPosition()
    {
        float interceptionPanelTopCoordinate = ((RectTransform)interceptionPanel.transform).rect.yMax
            + interceptionPanel.transform.position.y;

        const float topPadding = 60f;
        float listItemMaxTopCoordinate = interceptionPanelTopCoordinate - topPadding;

        return new Vector3(
            interceptionPanel.transform.position.x,
            listItemMaxTopCoordinate,
            interceptionPanel.transform.position.z);
    }

    private Vector3 GetListItemActualPosition(GameObject listItem, int index)
    {
        Vector3 initialPosition = listItem.transform.position;

        float listItemHeight = ((RectTransform)listItem.transform).rect.height;
        float listItemActualY = initialPosition.y - listItemHeight * index;

        return new Vector3(initialPosition.x, listItemActualY, initialPosition.z);
    }

    public void SelectInterceptorConfirmed(Guid id)
    {
        var dto = new TargetSelectedDto
        {
            SpaceCraftId = id,
            TargetId = alienTargetId
        };
        MessagingCenter.Send(this, GameEvent.InterceptorSelectConfirmed, dto);
        HideInterceptionPanel();
    }

    public void HideInterceptionPanel()
    {
        interceptionPanel.SetActive(false);
    }
}
