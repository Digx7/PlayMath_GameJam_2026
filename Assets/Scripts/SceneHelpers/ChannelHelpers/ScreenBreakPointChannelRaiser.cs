using UnityEngine;
using UnityEngine.Events;

namespace Digx7.Zygote
{
    public class ScreenBreakPointChannelRaiser : MonoBehaviour
    {
        #region Variables ==============================================
        [SerializeField] private ScreenBreakPointChannel channelToRaise;
        [SerializeField] private ScreenBreakPoint _data;
        #endregion

        #region Setup ==============================================

        #endregion

        #region Channel Response Functions ==============================================

        #endregion

        #region Main Functions ==============================================

        public void Raise(ScreenBreakPoint data)
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
