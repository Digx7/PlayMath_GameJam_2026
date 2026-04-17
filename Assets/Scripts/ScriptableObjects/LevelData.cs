using UnityEngine;
using Digx7.Grids;
using Digx7.Levels;
using System;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "NewLevelData", menuName = "ScriptableObjects/LevelData", order = 0)]
public class LevelData : ScriptableObject 
{
    [SerializeField]
    public Digx7.Grids.Grid grid;

    public List<TreasurePiece> treasureToFind;
    public List<TreasurePieceRotation> treasureRotations;
    public List<String> hints;
    public List<CountToolPair> tools;
    public LevelData nextLevel;
    public LevelData GetNextLevel(LevelGeneratorTemplateData templateData = null)
    {
        if(isRandomLevel)
        {
            nextLevel = Digx7.Levels.LevelGenerator.TryGenerateRandomLevelFromTemplate(templateData);
        }

        return nextLevel;
    }
    public bool isLastLevel = false;
    public bool isRandomLevel = false;

    public void SetGrid(List<CoordinateFlagPair> newData, int newX_Length, int newY_Length, GridTypes gridType = GridTypes.Coordinate, Vector2Int newOrigin = default(Vector2Int))
    {
        Debug.Log($"LevelData {name} had a new grid set");
        grid = new Digx7.Grids.Grid(newData, newX_Length, newY_Length, gridType);
        grid.origin = newOrigin;
    }
    public void SetTreasureToFind(List<TreasurePiece> newTreasureToFind)
    {
        treasureToFind = newTreasureToFind;
        treasureRotations = new List<TreasurePieceRotation>();
        for (int i = 0; i < newTreasureToFind.Count; i++)
        {
            treasureRotations.Add(TreasurePieceRotation.None);
        }
    }

    public bool DoesSpaceContainTreasure(Vector2Int coordinates)
    {
        if(!grid.IsCoordinateInGrid(coordinates)) return false;

        if(grid.GetFlagofGridSpace(coordinates).Contains("T")) return true;
        else return false;
    }

    public bool DoesSpaceContainTreasure(int x, int y)
    {
        Vector2Int coordinates = new Vector2Int(x,y);
        return DoesSpaceContainTreasure(coordinates);
    }

    public bool TryGetSpaceSubSprite(out SubSprite subSprite, Vector2Int coordinate)
    {
        Debug.Log($"LevelData TryGetSpaceSubSprite {coordinate}");
        
        subSprite = new SubSprite();

        if(!grid.IsCoordinateInGrid(coordinate))
        {
            Debug.Log($"LevelData TryGetSpaceSubSprite exited early because {coordinate} was outside the grid");
            return false;
        }

        string[] spaceFlags = grid.GetFlagsofGridSpace(coordinate);
        if(spaceFlags.Length == 3)
        {
            string itemID = spaceFlags[1];
            string subID = spaceFlags[2];

            TreasurePiece treasure = treasureToFind.Find(i => i.ID == itemID);

            if (treasure != null)
            {
                subSprite = treasure.subSprites.Find(j => j.SubID == subID);

                Debug.Log($"LevelData TryGetSpaceSubSprite returned subSprite {subSprite.SubID}");
                return true;
            }
            else
            {
                Debug.Log($"LevelData TryGetSpaceSubSprite exited early because no treasure was found at {coordinate}\nInstead we found the flag {grid.GetFlagofGridSpace(coordinate)}");
                return false;
            }
        }
        else
        {
            Debug.Log($"LevelData TryGetSpaceSubSprite exited early because spaceFlags.Length != 3 but instead equals {spaceFlags.Length}");
            return false;
        }
    }

    public bool TryGetSpaceRotation(out TreasurePieceRotation treasurePieceRotation, Vector2Int coordinate)
    {
        
        treasurePieceRotation = TreasurePieceRotation.None;
        
        if(!grid.IsCoordinateInGrid(coordinate))
        {
            return false;
        }

        string[] spaceFlags = grid.GetFlagsofGridSpace(coordinate);
        if (spaceFlags.Length == 3)
        {
            string itemID = spaceFlags[1];
            string subID = spaceFlags[2];

            for (int i = 0; i < treasureToFind.Count; i++)
            {
                if (treasureToFind[i].ID == itemID)
                {
                    treasurePieceRotation = treasureRotations[i];
                    return true;
                }
            }
        }

        return false;
    }

    [ContextMenu("Print Grid")]
    public void PrintGrid()
    {
        Debug.Log($"Printing Grid for {name}");
        grid.PrintGrid();
    }
}