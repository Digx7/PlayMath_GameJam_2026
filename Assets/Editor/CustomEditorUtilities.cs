using UnityEngine;
using UnityEditor;
using System;

namespace Digx7.Editor
{
    public static class ScreenCaptureUtility
    {
        public static void TakeScreenShot()
        {
#if UNITY_EDITOR

            string filePath_Editor = "Assets/Editor/TestLevelScreenShots/";
            string fileName = $"{filePath_Editor}{LevelManager.Instance.CurrentLevel.name}_{System.DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss")}.png";
            ScreenCapture.CaptureScreenshot(fileName);
#endif
        }
    }
}