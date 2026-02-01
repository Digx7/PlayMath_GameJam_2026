using UnityEngine;
using UnityEngine.Events;

public class QuestDataChannelRaiser : MonoBehaviour
{
    [SerializeField] private QuestDataChannel channelToRaise;
    [SerializeField] private QuestData m_data;

    public void Raise(QuestData m_data)
    {
        channelToRaise.Raise(m_data);
    }
}
