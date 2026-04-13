using UnityEngine;

[RequireComponent(typeof(RectTransform))]
public class RectTransformHelper : MonoBehaviour {
    private RectTransform rectTransform;

    private void Awake() {
        rectTransform = GetComponent<RectTransform>();
    }

    public void SetSize(Vector2 size)
    {
        rectTransform.sizeDelta = size; 
    }

    public void SetHeight(float height)
    {
        rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, height); 
    }

    public void SetWidth(float width)
    {
        rectTransform.sizeDelta = new Vector2(width, rectTransform.sizeDelta.y); 
    }
}