using UnityEngine;
using UnityEngine.Events;

namespace Digx7.Zygote
{
    public class BreakPoinDataAndUnityEventPairChannelListener : MonoBehaviour 
    {
        #region Variables ==============================================
        [SerializeField] private BreakPoinDataAndUnityEventPairChannel channelToListenTo;

        public BreakPoinDataAndUnityEventPairEvent onChannelRaised;

        public bool checkLastValueOnStart;
        public bool shouldFilterValue = false;
        public bool shouldPassHeardDataThrough = true;

        public BreakPoinDataAndUnityEventPair filter;
        public BreakPoinDataAndUnityEventPair outgoingDataIfNotPassHeardDataThrough;
        #endregion

        #region Setup ==============================================

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

        #endregion

        #region Channel Response Functions ==============================================

        public void OnHearChannel(BreakPoinDataAndUnityEventPair data)
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

        #endregion

        #region Main Functions ==============================================

        public void SendOutResponse(BreakPoinDataAndUnityEventPair incomingData)
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

        #endregion
    }
}