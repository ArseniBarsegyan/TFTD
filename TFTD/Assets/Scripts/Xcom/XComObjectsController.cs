using Assets.Scripts.Messaging;
using System;
using UnityEngine;

[RequireComponent(typeof(InterceptorsController))]
[RequireComponent(typeof(XComBasesController))]
public class XComObjectsController : MonoBehaviour
{
    void Awake()
    {
        Interceptors = GetComponent<InterceptorsController>();
        BasesController = GetComponent<XComBasesController>();

        MessagingCenter.Subscribe<GameEventsController, GeoPosition>
            (this, GameEvent.XComBaseCreated,
            (controller, geoPosition) =>
            {
                BasesController.CreateXComBase(geoPosition);
            });

        MessagingCenter.Subscribe<SelectInterceptor, Guid>
            (this, GameEvent.AlienTargetClicked,
            (target, id) =>
            {
                // TODO: create interceptor game object, and send it to target
                Debug.Log("Interceptor guid " + id);
            });
    }

    void Destroy()
    {
        MessagingCenter.Unsubscribe<GameEventsController>(this, GameEvent.XComBaseCreated);
        MessagingCenter.Unsubscribe<SelectInterceptor>(this, GameEvent.AlienTargetClicked);
    }

    public static InterceptorsController Interceptors { get; set; }
    public static XComBasesController BasesController { get; set; }
}
