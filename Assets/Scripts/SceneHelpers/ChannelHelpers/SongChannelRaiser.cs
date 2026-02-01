using UnityEngine;
using UnityEngine.Events;

public class SongChannelRaiser : MonoBehaviour
{
    [SerializeField] private SongChannel channelToRaise;
    [SerializeField] private SongData m_data;

    public void Raise(SongData m_data)
    {
        channelToRaise.Raise(m_data);
    }
}
