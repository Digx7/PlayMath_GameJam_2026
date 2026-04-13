using UnityEngine;
using UnityEngine.UI;
using Digx7.Zygote;
using Digx7.Utils;
using System.Collections.Generic;
using System.Collections;
using System.Threading.Tasks;

[RequireComponent(typeof(RectTransform))]
[ExecuteAlways]
public class ResponsiveUIHelper : MonoBehaviour 
{
    public List<UIResponsiveBreakPoint> breakPoints;

    public bool updateInEditMode = false;

    private RectTransform rectTransform;

    private void Awake() 
    {
        rectTransform = GetComponent<RectTransform>();
    }

    private void Start() 
    {
        UpdateUI();
    }

    private void OnRectTransformDimensionsChange() 
    {
        if(Application.isPlaying || updateInEditMode)
        {
            UpdateUI();
        }
    }

    public void UpdateUI() 
    {
        if(rectTransform == null)
        {
            rectTransform = GetComponent<RectTransform>();
            if(rectTransform == null)
            {
                Debug.LogWarning("ResponsiveUIHelper: No RectTransform found.");
                return;
            }
        }
        
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

    public void ApplyBreakPoint(UIResponsiveBreakPoint breakPoint)
    {   
        rectTransform.anchorMin = breakPoint.AnchorMinPoints;
        rectTransform.anchorMax = breakPoint.AnchorMaxPoints;
        breakPoint.onBreakPointApplied?.Invoke();
        if(Application.isPlaying)
        {
            StartCoroutine(DelayAncorZero(0.1f)); // Delay to ensure anchors are applied before centering
        }
        else
        {
            DelayAncorZeroAsync(0.1f); // Delay to ensure anchors are applied before centering
        }
    }

    private bool IsWithinBreakPoint(ScreenInfo screenInfo, UIResponsiveBreakPoint breakPoint) 
    {
        
        if (screenInfo.width < breakPoint.screenBreakPoint.minScreenWidth || screenInfo.width > breakPoint.screenBreakPoint.maxScreenWidth)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    private IEnumerator DelayAncorZero(float delay)
    {
        yield return new WaitForSeconds(delay);
        rectTransform.anchoredPosition = Vector2.zero; // Center the UI element
    }

    private async Task DelayAncorZeroAsync(float delay)
    {
        await Task.Delay((int)(delay * 1000));
        rectTransform.anchoredPosition = Vector2.zero; // Center the UI element
    }

}