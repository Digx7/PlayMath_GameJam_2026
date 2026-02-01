using UnityEngine;
using System;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "NewLevelData", menuName = "ScriptableObjects/LevelData", order = 0)]
public class LevelData : ScriptableObject 
{
    [SerializeField]
    public Grid grid;

    public List<TreasurePiece> treasureToFind;

    public void SetGrid(int[][] newGrid)
    {
        Debug.Log($"LevelData {name} had a new grid set");
        grid = new Grid(newGrid);
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
    // public int[][] data;

    // [SerializeField]
    // public Dictionary<Vector2Int, int> data;
    public List<CoordinateFlagPair> data;
    public int x_Length = 0;
    public int y_Length = 0;

    public Grid(int[][] data)
    {
        SetGrid(data);
    }

    public void SetGrid(int[][] newGrid)
    {
        data = new List<CoordinateFlagPair>();

        x_Length = newGrid.Length;
        y_Length = newGrid[0].Length;

        for (int x = 0; x < newGrid.Length; x++)
        {
            for (int y = 0; y < newGrid[x].Length; y++)
            {
                Vector2Int coords = new Vector2Int(x,y);
                CoordinateFlagPair coordinateFlagPair = new CoordinateFlagPair();

                coordinateFlagPair.coordinate = coords;
                coordinateFlagPair.flag = newGrid[x][y];

                data.Add(coordinateFlagPair);
            }
        }   

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