using UnityEngine;
using UnityEditor;
using Digx7.Grids;
using System;
using System.IO;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Digx7.Levels
{
    public static class LevelGenerator 
    {
        public static LevelData GenerateRandomLevelData()
        {
            // Grid MetaData ======================================
            int xLength = UnityEngine.Random.Range(3, 8);
            int yLength = xLength; 
            GridTypes gridType = UnityEngine.Random.value < 0.5f ? GridTypes.Coordinate : GridTypes.A4;
            Vector2Int origin = (gridType == GridTypes.Coordinate) ? new Vector2Int(UnityEngine.Random.Range(0, xLength), UnityEngine.Random.Range(0, yLength)) : Vector2Int.zero;

            List<CoordinateFlagPair> gridData = new List<CoordinateFlagPair>();
            List<TreasurePiece> treasureToFind = new List<TreasurePiece>();
            List<string> hints = new List<string>();
            List<CountToolPair> tools = new List<CountToolPair>();


            // Empty Grid ======================================
            for (int x = 0; x < xLength; x++)
            {
                for (int y = 0; y < yLength; y++)
                {
                    string flag = "0"; // Default flag for empty space
                    gridData.Add(new CoordinateFlagPair(new Vector2Int(x, y), flag));
                }
            }

            
            // Treasure MetaData ======================================
            
            int numberOfTreasures = UnityEngine.Random.Range(1, 4);
            TreasurePiece[] treasurePrefabs = Resources.LoadAll<TreasurePiece>("ScriptableObjects/TreasurePieces");
            Debug.Log($"Found {treasurePrefabs.Length} treasure prefabs in Resources/ScriptableObjects/TreasurePieces");
            int randomTreasureIndex = 0;
            
            for (int i = 0; i < numberOfTreasures; i++)
            {
                int treasureAttempts = 0;
                int maxTreasureAttempts = 100;
                bool validTreasureFound = false;

                while (!validTreasureFound && treasureAttempts < maxTreasureAttempts)
                {
                    randomTreasureIndex = UnityEngine.Random.Range(0, treasurePrefabs.Length);
                    TreasurePiece treasure = Resources.Load<TreasurePiece>($"ScriptableObjects/TreasurePieces/{treasurePrefabs[randomTreasureIndex].name}");

                    if (!treasureToFind.Contains(treasure))
                    {
                        treasureToFind.Add(treasure);
                        validTreasureFound = true;
                    }
                    else
                    {
                        treasureAttempts++;
                        Debug.LogWarning($"Treasure {treasure.name} already selected. Attempting to select a different treasure. Attempt {treasureAttempts}/{maxTreasureAttempts}");
                    }
                }
            }

            // Place Treasures in Grid ======================================
            int treasureIndex = 0;
            int placedTreasures = 0;
            List<int> treasureIndicesToRemove = new List<int>();
            int placedSubSprites = 0;
            int attempts = 0;
            int maxAttempts = 100; // To prevent infinite loops in case of very small grids

            while (treasureIndex < treasureToFind.Count)
            {
                
                
                // Get random origin coordinate for the treasure piece
                int randomX = UnityEngine.Random.Range(0, xLength);
                int randomY = UnityEngine.Random.Range(0, yLength);
                Vector2Int randomPieceOrigin = new Vector2Int(randomX, randomY);

                // Check if location is valid for placing the treasure piece (check bounds and if it overlaps with existing treasures)
                bool pieceIsValid = true;

                List<Vector2Int> coordinatesToCheck = new List<Vector2Int>();
                coordinatesToCheck.Add(randomPieceOrigin);
                coordinatesToCheck.AddRange(treasureToFind[treasureIndex].subSprites.ConvertAll(sub => randomPieceOrigin + sub.relativePosition));

                foreach (Vector2Int coordinate in coordinatesToCheck)
                {
                    if (!GridUtils.IsCoordinateInGrid(coordinate, xLength, yLength))
                    {
                        Debug.LogWarning($"Coordinate {coordinate} is out of grid bounds. Piece placement is invalid.");
                        pieceIsValid = false;
                        break;
                    }
                    else if (gridData[GridUtils.CoordinateToIndex(coordinate, xLength, yLength)].flag.StartsWith("T_"))
                    {
                        Debug.LogWarning($"Coordinate {coordinate} is already occupied by a treasure. Piece placement is invalid.");
                        pieceIsValid = false;
                        break;
                    }
                }

                

                if (pieceIsValid)
                {
                    for (int j = 0; j < coordinatesToCheck.Count; j++)
                    {
                        Vector2Int coordinate = coordinatesToCheck[j];
                        string flag = $"T_{treasureToFind[treasureIndex].name}_{j}"; // "T" for treasure, add sub index for sub sprites

                        gridData[GridUtils.CoordinateToIndex(coordinate, xLength, yLength)] = new CoordinateFlagPair(coordinate, flag);
                        placedSubSprites++;
                    }
                    
                    // Add hints
                    if(gridType == GridTypes.Coordinate)
                    {
                        hints.Add($"{GridUtils.SpreadSheetCoordinateToGameCoordinate(randomPieceOrigin, origin)}"); // Hint is the index of the origin coordinate in the grid data list
                    }
                    else if(gridType == GridTypes.A4)
                    {
                        hints.Add($"{GridUtils.SpreadSheetCoordinateToGameA4(randomPieceOrigin)}");
                    }


                    attempts = 0; // Reset attempts for next piece
                    treasureIndex++;
                    placedTreasures++;
                }
                else
                {
                    attempts++;
                    if (attempts >= maxAttempts)
                    {
                        Debug.LogWarning($"Could not place treasure piece {treasureToFind[treasureIndex].name} after {maxAttempts} attempts. Skipping this piece.");
                        attempts = 0; // Reset attempts for next piece
                        treasureIndicesToRemove.Add(treasureIndex);
                        treasureIndex++;
                    }
                }
            }

            // Remove any treasures that we failed to place from the treasureToFind list
            treasureIndicesToRemove.Reverse(); // Reverse the list so we remove from the end first and don't mess up the indices
            foreach (int index in treasureIndicesToRemove)
            {
                treasureToFind.RemoveAt(index);
            }

            // Tools MetaData ======================================
            tools.Add(new CountToolPair(UnityEngine.Random.Range(placedSubSprites, placedSubSprites * 2), Resources.Load<Tool>("ScriptableObjects/Tools/Shovel")));

            int randomHasPickaxe = UnityEngine.Random.Range(0, 2);
            if (randomHasPickaxe == 1)        
            {
                int numberOfPickaxe = (int)Mathf.FloorToInt(UnityEngine.Random.Range(placedSubSprites/2, placedSubSprites));
                if(numberOfPickaxe < 1) numberOfPickaxe = 1; // Ensure at least 1 pickaxe if we decide to include it
                
                tools.Add(new CountToolPair(numberOfPickaxe, Resources.Load<Tool>("ScriptableObjects/Tools/Pickaxe")));
            }

            int randomHasBomb = UnityEngine.Random.Range(0, 2);
            if (randomHasBomb == 1)        
            {
                int numberOfBombs = (int)Mathf.FloorToInt(UnityEngine.Random.Range(placedSubSprites/4, placedSubSprites/2));
                if(numberOfBombs < 1) numberOfBombs = 1; //
                
                tools.Add(new CountToolPair(numberOfBombs, Resources.Load<Tool>("ScriptableObjects/Tools/Bomb")));
            }

            // Create a new LevelData ScriptableObject and populate it with the generated data
            LevelData levelDataSO = ScriptableObject.CreateInstance<LevelData>();
            levelDataSO.SetGrid(gridData, xLength, yLength, gridType, origin);
            levelDataSO.SetTreasureToFind(treasureToFind);
            levelDataSO.hints = hints;
            levelDataSO.tools = tools;

            return levelDataSO;
        }
    
        #if UNITY_EDITOR
        public static void SaveLevelDataAsAsset(LevelData levelData, string path)
        {
            // Save the generated LevelData as an asset
            string levelName = $"RandomLevel_{Guid.NewGuid()}";
            string assetPath = $"{path}/{levelName}.asset";

            Directory.CreateDirectory($"{path}");

            AssetDatabase.CreateAsset(levelData, assetPath);
            AssetDatabase.SaveAssets();

            Debug.Log($"LevelData saved as asset at: {assetPath}");
        }
        #endif
    }
}