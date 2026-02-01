using UnityEngine;
using UnityEngine.Events;

public class DigDataChannelRaiser : MonoBehaviour
{
    [SerializeField] private DigDataChannel channelToRaise;
    [SerializeField] private DigData m_data;

    public void Raise(DigData data)
    {
        channelToRaise.Raise(data);
    }

    public void RaiseMData()
    {
        channelToRaise.Raise(m_data);
    }
}
