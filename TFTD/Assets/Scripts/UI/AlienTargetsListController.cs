using Assets.Scripts.Messaging;
using UnityEngine;

public class AlienTargetsListController : MonoBehaviour
{
    [SerializeField] private GameObject alienTargetsPanel;
    [SerializeField] private GameObject alienListViewItemPrefab;

    void Awake()
    {
        MessagingCenter.Subscribe<GameEventsController, AlienSubDto>
        (this, GameEvent.AlienSubSpawn,
            (controller, dto) =>
            {
                AddSpawnedAlienSubToList(dto);
            });
    }

    void Destroy()
    {
        MessagingCenter.Unsubscribe<GameEventsController, AlienSubDto>(this,
            GameEvent.AlienSubSpawn);
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
        }
    }
}
