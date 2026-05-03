using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;

namespace Digx7.Zygote
{
    [CreateAssetMenu(fileName = "NewScreenBreakPointDataChannel", menuName = "ScriptableObjects/Channels/ScreenBreakPointData", order = 1)]
    public class ScreenBreakPointDataChannel : ScriptableObject
    {
        #region Variables ==============================================

        public bool debug = true;
        public ScreenBreakPointDataEvent channelEvent = new ScreenBreakPointDataEvent();
    
        public ScreenBreakPointData lastValue { get; private set; }
        #endregion

        #region Setup ==============================================
    
        private void OnEnable()
        {
            ResetLastValue();
        }
    
        public void ResetLastValue()
        {
            lastValue = null;
        }

        #endregion

        #region Main Functions ==============================================

        public void Raise(ScreenBreakPointData value)
        {
            if (debug) Debug.Log("Raised Channel: " + this.name + " with value " + value);
        
            lastValue = value;
            channelEvent.Invoke(value);
        }

        #endregion    
    }
}