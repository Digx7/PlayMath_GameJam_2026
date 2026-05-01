using UnityEngine;
using UnityEngine.UI;

public class LayoutGroupHelper : MonoBehaviour 
{
    public LayoutGroup target;

    public void SetPaddingLeft(int newLeftPadding)
    {
        RectOffset padding = target.padding;
        padding.left = newLeftPadding;
        target.padding = padding;
    }

    public void SetPaddingRight(int newRightPadding)
    {
        RectOffset padding = target.padding;
        padding.right = newRightPadding;
        target.padding = padding;
    }

    public void SetPaddingLeftAndRight(Vector2Int newLeftRightPadding)
    {
        
    }

    public void SetPaddingTop(int newTopPadding)
    {
        RectOffset padding = target.padding;
        padding.top = newTopPadding;
        target.padding = padding;
    }

    public void SetPaddingBottom(int newBottomPadding)
    {
        RectOffset padding = target.padding;
        padding.bottom = newBottomPadding;
        target.padding = padding;
    }
}