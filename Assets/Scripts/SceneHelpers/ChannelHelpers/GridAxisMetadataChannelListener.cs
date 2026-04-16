using UnityEngine;
using UnityEngine.Events;

namespace Digx7.Zygote
{
    public class GridAxisMetadataChannelListener : MonoBehaviour 
    {
        #region Variables ==============================================
        [SerializeField] private GridAxisMetadataChannel channelToListenTo;

        public GridAxisMetadataEvent onChannelRaised;

        public bool checkLastValueOnStart;
        public bool shouldFilterValue = false;
        public bool shouldPassHeardDataThrough = true;

        public GridAxisMetadata filter;
        public GridAxisMetadata outgoingDataIfNotPassHeardDataThrough;
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

        public void OnHearChannel(GridAxisMetadata data)
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

        public void SendOutResponse(GridAxisMetadata incomingData)
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