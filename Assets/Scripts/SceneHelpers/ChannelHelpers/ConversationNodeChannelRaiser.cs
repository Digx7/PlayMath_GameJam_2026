using UnityEngine;
using UnityEngine.Events;

public class ConversationNodeChannelRaiser : MonoBehaviour
{
    [SerializeField] private ConversationNodeChannel channelToRaise;
    [SerializeField] private ConversationNode m_data;

    public void Raise(ConversationNode m_data)
    {
        channelToRaise.Raise(m_data);
    }
}
