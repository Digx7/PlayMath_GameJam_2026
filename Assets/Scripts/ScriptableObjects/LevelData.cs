using UnityEngine;
using Digx7.Grids;
using System;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "NewLevelData", menuName = "ScriptableObjects/LevelData", order = 0)]
public class LevelData : ScriptableObject 
{
    [SerializeField]
    public Digx7.Grids.Grid grid;

    public List<TreasurePiece> treasureToFind;

    public void SetGrid(List<CoordinateFlagPair> newData, int newX_Length, int newY_Length)
    {
        Debug.Log($"LevelData {name} had a new grid set");
        grid = new Digx7.Grids.Grid(newData, newX_Length, newY_Length);
    }
    public void SetTreasureToFind(List<TreasurePiece> newTreasureToFind){treasureToFind = newTreasureToFind;}

    public bool DoesSpaceContainTreasure(Vector2Int coordinates)
    {
        if(!grid.IsCoordinateInGrid(coordinates)) return false;

        if(grid.GetFlagofGridSpace(coordinates) > 10) return true;
        else return false;
    }

    public bool DoesSpaceContainTreasure(int x, int y)
    {
        Vector2Int coordinates = new Vector2Int(x,y);
        return DoesSpaceContainTreasure(coordinates);
    }

    [ContextMenu("Print Grid")]
    public void PrintGrid()
    {
        Debug.Log($"Printing Grid for {name}");
        grid.PrintGrid();
    }
}