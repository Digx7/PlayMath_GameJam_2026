using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Digx7.Grids;
using System;

public class GridUIManager : MonoBehaviour 
{
    public LevelDataChannel OnSetupLevel;
    
    public GameObject gridButtonPrefab;
    public Transform gridButtonHolder;
    public GridLayoutGroup gridLayoutGroup;

    public GameObject gridLabelPrefab;
    public Transform gridLabelTopHolder;
    public Transform gridLabelLeftHolder;

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

        for (int y = 0; y < levelDataSO.grid.y_Length; y++)
        {
            // GameObject obj_Label_Left = Instantiate(gridLabelPrefab, gridLabelLeftHolder);
            // TextMeshProUGUI leftLabelTextMeshPro = obj_Label_Left.GetComponentInChildren<TextMeshProUGUI>();
            // leftLabelTextMeshPro.text = (y + 1).ToString();
            
            for (int x = 0; x < levelDataSO.grid.x_Length; x++)
            {
                GameObject obj = Instantiate(gridButtonPrefab, gridButtonHolder);
                GridButtonHelper gridButtonHelper = obj.GetComponentInChildren<GridButtonHelper>();

                Vector2Int coordintate = new Vector2Int(x,y);
                gridButtonHelper.Coordinate = coordintate;

                Vector2Int gameCoordinate = GridUtils.SpreadSheetCoordinateToGameCoordinate(coordintate, levelDataSO.grid.origin);

                if (gameCoordinate.x == 0 && gameCoordinate.y == 0)
                {
                    // Origin
                    gridButtonHelper.graphNumberTMPro.text = "0";
                    gridButtonHelper.verticalAxis.SetActive(true);
                    gridButtonHelper.horizontalAxis.SetActive(true);
                }
                else if (gameCoordinate.y == 0)
                {
                    // X axis
                    gridButtonHelper.graphNumberTMPro.text = gameCoordinate.x.ToString();
                    gridButtonHelper.horizontalAxis.SetActive(true);
                }
                else if (gameCoordinate.x == 0)
                {
                    // Y axis
                    gridButtonHelper.graphNumberTMPro.text = gameCoordinate.y.ToString();
                    gridButtonHelper.verticalAxis.SetActive(true);
                }
            }
        }

        for (int x = 0; x < levelDataSO.grid.x_Length; x++)
        {
            // GameObject obj_Label_Top = Instantiate(gridLabelPrefab, gridLabelTopHolder);
            // TextMeshProUGUI topLabelTextMeshPro = obj_Label_Top.GetComponentInChildren<TextMeshProUGUI>();
            // topLabelTextMeshPro.text = Convert.ToChar(x + 65).ToString();
        }
    }
}