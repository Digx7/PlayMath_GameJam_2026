using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;

namespace Digx7.Zygote
{
    [CreateAssetMenu(fileName = "NewScreenInfoDataChannel", menuName = "ScriptableObjects/Channels/ScreenInfo", order = 1)]
    public class ScreenInfoChannel : ScriptableObject
    {

        public bool debug = true;
        public ScreenInfoEvent channelEvent = new ScreenInfoEvent();
    
        public ScreenInfo lastValue { get; private set; }
    
        private void OnEnable()
        {
            ResetLastValue();
        }
    
        public void ResetLastValue()
        {
            lastValue = new ScreenInfo();
        }

        public void Raise(ScreenInfo value)
        {
            if (debug) Debug.Log("Raised Channel: " + this.name + " with value " + value);
        
            lastValue = value;
            channelEvent.Invoke(value);
        }    
    }
}