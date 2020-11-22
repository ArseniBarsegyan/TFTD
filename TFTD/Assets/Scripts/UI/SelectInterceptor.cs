using Assets.Scripts.Messaging;
using System;
using UnityEngine;

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
                interceptorListItem.transform.position = interceptionPanel.transform.position;
                interceptorListItem.transform.SetParent(interceptionPanel.transform, true);

                var selectableListItem = interceptorListItem.GetComponent<SelectableInterceptorListItem>();
                if (selectableListItem != null)
                {
                    selectableListItem.InterceptorId = allInterceptors[i].Id;
                }
            }
        }
    }
    
    public void SelectInterceptorConfirmed(Guid id)
    {
        var dto = new TargetSelectedDto 
        {
            SpaceCraftId = id,
            TargetId = alienTargetId
        };
        MessagingCenter.Send(this, GameEvent.InterceptorSelected, dto);
    }

    public void HideInterceptionPanel()
    {
        interceptionPanel.SetActive(false);
    }
}
