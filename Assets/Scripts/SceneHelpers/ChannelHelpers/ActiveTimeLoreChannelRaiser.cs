using UnityEngine;
using UnityEngine.Events;

public class ActiveTimeLoreChannelRaiser : MonoBehaviour
{
    [SerializeField] private ActiveTimeLoreChannel channelToRaise;
    [SerializeField] private ActiveTimeLore m_data;

    public void Raise(ActiveTimeLore m_data)
    {
        channelToRaise.Raise(m_data);
    }
}
