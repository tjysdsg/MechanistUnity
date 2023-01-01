using UnityEngine;
using UnityEngine.Events;

public class BaseEventChannelSO<T> : ScriptableObject
{
    public UnityAction<T> OnEventRaised;

    public void RaiseEvent(T value)
    {
        if (OnEventRaised != null)
            OnEventRaised.Invoke(value);
    }
}

public class BaseEventChannelSO : ScriptableObject
{
    public UnityAction OnEventRaised;

    public void RaiseEvent()
    {
        if (OnEventRaised != null)
            OnEventRaised.Invoke();
    }
}