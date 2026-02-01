using UnityEngine;
using UnityEngine.Events;

public class LevelDataChannelRaiser : MonoBehaviour
{
    [SerializeField] private LevelDataChannel channelToRaise;
    [SerializeField] private LevelData m_data;

    public void Raise(LevelData data)
    {
        channelToRaise.Raise(data);
    }

    public void RaiseMData()
    {
        channelToRaise.Raise(m_data);
    }
}