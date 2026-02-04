using UnityEngine;
using UnityEngine.Events;
using System;
using System.Collections.Generic;


public class LevelRuntimeData : MonoBehaviour 
{
    [Header("InComing Channels")]
    public Vector2IntChannel TryDigChannel;
    public ToolChannel TryChangeTool;
    
    // Level Data
    public LevelData levelDataSO;


    // Runtime Data
    [SerializeField] public Digx7.Grids.Grid modifiedGrid;
    [SerializeField] public List<TreasureRuntimeData> treasureRuntimeDatas;
    [SerializeField] public List<CountToolPair> tools;
    private int selectedToolIndex = 0;
    public bool levelFinished;

    [Header("Events")]
    public LevelDataEvent OnSetup;
    public DigDataEvent OnDig;
    public StringEvent OnFullyDigUpPiece;
    public ToolEvent OnTryUseEmptyTool;
    public UnityEvent OnFinishLevel;
    public UnityEvent OnWinLevel;
    public UnityEvent OnLoseLevel;

    private void Awake() 
    {
        modifiedGrid = new Digx7.Grids.Grid(levelDataSO.grid.x_Length, levelDataSO.grid.y_Length);

        treasureRuntimeDatas = new List<TreasureRuntimeData>();
        for (int i = 0; i < levelDataSO.treasureToFind.Count; i++)
        {
            TreasureRuntimeData tRunTime = new TreasureRuntimeData(levelDataSO.treasureToFind[i]);
            tRunTime.OnFullyDigUp.AddListener((string output) => OnFullyDigUpPiece.Invoke(output));
            treasureRuntimeDatas.Add(tRunTime);
        }
        tools = new List<CountToolPair>();
        for (int i = 0; i < levelDataSO.tools.Count; i++)
        {
            tools.Add(levelDataSO.tools[i]);
        }

        OnSetup.Invoke(levelDataSO);
    }

    private void OnEnable() {
        TryDigChannel.channelEvent.AddListener(TryDigInSpace);
        TryChangeTool.channelEvent.AddListener(ChangeTool);
    }

    private void OnDisable() {
        TryDigChannel.channelEvent.RemoveListener(TryDigInSpace);
        TryChangeTool.channelEvent.AddListener(ChangeTool);
    }

    public void ChangeTool(Tool newTool)
    {
        for (int i = 0; i < tools.Count; i++)
        {
            if(tools[i].tool == newTool) 
            {
                selectedToolIndex = i;
            }
        }
    }

    public void TryDigInSpace(Vector2Int digCoordinates)
    {
        if(modifiedGrid.GetFlagofGridSpace(digCoordinates) != "0") return;
        
        if(tools[selectedToolIndex].count > 0)
        {
            if(modifiedGrid.GetFlagofGridSpace(digCoordinates) == "0")
            {
                CountToolPair countToolPair = tools[selectedToolIndex];
                countToolPair.count--;
                tools[selectedToolIndex] = countToolPair;
            }

            DigInSpace(digCoordinates);
            CheckIfLevelIsFinished();
        }
        else
        {
            OnTryUseEmptyTool.Invoke(tools[selectedToolIndex].tool);
        }
    }

    private void DigInSpace(Vector2Int digCoordinates)
    {
        DigData digData = new DigData();
        digData.coordinate = digCoordinates;
        digData.toolUsed = tools[selectedToolIndex].tool;

        string spaceFlag = levelDataSO.grid.GetFlagofGridSpace(digCoordinates);
        
        if(levelDataSO.DoesSpaceContainTreasure(digCoordinates))
        {
            if(modifiedGrid.GetFlagofGridSpace(digCoordinates) == "0")
            {
                modifiedGrid.UpdateCoordinateFlag(digCoordinates, "T");
                digData.result = DigResult.FOUND_NEW_TREASURE;
            }
            else
            {
                digData.result = DigResult.FOUND_OLD_TREASURE;
            }

            
        }
        else
        {
            if(modifiedGrid.GetFlagofGridSpace(digCoordinates) == "0")
            {
                modifiedGrid.UpdateCoordinateFlag(digCoordinates, "E");
                digData.result = DigResult.FOUND_NEW_EMPTY;
            }
            else
            {
                digData.result = DigResult.FOUND_OLD_EMPTY;
            }

            
        }

        if(spaceFlag != "0")
        {
            string[] spaceFlagStrings = spaceFlag.Split('_');
            digData.spaceID = spaceFlagStrings[0];
            digData.itemID = spaceFlagStrings[1];
            digData.itemSubID = spaceFlagStrings[2];
        }
        else
        {
            digData.spaceID = "Null";
            digData.itemID = "Null";
            digData.itemSubID = "Null";
        }

        OnDig.Invoke(digData);

        AddFoundTreasureToFoundList(digData);
    }

