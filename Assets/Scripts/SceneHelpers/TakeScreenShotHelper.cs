using UnityEngine;

public class TakeScreenShotHelper : MonoBehaviour {
    public string filePath_Editor = "Assets/Editor/TestLevelScreenShots/";

    private string fileName;

    public void TakeScreenShot() 
    {
#if UNITY_EDITOR

        fileName = $"{filePath_Editor}{LevelManager.Instance.CurrentLevel.name}_{System.DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss")}.png";

#else

        fileName = $"{Application.persistentDataPath}/TreasureHuntScreenShot_{System.DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss")}.png";

#endif
        
        ScreenCapture.CaptureScreenshot(fileName);
    }
}