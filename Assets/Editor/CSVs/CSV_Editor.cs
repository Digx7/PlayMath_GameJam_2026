using UnityEngine;
using UnityEditor;
using Digx7.Grids;
using System;
using System.IO;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace CSVTools
{
    public delegate void EntryToSO(List<string> entry);
    public delegate void AllEntriestoSO(List<List<string>> allEntries);
    public delegate string SOToEntry<T>(T so) where T : ScriptableObject;
    public delegate string[] SOToMultipleEntries<T>(T so) where T : ScriptableObject;
    
    public static class CSV_Editor
    {
        #region Main

        /// <summary>
        /// Imports data from a CSV file
        /// </summary>
        /// <param name="csvPathUnTagged">Absolute path to the CSV file of untagged data</param>
        /// <param name="csvPathTagged">Absolute path to the CSV file of tagged data</param>
        /// <param name="entryIndexesToTag">Entry indexes to add Tags from</param>
        /// <param name="entryToGameData">Delegate for processing CSV entry to ScriptableObject</param>
        public static void Import(string csvPathUnTagged, string csvPathTagged, int[] entryIndexesToTag, EntryToSO entryToGameData)
        {
            TagCSV(csvPathUnTagged, csvPathTagged, entryIndexesToTag);
            List<List<string>> data = ParseCSV(csvPathTagged);

            foreach (List<string> entry in data)
            {
                entryToGameData(entry);
            }
        }

        public static void Import(string csvPath, EntryToSO entryToGameData)
        {
            List<List<string>> data = ParseCSV(csvPath);

            foreach (List<string> entry in data)
            {
                entryToGameData(entry);
            }
        }

        public static void ImportAllEntriesAtOnce(string csvPath, AllEntriestoSO allEntriesToGameData)
        {
            List<List<string>> data = ParseCSV(csvPath);
            allEntriesToGameData(data);
        }

        /// <summary>
        /// Exports data to a CSV file
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="csvPathUnTagged">Absolute path to the CSV file of untagged data</param>
        /// <param name="csvPathTagged">Absolute path to the CSV file of tagged data</param>
        /// <param name="scriptableObjectDirPath">Relative path to the ScriptableObject Directory</param>
        /// <param name="entryIndexesToUnTag">Entry indexes to remove Tags from</param>
        /// <param name="gameDataToString">Delegate for processing ScriptableObject to CSV entry</param>
        public static void Export<T>(string csvPathUnTagged, string csvPathTagged, string scriptableObjectDirPath, int[] entryIndexesToUnTag, SOToEntry<T> gameDataToString) where T : ScriptableObject
        {
            List<T> scriptableObjects = CSV_SOHelpers.LoadAllScriptableObjectsInDir<T>(scriptableObjectDirPath);
            string[] newLines = new string[scriptableObjects.Count + 1];
            newLines[0] = GetCSVHeaderLine(csvPathTagged);

            for (int i = 0; i < scriptableObjects.Count; i++)
            {
                newLines[i + 1] = gameDataToString(scriptableObjects[i]);
                newLines[i + 1] = Regex.Replace(newLines[i + 1], "(?<!\r)\n", "\r\n");
            }

            File.WriteAllLines(csvPathTagged, newLines);
            UnTagCSV(csvPathUnTagged, csvPathTagged, entryIndexesToUnTag);
        }

        public static void Export<T>(string csvPath, string scriptableObjectDirPath, SOToEntry<T> gameDataToString) where T : ScriptableObject
        {
            List<T> scriptableObjects = CSV_SOHelpers.LoadAllScriptableObjectsInDir<T>(scriptableObjectDirPath);
            string[] newLines = new string[scriptableObjects.Count + 1];
            newLines[0] = GetCSVHeaderLine(csvPath);

            for (int i = 0; i < scriptableObjects.Count; i++)
            {
                newLines[i + 1] = gameDataToString(scriptableObjects[i]);
                newLines[i + 1] = Regex.Replace(newLines[i + 1], "(?<!\r)\n", "\r\n");
            }

            File.WriteAllLines(csvPath, newLines);
        }

        public static void ExportObjectAsMultipleEntries<T>(string csvPath, string scriptableObjectPath, SOToMultipleEntries<T> gameDataToStrings) where T : ScriptableObject
        {
            T scriptableObject = CSV_SOHelpers.LoadScriptableObjectAtPath<T>(scriptableObjectPath);
            string[] newLines = gameDataToStrings(scriptableObject);

            // for (int i = 0; i < newLines.Length; i++)
            // {
            //     newLines[i] = Regex.Replace(newLines[i], "(?<!\r)\n", "\r\n");
            // }

            File.WriteAllLines(csvPath, newLines);
        }

        /// <summary>
        /// Parses the CSV file at the given <c>path</c>
        /// </summary>
        /// <param name="path">Absolute path to the CSV file to parse</param>
        /// <returns></returns>
        public static List<List<string>> ParseCSV(string path)
        {
            Debug.Log($"Attempting to Parse CSV at path: {path}");

            List<List<string>> data = new List<List<string>>();

            string allText = File.ReadAllText(path);

            allText = Regex.Replace(allText, "(?<!\r)\n", "\r\n");

            string[] allLines = allText.Split(CSV_UserData.ROW_DELIMITER, System.StringSplitOptions.RemoveEmptyEntries);

            Debug.Log($"allText size: {allText.Length} allLines.Length: {allLines.Length}");

            foreach (string line in allLines)
            {
                string[] entry = line.Split(CSV_UserData.COLUMN_DELIMITER);
                if (entry[0] == "Name") continue;

                data.Add(new List<string>(entry));
            }

            return data;
        }

        /// <summary>
        /// Gets the Header row of a CSV file
        /// </summary>
        /// <param name="path"></param>
        /// <returns>string</returns>
        public static string GetCSVHeaderLine(string path)
        {
            string[] allLines = File.ReadAllLines(path);
            return allLines[0];
        }
    
        #endregion

        #region Tags

        /// <summary>
        /// Copies all the data from the CSV file at <c>csvPathUnTagged</c> to the CSV file at <c>csvPathTagged</c>.<br/>
        /// In the process it adds tags from the entries at the <c>entryIndexesToUnTag</c>.
        /// </summary>
        /// <param name="csvPathUnTagged">Path to the CSV file for the UnTagged data</param>
        /// <param name="csvPathTagged">Path to the CSV file for the Tagged data</param>
        /// <param name="entryIndexesToUnTag">Entry indexes to add Tags from</param>
        public static void TagCSV(string csvPathUnTagged, string csvPathTagged, int[] entryIndexesToTag)
        {
            List<List<string>> rawData = ParseCSV(csvPathUnTagged);

            int lineCount = rawData.Count + 1;
            string[] newLines = new string[lineCount];
            newLines[0] = GetCSVHeaderLine(csvPathUnTagged);

            for (int i = 0; i < rawData.Count; i++)
            {
                for (int j = 0; j < entryIndexesToTag.Length; j++)
                {
                    rawData[i][entryIndexesToTag[j]] = AddTags(rawData[i][entryIndexesToTag[j]]);
                }
                
                newLines[i + 1] = string.Join(CSV_UserData.COLUMN_DELIMITER, rawData[i]);
                newLines[i + 1] += "$";
            }

            File.WriteAllLines(csvPathTagged, newLines);
        }

        /// <summary>
        /// Copies all the data from the CSV file at <c>csvPathTagged</c> to the CSV file at <c>csvPathUnTagged</c>.<br/>
        /// In the process it removes all the tags from the entries at the <c>entryIndexesToUnTag</c>.
        /// </summary>
        /// <param name="csvPathUnTagged">Path to the CSV file for the UnTagged data</param>
        /// <param name="csvPathTagged">Path to the CSV file for the Tagged data</param>
        /// <param name="entryIndexesToUnTag">Entry indexes to remove Tags from</param>
        public static void UnTagCSV(string csvPathUnTagged, string csvPathTagged, int[] entryIndexesToUnTag)
        {
            List<List<string>> rawData = ParseCSV(csvPathTagged);

            int lineCount = rawData.Count + 1;
            string[] newLines = new string[lineCount];
            newLines[0] = GetCSVHeaderLine(csvPathTagged);

            for (int i = 0; i < rawData.Count; i++)
            {
                for (int j = 0; j < entryIndexesToUnTag.Length; j++)
                {
                    rawData[i][entryIndexesToUnTag[j]] = RemoveTags(rawData[i][entryIndexesToUnTag[j]]);
                }
                
                newLines[i + 1] = string.Join(CSV_UserData.COLUMN_DELIMITER, rawData[i]);
                newLines[i + 1] += "$";
            }

            File.WriteAllLines(csvPathUnTagged, newLines);
        }

        /// <summary>
        /// Takes in a string, adds tags and returns the edited string
        /// </summary>
        /// <param name="input"></param>
        /// <returns>The input with with the tags added</returns>
        private static string AddTags(string input)
        {
            input = input.Replace("\"", "”");
            
            string csvPath_TagData = Application.dataPath + CSV_UserData.AUTOTAGS_CSV_PATH;
            List<List<string>> tagData = ParseCSV(csvPath_TagData);

            foreach (List<string> entry in tagData)
            {
                string og = entry[0];
                string replace = entry[1] + " " + entry[0] + " " + entry[2];
                string[] filters = entry[3].Split(CSV_UserData.LIST_DELIMITER);
                string pattern = $"{og}";

                // makes it so the pattern ends up being "og(?! filter)(?! filter)(?! filter)" where "(?! filter)" repeats for every filter
                foreach (string filter in filters)
                {
                    if(filter == "") continue;
                    
                    pattern += $"(?! {filter})";
                }

                input = Regex.Replace(input, pattern, replace);

                // input = input.Replace(og, replace);
            }

            return input;
        }

        /// <summary>
        /// Takes in a string, removes tags and returns the edited string
        /// </summary>
        /// <param name="input"></param>
        /// <returns>The input with with the tags removed</returns>
        private static string RemoveTags(string input)
        {
            input = input.Replace("\"", "”");
            
            // Remove tags
            input = Regex.Replace(input,@"<([^>]*)>", "");

            // Remove 2 or more consecutive spaces, not proceded by a period.
            input = Regex.Replace(input, @"(?<!\.)( {2,})", " ");

            // Replace 3 or more consecutive line breaks with just 2 line breaks
            input = Regex.Replace(input, "\r\n{3,}", "\r\n\r\n");

            return input;
        }

        #endregion
        }

    public static class CSV_UserData
    {
        // #region Trains
        // public const string TRAINS_CSV_PATH_UNTAGGED = "/Editor/CSVs/Trains_UnTagged.csv";
        // public const string TRAINS_CSV_PATH_TAGGED = "/Editor/CSVs/Trains_Tagged.csv";
        // public const string TRAINS_SO_PATH = "Assets/Resources/ScriptableObjects/Trains/";
        // public static int[] TRAINS_TAG_INDEXS = new int[]{5};
        // #endregion

        // #region Resources
        // public const string RESOURCES_CSV_PATH_UNTAGGED = "/Editor/CSVs/Resources_UnTagged.csv";
        // public const string RESOURCES_CSV_PATH_TAGGED = "/Editor/CSVs/Resources_Tagged.csv";
        // public const string RESOURCES_SO_PATH = "Assets/Resources/ScriptableObjects/ResourceSOs/";
        // public static int[] RESOURCES_TAG_INDEXS = new int[]{2};
        // #endregion

        // #region Unlocks
        // public const string UNLOCKS_CSV_PATH_UNTAGGED = "/Editor/CSVs/Unlocks_UnTagged.csv";
        // public const string UNLOCKS_CSV_PATH_TAGGED = "/Editor/CSVs/Unlocks_Tagged.csv";
        // public const string UNLOCKS_SO_PATH = "Assets/Resources/ScriptableObjects/Unlocks/";
        // public static int[] UNLOCKS_TAG_INDEXS = new int[]{9};
        // #endregion

        // #region Buffs
        // public const string BUFFS_CSV_PATH_UNTAGGED = "/Editor/CSVs/Buffs_UnTagged.csv";
        // public const string BUFFS_CSV_PATH_TAGGED = "/Editor/CSVs/Buffs_Tagged.csv";
        // public const string BUFFS_SO_PATH = "Assets/Resources/ScriptableObjects/Buffs/";
        // public static int[] BUFFS_TAG_INDEXS = new int[]{7, 8};
        // #endregion

        // #region Stations
        // public const string STATIONS_CSV_PATH_UNTAGGED = "/Editor/CSVs/Stations_UnTagged.csv";
        // public const string STATIONS_CSV_PATH_TAGGED = "/Editor/CSVs/Stations_Tagged.csv";
        // public const string STATIONS_SO_PATH = "Assets/Resources/ScriptableObjects/Stations/";
        // public static int[] STATIONS_TAG_INDEXS = new int[]{2};
        // #endregion

        // #region Guides
        // public const string GUIDES_CSV_PATH_UNTAGGED = "/Editor/CSVs/Guides_UnTagged.csv";
        // public const string GUIDES_CSV_PATH_TAGGED = "/Editor/CSVs/Guides_Tagged.csv";
        // public const string GUIDES_SO_PATH = "Assets/Resources/ScriptableObjects/Guides/";
        // public static int[] GUIDES_TAG_INDEXS = new int[]{3};
        // #endregion

        #region LevelData
        public const string LEVELDATA_CSV_DIR = "/Editor/CSVs/LevelData/";
        public const string LEVELDATA_SO_DIR = "Assets/Resources/ScriptableObjects/LevelData/";
        public const string TREASUREPIECEDATA_SO_DIR = "Assets/Resources/ScriptableObjects/TreasurePieces/";
        public const string TOOL_SO_DIR = "Assets/Resources/ScriptableObjects/Tools/";
        #endregion

        #region General
        public const string AUTOTAGS_CSV_PATH = "/Editor/CSVs/AutoTags.csv";

        public const char COLUMN_DELIMITER = '|';
        public const string ROW_DELIMITER = "$\r\n";
        public const string LIST_DELIMITER = ",";
        public const char STRING_DELIMITER = '\'';
        #endregion
    }

    public static class CSV_UserFunctions
    {
        [MenuItem("Utilities/CSV/General/ImportAll")]
        public static void ImportAll()
        {
            // ImportTrains();
            // ImportResources();
            // ImportUnlocks();
            // ImportBuffs();
            // ImportStations();
            // ImportGuides();
        }

        [MenuItem("Utilities/CSV/General/ExportAll")]
        public static void ExportAll()
        {
            // ExportTrains();
            // ExportResources();
            // ExportUnlocks();
            // ExportBuffs();
            // ExportStations();
            // ExportGuides();
        }

        // #region Trains

        // [MenuItem("Utilities/CSV/Trains/Import")]
        // public static void ImportTrains()
        // {
        //     CSV_Editor.Import(Application.dataPath + CSV_UserData.TRAINS_CSV_PATH_UNTAGGED, Application.dataPath + CSV_UserData.TRAINS_CSV_PATH_TAGGED, CSV_UserData.TRAINS_TAG_INDEXS, EntryToTrain);
        // }

        // [MenuItem("Utilities/CSV/Trains/Export")]
        // public static void ExportTrains()
        // {
        //     CSV_Editor.Export<TrainCarData>(Application.dataPath + CSV_UserData.TRAINS_CSV_PATH_UNTAGGED, Application.dataPath + CSV_UserData.TRAINS_CSV_PATH_TAGGED, CSV_UserData.TRAINS_SO_PATH, CSV_UserData.TRAINS_TAG_INDEXS, TrainToEntry);
        // }
        
        // public static void EntryToTrain(List<string> entry)
        // {
        //     string assetName = entry[0];
        //     string assetPath = $"{CSV_UserData.TRAINS_SO_PATH}{assetName}.asset";

        //     Debug.Log($"Processing Train {assetName}");

        //     TrainCarData trainSO = CSV_SOHelpers.LoadOrCreateScriptableObjectInstance<TrainCarData>(assetPath, assetName);

        //     // Parsing
        //     trainSO.name = assetName;

        //     // Sprites

        //     // trainSO.sprite = (entry[1] != "" && AssetDatabase.AssetPathExists(entry[1])) ? (Sprite)AssetDatabase.LoadAssetAtPath(entry[1], typeof(Sprite)) : null;
        //     // trainSO.spriteOutlineClean = (entry[2] != "" && AssetDatabase.AssetPathExists(entry[2])) ? (Sprite)AssetDatabase.LoadAssetAtPath(entry[2], typeof(Sprite)) : null;
        //     // trainSO.spriteOutlineRough = (entry[3] != "" && AssetDatabase.AssetPathExists(entry[3])) ? (Sprite)AssetDatabase.LoadAssetAtPath(entry[3], typeof(Sprite)) : null;
        //     // trainSO.spriteOutlineRoughLine = (entry[4] != "" && AssetDatabase.AssetPathExists(entry[4])) ? (Sprite)AssetDatabase.LoadAssetAtPath(entry[4], typeof(Sprite)) : null;
        //     trainSO.visualPrefab = (entry[1] != "" && AssetDatabase.AssetPathExists(entry[1])) ? (GameObject)AssetDatabase.LoadAssetAtPath(entry[1], typeof(GameObject)) : null;


        //     // Summary
        //     trainSO.summary = entry[5].Trim(CSV_UserData.STRING_DELIMITER);
        //     trainSO.summary = trainSO.summary.Replace("”", "\"");

        //     // Purchase Cost
        //     if (AssetDatabase.AssetPathExists($"Assets/Resources/ScriptableObjects/ResourceSOs/{entry[6]}.asset"))
        //     {
        //         trainSO.purchaseInfo.resourceSO = (ResourceSO)AssetDatabase.LoadAssetAtPath($"Assets/Resources/ScriptableObjects/ResourceSOs/{entry[6]}.asset", typeof(ResourceSO));
        //         trainSO.purchaseInfo.amount = int.Parse(entry[7]);
        //     }
        //     else
        //     {
        //         Debug.Log($"Failed: to find resource {entry[6]} at Assets/Resources/ScriptableObjects/ResourceSOs/{entry[6]}.asset");
        //     }

        //     try
        //     {
        //         // Sell Cost
        //         trainSO.canSell = bool.Parse(entry[8]);
        //     }
        //     catch (System.Exception)
        //     {
        //         Debug.Log($"Tried parsing Entry[8] = {entry[8]} as a bool but it failed");
        //         throw;
        //     }

            
        //     if (AssetDatabase.AssetPathExists($"Assets/Resources/ScriptableObjects/ResourceSOs/{entry[9]}.asset"))
        //     {
        //         trainSO.sellInfo.resourceSO = (ResourceSO)AssetDatabase.LoadAssetAtPath($"Assets/Resources/ScriptableObjects/ResourceSOs/{entry[9]}.asset", typeof(ResourceSO));
        //         trainSO.sellInfo.amount = int.Parse(entry[10]);
        //     }
        //     else
        //     {
        //         Debug.Log($"Failed: to find resource {entry[9]} at Assets/Resources/ScriptableObjects/ResourceSOs/{entry[9]}.asset");
        //     }

        //     try
        //     {
        //         // Repair Cost
        //         trainSO.canRepair = bool.Parse(entry[11]);
        //     }
        //     catch (System.Exception)
        //     {
        //         Debug.Log($"Tried parsing Entry[11] = {entry[11]} as a bool but it failed");
        //         throw;
        //     }            
            
        //     if (AssetDatabase.AssetPathExists($"Assets/Resources/ScriptableObjects/ResourceSOs/{entry[12]}.asset"))
        //     {
        //         trainSO.repairInfo.resourceSO = (ResourceSO)AssetDatabase.LoadAssetAtPath($"Assets/Resources/ScriptableObjects/ResourceSOs/{entry[12]}.asset", typeof(ResourceSO));
        //         trainSO.repairInfo.amount = int.Parse(entry[13]);
        //     }
        //     else
        //     {
        //         Debug.Log($"Failed: to find resource {entry[12]} at Assets/Resources/ScriptableObjects/ResourceSOs/{entry[12]}.asset");
        //     }

        //     // Stats
        //     trainSO.stats.carType = entry[14];
        //     trainSO.stats.maxHealth = int.Parse(entry[15]);
        //     trainSO.stats.baseWarmth = int.Parse(entry[16]);
        //     trainSO.stats.baseArmor = int.Parse(entry[17]);
        //     trainSO.stats.isEngine = bool.Parse(entry[18]);

        //     // Speed Options
        //     string nOption = "";
        //     try
        //     {
        //         if (entry[19] != "")
        //         {
        //             trainSO.stats.speedOptions.Clear();
        //             string[] speedOptions = entry[19].Split(CSV_UserData.LIST_DELIMITER);
        //             foreach (string option in speedOptions)
        //             {
        //                 nOption = option;
        //                 trainSO.stats.speedOptions.Add(int.Parse(option));
        //             }
        //         }
        //     }
        //     catch (System.Exception)
        //     {
        //         Debug.LogWarning($"Error in speed options on import for {assetName}, failed to Parse option {nOption}");
        //         // throw;
        //     }

            

        //     trainSO.stats.maxWorkers = int.Parse(entry[20]);
        //     trainSO.stats.minWorkers = int.Parse(entry[21]);
        //     trainSO.stats.maxEngineers = int.Parse(entry[22]);
        //     trainSO.stats.minEngineers = int.Parse(entry[23]);

        //     // Buffs
        //     trainSO.stats.neighborBuffData.Clear();
        //     string[] buffNames = entry[24].Split(CSV_UserData.LIST_DELIMITER);
        //     foreach (string buffName in buffNames)
        //     {
        //         string buffPath = $"Assets/Resources/ScriptableObjects/Buffs/{buffName}.asset";
        //         if (AssetDatabase.AssetPathExists(buffPath))
        //         {
        //             trainSO.stats.neighborBuffData.Add((NeighborBuffData)AssetDatabase.LoadAssetAtPath(buffPath, typeof(NeighborBuffData)));
        //         }
        //         else
        //         {
        //             Debug.LogWarning($"Failed to add Buff to {assetName}\nBecause no Buff exists at the given path {buffPath}");
        //         }
        //     }

        //     CSV_SOHelpers.CreateNewScriptableObjectIfAssetDoesntExist<TrainCarData>(trainSO, assetPath);
        // }

        // public static string TrainToEntry(TrainCarData trainSOs)
        // {
        //     string[] entry = new string[26];

        //     // Name
        //     entry[0] = trainSOs.name;

        //     // Sprites
        //     // entry[1] = (trainSOs.sprite != null) ? AssetDatabase.GetAssetPath(trainSOs.sprite) : "";
        //     // entry[2] = (trainSOs.spriteOutlineClean != null) ? AssetDatabase.GetAssetPath(trainSOs.spriteOutlineClean) : "";
        //     // entry[3] = (trainSOs.spriteOutlineRough != null) ? AssetDatabase.GetAssetPath(trainSOs.spriteOutlineRough) : "";
        //     // entry[4] = (trainSOs.spriteOutlineRoughLine != null) ? AssetDatabase.GetAssetPath(trainSOs.spriteOutlineRoughLine) : "";
        //     entry[1] = (trainSOs.visualPrefab != null) ? AssetDatabase.GetAssetPath(trainSOs.visualPrefab) : "";

        //     // Summary
        //     entry[5] = $"{CSV_UserData.STRING_DELIMITER}{trainSOs.summary.Replace("\"", "”")}{CSV_UserData.STRING_DELIMITER}";

        //     // Purchase Cost
        //     entry[6] = (trainSOs.purchaseInfo.resourceSO != null) ? trainSOs.purchaseInfo.resourceSO.name : "";
        //     entry[7] = trainSOs.purchaseInfo.amount.ToString();

        //     // Sell Cost
        //     entry[8] = trainSOs.canSell.ToString();
        //     entry[9] = (trainSOs.sellInfo.resourceSO != null) ? trainSOs.sellInfo.resourceSO.name : "";
        //     entry[10] = trainSOs.sellInfo.amount.ToString();

        //     // Repair Cost
        //     entry[11] = trainSOs.canRepair.ToString();
        //     entry[12] = (trainSOs.repairInfo.resourceSO != null) ? trainSOs.repairInfo.resourceSO.name : "";
        //     entry[13] = trainSOs.repairInfo.amount.ToString();

        //     // Stats
        //     entry[14] = trainSOs.stats.carType;
        //     entry[15] = trainSOs.stats.maxHealth.ToString();
        //     entry[16] = trainSOs.stats.baseWarmth.ToString();
        //     entry[17] = trainSOs.stats.baseArmor.ToString();
        //     entry[18] = trainSOs.stats.isEngine.ToString();

        //     // Speed Options
        //     List<int> speedOptions = new List<int>();
        //     for (int j = 0; j < trainSOs.stats.speedOptions.Count; j++)
        //     {
        //         speedOptions.Add(trainSOs.stats.speedOptions[j]);
        //     }
        //     entry[19] = string.Join(CSV_UserData.LIST_DELIMITER, speedOptions);

        //     entry[20] = trainSOs.stats.maxWorkers.ToString();
        //     entry[21] = trainSOs.stats.minWorkers.ToString();
        //     entry[22] = trainSOs.stats.maxEngineers.ToString();
        //     entry[23] = trainSOs.stats.minEngineers.ToString();

        //     // Buffs
        //     List<string> buffNames = new List<string>();
        //     for (int j = 0; j < trainSOs.stats.neighborBuffData.Count; j++)
        //     {
        //         buffNames.Add(trainSOs.stats.neighborBuffData[j].name);
        //     }
        //     entry[24] = string.Join(CSV_UserData.LIST_DELIMITER, buffNames);

        //     // Row Delimiter
        //     entry[25] = "$";

        //     return string.Join(CSV_UserData.COLUMN_DELIMITER, entry);
        // }

        // #endregion

        // #region Resources

        // [MenuItem("Utilities/CSV/Resources/Import")]
        // public static void ImportResources()
        // {
        //     CSV_Editor.Import(Application.dataPath + CSV_UserData.RESOURCES_CSV_PATH_UNTAGGED, Application.dataPath + CSV_UserData.RESOURCES_CSV_PATH_TAGGED, CSV_UserData.RESOURCES_TAG_INDEXS, EntryToResource);
        // }

        // [MenuItem("Utilities/CSV/Resources/Export")]
        // public static void ExportResources()
        // {
        //     CSV_Editor.Export<ResourceSO>(Application.dataPath + CSV_UserData.RESOURCES_CSV_PATH_UNTAGGED, Application.dataPath + CSV_UserData.RESOURCES_CSV_PATH_TAGGED, CSV_UserData.RESOURCES_SO_PATH, CSV_UserData.RESOURCES_TAG_INDEXS, ResourceToEntry);
        // }
        
        // public static void EntryToResource(List<string> entry)
        // {
        //     string assetName = entry[0];
        //     string assetPath = $"{CSV_UserData.RESOURCES_SO_PATH}{assetName}.asset";

        //     ResourceSO resourceSO = CSV_SOHelpers.LoadOrCreateScriptableObjectInstance<ResourceSO>(assetPath, assetName);

        //     // Parsing
        //     resourceSO.name = assetName;

        //     // Sprite
        //     string spritePath = $"Assets/Art/InkScape/Resources/{entry[1]}.png";
        //     if (AssetDatabase.AssetPathExists(spritePath))
        //     {
        //         resourceSO.sprite = (Sprite)AssetDatabase.LoadAssetAtPath(spritePath, typeof(Sprite));
        //     }
        //     else
        //     {
        //         Debug.LogWarning($"No sprite found at the path: {spritePath}");
        //     }

        //     resourceSO.description = entry[2].Trim(CSV_UserData.STRING_DELIMITER);
        //     resourceSO.description = resourceSO.description.Replace("”", "\"");
        //     resourceSO.baseMaxStorage = int.Parse(entry[3]);
        //     resourceSO.isSpecial = bool.Parse(entry[4]);

        //     CSV_SOHelpers.CreateNewScriptableObjectIfAssetDoesntExist<ResourceSO>(resourceSO, assetPath);
        // }

        // public static string ResourceToEntry(ResourceSO resourceSOs)
        // {
        //     string[] entry = new string[6];

        //     // Parse
        //     entry[0] = resourceSOs.name;

        //     if (resourceSOs.sprite != null) entry[1] = resourceSOs.sprite.name.ToString();

        //     entry[2] = $"{CSV_UserData.STRING_DELIMITER}{resourceSOs.description.Replace("\"", "”")}{CSV_UserData.STRING_DELIMITER}";
        //     entry[3] = resourceSOs.baseMaxStorage.ToString();
        //     entry[4] = resourceSOs.isSpecial.ToString();

        //     entry[5] = "$";

        //     return string.Join(CSV_UserData.COLUMN_DELIMITER, entry);
        // }

        // #endregion

        // #region Unlocks

        // [MenuItem("Utilities/CSV/Unlocks/Import")]
        // public static void ImportUnlocks()
        // {
        //     CSV_Editor.Import(Application.dataPath + CSV_UserData.UNLOCKS_CSV_PATH_UNTAGGED, Application.dataPath + CSV_UserData.UNLOCKS_CSV_PATH_TAGGED, CSV_UserData.UNLOCKS_TAG_INDEXS, EntryToUnlocks);
        //     CSV_Editor.Import(Application.dataPath + CSV_UserData.UNLOCKS_CSV_PATH_UNTAGGED, Application.dataPath + CSV_UserData.UNLOCKS_CSV_PATH_TAGGED, CSV_UserData.UNLOCKS_TAG_INDEXS, EntryToUnlocks);
        // }

        // [MenuItem("Utilities/CSV/Unlocks/Export")]
        // public static void ExportUnlocks()
        // {
        //     CSV_Editor.Export<UnlockAbleTech>(Application.dataPath + CSV_UserData.UNLOCKS_CSV_PATH_UNTAGGED, Application.dataPath + CSV_UserData.UNLOCKS_CSV_PATH_TAGGED, CSV_UserData.UNLOCKS_SO_PATH, CSV_UserData.UNLOCKS_TAG_INDEXS, UnlocksToEntry);
        // }
        
        // public static void EntryToUnlocks(List<string> entry)
        // {
        //     string assetName = entry[0];
        //     string assetPath = $"{CSV_UserData.UNLOCKS_SO_PATH}{assetName}.asset";

        //     Debug.Log($"Processing Unlock {assetName}");

        //     string researchSOPath = $"Assets/Resources/ScriptableObjects/ResourceSOs/{entry[2]}.asset";
        //     string unlockSOPath = $"Assets/Resources/ScriptableObjects/ResourceSOs/{entry[4]}.asset";

        //     UnlockAbleTech unlockSO = CSV_SOHelpers.LoadOrCreateScriptableObjectInstance<UnlockAbleTech>(assetPath, assetName);

        //     // Parsing
        //     unlockSO.name = assetName;
        //     unlockSO.title = entry[1];

        //     // Research Cost
        //     if (!string.IsNullOrEmpty(entry[2]))
        //     {
        //         if(AssetDatabase.AssetPathExists(researchSOPath))
        //         {
        //             unlockSO.researchCost.resourceSO = (ResourceSO)AssetDatabase.LoadAssetAtPath(researchSOPath, typeof(ResourceSO));
        //             unlockSO.researchCost.amount = int.Parse(entry[3]);
        //         }
        //         else
        //         {
        //             Debug.LogWarning($"Failed to add Research to {assetName}\nBecause no ResourceSO exists at the given path {researchSOPath}");
        //         }
        //     }

        //     // Unlock Cost
        //     if (!string.IsNullOrEmpty(entry[2]))
        //     {
        //         if (AssetDatabase.AssetPathExists(unlockSOPath))
        //         {
        //             unlockSO.unlockCost.resourceSO = (ResourceSO)AssetDatabase.LoadAssetAtPath(unlockSOPath, typeof(ResourceSO));
        //             unlockSO.unlockCost.amount = int.Parse(entry[5]);
        //         }
        //         else
        //         {
        //             Debug.LogWarning($"Failed to add Unlock to {assetName}\nBecause no ResourceSO exists at the given path {unlockSOPath}");
        //         }
        //     }

        //     // Requirements
        //     if(unlockSO.requirements != null)unlockSO.requirements.Clear();
        //     string[] requirementNames = entry[6].Split(CSV_UserData.LIST_DELIMITER);
        //     foreach (string requirementName in requirementNames)
        //     {
        //         string requirementPath = $"Assets/Resources/ScriptableObjects/Unlocks/{requirementName}.asset";
        //         if (AssetDatabase.AssetPathExists(requirementPath))
        //         {
        //             unlockSO.requirements.Add((UnlockAbleTech)AssetDatabase.LoadAssetAtPath(requirementPath, typeof(UnlockAbleTech)));
        //         }
        //         else
        //         {
        //             Debug.LogWarning($"Failed to add Requirement to {assetName}\nBecause no Unlock exists at the given path {requirementPath}");
        //         }
        //     }

        //     // Sprite
        //     string spritePath = $"Assets/Art/Krita/TrainCars/{entry[7]}.png";
        //     if (AssetDatabase.AssetPathExists(spritePath))
        //     {
        //         unlockSO.sprite = (Sprite)AssetDatabase.LoadAssetAtPath(spritePath, typeof(Sprite));
        //     }
        //     else
        //     {
        //         Debug.LogWarning($"No sprite found at the path: {spritePath}");
        //     }
        //     unlockSO.spriteHasWheels = bool.Parse(entry[8]);

        //     // Description
        //     unlockSO.description = entry[9].Trim(CSV_UserData.STRING_DELIMITER);
        //     unlockSO.description = unlockSO.description.Replace("”", "\"");

        //     CSV_SOHelpers.CreateNewScriptableObjectIfAssetDoesntExist<UnlockAbleTech>(unlockSO, assetPath);
        // }

        // public static string UnlocksToEntry(UnlockAbleTech unlockSOs)
        // {
        //     string[] entry = new string[11];

        //     // Name and Title
        //     entry[0] = unlockSOs.name;
        //     entry[1] = unlockSOs.title;

        //     // Research Cost
        //     if (unlockSOs.researchCost.resourceSO != null)
        //     {
        //         entry[2] = unlockSOs.researchCost.resourceSO.name;
        //         entry[3] = unlockSOs.researchCost.amount.ToString();
        //     }

        //     // Unlock Cost
        //     if (unlockSOs.unlockCost.resourceSO != null)
        //     {
        //         entry[4] = unlockSOs.unlockCost.resourceSO.name;
        //         entry[5] = unlockSOs.unlockCost.amount.ToString();
        //     }

        //     // Requirements
        //     List<string> requirementNames = new List<string>();
        //     for (int j = 0; j < unlockSOs.requirements.Count; j++)
        //     {
        //         requirementNames.Add(unlockSOs.requirements[j].name);
        //     }
        //     entry[6] = string.Join(CSV_UserData.LIST_DELIMITER, requirementNames);

        //     // Sprite
        //     if (unlockSOs.sprite != null) entry[7] = unlockSOs.sprite.name.ToString();
        //     entry[8] = unlockSOs.spriteHasWheels.ToString();

        //     // Description
        //     entry[9] = $"{CSV_UserData.STRING_DELIMITER}{unlockSOs.description.Replace("\"", "”")}{CSV_UserData.STRING_DELIMITER}";

        //     // Row Delimiter
        //     entry[10] = "$";

        //     return string.Join(CSV_UserData.COLUMN_DELIMITER, entry);
        // }
     
        // #endregion

        // #region Buffs

        // [MenuItem("Utilities/CSV/Buffs/Import")]
        // public static void ImportBuffs()
        // {
        //     CSV_Editor.Import(Application.dataPath + CSV_UserData.BUFFS_CSV_PATH_UNTAGGED, Application.dataPath + CSV_UserData.BUFFS_CSV_PATH_TAGGED, CSV_UserData.BUFFS_TAG_INDEXS, EntryToBuffs);
        // }

        // [MenuItem("Utilities/CSV/Buffs/Export")]
        // public static void ExportBuffs()
        // {
        //     CSV_Editor.Export<NeighborBuffData>(Application.dataPath + CSV_UserData.BUFFS_CSV_PATH_UNTAGGED, Application.dataPath + CSV_UserData.BUFFS_CSV_PATH_TAGGED, CSV_UserData.BUFFS_SO_PATH, CSV_UserData.BUFFS_TAG_INDEXS, BuffsToEntry);
        // }
        
        // public static void EntryToBuffs(List<string> entry)
        // {
        //     string assetName = entry[0];
        //     string assetPath = $"{CSV_UserData.BUFFS_SO_PATH}{assetName}.asset";

        //     Debug.Log($"Processing Buff {assetName}");

        //     NeighborBuffData neighborBuffData = CSV_SOHelpers.LoadOrCreateScriptableObjectInstance<NeighborBuffData>(assetPath, assetName);

        //     // Parsing
        //     neighborBuffData.name = assetName;

        //     // Direction
        //     switch (entry[1])
        //     {
        //         case "Ahead":
        //             neighborBuffData.neighborBuff.direction = NeighborDirection.Ahead;
        //             break;
        //         case "Behind":
        //             neighborBuffData.neighborBuff.direction = NeighborDirection.Behind;
        //             break;
        //         case "AheadAndBehind":
        //             neighborBuffData.neighborBuff.direction = NeighborDirection.AheadAndBehind;
        //             break;
        //         default:
        //             break;
        //     }

        //     // Range
        //     neighborBuffData.neighborBuff.range = int.Parse(entry[2]);

        //     // Sprite
        //     if (AssetDatabase.AssetPathExists(entry[3]))
        //     {
        //         neighborBuffData.neighborBuff.buff.sprite = (Sprite)AssetDatabase.LoadAssetAtPath(entry[3], typeof(Sprite));
        //     }

        //     // Name
        //     neighborBuffData.neighborBuff.buff.name = entry[4];

        //     // Stat To Effect
        //     neighborBuffData.neighborBuff.buff.statToEffect = entry[5];

        //     // Amount
        //     neighborBuffData.neighborBuff.buff.amount = int.Parse(entry[6]);

        //     // Giving Description
        //     neighborBuffData.neighborBuff.buff.givingDescription = entry[7].Trim(CSV_UserData.STRING_DELIMITER);
        //     neighborBuffData.neighborBuff.buff.givingDescription = neighborBuffData.neighborBuff.buff.givingDescription.Replace("”", "\"");

        //     // Applied Description
        //     neighborBuffData.neighborBuff.buff.appliedDescription = entry[8].Trim(CSV_UserData.STRING_DELIMITER);
        //     neighborBuffData.neighborBuff.buff.appliedDescription = neighborBuffData.neighborBuff.buff.appliedDescription.Replace("”", "\"");

        //     CSV_SOHelpers.CreateNewScriptableObjectIfAssetDoesntExist<NeighborBuffData>(neighborBuffData, assetPath);
        // }

        // public static string BuffsToEntry(NeighborBuffData buffSOs)
        // {
        //     string[] entry = new string[10];

        //     // Name and Title
        //     entry[0] = buffSOs.name;

        //     // Direction
        //     entry[1] = buffSOs.neighborBuff.direction.ToString();

        //     // Range
        //     entry[2] = buffSOs.neighborBuff.range.ToString();

        //     // Sprite Path
        //     entry[3] = AssetDatabase.GetAssetPath(buffSOs.neighborBuff.buff.sprite);

        //     // Buff Name
        //     entry[4] = buffSOs.neighborBuff.buff.name;

        //     // Stat to Effect
        //     entry[5] = buffSOs.neighborBuff.buff.statToEffect;

        //     // Amount
        //     entry[6] = buffSOs.neighborBuff.buff.amount.ToString();

        //     // Giving Description
        //     entry[7] = $"{CSV_UserData.STRING_DELIMITER}{buffSOs.neighborBuff.buff.givingDescription.Replace("\"", "”")}{CSV_UserData.STRING_DELIMITER}";

        //     // Applied Description
        //     entry[8] = $"{CSV_UserData.STRING_DELIMITER}{buffSOs.neighborBuff.buff.appliedDescription.Replace("\"", "”")}{CSV_UserData.STRING_DELIMITER}";

        //     // Row Delimiter
        //     entry[9] = "$";

        //     return string.Join(CSV_UserData.COLUMN_DELIMITER, entry);
        // }

        // #endregion

        // #region Stations

        // [MenuItem("Utilities/CSV/Stations/Import")]
        // public static void ImportStations()
        // {
        //     CSV_Editor.Import(Application.dataPath + CSV_UserData.STATIONS_CSV_PATH_UNTAGGED, Application.dataPath + CSV_UserData.STATIONS_CSV_PATH_TAGGED, CSV_UserData.STATIONS_TAG_INDEXS, EntryToStations);
        // }

        // [MenuItem("Utilities/CSV/Stations/Export")]
        // public static void ExportStations()
        // {
        //     CSV_Editor.Export<StationData>(Application.dataPath + CSV_UserData.STATIONS_CSV_PATH_UNTAGGED, Application.dataPath + CSV_UserData.STATIONS_CSV_PATH_TAGGED, CSV_UserData.STATIONS_SO_PATH, CSV_UserData.STATIONS_TAG_INDEXS, StationsToEntry);
        // }
        
        // public static void EntryToStations(List<string> entry)
        // {
        //     string assetName = entry[0];
        //     string assetPath = $"{CSV_UserData.STATIONS_SO_PATH}{assetName}.asset";

        //     Debug.Log($"Processing Station {assetName}");

        //     StationData stationSO = CSV_SOHelpers.LoadOrCreateScriptableObjectInstance<StationData>(assetPath, assetName);

        //     // Parsing
        //     stationSO.name = assetName;

        //     // Sprites
        //     if (AssetDatabase.AssetPathExists(entry[1])) stationSO.sprite = (Sprite)AssetDatabase.LoadAssetAtPath(entry[1], typeof(Sprite));

        //     // Summary
        //     stationSO.summary = entry[2].Trim(CSV_UserData.STRING_DELIMITER);
        //     stationSO.summary = stationSO.summary.Replace("”", "\"");

        //     // Purchase Cost
        //     if (AssetDatabase.AssetPathExists($"Assets/Resources/ScriptableObjects/ResourceSOs/{entry[3]}.asset"))
        //     {
        //         stationSO.purchaseInfo.resourceSO = (ResourceSO)AssetDatabase.LoadAssetAtPath($"Assets/Resources/ScriptableObjects/ResourceSOs/{entry[3]}.asset", typeof(ResourceSO));
        //         stationSO.purchaseInfo.amount = int.Parse(entry[4]);
        //     }

        //     try
        //     {
        //         // Sell Cost
        //         stationSO.canSell = bool.Parse(entry[5]);
        //     }
        //     catch (System.Exception)
        //     {
        //         Debug.Log($"Tried parsing Entry[5] = {entry[5]} as a bool but it failed");
        //         throw;
        //     }

            
        //     if (AssetDatabase.AssetPathExists($"Assets/Resources/ScriptableObjects/ResourceSOs/{entry[6]}.asset"))
        //     {
        //         stationSO.sellInfo.resourceSO = (ResourceSO)AssetDatabase.LoadAssetAtPath($"Assets/Resources/ScriptableObjects/ResourceSOs/{entry[6]}.asset", typeof(ResourceSO));
        //         stationSO.sellInfo.amount = int.Parse(entry[7]);
        //     }

        //     try
        //     {
        //         // Repair Cost
        //         stationSO.canSell = bool.Parse(entry[8]);
        //     }
        //     catch (System.Exception)
        //     {
        //         Debug.Log($"Tried parsing Entry[8] = {entry[8]} as a bool but it failed");
        //         throw;
        //     }

        //     if (AssetDatabase.AssetPathExists($"Assets/Resources/ScriptableObjects/ResourceSOs/{entry[9]}.asset"))
        //     {
        //         stationSO.repairInfo.resourceSO = (ResourceSO)AssetDatabase.LoadAssetAtPath($"Assets/Resources/ScriptableObjects/ResourceSOs/{entry[9]}.asset", typeof(ResourceSO));
        //         stationSO.repairInfo.amount = int.Parse(entry[10]);
        //     }

        //     // Stats
        //     stationSO.stats.stationType = entry[11];
        //     stationSO.stats.maxHealth = int.Parse(entry[12]);
        //     stationSO.stats.baseWarmth = int.Parse(entry[13]);
        //     stationSO.stats.maxWorkers = int.Parse(entry[14]);
        //     stationSO.stats.minWorkers = int.Parse(entry[15]);
        //     stationSO.stats.maxEngineers = int.Parse(entry[16]);
        //     stationSO.stats.minEngineers = int.Parse(entry[17]);

        //     CSV_SOHelpers.CreateNewScriptableObjectIfAssetDoesntExist<StationData>(stationSO, assetPath);
        // }

        // public static string StationsToEntry(StationData stationSOs)
        // {
        //     string[] entry = new string[19];

        //     // Name
        //     entry[0] = stationSOs.name;

        //     // Sprites
        //     if (stationSOs.sprite != null) entry[1] = AssetDatabase.GetAssetPath(stationSOs.sprite);

        //     // Summary
        //     entry[2] = $"{CSV_UserData.STRING_DELIMITER}{stationSOs.summary.Replace("\"", "”")}{CSV_UserData.STRING_DELIMITER}";

        //     // Purchase Cost
        //     if(stationSOs.purchaseInfo.resourceSO != null)
        //     {
        //         entry[3] = stationSOs.purchaseInfo.resourceSO.name;
        //         entry[4] = stationSOs.purchaseInfo.amount.ToString();
        //     }

        //     // Sell Cost
        //     if(stationSOs.sellInfo.resourceSO != null && stationSOs.canSell)
        //     {
        //         entry[5] = stationSOs.canSell.ToString();
        //         entry[6] = stationSOs.sellInfo.resourceSO.name;
        //         entry[7] = stationSOs.sellInfo.amount.ToString();
        //     }

        //     // Repair Cost
        //     if (stationSOs.repairInfo.resourceSO != null && stationSOs.canRepair)
        //     {
        //         entry[8] = stationSOs.canRepair.ToString();
        //         entry[9] = stationSOs.repairInfo.resourceSO.name;
        //         entry[10] = stationSOs.repairInfo.amount.ToString();
        //     }

        //     // Stats
        //     entry[11] = stationSOs.stats.stationType;
        //     entry[12] = stationSOs.stats.maxHealth.ToString();
        //     entry[13] = stationSOs.stats.baseWarmth.ToString();
        //     entry[14] = stationSOs.stats.maxWorkers.ToString();
        //     entry[15] = stationSOs.stats.minWorkers.ToString();
        //     entry[16] = stationSOs.stats.maxEngineers.ToString();
        //     entry[17] = stationSOs.stats.minEngineers.ToString();

        //     // Row Delimiter
        //     entry[18] = "$";

        //     return string.Join(CSV_UserData.COLUMN_DELIMITER, entry);
        // }

        // #endregion

        // #region Guides
        // [MenuItem("Utilities/CSV/Guides/Import")]
        // public static void ImportGuides()
        // {
        //     CSV_Editor.Import(Application.dataPath + CSV_UserData.GUIDES_CSV_PATH_UNTAGGED, Application.dataPath + CSV_UserData.GUIDES_CSV_PATH_TAGGED, CSV_UserData.GUIDES_TAG_INDEXS, EntryToGuides);
        // }

        // [MenuItem("Utilities/CSV/Guides/Export")]
        // public static void ExportGuides()
        // {
        //     CSV_Editor.Export<GuideData>(Application.dataPath + CSV_UserData.GUIDES_CSV_PATH_UNTAGGED, Application.dataPath + CSV_UserData.GUIDES_CSV_PATH_TAGGED, CSV_UserData.GUIDES_SO_PATH, CSV_UserData.GUIDES_TAG_INDEXS, GuidesToEntry);
        // }
        
        // public static void EntryToGuides(List<string> entry)
        // {
        //     string assetName = entry[0];
        //     string assetPath = $"{CSV_UserData.GUIDES_SO_PATH}{assetName}.asset";

        //     Debug.Log($"Processing Guild {assetName}");

        //     GuideData guildSO = CSV_SOHelpers.LoadOrCreateScriptableObjectInstance<GuideData>(assetPath, assetName);

        //     // Parsing

        //     // File Name
        //     guildSO.name = assetName;

        //     // Title
        //     guildSO.title = entry[1];

        //     // Sprite
        //     if (AssetDatabase.AssetPathExists(entry[2])) guildSO.sprite = (Sprite)AssetDatabase.LoadAssetAtPath(entry[2], typeof(Sprite));

            

        //     // Text
        //     guildSO.text = entry[3].Trim(CSV_UserData.STRING_DELIMITER);
        //     guildSO.text = guildSO.text.Replace("”", "\"");

        //     CSV_SOHelpers.CreateNewScriptableObjectIfAssetDoesntExist<GuideData>(guildSO, assetPath);
        // }

        // public static string GuidesToEntry(GuideData guildSOs)
        // {
        //     string[] entry = new string[5];

        //     // Name
        //     entry[0] = guildSOs.name;

        //     // Title
        //     entry[1] = guildSOs.title;

        //     // Image
        //     if (guildSOs.sprite != null) entry[2] = AssetDatabase.GetAssetPath(guildSOs.sprite);

        //     // Text
        //     entry[3] = $"{CSV_UserData.STRING_DELIMITER}{guildSOs.text.Replace("\"", "”")}{CSV_UserData.STRING_DELIMITER}";

        //     // Row Delimiter
        //     entry[4] = "$";

        //     return string.Join(CSV_UserData.COLUMN_DELIMITER, entry);
        // }
    
        // #endregion 
    
        #region LevelData
        [MenuItem("Utilities/CSV/LevelData/Import")]
        public static void ImportLevelData()
        {
            string[] csvFiles = Directory.GetFiles(Application.dataPath + CSV_UserData.LEVELDATA_CSV_DIR, "*.csv", SearchOption.TopDirectoryOnly);
            
            foreach (string csvFile in csvFiles)
            { 
                CSV_Editor.ImportAllEntriesAtOnce(csvFile, AllEntriesToLevelData);
            }
        }

        [MenuItem("Utilities/CSV/LevelData/Export")]
        public static void ExportLevelData()
        {
            string[] csvFiles = Directory.GetFiles(Application.dataPath + CSV_UserData.LEVELDATA_CSV_DIR, "*.csv", SearchOption.TopDirectoryOnly);
            string[] levelDataFiles = Directory.GetFiles(CSV_UserData.LEVELDATA_SO_DIR, "*.asset", SearchOption.TopDirectoryOnly);
            
            // C:\Users\Digx7\Desktop\Files\Work\Git_Repos\PlayMath_GameJam_2026\Assets\ScriptableObjects\LevelData
            // C:\Users\Digx7\Desktop\Files\Work\Git_Repos\PlayMath_GameJam_2026\Assets\ScriptableObjects\LevelData


            for (int i = 0; i < csvFiles.Length; i++)
            {
                CSV_Editor.ExportObjectAsMultipleEntries<LevelData>(csvFiles[i], levelDataFiles[i], LevelDataToEntry);
            }
            
        }

        public static void AllEntriesToLevelData(List<List<string>> allEntries)
        {
            string assetName = allEntries[0][0];
            string assetPath = $"{CSV_UserData.LEVELDATA_SO_DIR}{assetName}.asset";

            Debug.Log($"Processing Level Data {assetName}");

            LevelData levelDataSO = CSV_SOHelpers.LoadOrCreateScriptableObjectInstance<LevelData>(assetPath, assetName);

            // Parsing
            levelDataSO.name = assetName;

            // Grid =================================
            Debug.Log($"Processing Level Data {assetName} grid");
            int x_Length = allEntries[0].Count - 1;
            // int y_Length = allEntries.Count - 1;
            int y_Length = x_Length;

            List<CoordinateFlagPair> newGrid = new List<CoordinateFlagPair>();

            for (int x = 0; x < x_Length; x++)
            {
                for (int y = 1; y < y_Length + 1; y++)
                {
                    CoordinateFlagPair coordinateFlagPair = new CoordinateFlagPair();
                    coordinateFlagPair.coordinate = new Vector2Int(x,y - 1);
                    coordinateFlagPair.flag = allEntries[y][x];

                    newGrid.Add(coordinateFlagPair);
                }
            }

            // MetaData ================================
            int metaDataStartIndex = y_Length + 2;
            int treasureToFindStartIndex = metaDataStartIndex + 3;
            int treasureRotationsStartIndex = treasureToFindStartIndex + 2;
            int hintsStartIndex = treasureRotationsStartIndex + 2;
            int toolsStartIndex = hintsStartIndex + 2;

            Debug.Log($"Processing Level Data {assetName} metadata\nStarting at line {metaDataStartIndex}");

            GridTypes gridType = (GridTypes)int.Parse(allEntries[metaDataStartIndex][1]);
            Vector2Int origin = new Vector2Int(int.Parse(allEntries[metaDataStartIndex + 1][1]), int.Parse(allEntries[metaDataStartIndex + 1][2]));

            // Treasure To Find ================================
            Debug.Log($"Processing Level Data {assetName} treasure to find\nStarting at line {treasureToFindStartIndex}");
            List<TreasurePiece> treasureToFind = new List<TreasurePiece>();
            for (int i = treasureToFindStartIndex; i < allEntries.Count; i++)
            {
                if (allEntries[i][0] == "--TreasureRotations--")
                {
                    treasureRotationsStartIndex = i + 1;
                    break;
                }

                TreasurePiece treasurePiece = (TreasurePiece)AssetDatabase.LoadAssetAtPath($"{CSV_UserData.TREASUREPIECEDATA_SO_DIR}{allEntries[i][0]}.asset", typeof(TreasurePiece));
                treasureToFind.Add(treasurePiece);
            }

            // Treasure Rotations ================================
            Debug.Log($"Processing Level Data {assetName} treasure rotations\nStarting at line {treasureRotationsStartIndex}");
            List<TreasurePieceRotation> treasureRotations = new List<TreasurePieceRotation>();
            for (int i = treasureRotationsStartIndex; i < allEntries.Count; i++)
            {
                if (allEntries[i][0] == "--Hints--")
                {
                    hintsStartIndex = i + 1;
                    break;
                }

                TreasurePieceRotation treasureRotation = (TreasurePieceRotation)Enum.Parse(typeof(TreasurePieceRotation), allEntries[i][0]);
                treasureRotations.Add(treasureRotation);
            }

            // Hints ======================================
            Debug.Log($"Processing Level Data {assetName} hints\nStarting at line {hintsStartIndex}");
            List<string> hints = new List<string>();

            for (int i = hintsStartIndex; i < allEntries.Count; i++)
            {
                if (allEntries[i][0] == "--Tools--")
                {
                    toolsStartIndex = i + 1;
                    break;
                }

                hints.Add(allEntries[i][0]);
            }

            // Tools ======================================
            Debug.Log($"Processing Level Data {assetName} tools\nStarting at line {toolsStartIndex} out of {allEntries.Count - 1} lines");
            List<CountToolPair> tools = new List<CountToolPair>();

            for (int i = toolsStartIndex; i < allEntries.Count; i++)
            {
                Debug.Log($"Processing Level Data {assetName} toolIndex {i} out of all entries count {allEntries.Count}");
                
                string toolName = allEntries[i][0];
                int toolCount = int.Parse(allEntries[i][1]);

                Tool toolSO = (Tool)AssetDatabase.LoadAssetAtPath($"{CSV_UserData.TOOL_SO_DIR}{toolName}.asset", typeof(Tool));
                CountToolPair countToolPair = new CountToolPair(toolCount, toolSO);
                tools.Add(countToolPair);
            }


            levelDataSO.SetGrid(newGrid, x_Length, y_Length, gridType, origin);
            levelDataSO.treasureToFind = treasureToFind;
            levelDataSO.treasureRotations = treasureRotations;
            levelDataSO.hints = hints;
            levelDataSO.tools = tools;

            CSV_SOHelpers.CreateNewScriptableObjectIfAssetDoesntExist<LevelData>(levelDataSO, assetPath);
        }

        public static string[] LevelDataToEntry(LevelData levelDataSOs)
        {
            int outputLength = levelDataSOs.grid.y_Length + 8 + levelDataSOs.treasureToFind.Count + levelDataSOs.treasureRotations.Count + levelDataSOs.hints.Count + levelDataSOs.tools.Count;
            int gridStartIndex = 0;
            int metaDataStartIndex = levelDataSOs.grid.y_Length + 1;
            int treasureToFindStartIndex = metaDataStartIndex + 6;
            int treasureRotationsStartIndex = treasureToFindStartIndex + levelDataSOs.treasureToFind.Count + 1;
            int hintsStartIndex = treasureRotationsStartIndex + levelDataSOs.treasureRotations.Count + 1;
            int toolsStartIndex = hintsStartIndex + levelDataSOs.hints.Count + 1;

            string[] lines = new string[outputLength];

            // Grid ==================================
            // Header
            string gridHeader = $"{levelDataSOs.name}";
            if(levelDataSOs.grid.x_Length > 2)
            {
                for (int x = 1; x < levelDataSOs.grid.x_Length; x++)
                {
                    gridHeader += $"{CSV_UserData.COLUMN_DELIMITER}---";
                }
            }
            gridHeader += $"{CSV_UserData.COLUMN_DELIMITER}$";
            lines[gridStartIndex] = gridHeader;

            // Data
            for (int y = 0; y < levelDataSOs.grid.y_Length; y++)
            {
                for (int x = 0; x < levelDataSOs.grid.x_Length; x++)
                {
                    lines[gridStartIndex + y + 1] += $"{levelDataSOs.grid.GetFlagofGridSpace(x,y)}{CSV_UserData.COLUMN_DELIMITER}";
                }
                lines[gridStartIndex + y + 1] += "$";
            }

            // MetaData ================================
            // Header
            string metaDataHeader = $"--MetaData--";
            if(levelDataSOs.grid.x_Length > 2)
            {
                for (int x = 1; x < levelDataSOs.grid.x_Length; x++)
                {
                    metaDataHeader += $"{CSV_UserData.COLUMN_DELIMITER}---";
                }
            }
            metaDataHeader += $"{CSV_UserData.COLUMN_DELIMITER}$";
            lines[metaDataStartIndex] = metaDataHeader;

            // Data
            string gridTypeLine = $"Grid_Type";
            if(levelDataSOs.grid.x_Length > 2)
            {
                for (int x = 1; x < levelDataSOs.grid.x_Length; x++)
                {
                    if(x == 1)
                    {
                        gridTypeLine += $"{CSV_UserData.COLUMN_DELIMITER}{(int)levelDataSOs.grid.gridType}";
                    }
                    else
                    {
                        gridTypeLine += $"{CSV_UserData.COLUMN_DELIMITER}---";
                    }
                }
            }
            gridTypeLine += $"{CSV_UserData.COLUMN_DELIMITER}$";
            lines[metaDataStartIndex + 1] = gridTypeLine;

            string originLine = $"Origin";
            if(levelDataSOs.grid.x_Length > 2)
            {
                for (int x = 1; x < levelDataSOs.grid.x_Length; x++)
                {
                    if(x == 1)
                    {
                        originLine += $"{CSV_UserData.COLUMN_DELIMITER}{levelDataSOs.grid.origin.x}";
                    }
                    else if(x == 2)
                    {
                        originLine += $"{CSV_UserData.COLUMN_DELIMITER}{levelDataSOs.grid.origin.y}";
                    }
                    else
                    {
                        originLine += $"{CSV_UserData.COLUMN_DELIMITER}---";
                    }
                }
            }
            originLine += $"{CSV_UserData.COLUMN_DELIMITER}$";
            lines[metaDataStartIndex + 2] = originLine;

            string isLastLevelLine = $"isLastLevel";
            if(levelDataSOs.grid.x_Length > 2)
            {
                for (int x = 1; x < levelDataSOs.grid.x_Length; x++)
                {
                    if(x == 1)
                    {
                        isLastLevelLine += $"{CSV_UserData.COLUMN_DELIMITER}{levelDataSOs.isLastLevel}";
                    }
                    else
                    {
                        isLastLevelLine += $"{CSV_UserData.COLUMN_DELIMITER}---";
                    }
                }
            }
            isLastLevelLine += $"{CSV_UserData.COLUMN_DELIMITER}$";
            lines[metaDataStartIndex + 3] = isLastLevelLine;

            string isRandomLevelLine = $"isRandomLevel";
            if(levelDataSOs.grid.x_Length > 2)
            {
                for (int x = 1; x < levelDataSOs.grid.x_Length; x++)
                {
                    if(x == 1)
                    {
                        isRandomLevelLine += $"{CSV_UserData.COLUMN_DELIMITER}{levelDataSOs.isRandomLevel}";
                    }
                    else
                    {
                        isRandomLevelLine += $"{CSV_UserData.COLUMN_DELIMITER}---";
                    }
                }
            }
            isRandomLevelLine += $"{CSV_UserData.COLUMN_DELIMITER}$";
            lines[metaDataStartIndex + 4] = isRandomLevelLine;

            string nextLevelLine = $"Next_Level";
            if(levelDataSOs.grid.x_Length > 2)
            {
                for (int x = 1; x < levelDataSOs.grid.x_Length; x++)
                {
                    if(x == 1 && levelDataSOs.nextLevel != null)
                    {
                        nextLevelLine += $"{CSV_UserData.COLUMN_DELIMITER}{AssetDatabase.GetAssetPath(levelDataSOs.nextLevel)}";
                    }
                    else
                    {
                        nextLevelLine += $"{CSV_UserData.COLUMN_DELIMITER}---";
                    }
                }
            }
            nextLevelLine += $"{CSV_UserData.COLUMN_DELIMITER}$";
            lines[metaDataStartIndex + 5] = nextLevelLine;

            // Treasure To Find ================================
            // Header
            string treasureToFindHeader = $"--TreasureToFind--";
            if(levelDataSOs.grid.x_Length > 2)
            {
                for (int x = 1; x < levelDataSOs.grid.x_Length; x++)
                {
                    treasureToFindHeader += $"{CSV_UserData.COLUMN_DELIMITER}---";
                }
            }
            treasureToFindHeader += $"{CSV_UserData.COLUMN_DELIMITER}$";
            lines[treasureToFindStartIndex] = treasureToFindHeader;

            // Data
            for (int i = 0; i < levelDataSOs.treasureToFind.Count; i++)
            {
                string treasureLine = $"{levelDataSOs.treasureToFind[i].name}";
                if(levelDataSOs.grid.x_Length > 2)
                {
                    for (int x = 1; x < levelDataSOs.grid.x_Length; x++)
                    {
                        treasureLine += $"{CSV_UserData.COLUMN_DELIMITER}---";
                    }
                }
                treasureLine += $"{CSV_UserData.COLUMN_DELIMITER}$";
                lines[treasureToFindStartIndex + i + 1] = treasureLine;
            }

            // Treasure Rotations ================================
            // Header
            string treasureRotationsHeader = $"--TreasureRotations--";
            if(levelDataSOs.grid.x_Length > 2)
            {
                for (int x = 1; x < levelDataSOs.grid.x_Length; x++)
                {
                    treasureRotationsHeader += $"{CSV_UserData.COLUMN_DELIMITER}---";
                }
            }
            treasureRotationsHeader += $"{CSV_UserData.COLUMN_DELIMITER}$";
            lines[treasureRotationsStartIndex] = treasureRotationsHeader;

            // Data
            for (int i = 0; i < levelDataSOs.treasureRotations.Count; i++)
            {
                string treasureRotationLine = $"{levelDataSOs.treasureRotations[i]}";
                if(levelDataSOs.grid.x_Length > 2)
                {
                    for (int x = 1; x < levelDataSOs.grid.x_Length; x++)
                    {
                        treasureRotationLine += $"{CSV_UserData.COLUMN_DELIMITER}---";
                    }
                }
                treasureRotationLine += $"{CSV_UserData.COLUMN_DELIMITER}$";
                lines[treasureRotationsStartIndex + i + 1] = treasureRotationLine;
            }


            // Hints ================================
            // Header
            string hintsHeader = $"--Hints--";
            if(levelDataSOs.grid.x_Length > 2)
            {
                for (int x = 1; x < levelDataSOs.grid.x_Length; x++)
                {
                    hintsHeader += $"{CSV_UserData.COLUMN_DELIMITER}---";
                }
            }
            hintsHeader += $"{CSV_UserData.COLUMN_DELIMITER}$";
            lines[hintsStartIndex] = hintsHeader;

            // Data
            for (int i = 0; i < levelDataSOs.hints.Count; i++)
            {
                string hintsLine = $"{levelDataSOs.hints[i]}";
                if(levelDataSOs.grid.x_Length > 2)
                {
                    for (int x = 1; x < levelDataSOs.grid.x_Length; x++)
                    {
                        hintsLine += $"{CSV_UserData.COLUMN_DELIMITER}---";
                    }
                }
                hintsLine += $"{CSV_UserData.COLUMN_DELIMITER}$";
                lines[hintsStartIndex + i + 1] = hintsLine;
            }

            // Tools ================================
            // Header
            string toolsHeader = $"--Tools--";
            if(levelDataSOs.grid.x_Length > 2)
            {
                for (int x = 1; x < levelDataSOs.grid.x_Length; x++)
                {
                    toolsHeader += $"{CSV_UserData.COLUMN_DELIMITER}---";
                }
            }
            toolsHeader += $"{CSV_UserData.COLUMN_DELIMITER}$";
            lines[toolsStartIndex] = toolsHeader;

            // Data
            for (int i = 0; i < levelDataSOs.tools.Count; i++)
            {
                string toolsLine = $"{levelDataSOs.tools[i].tool.name}";
                if(levelDataSOs.grid.x_Length > 2)
                {
                    for (int x = 1; x < levelDataSOs.grid.x_Length; x++)
                    {
                        if(x == 1)
                        {
                            toolsLine += $"{CSV_UserData.COLUMN_DELIMITER}{levelDataSOs.tools[i].count}";
                        }
                        else
                        {
                            toolsLine += $"{CSV_UserData.COLUMN_DELIMITER}---";
                        }
                    }
                }
                toolsLine += $"{CSV_UserData.COLUMN_DELIMITER}$";
                lines[toolsStartIndex + i + 1] = toolsLine;
            }

            return lines;
        }

        #endregion

    }

    public static class CSV_SOHelpers
    {
        /// <summary>
        ///     Checks the given assetPath for a ScriptableObject.<br/>
        ///     If one exists it loads it using AssetDataBase.Load() <br/>
        ///     If one doesn't exist it creates a new instance using ScriptableObject.CreateInstance()
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="assetPath">Relative path to Load the asset from</param>
        /// <param name="assetName">The name of the asset to look for</param>
        /// <returns>ScriptableObject that was created or loaded</returns>
        public static T LoadOrCreateScriptableObjectInstance<T>(string assetPath, string assetName) where T : ScriptableObject
        {
            T obj;

            if (AssetDatabase.AssetPathExists(assetPath))
            {
                obj = (T)AssetDatabase.LoadAssetAtPath(assetPath, typeof(T));
            }
            else
            {
                obj = ScriptableObject.CreateInstance<T>();
            }

            return obj;
        }

        /// <summary>
        ///     Checks if the ScriptableObject at the given asset path exists or not.<br/>
        ///     If it doesn't exits it creates a new asset using AssetDataBase.Create()
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="so"></param>
        /// <param name="assetPath">Relative path to the asset</param>
        public static void CreateNewScriptableObjectIfAssetDoesntExist<T>(T so, string assetPath) where T : ScriptableObject
        {
            if (!AssetDatabase.AssetPathExists(assetPath)) AssetDatabase.CreateAsset(so, assetPath);
        }

        /// <summary>
        /// Loads All ScriptableObjects of the given type from the provided directory path
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dirPath">Relative path to the asset SO Dir</param>
        /// <returns>List of AllScriptableObjects</returns>
        public static List<T> LoadAllScriptableObjectsInDir<T>(string dirPath) where T : ScriptableObject
        {
            string[] files = Directory.GetFiles(dirPath, "*.asset", SearchOption.TopDirectoryOnly);
            List<T> SOs = new List<T>();
            foreach (var file in files)
            {
                T so = (T)AssetDatabase.LoadAssetAtPath(file, typeof(T));
                if (so != null) SOs.Add(so);
            }

            return SOs;
        }

        public static T LoadScriptableObjectAtPath<T>(string path) where T : ScriptableObject
        {
            T so = (T)AssetDatabase.LoadAssetAtPath(path, typeof(T));
            return so;
        }
    }
}


