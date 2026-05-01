using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LevelStageElement : UIElement 
{
    [SerializeField] TextMeshProUGUI label;
    
    [SerializeField] Transform levelHolder;
    [SerializeField] GameObject levelPrefab;


    public void SetLabel(string text)
    {
        label.text = $"Stage {text}";
    }
    public void AddLevel(LevelData data)
    {
        var level = Instantiate(levelPrefab, levelHolder).GetComponent<LevelUIButtonHelper>();
        level.Setup(data);
    }
}