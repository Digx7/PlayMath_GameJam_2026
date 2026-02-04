using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "NewToolChannel", menuName = "ScriptableObjects/Channels/Tool", order = 1)]
public class ToolChannel : ScriptableObject
{
    public ToolEvent channelEvent = new ToolEvent();

    public void Raise(Tool value)
    {
        channelEvent.Invoke(value);
    }

    
}
