using Assets.Scripts.Messaging;

using System.Collections;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;
using UnityEngine.UI;

public class InterceptorContextMenu : MonoBehaviour
{
    [SerializeField] private GameObject interceptorContextMenuPanel;
    [SerializeField] private GameObject interceptorContextMenuText;

    private List<InterceptorDto> _requests = new List<InterceptorDto>();

    void Awake()
    {
        interceptorContextMenuPanel.SetActive(false);

        MessagingCenter.Subscribe<InterceptorsController, InterceptorDto>(
            this,
            GameEvent.InterceptorControllerRequestContextAction,
            (controller, dto) =>
            {
                _requests.Add(dto);
                TryAskPlayer();
            });
    }

    private void TryAskPlayer()
    {
        interceptorContextMenuPanel.SetActive(true);
        StartCoroutine(AskCoroutine());
    }

    private IEnumerator AskCoroutine()
    {
        while (_requests.Any())
        {
            var dto = _requests.LastOrDefault();

            var engageMessage = interceptorContextMenuText.GetComponent<Text>();

            if (engageMessage == null)
                yield return null;

            if (dto.InterceptorType == InterceptorType.Triton)
            {
                engageMessage.text = $"{dto.Name}: Begin landing mission?";
            }
            else
            {
                engageMessage.text = $"{dto.Name}: Start fight?";
            }
            yield return null;
        }
        interceptorContextMenuPanel.SetActive(false);
        yield return null;
    }

    public void ConfirmEngage()
    {
        MessagingCenter.Send(this, GameEvent.InterceptorContextMenuEngageConfirmed);
        _requests.Remove(_requests.LastOrDefault());
    }

    public void ContinueAction()
    {
        MessagingCenter.Send(this, GameEvent.InterceptorContextMenuContinueAction);
        _requests.Remove(_requests.LastOrDefault());
    }

    public void ReturnToBase()
    {
        MessagingCenter.Send(this, GameEvent.InterceptorContextMenuReturnToBaseAction);
        _requests.Remove(_requests.LastOrDefault());
    }

    void Destroy()
    {
        MessagingCenter.Unsubscribe<InterceptorsController, InterceptorDto>(
            this,
            GameEvent.InterceptorControllerRequestContextAction);
    }
}
