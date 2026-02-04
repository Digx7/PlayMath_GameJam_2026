using UnityEngine;
using TMPro;
using System;
using System.Collections.Generic;

public class LevelSelectionUIWidget : UIMenu
{
    [SerializeField] UIWidgetData mainMenuWidgetData;
    
    [Header("OutGoing Channels")]
    [SerializeField] UIWidgetDataChannel requestLoadUIWidgetChannel;
    [SerializeField] UIWidgetDataChannel requestUnLoadUIWidgetChannel;

    [Header("References")]
    public GameObject levelButtonPrefab;
    public Transform levelButtonHolder;
    public List<LevelData> levels;

    public override void Setup(UIWidgetData newUIWidgetData)
    {
        base.Setup(newUIWidgetData);
        SetupLevels();
    }

    public override void Teardown()
    {
        base.Teardown();
    }

    public void OnClickBack()
    {
        requestLoadUIWidgetChannel.Raise(mainMenuWidgetData);
        requestUnLoadUIWidgetChannel.Raise(ownUIWidgetData);
    }

    public void SetupLevels()
    {
        for (int i = 0; i < levels.Count; i++)
        {
            GameObject obj = Instantiate(levelButtonPrefab, levelButtonHolder);
            LevelUIButtonHelper levelUIButtonHelper = obj.GetComponent<LevelUIButtonHelper>();
            levelUIButtonHelper.Setup(levels[i]);
        }
    }
}
