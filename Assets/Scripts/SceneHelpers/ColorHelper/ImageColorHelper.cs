using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;

public class ImageColorHelper : ColorHelper
{
    public List<Image> images;
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
        for (int i = 0; i < images.Count; i++)
        {
            images[i].color = newColor;
        }
        lastColor = newColor;
    }
}