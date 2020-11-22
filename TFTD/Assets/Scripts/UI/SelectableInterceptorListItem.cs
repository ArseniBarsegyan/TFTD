using Assets.Scripts.Messaging;
using System;
using UnityEngine;

public class SelectableInterceptorListItem : MonoBehaviour
{
    public Guid InterceptorId { get; set; }

    public void Selected()
    {
        MessagingCenter.Send(
            this,
            GameEvent.InterceptorListItemSelected,
            InterceptorId);
    }
}
