using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;

namespace Digx7.Zygote
{
    [CreateAssetMenu(fileName = "NewUIResponsiveBreakPointDataChannel", menuName = "ScriptableObjects/Channels/UIResponsiveBreakPoint", order = 1)]
    public class UIResponsiveBreakPointChannel : ScriptableObject
    {

        public bool debug = true;
        public UIResponsiveBreakPointEvent channelEvent = new UIResponsiveBreakPointEvent();
    
        public UIResponsiveBreakPoint lastValue { get; private set; }
    
        private void OnEnable()
        {
            ResetLastValue();
        }
    
        public void ResetLastValue()
        {
            lastValue = new UIResponsiveBreakPoint();
        }

        public void Raise(UIResponsiveBreakPoint value)
        {
            if (debug) Debug.Log("Raised Channel: " + this.name + " with value " + value);
        
            lastValue = value;
            channelEvent.Invoke(value);
        }    
    }
}