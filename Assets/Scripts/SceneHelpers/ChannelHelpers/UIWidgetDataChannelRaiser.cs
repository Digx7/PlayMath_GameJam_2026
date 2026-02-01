using UnityEngine;
using UnityEngine.Events;

public class UIWidgetDataChannelRaiser : MonoBehaviour
{
    [SerializeField] private UIWidgetDataChannel channelToRaise;
    [SerializeField] private UIWidgetData m_data;

    public void Raise(UIWidgetData m_data)
    {
        channelToRaise.Raise(m_data);
    }
}
