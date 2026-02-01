using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class SpriteRendererColorHelper : ColorHelper
{
    public List<SpriteRenderer> spriteRenderers;
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
        for (int i = 0; i < spriteRenderers.Count; i++)
        {
            spriteRenderers[i].color = newColor;
        }
        lastColor = newColor;
    }
}