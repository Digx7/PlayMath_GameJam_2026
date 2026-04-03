using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;
using Digx7.Grids;

namespace Digx7.Levels
{
    [CreateAssetMenu(fileName = "NewLevelGeneratorTemplateData", menuName = "ScriptableObjects/Data/LevelGeneratorTemplateData", order = 1)]
    public class LevelGeneratorTemplateData: ScriptableObject
    {
        public int minGridSize = 4;
        public int maxGridSize = 4;
        public List<GridTypes> gridTypesToUse = new List<GridTypes>{GridTypes.Coordinate};
        public GridTypes GetRandomGridType()
        {
            return gridTypesToUse[UnityEngine.Random.Range(0, gridTypesToUse.Count)];
        }

        public int minTreasureAmount = 1;
        public int maxTreasureAmount = 1;
        public List<TreasurePiece> treasurePiecesToUse = new List<TreasurePiece>();
        public TreasurePiece GetRandomTreasurePiece()
        {
            return treasurePiecesToUse[UnityEngine.Random.Range(0, treasurePiecesToUse.Count)];
        }
        public List<TreasurePiece> GetRandomListOfTreasuresToFind()
        {
            int numberOfTreauresToFind = UnityEngine.Random.Range(minTreasureAmount, maxTreasureAmount + 1);

            if (numberOfTreauresToFind > treasurePiecesToUse.Count) numberOfTreauresToFind = treasurePiecesToUse.Count;

            List<TreasurePiece> treasureToFind = new List<TreasurePiece>();

            for (int i = 0; i < numberOfTreauresToFind; i++)
            {
                int treasureAttempts = 0;
                int maxTreasureAttempts = 100;
                bool validTreasureFound = false;
                int randomTreasureIndex = 0;

                while (!validTreasureFound && treasureAttempts < maxTreasureAttempts)
                {
                    randomTreasureIndex = UnityEngine.Random.Range(0, treasurePiecesToUse.Count);
                    TreasurePiece treasure = treasurePiecesToUse[i];

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

            return treasureToFind;
        }

        public List<TreasurePieceRotation> treasurePieceRotationsToUse = new List<TreasurePieceRotation>{TreasurePieceRotation.None};
        public TreasurePieceRotation GetRandomRotation()
        {
            return treasurePieceRotationsToUse[UnityEngine.Random.Range(0, treasurePieceRotationsToUse.Count)];
        }

        public bool useShovel = true;
        public int minShovelsToUse = 3;
        public int maxShovelsToUse = 3;
        public Tool shovelTool;
        public bool TryGetShovelCountToolPair(out CountToolPair countToolPair)
        {
            countToolPair = new CountToolPair(UnityEngine.Random.Range(minShovelsToUse, maxShovelsToUse + 1), shovelTool);

            if(useShovel)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool usePickAxe = true;
        public int minPickAxesToUse = 1;
        public int maxPickAxesToUse = 1;
        public Tool pickAxeTool;
        public bool TryGetPickAxeCountToolPair(out CountToolPair countToolPair)
        {
            countToolPair = new CountToolPair(UnityEngine.Random.Range(minPickAxesToUse, maxPickAxesToUse + 1), pickAxeTool);

            if(usePickAxe)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool useBomb = true;
        public int minBombsToUse = 1;
        public int maxBombsToUse = 1;
        public Tool bombTool;
        public bool TryGetBombCountToolPair(out CountToolPair countToolPair)
        {
            countToolPair = new CountToolPair(UnityEngine.Random.Range(minBombsToUse, maxBombsToUse + 1), bombTool);

            if(useBomb)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public void OnEnable() {
            shovelTool = Resources.Load<Tool>("ScriptableObjects/Tools/Shovel");
            pickAxeTool = Resources.Load<Tool>("ScriptableObjects/Tools/Pickaxe");
            bombTool = Resources.Load<Tool>("ScriptableObjects/Tools/Bomb");
        }

        #if UNITY_EDITOR

        [ContextMenu("Generate Level")]
        public void GenerateLevel()
        {
            LevelGenerator.SaveLevelDataAsAsset(LevelGenerator.GenerateRandomLevelFromTemplate(this), "Assets/GeneratedLevelsFromTemplate");
        }

        public void GenerateLevel(string levelName)
        {
            LevelGenerator.SaveLevelDataAsAssetWithName(LevelGenerator.GenerateRandomLevelFromTemplate(this), "Assets/GeneratedLevelsFromTemplate", levelName);
        }

        public void GenerateLevel(string path, string levelName)
        {
            LevelGenerator.SaveLevelDataAsAssetWithName(LevelGenerator.GenerateRandomLevelFromTemplate(this), path, levelName);
        }

        #endif
    }
}