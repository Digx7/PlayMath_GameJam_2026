using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "NewLevelDataChannel", menuName = "ScriptableObjects/Channels/LevelData", order = 1)]
public class LevelDataChannel : ScriptableObject
{

    public LevelDataEvent channelEvent = new LevelDataEvent();

    public void Raise(LevelData value)
    {
        channelEvent.Invoke(value);
    }
}
