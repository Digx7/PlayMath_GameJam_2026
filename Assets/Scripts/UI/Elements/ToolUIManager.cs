using UnityEngine;
using UnityEngine.UI;

public class ToolUIManager : MonoBehaviour {
    public LevelDataChannel OnSetupLevel;
    
    public GameObject toolButtonPrefab;
    public Transform toolButtonHolder;

    private LevelData levelDataSO;

    private void OnEnable() {
        OnSetupLevel.channelEvent.AddListener(SetupTools);
    }

    private void OnDisable() {
        OnSetupLevel.channelEvent.RemoveListener(SetupTools);
    }

    public void SetupTools(LevelData levelData)
    {
        ClearGrid();
        levelDataSO = levelData;

        for (int i = 0; i < levelDataSO.tools.Count; i++)
        {
            GameObject obj_tool = Instantiate(toolButtonPrefab, toolButtonHolder);
            ToolUIButtonHelper toolUIButtonHelper = obj_tool.GetComponent<ToolUIButtonHelper>();
            toolUIButtonHelper.Setup(levelDataSO.tools[i]);

            if (i == 0)
            {
                Toggle toggle = obj_tool.GetComponent<Toggle>();
                toggle.isOn = true;
            }
        }

        
    }

    private void ClearGrid()
    {
        foreach (Transform child in toolButtonHolder)
        {
            Destroy(child.gameObject);
        }
    }
}