using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using TMPro;

public class LevelUIButtonHelper : MonoBehaviour {
    [Header("References")]
    public TextMeshProUGUI label;

    private LevelData m_levelData;

    public void Setup(LevelData levelData)
    {
        m_levelData = levelData;
        label.text = levelData.name;
    }

    public void OnClick()
    {
        if(m_levelData != null) UnityEngine.SceneManagement.SceneManager.LoadScene(m_levelData.name);
    }
}