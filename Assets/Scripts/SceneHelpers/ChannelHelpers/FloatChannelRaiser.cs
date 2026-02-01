using UnityEngine;
using UnityEngine.Events;

public class FloatChannelRaiser : MonoBehaviour
{
    [SerializeField] private FloatChannel channelToRaise;
    [SerializeField] private float m_data;

    public void Raise(float m_data)
    {
        channelToRaise.Raise(m_data);
    }
}
