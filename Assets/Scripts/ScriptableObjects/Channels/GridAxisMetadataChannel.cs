using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;

namespace Digx7.Zygote
{
    [CreateAssetMenu(fileName = "NewGridAxisMetadataDataChannel", menuName = "ScriptableObjects/Channels/GridAxisMetadata", order = 1)]
    public class GridAxisMetadataChannel : ScriptableObject
    {

        public bool debug = true;
        public GridAxisMetadataEvent channelEvent = new GridAxisMetadataEvent();
    
        public GridAxisMetadata lastValue { get; private set; }
    
        private void OnEnable()
        {
            ResetLastValue();
        }
    
        public void ResetLastValue()
        {
            lastValue = new GridAxisMetadata();
        }

        public void Raise(GridAxisMetadata value)
        {
            if (debug) Debug.Log("Raised Channel: " + this.name + " with value " + value);
        
            lastValue = value;
            channelEvent.Invoke(value);
        }    
    }
}