using UnityEngine;
using System.Collections.Generic;
using TMPro;

public class TextMeshProColorHelper : ColorHelper 
{
    public List<TextMeshProUGUI> textMeshProElements;
    public Color startingColor;
    private Color lastColor;

    private void Start()
    {
        lastColor = startingColor;
        UpdateColor(startingColor);
    }

    protected override Color GetOGColor()
    {
        return lastColor;
    }

    protected override void UpdateColor(Color newColor)
    {
        for (int i = 0; i < textMeshProElements.Count; i++)
        {
            textMeshProElements[i].color = newColor;
        }
        lastColor = newColor;
    }
}