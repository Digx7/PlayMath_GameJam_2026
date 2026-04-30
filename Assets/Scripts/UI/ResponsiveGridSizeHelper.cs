using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

[RequireComponent(typeof(RectTransform))]
public class ResponsiveGridSizeHelper : MonoBehaviour {
    public List<GridLayoutGroup> gridLayoutGroups;
    public int maxRowCount = 5;
    
    private RectTransform rectTransform;

    private void Awake() {
        rectTransform = GetComponent<RectTransform>();
    }

    private void OnRectTransformDimensionsChange() {
        UpdateGridCellSize();
    }

    public void UpdateGridCellSize() {
        foreach (var gridLayoutGroup in gridLayoutGroups) {
            if (gridLayoutGroup != null) {
                // Update the cell size based on the RectTransform's size
                int sizeWidth = Mathf.CeilToInt(rectTransform.rect.width / maxRowCount);
                int sizeHeight = Mathf.CeilToInt(rectTransform.rect.height / maxRowCount);
    
                int size = Mathf.Min(sizeWidth, sizeHeight);

                gridLayoutGroup.cellSize = new Vector2(size, size);
            }
        }
    }
}