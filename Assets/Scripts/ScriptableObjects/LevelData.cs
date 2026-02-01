using UnityEngine;
using System;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "NewLevelData", menuName = "ScriptableObjects/LevelData", order = 0)]
public class LevelData : ScriptableObject 
{
    [SerializeField]
    public Grid grid;

    public List<TreasurePiece> treasureToFind;

    public void SetGrid(List<CoordinateFlagPair> newData, int newX_Length, int newY_Length)
    {
        Debug.Log($"LevelData {name} had a new grid set");
        grid = new Grid(newData, newX_Length, newY_Length);
    }
    public void SetTreasureToFind(List<TreasurePiece> newTreasureToFind){treasureToFind = newTreasureToFind;}

    public bool DoesSpaceContainTreasure(int x, int y)
    {
        if(!grid.IsCoordinateInGrid(x,y)) return false;

        if(grid.GetIDofGridSpace(x,y) > 10) return true;
        else return false;
    }

    [ContextMenu("Print Grid")]
    public void PrintGrid()
    {
        Debug.Log($"Printing Grid for {name}");
        grid.PrintGrid();
    }
}

[System.Serializable]
public class Grid
{
    public List<CoordinateFlagPair> data;
    public int x_Length = 0;
    public int y_Length = 0;

    public Grid(List<CoordinateFlagPair> newData, int newX_Length, int newY_Length)
    {
        SetGrid(newData, newX_Length, newY_Length);
    }

    public void SetGrid(List<CoordinateFlagPair> newData, int newX_Length, int newY_Length)
    {
        data = newData;

        x_Length = newX_Length;
        y_Length = newY_Length;

        PrintGrid();
    }

    public int GetIDofGridSpace(Vector2Int coordinate)
    {
        return GetIDofGridSpace(coordinate.x, coordinate.y);
    }

    public int GetIDofGridSpace(int x, int y)
    {
        // TODO
        return 0;
    }

    public bool IsCoordinateInGrid(Vector2Int coordinate)
    {
        return IsCoordinateInGrid(coordinate.x, coordinate.y);
    }

    public bool IsCoordinateInGrid(int x, int y)
    {
        // TODO

        return true;
    }

    public void PrintGrid()
    {
        Debug.Log($"Printing Grid");
        if(data == null)
        {
            Debug.LogWarning($"Grid is UNDEFINED");
        }
        Debug.Log($"Size {x_Length},{y_Length}");
        
        for (int i = 0; i < data.Count; i++)
        {
            Debug.Log($"{data[i].flag}");
        }
    }
}

[System.Serializable]
public struct CoordinateFlagPair
{
    public Vector2Int coordinate;
    public int flag;
}