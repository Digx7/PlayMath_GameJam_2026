using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using System.Collections;

public class OverlayPieceElement : MonoBehaviour 
{
    [Header("Runtime Data")]
    [SerializeField]private string _treasureID;
    public string TreasureID 
    { 
        get
        { 
            return _treasureID; 
        } 
        set
        { 
            _treasureID = value; 
            stringChannelListener.dataToListenFor = _treasureID;
        } 
    }

    [Header("References")]
    public TextMeshProUGUI hintText;
    public Image treasureImage;
    public Image rotationImage;
    public StringChannelListener stringChannelListener;
    public Animator animator;
    public string fadeInTriggerName;
    public AudioSource tornadoLabelSFX;

    private bool isRotated = false;



    public void SetIsRotated()
    {
        rotationImage.gameObject.SetActive(true);
        isRotated = true;
    }

    public void StartAnimationDelay(float delay)
    {
        StartCoroutine(FadeInDelay(delay));
    }

    public void PlayTornadoLabelSFX()
    {
        if(isRotated)
        {
            tornadoLabelSFX.Play();
        }
    }

    private IEnumerator FadeInDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        animator.SetTrigger(fadeInTriggerName);
    }
}