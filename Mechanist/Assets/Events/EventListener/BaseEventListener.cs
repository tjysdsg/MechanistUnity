using UnityEngine;
using UnityEngine.Events;

public class BaseEventListener<T> : MonoBehaviour
{
    [SerializeField] private BaseEventChannelSO<T> channel = default;

    public UnityEvent<T> onEventRaised;

    private void OnEnable()
    {
        if (channel != null)
            channel.OnEventRaised += Respond;
    }

    private void OnDisable()
    {
        if (channel != null)
            channel.OnEventRaised -= Respond;
    }

    private void Respond(T value)
    {
        if (onEventRaised != null)
            onEventRaised.Invoke(value);
    }
}

public class BaseEventListener : MonoBehaviour
{
    [SerializeField] private BaseEventChannelSO channel = default;

    public UnityEvent onEventRaised;

    private void OnEnable()
    {
        if (channel != null)
            channel.OnEventRaised += Respond;
    }

    private void OnDisable()
    {
        if (channel != null)
            channel.OnEventRaised -= Respond;
    }

    private void Respond()
    {
        if (onEventRaised != null)
            onEventRaised.Invoke();
    }
}