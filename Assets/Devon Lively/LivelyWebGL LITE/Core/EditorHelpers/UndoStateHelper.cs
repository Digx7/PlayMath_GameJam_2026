using UnityEngine;

namespace DevonLively.LivelyWebGLLite.Editor
{
    public static class UndoStateHelper
    {
        private static EditorData instance;
        public static EditorData Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = ScriptableObject.CreateInstance<EditorData>();
                    instance.hideFlags = HideFlags.HideAndDontSave;
                }
                return instance;
            }
        }
    }

    public class EditorData : ScriptableObject
    {
        public bool changesMade = false;

        [Tooltip("Toggles help boxes on and off in the editor")]
        public bool showHelp = true;
        public bool showDebugLogs = true;

        public string resolutionInputX = "1920";
        public string resolutionInputY = "1080";

        public string aspectRatioOutput = "16:9";
        public string extractedHref = "https://fonts.googleapis.com/css2?family=Roboto:ital,wght@0,100..900;1,100..900&display=swap";
        public string extractedFontFamily = "Roboto";

        public string fontLinkEmbedInput = @"<link rel=""preconnect"" href=""https://fonts.googleapis.com"">
            <link rel=""preconnect"" href=""https://fonts.gstatic.com"" crossorigin>
            <link href=""https://fonts.googleapis.com/css2?family=Roboto:ital,wght@0,100..900;1,100..900&display=swap"" rel=""stylesheet"">";

        public string fontCssBlockInput = @".roboto-<uniquifier> {
                font-family: ""Roboto"", sans-serif;
                font-optical-sizing: auto;
                font-weight: <weight>;
                font-style: normal;
                font-variation-settings:
                ""wdth"" 100;
            }";
    }
}