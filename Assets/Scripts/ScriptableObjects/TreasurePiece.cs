using UnityEngine;
using UnityEditor;
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Digx7.Levels;

[CreateAssetMenu(fileName = "NewTreasurePiece", menuName = "ScriptableObjects/TreasurePiece", order = 0)]
public class TreasurePiece : ScriptableObject 
{
    public string ID;
    public Sprite mainSprite;
    public List<SubSprite> subSprites;

    #if UNITY_EDITOR

    [ContextMenu("Generate Test Levels")]
    public void StartGeneratingTestLevels()
    {
        // StartCoroutine(GenerateTestLevels());
        GenerateTestLevels();
    }

    public void GenerateTestLevels()
    {
        Debug.Log($"Genrating test levels for {name}");
        
        // For each template
        string[] files = Directory.GetFiles("Assets/ScriptableObjects/LevelTemplates/PieceTesting/", "*.asset", SearchOption.TopDirectoryOnly);
        string path = "Assets/Editor/TestLevelData";

        Debug.Log($"Found {files.Length} templates to use");

        List<LevelData> generatedLevels = new List<LevelData>();

        foreach (var file in files)
        {
            // Create level data
            LevelGeneratorTemplateData levelGeneratorTemplateData = (LevelGeneratorTemplateData)AssetDatabase.LoadAssetAtPath(file, typeof(LevelGeneratorTemplateData));
            Debug.Log($"{levelGeneratorTemplateData.name}");

            string newLevelName = $"{this.name} {levelGeneratorTemplateData.gameName}";

            levelGeneratorTemplateData.treasurePiecesToUse[0] = this;
            LevelData levelData = levelGeneratorTemplateData.GenerateLevel(path, newLevelName);

            generatedLevels.Add(levelData);
        }

        LevelData lastLevel = null;

        for (int i = 0; i < generatedLevels.Count; i++)
        {
            if (lastLevel != null)
            {
                lastLevel.nextLevel = generatedLevels[i];
            }

            lastLevel = generatedLevels[i];

            if (i == (generatedLevels.Count - 1))
            {
                generatedLevels[i].isLastLevel = true;
            }
        }

        AssetDatabase.SaveAssets();


    }

    #endif
}

[System.Serializable]
public struct SubSprite
{
    public string SubID;
    public Sprite subSprite;
    public Sprite subSprite_Cropped;
    public Sprite subSprite_Overflow;
    public Vector2Int relativePosition;
}

[System.Serializable]
public enum TreasurePieceRotation
{
    None = 0,
    Rotate90 = 1,
    Rotate180 = 2,
    Rotate270 = 3
}