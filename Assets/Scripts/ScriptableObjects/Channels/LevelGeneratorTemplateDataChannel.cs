using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;

namespace Digx7.Levels
{
    [CreateAssetMenu(fileName = "NewLevelGeneratorTemplateDataChannel", menuName = "ScriptableObjects/Channels/LevelGeneratorTemplateData", order = 1)]
    public class LevelGeneratorTemplateDataChannel : ScriptableObject
    {

        public bool debug = true;
        public LevelGeneratorTemplateDataEvent channelEvent = new LevelGeneratorTemplateDataEvent();
    
        public LevelGeneratorTemplateData lastValue { get; private set; }
    
        private void OnEnable()
        {
            ResetLastValue();
        }
    
        public void ResetLastValue()
        {
            lastValue = null;
        }

        public void Raise(LevelGeneratorTemplateData value)
        {
            if (debug) Debug.Log("Raised Channel: " + this.name + " with value " + value);
        
            lastValue = value;
            channelEvent.Invoke(value);
        }    
    }
}