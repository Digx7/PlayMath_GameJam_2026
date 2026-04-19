using UnityEngine;
using UnityEditor;
using Digx7.Grids;
using Digx7.Levels;
using System;
using System.IO;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

public class LevelAutoTester : EditorWindow
{
    public string startTime;
    
    [MenuItem("Tools/Level Auto Tester")]
    public static void ShowWindow()
    {   
        GetWindow<LevelAutoTester>("Level Auto Tester");
    }

    private void OnGUI()
    {
        string storyModeFirstLevelPath = "Assets/Resources/ScriptableObjects/LevelData/1-1.asset";
        string storyModeFolderPath = "Assets/Resources/ScriptableObjects/LevelData";

        string testLevelsFolderPath = "Assets/Editor/TestLevelData";
        
        if (GUILayout.Button("Run Story Level Tests"))
        {
            Debug.Log("LevelAutoTester: OnGUI()");
            RunLevelTestsAsync_TestLinking(storyModeFirstLevelPath, storyModeFolderPath);
        }

        if (GUILayout.Button("Run Editor Level Tests"))
        {
            Debug.Log("LevelAutoTester: OnGUI()");
            RunLevelTestsAsync(testLevelsFolderPath);
        }
    }

    private async Task RunLevelTestsAsync()
    {
        Debug.Log("LevelAutoTester: RunLevelTestsAsync()");
        
        string levelsFolderPath = "Assets/Editor/TestLevelData";
        string[] levelAssetPaths = Directory.GetFiles(levelsFolderPath, "*.asset", SearchOption.AllDirectories);
        startTime = DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");
        Debug.Log($"LevelAutoTester: Found {levelAssetPaths.Length} level assets to test");


        foreach (string assetPath in levelAssetPaths)
        {
            Debug.Log($"LevelAutoTester: Testing level at path: {assetPath}");
            
            LevelData levelData = AssetDatabase.LoadAssetAtPath<LevelData>(assetPath);
            if (levelData != null)
            {
                bool isValid = await ValidateLevelDataAsync(levelData);
                Debug.Log($"Level '{levelData.name}' validation result: {(isValid ? "Passed" : "Failed")}");
            }
            else
            {
                Debug.LogError($"Failed to load LevelData from path: {assetPath}");
            }
        }
    }

    private async Task RunLevelTestsAsync(string levelPath)
    {
        Debug.Log("LevelAutoTester: RunLevelTestsAsync()");
        
        // string levelsFolderPath = "Assets/Editor/TestLevelData";
        string[] levelAssetPaths = Directory.GetFiles(levelPath, "*.asset", SearchOption.AllDirectories);
        startTime = DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");
        Debug.Log($"LevelAutoTester: Found {levelAssetPaths.Length} level assets to test");


        foreach (string assetPath in levelAssetPaths)
        {
            Debug.Log($"LevelAutoTester: Testing level at path: {assetPath}");
            
            LevelData levelData = AssetDatabase.LoadAssetAtPath<LevelData>(assetPath);
            if (levelData != null)
            {
                bool isValid = await ValidateLevelDataAsync(levelData);
                Debug.Log($"Level '{levelData.name}' validation result: {(isValid ? "Passed" : "Failed")}");
            }
            else
            {
                Debug.LogError($"Failed to load LevelData from path: {assetPath}");
            }
        }
    }

