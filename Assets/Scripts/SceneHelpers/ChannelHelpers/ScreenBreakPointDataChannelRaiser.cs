using UnityEngine;
using UnityEngine.Events;

namespace Digx7.Zygote
{
    public class ScreenBreakPointDataChannelRaiser : MonoBehaviour
    {
        #region Variables ==============================================
        [SerializeField] private ScreenBreakPointDataChannel channelToRaise;
        [SerializeField] private ScreenBreakPointData _data;
        #endregion

        #region Setup ==============================================

        #endregion

        #region Channel Response Functions ==============================================

        #endregion

        #region Main Functions ==============================================

        public void Raise(ScreenBreakPointData data)
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
