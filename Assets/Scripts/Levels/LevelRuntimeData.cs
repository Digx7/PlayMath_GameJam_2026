using UnityEngine;
using UnityEngine.Events;

public class LevelRuntimeData : MonoBehaviour 
{
    public LevelData levelDataSO;

    [SerializeField]
    public Digx7.Grids.Grid modifiedGrid;

    public BooleanEvent OnDig;

    private void Awake() 
    {
        modifiedGrid = new Digx7.Grids.Grid(levelDataSO.grid.x_Length, levelDataSO.grid.y_Length);
    }

    public bool DigInSpace(Vector2Int digCoordinates)
    {
        DigData digData = new DigData();
        digData.coordinate = digCoordinates;

        string spaceFlag = levelDataSO.grid.GetFlagofGridSpace(digCoordinates);
        
        if(levelDataSO.DoesSpaceContainTreasure(digCoordinates))
        {
            if(modifiedGrid.GetFlagofGridSpace(digCoordinates) == "0")
            {
                modifiedGrid.UpdateCoordinateFlag(digCoordinates, "2");
                digData.result = DigResult.FOUND_NEW_TREASURE;
            }
            else
            {
                digData.result = DigResult.FOUND_OLD_TREASURE;
            }

            string spaceFlagString = spaceFlag;
        }
        else
        {
            if(modifiedGrid.GetFlagofGridSpace(digCoordinates) == "0")
            {
                modifiedGrid.UpdateCoordinateFlag(digCoordinates, "1");
                digData.result = DigResult.FOUND_NEW_EMPTY;
            }
            else
            {
                digData.result = DigResult.FOUND_OLD_EMPTY;
            }

            digData.treasureID = "-1";
            digData.treasureSubID = "-1";
        }

        return true;
    }
}

[System.Serializable]
public enum DigResult
{
    FOUND_NEW_TREASURE,
    FOUND_OLD_TREASURE,
    FOUND_NEW_EMPTY,
    FOUND_OLD_EMPTY
}

[System.Serializable]
public struct DigData
{
    public DigResult result;
    public Vector2Int coordinate;
    public string treasureID;
    public string treasureSubID;
    public Sprite treasureSprite;
    public Sprite treasureSubSprite;
}