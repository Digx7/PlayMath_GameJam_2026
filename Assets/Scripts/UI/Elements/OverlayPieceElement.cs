using UnityEngine;
using UnityEngine.UI;
using TMPro;

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



    public void SetIsRotated()
    {
        rotationImage.gameObject.SetActive(true);
    }
}