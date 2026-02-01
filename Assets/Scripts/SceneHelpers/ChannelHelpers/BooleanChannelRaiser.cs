using UnityEngine;
using UnityEngine.Events;

public class BooleanChannelRaiser : MonoBehaviour
{
    [SerializeField] private BooleanChannel channelToRaise;
    [SerializeField] private bool m_data;

    public void Raise(bool m_data)
    {
        channelToRaise.Raise(m_data);
    }
}
