using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;
using Digx7.Grids;
using Digx7.Zygote;
using System;
using System.Collections;
using System.Collections.Generic;

public class GridButtonHelper : MonoBehaviour 
{
    public DigDataChannel onDig;

    [Header("Coorinate")]
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
    [SerializeField] List<TextMeshProUGUI> graphNumberTMPros;
    public void SetGraphNumberText(string text, Quadrant quadrant)
    {
        graphNumberTMPros[(int)quadrant].text = text;
    }

    public void SetCoordinateNumber(Vector2Int coordinate, GridAxisMetadata gridAxisMetadata)
    {
        if(coordinate.x == 0 && coordinate.y == 0)
        {
            // origin

            if(gridAxisMetadata.xAxisOnTop && gridAxisMetadata.yAxisOnLeft)
            {
                // Origin is at the top left
                SetGraphNumberText("0", Quadrant.TopLeft);
            }
            else if(gridAxisMetadata.xAxisOnBottom && gridAxisMetadata.yAxisOnLeft)
            {
                // Origin is at the bottom left
                SetGraphNumberText("0", Quadrant.BottomLeft);
            }
            else if(gridAxisMetadata.xAxisOnTop && gridAxisMetadata.yAxisOnRight)
            {
                // Origin is at the top right
                SetGraphNumberText("0", Quadrant.TopRight);
            }
            else if(gridAxisMetadata.xAxisOnBottom && gridAxisMetadata.yAxisOnRight)
            {
                // Origin is at the bottom right
                SetGraphNumberText("0", Quadrant.BottomRight);
            }
            else if(gridAxisMetadata.xAxisOnTop)
            {
                // Origin is along the top
                SetGraphNumberText("0", Quadrant.TopLeft);
            }
            else if(gridAxisMetadata.xAxisOnBottom)
            {
                // Origin is along the bottom
                SetGraphNumberText("0", Quadrant.BottomLeft);
            }
            else if(gridAxisMetadata.yAxisOnLeft)
            {
                // Origin is along the left
                SetGraphNumberText("0", Quadrant.BottomLeft);
            }
            else if(gridAxisMetadata.yAxisOnRight)
            {
                // Origin is along the right
                SetGraphNumberText("0", Quadrant.BottomRight);
            }
            else if(gridAxisMetadata.originInMiddle)
            {
                // Origin is in the middle somewhere
                SetGraphNumberText("0", Quadrant.BottomLeft);
            }

        }
        else if(coordinate.y == 0)
        {
            // x axis



            if(gridAxisMetadata.xAxisOnTop && gridAxisMetadata.yAxisOnLeft)
            {
                // Origin is at the top left
                SetGraphNumberText(coordinate.x.ToString(), Quadrant.TopLeft);
            }
            else if(gridAxisMetadata.xAxisOnBottom && gridAxisMetadata.yAxisOnLeft)
            {
                // Origin is at the bottom left
                SetGraphNumberText(coordinate.x.ToString(), Quadrant.BottomLeft);
            }
            else if(gridAxisMetadata.xAxisOnTop && gridAxisMetadata.yAxisOnRight)
            {
                // Origin is at the top right
                SetGraphNumberText(coordinate.x.ToString(), Quadrant.TopRight);
            }
            else if(gridAxisMetadata.xAxisOnBottom && gridAxisMetadata.yAxisOnRight)
            {
                // Origin is at the bottom right
                SetGraphNumberText(coordinate.x.ToString(), Quadrant.BottomRight);
            }
            else if(gridAxisMetadata.xAxisOnTop)
            {
                // Origin is along the top
                SetGraphNumberText(coordinate.x.ToString(), Quadrant.TopLeft);
            }
            else if(gridAxisMetadata.xAxisOnBottom)
            {
                // Origin is along the bottom
                SetGraphNumberText(coordinate.x.ToString(), Quadrant.BottomLeft);
            }
            else if(gridAxisMetadata.yAxisOnLeft)
            {
                // Origin is along the left
                SetGraphNumberText(coordinate.x.ToString(), Quadrant.BottomLeft);
            }
            else if(gridAxisMetadata.yAxisOnRight)
            {
                // Origin is along the right
                SetGraphNumberText(coordinate.x.ToString(), Quadrant.BottomRight);
            }
            else if(gridAxisMetadata.originInMiddle)
            {
                // Origin is in the middle somewhere
                SetGraphNumberText(coordinate.x.ToString(), Quadrant.BottomLeft);
            }


        }
        else if(coordinate.x == 0)
        {
            // y axis

            
            if(gridAxisMetadata.xAxisOnTop && gridAxisMetadata.yAxisOnLeft)
            {
                // Origin is at the top left
                SetGraphNumberText(coordinate.y.ToString(), Quadrant.TopLeft);
            }
            else if(gridAxisMetadata.xAxisOnBottom && gridAxisMetadata.yAxisOnLeft)
            {
                // Origin is at the bottom left
                SetGraphNumberText(coordinate.y.ToString(), Quadrant.BottomLeft);
            }
            else if(gridAxisMetadata.xAxisOnTop && gridAxisMetadata.yAxisOnRight)
            {
                // Origin is at the top right
                SetGraphNumberText(coordinate.y.ToString(), Quadrant.TopRight);
            }
            else if(gridAxisMetadata.xAxisOnBottom && gridAxisMetadata.yAxisOnRight)
            {
                // Origin is at the bottom right
                SetGraphNumberText(coordinate.y.ToString(), Quadrant.BottomRight);
            }
            else if(gridAxisMetadata.xAxisOnTop)
            {
                // Origin is along the top
                SetGraphNumberText(coordinate.y.ToString(), Quadrant.TopLeft);
            }
            else if(gridAxisMetadata.xAxisOnBottom)
            {
                // Origin is along the bottom
                SetGraphNumberText(coordinate.y.ToString(), Quadrant.BottomLeft);
            }
            else if(gridAxisMetadata.yAxisOnLeft)
            {
                // Origin is along the left
                SetGraphNumberText(coordinate.y.ToString(), Quadrant.BottomLeft);
            }
            else if(gridAxisMetadata.yAxisOnRight)
            {
                // Origin is along the right
                SetGraphNumberText(coordinate.y.ToString(), Quadrant.BottomRight);
            }
            else if(gridAxisMetadata.originInMiddle)
            {
                // Origin is in the middle somewhere
                SetGraphNumberText(coordinate.y.ToString(), Quadrant.BottomLeft);
            }

        }
    }


