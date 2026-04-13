using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;

namespace Digx7.Zygote
{
    [CreateAssetMenu(fileName = "NewScreenBreakPointDataChannel", menuName = "ScriptableObjects/Channels/ScreenBreakPoint", order = 1)]
    public class ScreenBreakPointChannel : ScriptableObject
    {

        public bool debug = true;
        public ScreenBreakPointEvent channelEvent = new ScreenBreakPointEvent();
    
        public ScreenBreakPoint lastValue { get; private set; }
    
        private void OnEnable()
        {
            ResetLastValue();
        }
    
        public void ResetLastValue()
        {
            lastValue = new ScreenBreakPoint();
        }

        public void Raise(ScreenBreakPoint value)
        {
            if (debug) Debug.Log("Raised Channel: " + this.name + " with value " + value);
        
            lastValue = value;
            channelEvent.Invoke(value);
        }    
    }
}