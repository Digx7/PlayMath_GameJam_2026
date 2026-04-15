using UnityEngine;
using UnityEngine.Events;

namespace Digx7.Zygote
{
    public class QuadrantChannelRaiser : MonoBehaviour
    {
        #region Variables ==============================================
        [SerializeField] private QuadrantChannel channelToRaise;
        [SerializeField] private Quadrant _data;
        #endregion

        #region Setup ==============================================

        #endregion

        #region Channel Response Functions ==============================================

        #endregion

        #region Main Functions ==============================================

        public void Raise(Quadrant data)
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
