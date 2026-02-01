using UnityEngine;

public class LevelDataPrinter : MonoBehaviour {
    public LevelData levelData;

    private void Start() {
        levelData.PrintGrid();
    }
}