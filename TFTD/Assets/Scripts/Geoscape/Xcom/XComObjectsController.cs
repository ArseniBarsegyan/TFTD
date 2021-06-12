using Assets.Scripts.Messaging;
using UnityEngine;

[RequireComponent(typeof(InterceptorsController))]
[RequireComponent(typeof(XComBasesController))]
[RequireComponent(typeof(XComSoldiersController))]
public class XComObjectsController : MonoBehaviour
{
    void Awake()
    {
        InterceptorsController = GetComponent<InterceptorsController>();
        BasesController = GetComponent<XComBasesController>();
        SoldiersController = GetComponent<XComSoldiersController>();

        MessagingCenter.Subscribe<GameEventsController, GeoPosition>
            (this, GameEvent.GameEventsControllerXComBaseCreated,
            (controller, geoPosition) =>
            {
                BasesController.CreateXComBase(geoPosition);
            });

        MessagingCenter.Subscribe<SelectInterceptor, TargetSelectedDto>
            (this, GameEvent.SelectInterceptorSelectConfirmed,
            (component, dto) =>
            {
                InterceptorsController.StartIntercept(dto);
            });

        MessagingCenter.Subscribe<Interceptor>
            (this, GameEvent.InterceptorReturnedToBase,
            (interceptor) =>
            {
                InterceptorsController.ReturnInterceptorToBase(interceptor);
            });
    }

    void Destroy()
    {
        MessagingCenter.Unsubscribe<GameEventsController, GeoPosition>(this, GameEvent.GameEventsControllerXComBaseCreated);
        MessagingCenter.Unsubscribe<SelectInterceptor, TargetSelectedDto>(this, GameEvent.SelectInterceptorSelectConfirmed);
        MessagingCenter.Unsubscribe<Interceptor>(this, GameEvent.InterceptorReturnedToBase);
    }

    public static InterceptorsController InterceptorsController { get; set; }
    public static XComBasesController BasesController { get; set; }
    public static XComSoldiersController SoldiersController { get; set; }
}