    private async Task RunLevelTestsAsync_TestLinking(string beginingLevelPath, string allLevelDir)
    {
        Debug.Log("LevelAutoTester: RunLevelTestsAsync()");
        
        // string levelsFolderPath = "Assets/Editor/TestLevelData";
        string[] levelAssetPaths = Directory.GetFiles(allLevelDir, "*.asset", SearchOption.AllDirectories);
        int levelCount =levelAssetPaths.Length;
        startTime = DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");
        Debug.Log($"LevelAutoTester: Found {levelCount} level assets to test");

        LevelData startinglevelData = AssetDatabase.LoadAssetAtPath<LevelData>(beginingLevelPath);

        // Load level
        // Set Next Level in LevelManager
        LevelManager.Instance.SetCurrentLevel(startinglevelData);

        for (int i = 0; i < levelCount; i++)
        {
            Debug.Log($"Starting iteration {i}");
            bool isValid = await ValidateLevelDataWithLinkingAsync();
        }

        // foreach (string assetPath in levelAssetPaths)
        // {
        //     Debug.Log($"LevelAutoTester: Testing level at path: {assetPath}");
            
        //     LevelData levelData = AssetDatabase.LoadAssetAtPath<LevelData>(assetPath);
        //     if (levelData != null)
        //     {
        //         bool isValid = await ValidateLevelDataAsync(levelData);
        //         Debug.Log($"Level '{levelData.name}' validation result: {(isValid ? "Passed" : "Failed")}");
        //     }
        //     else
        //     {
        //         Debug.LogError($"Failed to load LevelData from path: {assetPath}");
        //     }
        // }
    }

    private async Task<bool> ValidateLevelDataAsync(LevelData levelData)
    {
        Debug.Log($"LevelAutoTester: Validating level '{levelData.name}'");
        
        // Load level
        // Set Next Level in LevelManager
        LevelManager.Instance.SetCurrentLevel(levelData);

        // Load Level Scene
        UnityEngine.SceneManagement.SceneManager.LoadScene("Level");
        Debug.Log($"LevelAutoTester: Loaded scene for level '{levelData.name}'");

        await Task.Delay(10000); // Wait for the scene to load

        // Reveal everything
        Channel channel = (Channel)AssetDatabase.LoadAssetAtPath("Assets/ScriptableObjects/Channels/Levels/OnFinishLevel.asset", typeof(Channel));
        channel.Raise();
        Debug.Log($"LevelAutoTester: Raised OnFinishLevel channel for level '{levelData.name}'");

        await Task.Delay(10000); // Wait for the level to reveal

        // Take a screenshot
        // Digx7.Editor.ScreenCaptureUtility.TakeScreenShot();

        string directoryPath = $"Assets/Editor/TestLevelScreenShots/{startTime}";
        if (!Directory.Exists(directoryPath))
        {
            Directory.CreateDirectory(directoryPath);
        }

        string fileName = $"{directoryPath}/{levelData.name}.png";
        ScreenCapture.CaptureScreenshot(fileName);

        Debug.Log($"LevelAutoTester: Took screenshot for level '{levelData.name}'");

        await Task.Delay(10000); // Wait for the screenshot to be taken

        return true; // Placeholder for actual validation result
    }

    private async Task<bool> ValidateLevelDataWithLinkingAsync()
    {
        // Load Level Scene
        UnityEngine.SceneManagement.SceneManager.LoadScene("Level");

        await Task.Delay(5000); // Wait for the scene to load
        LevelData levelData = LevelManager.Instance.CurrentLevel;
        Debug.Log($"LevelAutoTester: Loaded scene for level '{levelData.name}'");

        // Reveal everything
        Channel onFinishedLevelChannel = (Channel)AssetDatabase.LoadAssetAtPath("Assets/ScriptableObjects/Channels/Levels/OnFinishLevel.asset", typeof(Channel));
        onFinishedLevelChannel.Raise();

        await Task.Delay(5000); // Wait for the level to reveal

        // Take a screenshot
        // Digx7.Editor.ScreenCaptureUtility.TakeScreenShot();

        string directoryPath = $"Assets/Editor/TestLevelScreenShots/{startTime}";
        if (!Directory.Exists(directoryPath))
        {
            Directory.CreateDirectory(directoryPath);
        }

        string fileName = $"{directoryPath}/{levelData.name}.png";
        ScreenCapture.CaptureScreenshot(fileName);

        await Task.Delay(5000); // Wait for the screenshot

        // Reveal everything
        Channel requestMoveToNextLevel = (Channel)AssetDatabase.LoadAssetAtPath("Assets/ScriptableObjects/Channels/Levels/RequestMoveToNextLevel.asset", typeof(Channel));
        requestMoveToNextLevel.Raise();

        await Task.Delay(500);

        return true; // Placeholder for actual validation result
    }
}