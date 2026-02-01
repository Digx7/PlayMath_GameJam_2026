using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "NewVector3IntChannel", menuName = "ScriptableObjects/Channels/Vector3Int", order = 1)]
public class Vector3IntChannel : ScriptableObject
{
    public Vector3IntEvent channelEvent = new Vector3IntEvent();

    public void Raise(Vector3Int value)
    {
        channelEvent.Invoke(value);
    }
}
