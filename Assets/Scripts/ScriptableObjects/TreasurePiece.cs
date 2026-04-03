using UnityEngine;
using UnityEditor;
using UnityEditor.SceneTemplate;
using System;
using System.IO;
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

    [ContextMenu("Generate Test Level")]
    public void GenerateTestLevels()
    {
        Debug.Log($"Genrating test levels for {name}");
        
        // For each template
        string[] files = Directory.GetFiles("Assets/ScriptableObjects/LevelTemplates/PieceTesting/", "*.asset", SearchOption.TopDirectoryOnly);
        string path = "Assets/Editor/TestLevelData";

        Debug.Log($"Found {files.Length} templates to use");

        foreach (var file in files)
        {
            // Create level data
            LevelGeneratorTemplateData levelGeneratorTemplateData = (LevelGeneratorTemplateData)AssetDatabase.LoadAssetAtPath(file, typeof(LevelGeneratorTemplateData));
            Debug.Log($"{levelGeneratorTemplateData.name}");

            string newLevelName = $"{this.name}_{levelGeneratorTemplateData.name}";

            levelGeneratorTemplateData.treasurePiecesToUse[0] = this;
            LevelData levelData = levelGeneratorTemplateData.GenerateLevel(path, newLevelName);

            // Create test scene
            string testScenePath = $"Assets/Scenes/TestScene/{newLevelName}.unity";
            SceneTemplateAsset templateAsset = (SceneTemplateAsset)AssetDatabase.LoadAssetAtPath("Assets/Scenes/1-1.scenetemplate", typeof(SceneTemplateAsset));

            Debug.Log($"templateAsset = {templateAsset}");

            InstantiationResult result = SceneTemplateService.Instantiate(templateAsset, false, testScenePath);

            Debug.Log($"Scene: {result.scene} and SceneAsset: {result.sceneAsset}");
        }

        // Edit treaurePieces to include this this
        // Generate LevelData

        // Generate Scenes
    }

    #endif
}

[System.Serializable]
public struct SubSprite
{
    public string SubID;
    public Sprite subSprite;
    public Vector2Int relativePosition;
}

[System.Serializable]
public enum TreasurePieceRotation
{
    None,
    Rotate90,
    Rotate180,
    Rotate270
}