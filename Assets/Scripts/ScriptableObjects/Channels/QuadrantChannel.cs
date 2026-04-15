using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;

namespace Digx7.Zygote
{
    [CreateAssetMenu(fileName = "NewQuadrantDataChannel", menuName = "ScriptableObjects/Channels/Quadrant", order = 1)]
    public class QuadrantChannel : ScriptableObject
    {

        public bool debug = true;
        public QuadrantEvent channelEvent = new QuadrantEvent();
    
        public Quadrant lastValue { get; private set; }
    
        private void OnEnable()
        {
            ResetLastValue();
        }
    
        public void ResetLastValue()
        {
            lastValue = 0;
        }

        public void Raise(Quadrant value)
        {
            if (debug) Debug.Log("Raised Channel: " + this.name + " with value " + value);
        
            lastValue = value;
            channelEvent.Invoke(value);
        }    
    }
}