using Assets.Scripts.Messaging;
using UnityEngine;

public class ClickableAlienTarget : MonoBehaviour
{
    public AlienSubDto AlienSubDto;

    public void AlienTargetClicked()
    {
        MessagingCenter.Send(this, GameEvent.AlienTargetClicked, AlienSubDto.Id);
    }
}
