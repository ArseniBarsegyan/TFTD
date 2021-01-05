using Assets.Scripts.Messaging;

using System.Collections;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;
using UnityEngine.UI;

public class EngageConfirm : MonoBehaviour
{
    [SerializeField] private GameObject engageConfirmationPanel;
    [SerializeField] private GameObject engageMessageText;

    private List<InterceptorDto> _requests = new List<InterceptorDto>();

    void Awake()
    {
        engageConfirmationPanel.SetActive(false);

        MessagingCenter.Subscribe<Interceptor, InterceptorDto>(
            this, GameEvent.InterceptorEngageRequest, (interceptor, dto) =>
            {
                _requests.Add(dto);
                TryAskPlayer();
            });
    }

    private void TryAskPlayer()
    {
        engageConfirmationPanel.SetActive(true);
        StartCoroutine(AskCoroutine());
    }

    private IEnumerator AskCoroutine()
    {
        while (_requests.Any())
        {
            var dto = _requests.LastOrDefault();

            var engageMessage = engageMessageText.GetComponent<Text>();

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
        engageConfirmationPanel.SetActive(false);
        yield return null;
    }

    public void ConfirmEngage()
    {
        MessagingCenter.Send(this, GameEvent.InterceptorEngageRequestConfirmed);
        _requests.Remove(_requests.LastOrDefault());
    }

    public void ContinuePursuit()
    {
        MessagingCenter.Send(this, GameEvent.InterceptorEngageContinuePursuit);
        _requests.Remove(_requests.LastOrDefault());
    }

    public void ReturnToBase()
    {
        MessagingCenter.Send(this, GameEvent.InterceptorEngageReturnToBase);
        _requests.Remove(_requests.LastOrDefault());
    }

    void Destroy()
    {
        MessagingCenter.Unsubscribe<Interceptor, InterceptorDto>(this, GameEvent.InterceptorEngageRequest);
    }
}
