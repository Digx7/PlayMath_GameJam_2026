using UnityEngine;
using UnityEngine.Events;
using Digx7.Levels;

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

    public void RaiseRandom()
    {
        channelToRaise.Raise(LevelGenerator.GenerateRandomLevelData());
    }

    public void RaiseRandom(LevelGeneratorTemplateData levelGeneratorTemplateData)
    {
        channelToRaise.Raise(LevelGenerator.TryGenerateRandomLevelFromTemplate(levelGeneratorTemplateData));
    }
}