    [Header("Images")]
    [SerializeField] Image baseDisplayImage;
    [SerializeField] List<Sprite> baseSprites;
    public void RandomizeBaseSprite()
    {
        baseDisplayImage.sprite = baseSprites[UnityEngine.Random.Range(0,baseSprites.Count)];
    }
    [SerializeField] Image gridDisplayImage;
    [SerializeField] Image axisDisplayImage;
    [SerializeField] List<Sprite> gridSprites;
    [SerializeField] List<Sprite> gridAxisSprites;
    public void SetGridDisplayImage(GridTypes gridType, Vector2Int gameCoordinate)
    {
        switch (gridType)
        {
            case GridTypes.Coordinate:
                gridDisplayImage.sprite = gridSprites[0];
                if(gameCoordinate.x == 0 || gameCoordinate.y == 0)
                {
                    axisDisplayImage.gameObject.SetActive(true);
                    
                    if(gameCoordinate.x == 0 && gameCoordinate.y == 0)
                    {
                        // origin
                        axisDisplayImage.sprite = gridAxisSprites[0];
                    }
                    else if(gameCoordinate.y == 0)
                    {
                        // x axis
                        axisDisplayImage.sprite = gridAxisSprites[1];
                    }
                    else if(gameCoordinate.x == 0)
                    {
                        // y axis
                        axisDisplayImage.sprite = gridAxisSprites[2];
                    }
                }
                else
                {
                    axisDisplayImage.gameObject.SetActive(false);
                }
                break;
            case GridTypes.A4:
                gridDisplayImage.sprite = gridSprites[1];
                axisDisplayImage.gameObject.SetActive(false);
                break;
            default:
                break;
        }
    }

    [SerializeField] bool _isRock;
    [SerializeField] Image rockDisplayImage;
    [SerializeField] List<Sprite> rockDisplaySprites;
    public bool IsRock
    {
        get
        {
            return _isRock;
        }
        set
        {
            if(value is bool)
            {
                _isRock = value;

                rockDisplayImage.gameObject.SetActive(_isRock);
                if(_isRock && rockDisplaySprites.Count > 0)
                {
                    rockDisplayImage.sprite = rockDisplaySprites[UnityEngine.Random.Range(0, rockDisplaySprites.Count)];
                }
                else
                {
                    rockDisplayImage.gameObject.SetActive(false);
                }
            }
        }
    }

    [SerializeField] Transform treasureDisplayImageParent;
    [SerializeField] Image treasureDisplayImage;
    [SerializeField] Image treasureDisplayImage_Cropped;
    [SerializeField] Image treasureDisplayImage_Overflow;
    public void SetTreasureSprite(SubSprite treasureSubSprite, TreasurePieceRotation treasurePieceRotation)
    {
        treasureDisplayImage_Cropped.gameObject.SetActive(true);
        treasureDisplayImage_Cropped.sprite = treasureSubSprite.subSprite_Cropped;

        treasureDisplayImage_Overflow.gameObject.SetActive(true);
        treasureDisplayImage_Overflow.sprite = treasureSubSprite.subSprite_Overflow;

        switch (treasurePieceRotation)
        {
            case TreasurePieceRotation.None:
                break;
            case TreasurePieceRotation.Rotate90:
                treasureDisplayImageParent.Rotate(new Vector3(0,0,90));
                break;
            case TreasurePieceRotation.Rotate180:
                treasureDisplayImageParent.Rotate(new Vector3(0,0,180));
                break;
            case TreasurePieceRotation.Rotate270:
                treasureDisplayImageParent.Rotate(new Vector3(0,0,270));
                break;
            default:
                break;
        }
    }

    [SerializeField] Animator animator;
    [SerializeField] string fadeInTriggerName;
    [SerializeField] string winTriggerName;

    public BooleanEvent onFoundTreasure;
    public UnityEvent onFoundTreasure_Default;

    public BooleanEvent onFoundHazard;
    public UnityEvent onFoundHazard_Default;

    public BooleanEvent onFoundEmpty;
    public UnityEvent onFoundEmpty_Default;

    private void Start()
    {
        RandomizeBaseSprite();
    }

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
                    onFoundEmpty.Invoke(false);
                    onFoundHazard.Invoke(false);
                    onFoundTreasure_Default.Invoke();
                    break;
                case DigResult.FOUND_OLD_TREASURE:
                    onFoundTreasure.Invoke(true);
                    break;
                case DigResult.FOUND_NEW_EMPTY:
                    onFoundTreasure.Invoke(false);
                    onFoundEmpty.Invoke(true);
                    onFoundHazard.Invoke(false);
                    onFoundEmpty_Default.Invoke();
                    break;
                case DigResult.FOUND_OLD_EMPTY:
                    onFoundTreasure.Invoke(false);
                    break;
                default:
                    break;
            }
        }
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