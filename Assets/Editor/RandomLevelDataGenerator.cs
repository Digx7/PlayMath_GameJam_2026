using UnityEngine;
using UnityEditor;
using Digx7.Grids;
using Digx7.Levels;
using System;
using System.IO;
using System.Collections.Generic;
using System.Text.RegularExpressions;

public class RandomLevelDataGenerator : EditorWindow
{
    
    LevelGeneratorTemplateData selectedTemplate;
    string levelPathToSaveTo;
    string worldNumber;
    int startingLevelNumber;
    int numberOfLevelsToGenerate;

    [MenuItem("Tools/Random Level Data Generator")]
    public static void ShowWindow()
    {   
        GetWindow<RandomLevelDataGenerator>("Random Level Data Generator");
    }

    private void OnGUI()
    {
        
        selectedTemplate = (LevelGeneratorTemplateData)EditorGUILayout.ObjectField("Template", selectedTemplate, typeof(LevelGeneratorTemplateData), false);


        levelPathToSaveTo = EditorGUILayout.TextField("Level Path:", levelPathToSaveTo);

        worldNumber = EditorGUILayout.TextField("Level Name:", worldNumber);

        startingLevelNumber = EditorGUILayout.IntField("Starting Level Number", startingLevelNumber);
        numberOfLevelsToGenerate = EditorGUILayout.IntSlider("Number of Levels", numberOfLevelsToGenerate, 1, 10);

        if (GUILayout.Button("Generate Random Level Data"))
        {
            GenerateRandomLevelData();
        }
    }

    private void GenerateRandomLevelData()
    {
        
        LevelData lastLevelDataSO = null;
        for (int i = startingLevelNumber; i < (startingLevelNumber + numberOfLevelsToGenerate); i++)
        {
            LevelData levelDataSO = LevelGenerator.TryGenerateRandomLevelFromTemplate(selectedTemplate);

            if (lastLevelDataSO != null)
            {
                lastLevelDataSO.nextLevel = levelDataSO;
            }

            lastLevelDataSO = levelDataSO;

            LevelGenerator.SaveLevelDataAsAssetWithName(levelDataSO, levelPathToSaveTo, $"{worldNumber}-{i}");
        }
        
    }
}