using UnityEngine;
using UnityEngine.Events;

public class IntChannelRaiser : MonoBehaviour
{
    [SerializeField] private IntChannel channelToRaise;
    [SerializeField] private int m_data;

    public void Raise(int m_data)
    {
        channelToRaise.Raise(m_data);
    }
}
