using UnityEngine;

[RequireComponent(typeof(RectTransform))]
public class RectTransformHelper : MonoBehaviour {
    private RectTransform rectTransform;

    private void Awake() {
        rectTransform = GetComponent<RectTransform>();
    }

    public void SetSize(Vector2 size)
    {
        if(rectTransform == null) return;

        rectTransform.sizeDelta = size; 
    }

    public void SetHeight(float height)
    {
        if(rectTransform == null) return;
        
        rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, height); 
    }

    public void SetWidth(float width)
    {
        if(rectTransform == null) return;
        
        rectTransform.sizeDelta = new Vector2(width, rectTransform.sizeDelta.y); 
    }

    public void SetPivot(Vector2 pivot)
    {
        if(rectTransform == null) return;
        
        rectTransform.pivot = pivot; 
        rectTransform.anchoredPosition = Vector2.zero;
    }

    public void SetPivotX(float pivotX)
    {
        if(rectTransform == null) return;
        
        rectTransform.pivot = new Vector2(pivotX, rectTransform.pivot.y); 
        rectTransform.anchoredPosition = Vector2.zero;
    }

    public void SetPivotY(float pivotY)
    {
        if(rectTransform == null) return;
        
        rectTransform.pivot = new Vector2(rectTransform.pivot.x, pivotY); 
        rectTransform.anchoredPosition = Vector2.zero;
    }
}