using UnityEngine;
using UnityEngine.Events;
using Digx7.Zygote;
using System.Collections.Generic;

public class ResponsiveBreakpointHelper : MonoBehaviour 
{
    #region Variables ================================
    [Header("Variables")]
    public List<ScreenBreakPoint> breakPoints;
    public List<BreakPoinDataAndUnityEventPair> breakPointsDataAndEventPairs;
    public bool updateInEditMode = false;
    public bool updateOnStart = true;
    #endregion

    #region Setup ================================

    private void Start() 
    {
        if(updateOnStart)
        {
            UpdateUI();
        }
    }

    #endregion

    #region Main Methods ================================

    private void OnRectTransformDimensionsChange() 
    {
        if(Application.isPlaying || updateInEditMode)
        {
            UpdateUI();
        }
    }

    public void UpdateUI() 
    {   
        ScreenInfo currentScreenInfo = new ScreenInfo { width = Screen.width, height = Screen.height, isMobile = GameManager.IsMobileBrowser() };

        foreach (var breakPoint in breakPointsDataAndEventPairs) 
        {
            if (IsWithinBreakPoint(currentScreenInfo, breakPoint)) 
            {
                ApplyBreakPoint(breakPoint);
                break; // Exit after applying the first matching breakpoint
            }
        }
    }

    public void ApplyBreakPoint(BreakPoinDataAndUnityEventPair breakPoint)
    {   
        breakPoint.onBreakPointApplied?.Invoke();
    }

    private bool IsWithinBreakPoint(ScreenInfo screenInfo, BreakPoinDataAndUnityEventPair breakPoint) 
    {
        
        // if (screenInfo.width < breakPoint.screenBreakPointData.minScreenWidth || screenInfo.width > breakPoint.screenBreakPointData.maxScreenWidth)
        // {
        //     return false;
        // }
        // else
        // {
        //     return true;
        // }

        return breakPoint.screenBreakPointData.IsWithinBreakPoint(screenInfo);
    }

    #endregion
}