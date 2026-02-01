using UnityEngine;
using UnityEngine.Events;

public class Vector2ChannelRaiser : MonoBehaviour
{
    [SerializeField] private Vector2Channel channelToRaise;
    [SerializeField] private Vector2 m_data;

    public void Raise(Vector2 m_data)
    {
        channelToRaise.Raise(m_data);
    }
}
