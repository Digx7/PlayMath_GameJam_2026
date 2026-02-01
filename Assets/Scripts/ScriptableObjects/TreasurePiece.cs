using UnityEngine;
using System;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "NewTreasurePiece", menuName = "ScriptableObjects/TreasurePiece", order = 0)]
public class TreasurePiece : ScriptableObject 
{
    public string ID;
    public Sprite mainSprite;
    public List<SubSprite> subSprites;
}

[System.Serializable]
public struct SubSprite
{
    public string SubID;
    public Sprite subSprite;
}