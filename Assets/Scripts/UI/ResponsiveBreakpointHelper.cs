using UnityEngine;
using UnityEngine.Events;
using Digx7.Zygote;
using System.Collections.Generic;

public class ResponsiveBreakpointHelper : MonoBehaviour 
{
    public List<ScreenBreakPoint> breakPoints;
    public bool updateInEditMode = false;

    private void OnRectTransformDimensionsChange() 
    {
        if(Application.isPlaying || updateInEditMode)
        {
            UpdateUI();
        }
    }

    public void UpdateUI() 
    {   
        ScreenInfo currentScreenInfo = new ScreenInfo { width = Screen.width, height = Screen.height };

        foreach (var breakPoint in breakPoints) 
        {
            if (IsWithinBreakPoint(currentScreenInfo, breakPoint)) 
            {
                ApplyBreakPoint(breakPoint);
                break; // Exit after applying the first matching breakpoint
            }
        }
    }

    public void ApplyBreakPoint(ScreenBreakPoint breakPoint)
    {   
        breakPoint.onBreakPointApplied?.Invoke();
    }

    private bool IsWithinBreakPoint(ScreenInfo screenInfo, ScreenBreakPoint breakPoint) 
    {
        
        if (screenInfo.width < breakPoint.minScreenWidth || screenInfo.width > breakPoint.maxScreenWidth)
        {
            return false;
        }
        else
        {
            return true;
        }
    }
}