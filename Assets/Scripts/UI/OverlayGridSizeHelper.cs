using UnityEngine;

public class OverlayGridSizeHelper : MonoBehaviour 
{
    public SetGridLayoutSizeHelper gridLayoutSizeHelper;

    public int numberOfPieces = 3;
    public float lastCellSize = 400f;

    public void SetNumberOfPieces(int pieces) 
    {
        numberOfPieces = pieces;
        RefreshCellSize();
    }

    public void SetCellSize(float size) 
    {
        lastCellSize = size;
        RefreshCellSize();
    }

    private void RefreshCellSize()
    {
        float trueSize = lastCellSize;

        switch (numberOfPieces) 
        {
            case 1:
                trueSize *= 1.5f;
                break;
            case 2:
                trueSize *= 1.25f;
                break;
            case 3:
                trueSize *= 1f;
                break;
            default:
                Debug.LogWarning("Unsupported number of pieces: " + numberOfPieces);
                break;
        }

        if (gridLayoutSizeHelper != null) {
            gridLayoutSizeHelper.SetCellSize(trueSize);
        }
    }
}