using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;

namespace Digx7.Zygote
{
    [CreateAssetMenu(fileName = "NewBreakPoinDataAndUnityEventPairDataChannel", menuName = "ScriptableObjects/Channels/BreakPoinDataAndUnityEventPair", order = 1)]
    public class BreakPoinDataAndUnityEventPairChannel : ScriptableObject
    {

        public bool debug = true;
        public BreakPoinDataAndUnityEventPairEvent channelEvent = new BreakPoinDataAndUnityEventPairEvent();
    
        public BreakPoinDataAndUnityEventPair lastValue { get; private set; }
    
        private void OnEnable()
        {
            ResetLastValue();
        }
    
        public void ResetLastValue()
        {
            lastValue = new BreakPoinDataAndUnityEventPair();
        }

        public void Raise(BreakPoinDataAndUnityEventPair value)
        {
            if (debug) Debug.Log("Raised Channel: " + this.name + " with value " + value);
        
            lastValue = value;
            channelEvent.Invoke(value);
        }    
    }
}