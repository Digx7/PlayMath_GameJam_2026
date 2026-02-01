using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "NewVector2IntChannel", menuName = "ScriptableObjects/Channels/Vector2Int", order = 1)]
public class Vector2IntChannel : ScriptableObject
{

    public Vector2IntEvent channelEvent = new Vector2IntEvent();

    public void Raise(Vector2Int value)
    {
        channelEvent.Invoke(value);
    }
}