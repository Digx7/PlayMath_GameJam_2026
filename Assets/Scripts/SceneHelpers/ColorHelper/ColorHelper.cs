using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public abstract class ColorHelper : MonoBehaviour
{
    public List<Color> colors;
    private const float LERPTIME = 0.1f;
    private Color ogColor;
    private Color newColor;

    public void SetColorIndex(int index)
    {
        if (index >= colors.Count) return;
        if (gameObject.activeInHierarchy == false) return;

        ogColor = GetOGColor();
        newColor = colors[index];

        StopAllCoroutines();
        StartCoroutine(ColorFade());
    }

    IEnumerator ColorFade()
    {
        OnSetColorIndex();

        float time = 0f;
        while (time < LERPTIME)
        {
            time += Time.deltaTime;
            float t = time;

            t = t.Remap(0f, LERPTIME, 0f, 1f);
            Color updatingColor = Color.Lerp(ogColor, newColor, t);
            UpdateColor(updatingColor);
            yield return null;
        }
    }

    protected virtual void OnSetColorIndex()
    {

    }

    protected virtual Color GetOGColor()
    {
        return Color.white;
    }

    protected virtual void UpdateColor(Color newColor)
    {
        // Will be overriden by children
    }
}
