using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GridOverlayManager : MonoBehaviour {
    public LevelDataChannel OnSetupLevel;
    
    public GameObject overlayPrefab;
    public Transform overlayHolder;

    private LevelData levelDataSO;

    private void OnEnable() {
        OnSetupLevel.channelEvent.AddListener(SetupGrid);
    }

    private void OnDisable() {
        OnSetupLevel.channelEvent.RemoveListener(SetupGrid);
    }

    public void SetupGrid(LevelData levelData)
    {
        ClearOverlays();
        levelDataSO = levelData;


        for (int i = 0; i < levelDataSO.treasureToFind.Count; i++)
        {
            GameObject obj = Instantiate(overlayPrefab, overlayHolder);
            OverlayPieceElement overlayPieceElement = obj.GetComponent<OverlayPieceElement>();

            overlayPieceElement.TreasureID = levelDataSO.treasureToFind[i].ID;
            overlayPieceElement.treasureImage.sprite = levelDataSO.treasureToFind[i].mainSprite;
            if (levelDataSO.treasureRotations[i] != TreasurePieceRotation.None)
            {
                overlayPieceElement.SetIsRotated();
            }
            overlayPieceElement.hintText.text = levelDataSO.hints[i];

            overlayPieceElement.StartAnimationDelay(1f + (i * 0.1f));
        }
    }

    private void ClearOverlays()
    {
        foreach (Transform child in overlayHolder)
        {
            Destroy(child.gameObject);
        }
    }

}