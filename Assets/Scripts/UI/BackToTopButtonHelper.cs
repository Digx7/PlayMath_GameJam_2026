using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class BackToTopButtonHelper : MonoBehaviour 
{
    public List<GameObject> backToTopElements;

    public void OnScrollValueChanged(float value)
    {
        if (value < 0.9f)
        {
            foreach (var button in backToTopElements)
            {
                button.SetActive(true);
            }
        }
        else
        {
            foreach (var button in backToTopElements)
            {
                button.SetActive(false);
            }
        }
    }
}