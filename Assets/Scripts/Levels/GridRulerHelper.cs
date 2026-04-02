using UnityEngine;
using UnityEngine.Events;
using TMPro;
using System;
using System.Collections;

public class GridRulerHelper : MonoBehaviour 
{
    public TextMeshProUGUI textMeshProUGUI;
    public void SetText(string text)
    {
        textMeshProUGUI.text = text;
    }
    public Animator animator;
    public string fadeInTriggerName;
    public string winTriggerName;
    
    public void StartAnimationDelay(float delay)
    {
        StartCoroutine(AnimationTriggerDelay(fadeInTriggerName, delay));
    }

    public void WinAnimationDelay(float delay)
    {
        StartCoroutine(AnimationTriggerDelay(winTriggerName, delay));
    }

    private IEnumerator AnimationTriggerDelay(string triggerName, float delay)
    {
        yield return new WaitForSeconds(delay);
        animator.SetTrigger(triggerName);
    }
}