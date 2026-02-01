using UnityEngine;
using UnityEngine.Events;

public class GridButtonHelper : MonoBehaviour {
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

    public BooleanEvent onFoundTreasure;

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
        if(digData.coordinate == coordinate)
        {
            switch (digData.result)
            {
                case DigResult.FOUND_NEW_TREASURE:
                    onFoundTreasure.Invoke(true);
                    break;
                case DigResult.FOUND_OLD_TREASURE:
                    onFoundTreasure.Invoke(true);
                    break;
                case DigResult.FOUND_NEW_EMPTY:
                    onFoundTreasure.Invoke(false);
                    break;
                case DigResult.FOUND_OLD_EMPTY:
                    onFoundTreasure.Invoke(false);
                    break;
                default:
                    break;
            }
        }
    }
}