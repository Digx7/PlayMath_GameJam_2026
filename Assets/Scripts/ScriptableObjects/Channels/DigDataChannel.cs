using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "NewDigDataChannel", menuName = "ScriptableObjects/Channels/DigData", order = 1)]
public class DigDataChannel : ScriptableObject
{

    public DigDataEvent channelEvent = new DigDataEvent();

    public void Raise(DigData value)
    {
        channelEvent.Invoke(value);
    }

    
}
