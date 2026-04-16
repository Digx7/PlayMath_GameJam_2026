using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;

namespace Digx7.Zygote
{
    [CreateAssetMenu(fileName = "NewAxisTypeDataChannel", menuName = "ScriptableObjects/Channels/AxisType", order = 1)]
    public class AxisTypeChannel : ScriptableObject
    {

        public bool debug = true;
        public AxisTypeEvent channelEvent = new AxisTypeEvent();
    
        public AxisType lastValue { get; private set; }
    
        private void OnEnable()
        {
            ResetLastValue();
        }
    
        public void ResetLastValue()
        {
            lastValue = 0;
        }

        public void Raise(AxisType value)
        {
            if (debug) Debug.Log("Raised Channel: " + this.name + " with value " + value);
        
            lastValue = value;
            channelEvent.Invoke(value);
        }    
    }
}