using UnityEngine;
using System;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "NewTreasurePiece", menuName = "ScriptableObjects/TreasurePiece", order = 0)]
public class TreasurePiece : ScriptableObject 
{
    public int ID;
    public Sprite mainSprite;
    public List<SubSprite> subSprites;
}

[System.Serializable]
public struct SubSprite
{
    public int SubID;
    public Sprite subSprite;
}