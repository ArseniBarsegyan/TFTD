using Assets.Scripts.Messaging;

using UnityEngine;

public class AlienTargetsListController : MonoBehaviour
{
    [SerializeField] private GameObject alienTargetsPanel;
    [SerializeField] private GameObject alienListViewItemPrefab;

    void Awake()
    {
        MessagingCenter.Subscribe<GameEventsController, AlienSubDto>
        (this, GameEvent.GameEventsControllerAlienSubSpawn,
            (controller, dto) =>
            {
                AddSpawnedAlienSubToList(dto);
            });
    }

    void Destroy()
    {
        MessagingCenter.Unsubscribe<GameEventsController, AlienSubDto>(this,
            GameEvent.GameEventsControllerAlienSubSpawn);
    }

    void Start()
    {
        alienTargetsPanel.SetActive(false);
    }

    void AddSpawnedAlienSubToList(AlienSubDto dto)
    {
        alienTargetsPanel.SetActive(true);
        var alienSubListItem = Instantiate(alienListViewItemPrefab);
        if (alienSubListItem != null)
        {
            alienSubListItem.transform.position = alienTargetsPanel.transform.position;
            alienSubListItem.transform.SetParent(alienTargetsPanel.transform, true);
            var clickableAlienTarget = alienSubListItem.GetComponent<ClickableAlienTarget>();
            if (clickableAlienTarget != null)
            {
                clickableAlienTarget.AlienSubDto = dto;
            }
            else
            {
                Debug.Log("ClickableAlienTarget component is null");
            }
        }
    }
}
