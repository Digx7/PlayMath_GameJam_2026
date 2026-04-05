using UnityEngine;
using UnityEditor;
using TMPro;
using System;
using System.IO;
using System.Collections.Generic;

public class LevelSelectionUIWidget : UIMenu
{
    [SerializeField] UIWidgetData mainMenuWidgetData;
    
    [Header("OutGoing Channels")]
    [SerializeField] UIWidgetDataChannel requestLoadUIWidgetChannel;
    [SerializeField] UIWidgetDataChannel requestUnLoadUIWidgetChannel;

    [Header("References")]
    public GameObject levelButtonPrefab;
    public Transform levelButtonHolder;
    public List<LevelData> levels_LevelData;
    public List<string> levels_string;

    #if UNITY_EDITOR

    public List<LevelData> testLevelDatas;
    public string testLevelDataPath;

    #endif

    public override void Setup(UIWidgetData newUIWidgetData)
    {
        base.Setup(newUIWidgetData);
        SetupLevels();
    }

    public override void Teardown()
    {
        base.Teardown();
    }

    public void OnClickBack()
    {
        requestLoadUIWidgetChannel.Raise(mainMenuWidgetData);
        requestUnLoadUIWidgetChannel.Raise(ownUIWidgetData);
    }

    public void SetupLevels()
    {
        // for (int i = 0; i < levels_LevelData.Count; i++)
        // {
        //     GameObject obj = Instantiate(levelButtonPrefab, levelButtonHolder);
        //     LevelUIButtonHelper levelUIButtonHelper = obj.GetComponent<LevelUIButtonHelper>();
        //     levelUIButtonHelper.Setup(levels_LevelData[i]);
        // }

        LoadLevelButtons(levels_LevelData);

        #if UNITY_EDITOR

        testLevelDatas = new List<LevelData>();

        string[] files = Directory.GetFiles(testLevelDataPath, "*.asset", SearchOption.TopDirectoryOnly);

        foreach (var file in files)
        {
            LevelData testLevel = (LevelData)AssetDatabase.LoadAssetAtPath(file, typeof(LevelData));
            
            if(testLevel != null)
            {
                testLevelDatas.Add(testLevel);
            }
        }

        LoadLevelButtons(testLevelDatas);

        #endif

        for (int i = 0; i < levels_string.Count; i++)
        {
            GameObject obj = Instantiate(levelButtonPrefab, levelButtonHolder);
            LevelUIButtonHelper levelUIButtonHelper = obj.GetComponent<LevelUIButtonHelper>();
            levelUIButtonHelper.Setup(levels_string[i]);
        }
    }

    public void LoadLevelButtons(List<LevelData> levels)
    {
        for (int i = 0; i < levels.Count; i++)
        {
            GameObject obj = Instantiate(levelButtonPrefab, levelButtonHolder);
            LevelUIButtonHelper levelUIButtonHelper = obj.GetComponent<LevelUIButtonHelper>();
            levelUIButtonHelper.Setup(levels[i]);
        }
    }
}
