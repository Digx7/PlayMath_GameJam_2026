using UnityEngine;
using UnityEngine.Events;

public class Vector3ChannelRaiser : MonoBehaviour
{
    [SerializeField] private Vector3Channel channelToRaise;
    [SerializeField] private Vector3 m_data;

    public void Raise(Vector3 m_data)
    {
        channelToRaise.Raise(m_data);
    }
}
