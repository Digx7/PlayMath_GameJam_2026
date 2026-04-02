using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Digx7.Grids;
using System;

public class GridUIManager : MonoBehaviour 
{
    public LevelDataChannel OnSetupLevel;
    public Channel OnWinLevel;
    
    public GameObject gridButtonPrefab_Coordinate;
    public GameObject gridButtonPrefab_A4;
    public Transform gridButtonHolder;
    public GridLayoutGroup gridLayoutGroup;

    public GameObject gridLabelPrefab;
    public Transform gridLabelTopHolder;
    public Transform gridLabelLeftHolder;

    private LevelData levelDataSO;

    private void OnEnable() {
        OnSetupLevel.channelEvent.AddListener(SetupGrid);
        OnWinLevel.channelEvent.AddListener(OnWin);
    }

    private void OnDisable() {
        OnSetupLevel.channelEvent.RemoveListener(SetupGrid);
        OnWinLevel.channelEvent.RemoveListener(OnWin);
    }

    public void SetupGrid(LevelData levelData)
    {
        ClearGrid();
        
        levelDataSO = levelData;
        gridLayoutGroup.constraintCount = levelDataSO.grid.x_Length;

        for (int y = 0; y < levelDataSO.grid.y_Length; y++)
        {
            if(levelData.grid.gridType == GridTypes.A4)
            {
                GameObject obj_Label_Top = Instantiate(gridLabelPrefab, gridLabelTopHolder);
                // TextMeshProUGUI topLabelTextMeshPro = obj_Label_Top.GetComponentInChildren<TextMeshProUGUI>();
                // topLabelTextMeshPro.text = (y + 1).ToString();

                GridRulerHelper gridRulerHelper_Top = obj_Label_Top.GetComponentInChildren<GridRulerHelper>();
                gridRulerHelper_Top.SetText((y + 1).ToString());
                gridRulerHelper_Top.StartAnimationDelay(y * 0.1f);
            }
            
            for (int x = 0; x < levelDataSO.grid.x_Length; x++)
            {
                GameObject gridButtonPrefabToUse = (levelDataSO.grid.gridType == GridTypes.Coordinate) ? gridButtonPrefab_Coordinate : gridButtonPrefab_A4;

                GameObject obj = Instantiate(gridButtonPrefabToUse, gridButtonHolder);
                GridButtonHelper gridButtonHelper = obj.GetComponentInChildren<GridButtonHelper>();

                Vector2Int coordintate = new Vector2Int(x,y);
                gridButtonHelper.Coordinate = coordintate;
                bool subSpriteFound = levelDataSO.TryGetSpaceSubSprite(out Sprite subSprite, coordintate);
                bool rotationFound = levelDataSO.TryGetSpaceRotation(out TreasurePieceRotation treasurePieceRotation, coordintate);
                if( subSprite && rotationFound)
                {
                    gridButtonHelper.SetTreasureSprite(subSprite, treasurePieceRotation);
                    Debug.Log($"GridUIManager SubSprite added for coordinate {coordintate}");
                }
                else
                {
                    Debug.Log($"GridUIManager NO SubSprite added for coordinate {coordintate}");
                }
                gridButtonHelper.StartAnimationDelay(x * 0.1f + (y * 0.1f));

                if(levelDataSO.grid.gridType == GridTypes.Coordinate)
                {
                    
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
        }

        for (int x = 0; x < levelDataSO.grid.x_Length; x++)
        {
            if(levelDataSO.grid.gridType == GridTypes.A4)
            {
                GameObject obj_Label_Left = Instantiate(gridLabelPrefab, gridLabelLeftHolder);
                TextMeshProUGUI leftLabelTextMeshPro = obj_Label_Left.GetComponentInChildren<TextMeshProUGUI>();
                leftLabelTextMeshPro.text = Convert.ToChar(x + 65).ToString();

                GridRulerHelper gridRulerHelper_Left = obj_Label_Left.GetComponentInChildren<GridRulerHelper>();
                gridRulerHelper_Left.SetText(Convert.ToChar(x + 65).ToString());
                gridRulerHelper_Left.StartAnimationDelay(levelDataSO.grid.x_Length * 0.1f + (x * 0.1f));
            }
        }
    }

    public void OnWin()
    {
        foreach (Transform child in gridButtonHolder)
        {
            if(child.gameObject.TryGetComponent<GridButtonHelper>(out GridButtonHelper gridButtonHelper))
            {
                Vector2Int location = GridUtils.IndexToCoordinate(child.GetSiblingIndex(), levelDataSO.grid.x_Length, levelDataSO.grid.y_Length);
                
                gridButtonHelper.WinAnimationDelay(location.x * 0.1f + (location.y * 0.1f));
            }
        }

        if (levelDataSO.grid.gridType == GridTypes.A4)
        {
            foreach (Transform child in gridLabelLeftHolder)
            {
                if(child.gameObject.TryGetComponent<GridRulerHelper>(out GridRulerHelper gridRulerHelper))
                {
                    gridRulerHelper.WinAnimationDelay((child.GetSiblingIndex() * 0.1f) * 2f);
                }
            }

            foreach (Transform child in gridLabelTopHolder)
            {
                if(child.gameObject.TryGetComponent<GridRulerHelper>(out GridRulerHelper gridRulerHelper))
                {
                    gridRulerHelper.WinAnimationDelay((child.GetSiblingIndex() * 0.1f) * 2f);
                }
            }
        }

    }

    private void ClearGrid()
    {
        foreach (Transform child in gridButtonHolder)
        {
            Destroy(child.gameObject);
        }

        foreach (Transform child in gridLabelTopHolder)
        {
            Destroy(child.gameObject);
        }

        foreach (Transform child in gridLabelLeftHolder)
        {
            Destroy(child.gameObject);
        }
    }
}