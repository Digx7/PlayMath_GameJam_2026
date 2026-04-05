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
    public bool isOnLastLevel = false;
    public bool generateRandomLevelDataOnAwake = false;


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
    public LevelDataEvent OnSetNextLevel;
    public UnityEvent OnFinishLevel;
    public UnityEvent OnWinLevel;
    public UnityEvent OnLoseLevel;

    private void Awake() 
    {
        Debug.Log("Level Runtime Data Awake");
        
        if(generateRandomLevelDataOnAwake) 
        {
            levelDataSO = Digx7.Levels.LevelGenerator.GenerateRandomLevelData();
        }
        else
        {
            levelDataSO = LevelManager.Instance.MoveToNextLevel();
            if(levelDataSO == null)
            {
                Debug.LogError("NO valid Level data was found");
            }
            else
            {
                if(!levelDataSO.isLastLevel)
                {
                    OnSetNextLevel.Invoke(levelDataSO.nextLevel);
                }
            }
        }
        
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
        digData.result = DigResult.FOUND_OLD_EMPTY;
        digData.tileData = new Dictionary<Vector2Int, DigTileData>();
        digData.toolUsed = tools[selectedToolIndex].tool;

        for (int i = 0; i < digData.toolUsed.relativeSpacesToDig.Count; i++)
        {
            Vector2Int _digCoordinates = digCoordinates + digData.toolUsed.relativeSpacesToDig[i];
            if(levelDataSO.grid.IsCoordinateInGrid(_digCoordinates))
            {
                DigTileData digTileData = DigInTile(_digCoordinates);

                // Because Enums have an underlying int type we can compare them
                // I want DigResults with a lower int type to override higher int types for the main result
                if (digData.result > digTileData.result)digData.result = digTileData.result;

                digData.tileData[_digCoordinates] = digTileData;
            }
        }

        OnDig.Invoke(digData);

        AddFoundTreasureToFoundList(digData);
    }

    private DigTileData DigInTile(Vector2Int digCoordinates)
    {
        DigTileData digTileData = new DigTileData();
        digTileData.coordinate = digCoordinates;

        string spaceFlag = levelDataSO.grid.GetFlagofGridSpace(digCoordinates);

        if(levelDataSO.DoesSpaceContainTreasure(digCoordinates))
        {
            if(modifiedGrid.GetFlagofGridSpace(digCoordinates) == "0")
            {
                modifiedGrid.UpdateCoordinateFlag(digCoordinates, "T");
                digTileData.result = DigResult.FOUND_NEW_TREASURE;
            }
            else
            {
                digTileData.result = DigResult.FOUND_OLD_TREASURE;
            }

            
        }
        else
        {
            if(modifiedGrid.GetFlagofGridSpace(digCoordinates) == "0")
            {
                modifiedGrid.UpdateCoordinateFlag(digCoordinates, "E");
                digTileData.result = DigResult.FOUND_NEW_EMPTY;
            }
            else
            {
                digTileData.result = DigResult.FOUND_OLD_EMPTY;
            }

            
        }

        digTileData.tileFlag = new TileFlag();

        if(spaceFlag != "0")
        {
            string[] spaceFlagStrings = spaceFlag.Split('_');
            digTileData.tileFlag.spaceID = spaceFlagStrings[0];
            digTileData.tileFlag.itemID = spaceFlagStrings[1];
            digTileData.tileFlag.itemSubID = spaceFlagStrings[2];
        }
        else
        {
            digTileData.tileFlag.spaceID = "Null";
            digTileData.tileFlag.itemID = "Null";
            digTileData.tileFlag.itemSubID = "Null";
        }

        return digTileData;
    }

    private void AddFoundTreasureToFoundList(DigData digData)
    {
        // If found treasure add its piece to the list of pieces found
        // if(digData.spaceID == "T")
        // {
        //     for (int i = 0; i < treasureRuntimeDatas.Count; i++)
        //     {
        //         if(treasureRuntimeDatas[i].treasurePieceSO.ID == digData.itemID)
        //         {
        //             treasureRuntimeDatas[i].FindPiece(digData.itemSubID);
        //         }
        //     }
        // }
        foreach (KeyValuePair<Vector2Int, DigTileData> keyValuePair in digData.tileData)
        {
            if(keyValuePair.Value.tileFlag.spaceID == "T")
            {
                for (int i = 0; i < treasureRuntimeDatas.Count; i++)
                {
                    if(treasureRuntimeDatas[i].treasurePieceSO.ID == keyValuePair.Value.tileFlag.itemID)
                    {
                        treasureRuntimeDatas[i].FindPiece(keyValuePair.Value.tileFlag.itemSubID);
                    }
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
    public Dictionary<Vector2Int, DigTileData> tileData;
    public Tool toolUsed;
    // public Vector2Int coordinate;
    // public string spaceID;
    // public string itemID;
    // public string itemSubID;
    // public Sprite subSprite;
}

[System.Serializable]
public struct DigTileData
{
    public DigResult result;
    public Vector2Int coordinate;
    public TileFlag tileFlag;
    public Sprite subSprite;
}

[System.Serializable]
public struct TileFlag
{
    public string spaceID;
    public string itemID;
    public string itemSubID;
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