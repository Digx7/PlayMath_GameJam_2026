using UnityEngine;
using UnityEngine.Events;

public class LevelDataChannelRaiser : MonoBehaviour
{
    [SerializeField] private LevelDataChannel channelToRaise;
    [SerializeField] private LevelData m_data;
    public LevelData Data
    {
        private get
        {
            return m_data;
        }
        set
        {
            if(value is LevelData)
            {
                m_data = value;
            }
        }
    }

    public void Raise(LevelData data)
    {
        channelToRaise.Raise(data);
    }

    public void RaiseMData()
    {
        channelToRaise.Raise(m_data);
    }
}