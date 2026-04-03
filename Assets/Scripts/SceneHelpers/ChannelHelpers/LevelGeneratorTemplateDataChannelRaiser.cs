using UnityEngine;
using UnityEngine.Events;

namespace Digx7.Levels
{
    public class LevelGeneratorTemplateDataChannelRaiser : MonoBehaviour
    {
        [SerializeField] private LevelGeneratorTemplateDataChannel channelToRaise;
        [SerializeField] private LevelGeneratorTemplateData _data;

        public void Raise(LevelGeneratorTemplateData data)
        {
            channelToRaise.Raise(data);
        }

        public void Raise()
        {
            channelToRaise.Raise(_data);
        }
    }
}
