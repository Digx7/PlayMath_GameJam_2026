using UnityEngine;
using UnityEngine.Events;

namespace Digx7.Zygote
{
    public class BreakPoinDataAndUnityEventPairChannelRaiser : MonoBehaviour
    {
        #region Variables ==============================================
        [SerializeField] private BreakPoinDataAndUnityEventPairChannel channelToRaise;
        [SerializeField] private BreakPoinDataAndUnityEventPair _data;
        #endregion

        #region Setup ==============================================

        #endregion

        #region Channel Response Functions ==============================================

        #endregion

        #region Main Functions ==============================================

        public void Raise(BreakPoinDataAndUnityEventPair data)
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
