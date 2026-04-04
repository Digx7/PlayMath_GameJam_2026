using UnityEngine;
using UnityEngine.Events;
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
    
    [Header("Incoming Channels")]
    [SerializeField] private LevelDataChannel RequestSetNextLevel;

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
        RequestSetNextLevel.channelEvent.AddListener(OnRecieve_RequestSetNextLevel);
    }
    
    private void TearDownChannels()
    {
        RequestSetNextLevel.channelEvent.RemoveListener(OnRecieve_RequestSetNextLevel);
    }
    
    #endregion
    #region ChannelRespones
    
    public void OnRecieve_RequestSetNextLevel(LevelData data)
    {
        SetNextLevel(data);
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