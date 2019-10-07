using Assets.Scripts.Messaging;
using UnityEngine;

public class AlienTargetsController : MonoBehaviour
{
    [SerializeField] private GameObject alienTargetsPanel;

    void Start()
    {
        MessagingCenter.Subscribe<GameEventsController, AlienSubDto>
        (this, GameEvent.AlienSubSpawn,
            (controller, dto) =>
            {
                AddSpawnedAlienSubToList(dto);
            });
    }

    void AddSpawnedAlienSubToList(AlienSubDto dto)
    {
        alienTargetsPanel.SetActive(true);
    }
}
