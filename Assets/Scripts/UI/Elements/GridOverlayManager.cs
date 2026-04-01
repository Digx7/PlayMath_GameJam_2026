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
            StringChannelListener stringChannelListener = obj.GetComponentInChildren<StringChannelListener>();

            stringChannelListener.dataToListenFor = levelDataSO.treasureToFind[i].ID;

            TextMeshProUGUI hintText = obj.GetComponentInChildren<TextMeshProUGUI>();
            Image image = obj.GetComponentInChildren<Image>();
            image.sprite = levelDataSO.treasureToFind[i].mainSprite;

            hintText.text = levelDataSO.hints[i];
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