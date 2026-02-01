using UnityEngine;

public class LevelDataPrinter : MonoBehaviour {
    public LevelData levelData;

    private void Start() {
        // Debug.Log($"Printing Level {levelData.name}");
        // if(levelData.grid == null)
        // {
        //     Debug.LogWarning($"Grid is UNDEFINED");
        // }
        // Debug.Log($"Size {levelData.grid.Length},{levelData.grid[0].Length}");

        // for (int y = 0; y < levelData.grid[0].Length; y++)
        // {
        //     string row = "";
        //     for (int x = 0; x < levelData.grid.Length; x++)
        //     {
        //         row += levelData.grid[x][y].ToString();
        //         row += ", ";
        //     }
        //     Debug.Log(row);
        // }

        levelData.PrintGrid();
    }
}