using UnityEngine;
using UnityEngine.Events;

public class LevelRuntimeData : MonoBehaviour 
{
    public Vector2IntChannel TryDigChannel;
    
    public LevelData levelDataSO;

    [SerializeField]
    public Digx7.Grids.Grid modifiedGrid;

    public DigDataEvent OnDig;

    private void Awake() 
    {
        modifiedGrid = new Digx7.Grids.Grid(levelDataSO.grid.x_Length, levelDataSO.grid.y_Length);
    }

    private void OnEnable() {
        TryDigChannel.channelEvent.AddListener(DigInSpace);
    }

    private void OnDisable() {
        TryDigChannel.channelEvent.RemoveListener(DigInSpace);
    }

    public void DigInSpace(Vector2Int digCoordinates)
    {
        DigData digData = new DigData();
        digData.coordinate = digCoordinates;

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
    public string spaceID;
    public string itemID;
    public string itemSubID;
}