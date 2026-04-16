using UnityEngine;
using UnityEngine.Events;

namespace Digx7.Zygote
{
    public class AxisTypeChannelRaiser : MonoBehaviour
    {
        #region Variables ==============================================
        [SerializeField] private AxisTypeChannel channelToRaise;
        [SerializeField] private AxisType _data;
        #endregion

        #region Setup ==============================================

        #endregion

        #region Channel Response Functions ==============================================

        #endregion

        #region Main Functions ==============================================

        public void Raise(AxisType data)
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
