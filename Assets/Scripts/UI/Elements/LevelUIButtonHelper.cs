using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using TMPro;

public class LevelUIButtonHelper : MonoBehaviour {
    [Header("References")]
    public TextMeshProUGUI label;
    public LevelDataChannelRaiser levelDataChannelRaiser;

    // private LevelData m_levelData;
    private string m_sceneName;

    public void Setup(LevelData levelData)
    {
        // m_sceneName = levelData.name;
        label.text = levelData.name;

        levelDataChannelRaiser.Data = levelData;
    }

    public void Setup(string name)
    {
        // m_sceneName = name;
        label.text = name;
    }

    public void Setup(string labelText, string sceneName)
    {
        label.text = labelText;
        //  m_sceneName = sceneName;
    }

    public void OnClick()
    {
        // if(m_sceneName != null) UnityEngine.SceneManagement.SceneManager.LoadScene(m_sceneName);
    }
}