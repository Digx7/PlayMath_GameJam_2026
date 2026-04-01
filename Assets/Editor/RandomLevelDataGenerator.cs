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
    [MenuItem("Tools/Random Level Data Generator")]
    public static void ShowWindow()
    {
        GetWindow<RandomLevelDataGenerator>("Random Level Data Generator");
    }

    private void OnGUI()
    {
        if (GUILayout.Button("Generate Random Level Data"))
        {
            GenerateRandomLevelData();
        }
    }

    private void GenerateRandomLevelData()
    {
        LevelData levelDataSO = LevelGenerator.GenerateRandomLevelData();
        LevelGenerator.SaveLevelDataAsAsset(levelDataSO, "Assets/GeneratedLevels");
    }
}