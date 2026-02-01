using UnityEngine;
using UnityEngine.Events;

public class QuestObjectiveProgressChannelRaiser : MonoBehaviour
{
    [SerializeField] private QuestObjectiveProgressChannel channelToRaise;
    [SerializeField] private QuestObjectiveProgress m_data;

    public void Raise(QuestObjectiveProgress m_data)
    {
        channelToRaise.Raise(m_data);
    }
}
