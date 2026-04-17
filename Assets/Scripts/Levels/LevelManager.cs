using UnityEngine;
using UnityEngine.Events;
using Digx7.Levels;
using System;


public class LevelManager : Singleton<LevelManager>
{
    
    #region Variables
    [Header("References")]
    [SerializeField] private LevelData _currentLevel;
    public LevelData CurrentLevel
    {
        get
        {
            return _currentLevel;
        }
        private set
        {
            if(value is LevelData)
            {
                _currentLevel = value;
            }
        }
    }
    [SerializeField] private LevelData _nextLevel;
    [SerializeField] LevelGeneratorTemplateData _activeLevelTemplateData;
    public LevelGeneratorTemplateData ActiveLevelTemplate
    {
        get
        {
            return _activeLevelTemplateData;
        }
        private set
        {
            if(value is LevelGeneratorTemplateData)
            {
                _activeLevelTemplateData = value;
            }
        }
    }
    
    [Header("Incoming Channels")]
    [SerializeField] private LevelDataChannel RequestSetCurrentLevel;
    [SerializeField] private LevelDataChannel RequestSetNextLevel;
    [SerializeField] private Channel RequestMoveToNextLevel;
    [SerializeField] private LevelGeneratorTemplateDataChannel RequestSetActiveLevelTemplateData;

    // [Header("Outgoing Events")]
    #endregion
    #region Setup
    private void Start()
    {
        
    }
    
    private void OnEnable()
    {
        SetupChannels();
    }
    
    private void OnDisable()
    {
        TearDownChannels();
    }
    
    private void SetupChannels()
    {
        RequestSetCurrentLevel.channelEvent.AddListener(OnRecieve_RequestSetCurrentLevel);
        RequestSetNextLevel.channelEvent.AddListener(OnRecieve_RequestSetNextLevel);
        RequestMoveToNextLevel.channelEvent.AddListener(OnRecieve_RequestMoveToNextLevel);
        RequestSetActiveLevelTemplateData.channelEvent.AddListener(OnRecieve_RequestSetActiveLevelGeneratorTemplateData);
    }
    
    private void TearDownChannels()
    {
        RequestSetCurrentLevel.channelEvent.RemoveListener(OnRecieve_RequestSetCurrentLevel);
        RequestSetNextLevel.channelEvent.RemoveListener(OnRecieve_RequestSetNextLevel);
        RequestMoveToNextLevel.channelEvent.RemoveListener(OnRecieve_RequestMoveToNextLevel);
        RequestSetActiveLevelTemplateData.channelEvent.RemoveListener(OnRecieve_RequestSetActiveLevelGeneratorTemplateData);
    }
    
    #endregion
    #region ChannelRespones
    
    public void OnRecieve_RequestSetCurrentLevel(LevelData data)
    {
        CurrentLevel = data;
    }

    public void OnRecieve_RequestSetNextLevel(LevelData data)
    {
        SetNextLevel(data);
    }

    public void OnRecieve_RequestMoveToNextLevel()
    {
        MoveToNextLevel();
    }

    public void OnRecieve_RequestSetActiveLevelGeneratorTemplateData(LevelGeneratorTemplateData levelGeneratorTemplateData)
    {
        _activeLevelTemplateData = levelGeneratorTemplateData;
    }

    #endregion
    #region MainFunctions

    public void SetNextLevel(LevelData newNextLevel)
    {
        _nextLevel = newNextLevel;
        Debug.Log($"LevelManager: SetNextLevel({_nextLevel})");
    }

    public LevelData MoveToNextLevel()
    {
        Debug.Log($"LevelManager: MoveToNextLevel() -> {CurrentLevel}");
        
        CurrentLevel = _nextLevel;
        return CurrentLevel;
    }

    public LevelData PeekAtNextLevel()
    {
        return _nextLevel;
    }

    #endregion
}