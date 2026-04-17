using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Digx7.Grids;
using Digx7.Zygote;
using System;
using System.Collections.Generic;

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

    public Animator animator;
    public string tornadoTriggerName;

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

        if(levelDataSO.IsAnyTreasureRotated())
        {
            animator.SetTrigger(tornadoTriggerName);
        }

        bool axisSet = false;
        bool xAxisOnTop = false;
        bool xAxisOnBottom = false;
        bool yAxisOnLeft = false;
        bool yAxisOnRight = false;
        bool originInMiddle = false;

        GridAxisMetadata gridAxisMetadata = GetGridAxisMetadata();

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
                // GameObject gridButtonPrefabToUse = (levelDataSO.grid.gridType == GridTypes.Coordinate) ? gridButtonPrefab_Coordinate : gridButtonPrefab_A4;

                GameObject obj = Instantiate(gridButtonPrefab_A4, gridButtonHolder);
                GridButtonHelper gridButtonHelper = obj.GetComponentInChildren<GridButtonHelper>();

                // Set Coordinate
                Vector2Int coordintate = new Vector2Int(x,y);
                gridButtonHelper.Coordinate = coordintate;

                // Set Treasure Display Spirte
                bool subSpriteFound = levelDataSO.TryGetSpaceSubSprite(out SubSprite subSprite, coordintate);
                bool rotationFound = levelDataSO.TryGetSpaceRotation(out TreasurePieceRotation treasurePieceRotation, coordintate);
                if( subSpriteFound && rotationFound)
                {
                    gridButtonHelper.SetTreasureSprite(subSprite, treasurePieceRotation);
                    Debug.Log($"GridUIManager SubSprite added for coordinate {coordintate}");
                }
                else
                {
                    Debug.Log($"GridUIManager NO SubSprite added for coordinate {coordintate}");
                }

                // Set Grid
                Vector2Int gameCoordinate = levelDataSO.grid.gridType == GridTypes.Coordinate ? GridUtils.SpreadSheetCoordinateToGameCoordinate(coordintate, levelDataSO.grid.origin) : coordintate;
                gridButtonHelper.SetGridDisplayImage(levelDataSO.grid.gridType, gameCoordinate);
                
                // Adds coordinate axis labels
                if(levelDataSO.grid.gridType == GridTypes.Coordinate)
                {
                    
                    gridButtonHelper.SetCoordinateNumber(gameCoordinate, gridAxisMetadata);
                }
            
            
                gridButtonHelper.StartAnimationDelay(x * 0.1f + (y * 0.1f));
            
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

    private GridAxisMetadata GetGridAxisMetadata()
    {
        GridAxisMetadata output = new GridAxisMetadata();
        output.SetAllToFalse();

        Vector2Int spreadSheetOriginCoordinate = levelDataSO.grid.origin;

        if (spreadSheetOriginCoordinate.x == 0 && spreadSheetOriginCoordinate.y == 0)
        {
            // Origin is at the top left
            output.xAxisOnTop = true;
            output.yAxisOnLeft = true;
        }
        else if (spreadSheetOriginCoordinate.x == 0 && spreadSheetOriginCoordinate.y == levelDataSO.grid.y_Length - 1)
        {
            // Origin is at the bottom left
            output.xAxisOnBottom = true;
            output.yAxisOnLeft = true;
            
        }
        else if (spreadSheetOriginCoordinate.x == levelDataSO.grid.x_Length - 1 && spreadSheetOriginCoordinate.y == 0)
        {
            // Origin is at the top right
            output.xAxisOnTop = true;
            output.yAxisOnRight = true;
            
        }
        else if (spreadSheetOriginCoordinate.x == levelDataSO.grid.x_Length - 1 && spreadSheetOriginCoordinate.y == levelDataSO.grid.y_Length - 1)
        {
            // Origin is at the bottom right
            output.xAxisOnBottom = true;
            output.yAxisOnRight = true;
            
        }
        else if (spreadSheetOriginCoordinate.x == 0)
        {
            // Origin is along the left
            output.yAxisOnLeft = true;
            
        }
        else if (spreadSheetOriginCoordinate.x == levelDataSO.grid.x_Length - 1)
        {
            // Origin is along the right
            output.yAxisOnRight = true;
            
        }
        else if (spreadSheetOriginCoordinate.y == 0)
        {
            // Origin is along the top
            output.xAxisOnTop = true;
            
        }
        else if (spreadSheetOriginCoordinate.y == levelDataSO.grid.y_Length - 1)
        {
            // Origin is along the bottom
            output.xAxisOnBottom = true;
            
        }
        else
        {
            // Origin is in the middle somewhere
            output.originInMiddle = true;
        }

        return output;
    }

}