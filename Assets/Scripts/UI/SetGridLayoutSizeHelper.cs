using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(GridLayoutGroup))]
public class SetGridLayoutSizeHelper : MonoBehaviour {
    private GridLayoutGroup gridLayoutGroup;

    private void Awake() {
        gridLayoutGroup = GetComponent<GridLayoutGroup>();
    }

    public void SetCellSize(float size) {
        if (gridLayoutGroup != null) {
            gridLayoutGroup.cellSize = new Vector2(size, size);
        }
    }
}