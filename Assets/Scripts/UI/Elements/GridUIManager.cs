using UnityEngine;
using UnityEngine.UI;

public class GridUIManager : MonoBehaviour 
{
    public LevelDataChannel OnSetupLevel;
    
    public GameObject gridButtonPrefab;
    public Transform gridButtonHolder;
    public GridLayoutGroup gridLayoutGroup;

    private LevelData levelDataSO;

    private void OnEnable() {
        OnSetupLevel.channelEvent.AddListener(SetupGrid);
    }

    private void OnDisable() {
        OnSetupLevel.channelEvent.RemoveListener(SetupGrid);
    }

    public void SetupGrid(LevelData levelData)
    {
        levelDataSO = levelData;
        gridLayoutGroup.constraintCount = levelDataSO.grid.x_Length;
        
        // for (int i = 0; i < levelDataSO.grid.data.Count; i++)
        // {
        //     GameObject obj = Instantiate(gridButtonPrefab, gridButtonHolder);
        //     GridButtonHelper gridButtonHelper = obj.GetComponentInChildren<GridButtonHelper>();

        //     gridButtonHelper.Coordinate = levelDataSO.grid.data[i].coordinate;
        // }

        for (int y = 0; y < levelDataSO.grid.y_Length; y++)
        {
            for (int x = 0; x < levelDataSO.grid.x_Length; x++)
            {
                GameObject obj = Instantiate(gridButtonPrefab, gridButtonHolder);
                GridButtonHelper gridButtonHelper = obj.GetComponentInChildren<GridButtonHelper>();

                Vector2Int coordintate = new Vector2Int(x,y);
                gridButtonHelper.Coordinate = coordintate;
            }
        }
    }
}