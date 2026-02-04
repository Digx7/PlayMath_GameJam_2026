using UnityEngine;

[CreateAssetMenu(fileName = "NewTool", menuName = "ScriptableObjects/Tools", order = 0)]
public class Tool : ScriptableObject {
    public Sprite sprite;
}

[System.Serializable]
public struct CountToolPair
{
    public int count;
    public Tool tool;
}