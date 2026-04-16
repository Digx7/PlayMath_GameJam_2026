using UnityEngine;
using UnityEngine.Events;

namespace Digx7.Zygote
{
    public class GridAxisMetadataChannelRaiser : MonoBehaviour
    {
        #region Variables ==============================================
        [SerializeField] private GridAxisMetadataChannel channelToRaise;
        [SerializeField] private GridAxisMetadata _data;
        #endregion

        #region Setup ==============================================

        #endregion

        #region Channel Response Functions ==============================================

        #endregion

        #region Main Functions ==============================================

        public void Raise(GridAxisMetadata data)
        {
            channelToRaise.Raise(data);
        }

        public void Raise()
        {
            channelToRaise.Raise(_data);
        }

        #endregion
    }
}
