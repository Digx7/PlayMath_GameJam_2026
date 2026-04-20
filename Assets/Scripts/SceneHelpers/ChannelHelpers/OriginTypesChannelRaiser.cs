using UnityEngine;
using UnityEngine.Events;

namespace Digx7.Grids
{
    public class OriginTypesChannelRaiser : MonoBehaviour
    {
        #region Variables ==============================================
        [SerializeField] private OriginTypesChannel channelToRaise;
        [SerializeField] private OriginTypes _data;
        #endregion

        #region Setup ==============================================

        #endregion

        #region Channel Response Functions ==============================================

        #endregion

        #region Main Functions ==============================================

        public void Raise(OriginTypes data)
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