    private void AddFoundTreasureToFoundList(DigData digData)
    {
        // If found treasure add its piece to the list of pieces found
        if(digData.spaceID == "T")
        {
            for (int i = 0; i < treasureRuntimeDatas.Count; i++)
            {
                if(treasureRuntimeDatas[i].treasurePieceSO.ID == digData.itemID)
                {
                    treasureRuntimeDatas[i].FindPiece(digData.itemSubID);
                }
            }
        }
    }

    private void CheckIfLevelIsFinished()
    {
        // Check if we have found everything
        bool hasFoundAllTreasure = true;
        for (int i = 0; i < treasureRuntimeDatas.Count; i++)
        {
            if(treasureRuntimeDatas[i].fullyDugUp == false) hasFoundAllTreasure = false;
        }

        bool ranOutOFTools = true;
        for (int i = 0; i < tools.Count; i++)
        {
            if(tools[i].count > 0) ranOutOFTools = false;
        }

        if(hasFoundAllTreasure) 
        {
            levelFinished = true;
            OnWinLevel.Invoke();
            OnFinishLevel.Invoke();
        }
        else if(ranOutOFTools)
        {
            levelFinished = true;
            OnLoseLevel.Invoke();
            OnFinishLevel.Invoke();
        }
    }
}

[System.Serializable]
public enum DigResult
{
    FOUND_NEW_TREASURE,
    FOUND_OLD_TREASURE,
    FOUND_NEW_EMPTY,
    FOUND_OLD_EMPTY,
    FOUND_ROCK
}

[System.Serializable]
public struct DigData
{
    public DigResult result;
    public Vector2Int coordinate;
    public Tool toolUsed;
    public string spaceID;
    public string itemID;
    public string itemSubID;
    public Sprite subSprite;
}

[System.Serializable]
public class TreasureRuntimeData
{
    public TreasurePiece treasurePieceSO;
    public bool fullyDugUp;

    public List<FoundFlagPair> foundPieces;

    public StringEvent OnFullyDigUp;

    public TreasureRuntimeData(TreasurePiece newTreasurePieceSO)
    {
        treasurePieceSO = newTreasurePieceSO;
        fullyDugUp = false;

        foundPieces = new List<FoundFlagPair>();
        for (int i = 0; i < treasurePieceSO.subSprites.Count; i++)
        {
            FoundFlagPair foundFlagPair = new FoundFlagPair();
            foundFlagPair.found = false;
            foundFlagPair.subFlag = treasurePieceSO.subSprites[i].SubID;

            foundPieces.Add(foundFlagPair);
        }

        OnFullyDigUp = new StringEvent();
    }

    public void FindPiece(string subFlag)
    {
        for (int i = 0; i < foundPieces.Count; i++)
        {
            if(foundPieces[i].subFlag == subFlag)
            {
                FoundFlagPair foundFlagPair = foundPieces[i];
                foundFlagPair.found = true;
                foundPieces[i] = foundFlagPair;
            }
        }

        if(CheckIfIsFullyDugUp()) OnFullyDigUp.Invoke(treasurePieceSO.ID);
    }

    public bool CheckIfIsFullyDugUp()
    {
        for (int i = 0; i < foundPieces.Count; i++)
        {
            if(foundPieces[i].found == false) 
            {
                fullyDugUp = false;
                return false;
            }
        }
        fullyDugUp = true;
        return true;
    }
}

[System.Serializable]
public struct FoundFlagPair
{
    public bool found;
    public string subFlag;
}