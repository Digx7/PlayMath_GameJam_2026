using UnityEngine;
using UnityEngine.Events;

public class ToolChannelRaiser : MonoBehaviour
{
    [SerializeField] private ToolChannel channelToRaise;
    [SerializeField] private Tool m_value;

    public void RaiseMValue()
    {
        Debug.Log("ToolChannelRaiser: Raise() -> " + m_value);
        channelToRaise.Raise(m_value);
    }

    public void Raise(Tool value)
    {
        channelToRaise.Raise(value);
    }
}