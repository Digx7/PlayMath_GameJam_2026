using UnityEngine;
using UnityEngine.Events;

namespace Digx7.Levels
{
    public class LevelGeneratorTemplateDataChannelListener : MonoBehaviour 
    {
        [SerializeField] private LevelGeneratorTemplateDataChannel channelToListenTo;

        public LevelGeneratorTemplateDataEvent onChannelRaised;

        public bool checkLastValueOnStart;
        public bool shouldFilterValue = false;
        public bool shouldPassHeardDataThrough = true;

        public LevelGeneratorTemplateData filter;
        public LevelGeneratorTemplateData outgoingDataIfNotPassHeardDataThrough;

        private void Start()
        {
            if (checkLastValueOnStart) OnHearChannel(channelToListenTo.lastValue);
        }

        private void OnEnable()
        {
            channelToListenTo.channelEvent.AddListener(OnHearChannel);
        }

        private void OnDisable()
        {
            channelToListenTo.channelEvent.RemoveListener(OnHearChannel);
        }

        public void OnHearChannel(LevelGeneratorTemplateData data)
        {
            if(shouldFilterValue)
            {
                if(data == filter)
                {
                    SendOutResponse(data);
                }
            }
            else
            {
                SendOutResponse(data);
            }
        }

        public void SendOutResponse(LevelGeneratorTemplateData incomingData)
        {
            if(shouldPassHeardDataThrough) 
            {
                onChannelRaised.Invoke(incomingData);
            }
            else
            {
                onChannelRaised.Invoke(outgoingDataIfNotPassHeardDataThrough);
            }
        }
    }
}