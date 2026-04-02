using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;
using System;
using System.Collections;

public class GridButtonHelper : MonoBehaviour 
{
    public DigDataChannel onDig;

    [SerializeField]private Vector2Int coordinate;
    public Vector2Int Coordinate
    {
        get 
        {
            return coordinate;
        } 
        set
        {
            coordinate = value;
            vector2IntChannelRaiser.Data = value;
        }
    }
    public Vector2IntChannelRaiser vector2IntChannelRaiser;
    public TextMeshProUGUI graphNumberTMPro;
    public GameObject verticalAxis;
    public GameObject horizontalAxis;

    public Image treasureDisplayImage;
    public Sprite TreasureDisplaySprite 
    { 
        get 
        {
            return treasureDisplayImage.sprite;
        } 
        set 
        {
            treasureDisplayImage.gameObject.SetActive(true);
            treasureDisplayImage.sprite = value;
        }
    }

    public Animator animator;
    public string fadeInTriggerName;
    public string winTriggerName;

    public BooleanEvent onFoundTreasure;
    public UnityEvent onFoundTreasure_Default;
    public UnityEvent onFoundEmpty_Default;

    private void OnEnable() 
    {
        onDig.channelEvent.AddListener(Recieve_OnDig);
    }

    private void OnDisable() 
    {
        onDig.channelEvent.RemoveListener(Recieve_OnDig);
    }

    public void Recieve_OnDig(DigData digData)
    {
        if(digData.tileData.ContainsKey(coordinate))
        {
            switch (digData.tileData[coordinate].result)
            {
                case DigResult.FOUND_NEW_TREASURE:
                    onFoundTreasure.Invoke(true);
                    onFoundTreasure_Default.Invoke();
                    break;
                case DigResult.FOUND_OLD_TREASURE:
                    onFoundTreasure.Invoke(true);
                    break;
                case DigResult.FOUND_NEW_EMPTY:
                    onFoundTreasure.Invoke(false);
                    onFoundEmpty_Default.Invoke();
                    break;
                case DigResult.FOUND_OLD_EMPTY:
                    onFoundTreasure.Invoke(false);
                    break;
                default:
                    break;
            }
        }
        
        // if(digData.coordinate == coordinate)
        // {
        //     switch (digData.result)
        //     {
        //         case DigResult.FOUND_NEW_TREASURE:
        //             onFoundTreasure.Invoke(true);
        //             onFoundTreasure_Default.Invoke();
        //             break;
        //         case DigResult.FOUND_OLD_TREASURE:
        //             onFoundTreasure.Invoke(true);
        //             break;
        //         case DigResult.FOUND_NEW_EMPTY:
        //             onFoundTreasure.Invoke(false);
        //             onFoundEmpty_Default.Invoke();
        //             break;
        //         case DigResult.FOUND_OLD_EMPTY:
        //             onFoundTreasure.Invoke(false);
        //             break;
        //         default:
        //             break;
        //     }
        // }
    }

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