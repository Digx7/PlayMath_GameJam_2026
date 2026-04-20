using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;

namespace Digx7.Grids
{
    [CreateAssetMenu(fileName = "NewOriginTypesDataChannel", menuName = "ScriptableObjects/Channels/OriginTypes", order = 1)]
    public class OriginTypesChannel : ScriptableObject
    {

        public bool debug = true;
        public OriginTypesEvent channelEvent = new OriginTypesEvent();
    
        public OriginTypes lastValue { get; private set; }
    
        private void OnEnable()
        {
            ResetLastValue();
        }
    
        public void ResetLastValue()
        {
            lastValue = 0;
        }

        public void Raise(OriginTypes value)
        {
            if (debug) Debug.Log("Raised Channel: " + this.name + " with value " + value);
        
            lastValue = value;
            channelEvent.Invoke(value);
        }    
    }
}