using UnityEngine;
using System;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "NewTool", menuName = "ScriptableObjects/Tools", order = 0)]
public class Tool : ScriptableObject {
    public Sprite sprite;

    public List<Vector2Int> relativeSpacesToDig;
}

[System.Serializable]
public struct CountToolPair
{
    public int count;
    public Tool tool;
}