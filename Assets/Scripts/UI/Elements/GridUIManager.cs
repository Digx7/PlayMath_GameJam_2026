using UnityEngine;
using UnityEngine.UI;
using TMPro;
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
            GameObject obj_Label_Left = Instantiate(gridLabelPrefab, gridLabelLeftHolder);
            TextMeshProUGUI leftLabelTextMeshPro = obj_Label_Left.GetComponentInChildren<TextMeshProUGUI>();
            leftLabelTextMeshPro.text = (y + 1).ToString();
            
            for (int x = 0; x < levelDataSO.grid.x_Length; x++)
            {
                GameObject obj = Instantiate(gridButtonPrefab, gridButtonHolder);
                GridButtonHelper gridButtonHelper = obj.GetComponentInChildren<GridButtonHelper>();

                Vector2Int coordintate = new Vector2Int(x,y);
                gridButtonHelper.Coordinate = coordintate;
            }
        }

        for (int x = 0; x < levelDataSO.grid.x_Length; x++)
        {
            GameObject obj_Label_Top = Instantiate(gridLabelPrefab, gridLabelTopHolder);
            TextMeshProUGUI topLabelTextMeshPro = obj_Label_Top.GetComponentInChildren<TextMeshProUGUI>();
            topLabelTextMeshPro.text = Convert.ToChar(x + 65).ToString();
        }
    }
}