using DevonLively.Attributes;
using UnityEngine;
using UnityEngine.Serialization;
using static DevonLively.LivelyWebGLLite.LivelyWebGLLitePreset;

namespace DevonLively.LivelyWebGLLite
{
    [CreateAssetMenu(fileName = "Lively WebGL LITE Preset", menuName = "LivelyWebGL LITE/Lively WebGL LITE Preset")]
    public class LivelyWebGLLitePreset : ScriptableObject
    {
        public enum BorderStyles { Solid, Dashed, Dotted, Double, Groove, Ridge, Inset, Outset, None, Hidden }

        public enum DisplayScalingMode { AspectRatio, FreeAspect, FixedResolution }

        public enum ImageBackgroundSizeStyle{ Auto, Contain, Cover }

        public enum ImageBackgroundRepeatStyle{ NoRepeat, Repeat, RepeatX, RepeatY }

        [Tooltip("Basic Page Setup Properties")]
        public WGLSetup_PageSetup pageSetup;

        [Tooltip("Loading screen related properties")]
        public WGLSetup_LoadingScreen loadingScreen;

        private void OnValidate()
        {
            pageSetup.displaySetup.MaxDPR = Mathf.Round(pageSetup.displaySetup.MaxDPR * 4f) / 4f;
            pageSetup.displaySetup.DefaultDPR = Mathf.Round(pageSetup.displaySetup.DefaultDPR * 4f) / 4f;
            loadingScreen.usingBGImage = pageSetup.UseBackgroundImage;
        }
    }
    [System.Serializable]
    public class WGLSetup_PageSetup//7
    {
        [Header("Page Properties"), Tooltip("The title of the web page that will be seen in the browser tab; Also seen in the favorites menu.")]
        public string PageTitle = "Name Seen In Browser Tab";
        [Tooltip("The leftover visible area outside the game canvas when it doesn’t cover the whole screen")]
        public Color PageBackgroundColor = new Color32(25 , 25, 25 , 255);
        [Tooltip("When you press CTRL+S it usually opens the save menu, you can disable this. Why? when crouch is used with ctrl and movement it could cause game-play interruptions.")]
        public bool DisableSaveMenuPopup = true;
        //[Tooltip("When you press CTRL+W it usually closes the open tab; you can disable this. Why? when crouch is used with ctrl and movement it could cause player to close window.")]
        //public bool DisableCtrlWClosesTab = true;
        [Tooltip("when true, the loading screen and tap to start screen will share this background image. Background images (or loading screens) can be found under Template Data")]
        public bool UseBackgroundImage = true;
        [Tooltip("Do you want anything in the corner?")]
        public bool DisableFullscreenButton = false;
        [Tooltip("Does fullscreen hide the fullscreen button?")]
        public bool FullscreenHidesCornerButton = true;

        [HideIfFalse("UseBackgroundImage"), Tooltip("The background image related properties")]
        public BackgroundImageSettings BackgroundImageConfig;

        [System.Serializable]
        public class BackgroundImageSettings//2
        {
            [Tooltip("The background image sizing style")]
            public ImageBackgroundSizeStyle ImageSizeStyle = ImageBackgroundSizeStyle.Cover;
            [Tooltip("The background image repeat style")]
            public ImageBackgroundRepeatStyle ImageRepeatStyle = ImageBackgroundRepeatStyle.NoRepeat;
        }

        [Header("Font Properties"), Tooltip("Google Fonts related properties")]
        public GoogleFonts googleFonts;

        [System.Serializable]
        public class GoogleFonts//2
        {
            [Tooltip("The google font href retrieved from google fonts.")]
            public string FontHref = "https://fonts.googleapis.com/css2?family=Roboto:ital,wght@0,100..900;1,100..900&display=swap";
            [Tooltip("The font style used across the page, which is retrieved from the font href above.")]
            public string FontFamily = "\"Roboto\", sans-serif";
        }

        [Header("Display Properties"), Tooltip("Resolution related properties")]
        public DisplaySetup displaySetup;

        [System.Serializable]
        public class DisplaySetup//3
        {
            [Range(0.5f, 2), Tooltip("The DPR when the browser loads")]// for the user the first time, they can change it in Preformance Options
            public float DefaultDPR = 1;

            [Range(0.5f, 2), Tooltip("1.0\tFor performance-priority games (e.g. mobile)\r\n1.5\tBalanced visuals and performance\r\n2.0\tMax safe clarity on high-DPI monitors\r\n>2.0\tRisky — avoid unless you fully control target hardware")]
            public float MaxDPR = 2;

            [Tooltip("Aspect Ratio Mode: This mode resizes the Unity WebGL canvas to scale within the browser viewport while keeping a fixed aspect ratio (like 16:9 or 4:3).\r\n\r\nFullscreen Mode: This mode resizes the Unity WebGL canvas to fill the entire browser viewport, both visually and internally, without maintaining any aspect ratio. \r\n\r\nFixed Canvas Size: If a user wants a fixed canvas size that matches the input default width and height and doesn't change with the browser window size. Not responsive.")]
            [HideIfTrue("UseDynamicAspectRatio")]
            public DisplayScalingMode DisplayScalingMode = DisplayScalingMode.AspectRatio;

            [ConditionalEnum("DisplayScalingMode", (int)DisplayScalingMode.FixedResolution)]
            [Tooltip("Resolution Set if you want to maintain a fixed resolution, otherwise just leave it default;")]
            [HideIfTrue("UseDynamicAspectRatio")]
            public Vector2Int DefaultResolution = new Vector2Int(1920, 1080);

            [ConditionalEnum("DisplayScalingMode", (int)DisplayScalingMode.AspectRatio)]
            [HideIfTrue("UseDynamicAspectRatio")]
            [Tooltip("The aspect ratio of the default canvas. You can use the aspect ratio calculator in the LivelyWebGL Editor Window if you’re not sure what your aspect ratio is")]
            public Vector2Int AspectRatio = new Vector2Int(16, 9);

            public bool hideExperimentalSettings = true;
            [HideIfTrue("hideExperimentalSettings")]
            [Tooltip("[Experiemental] Let the browser calculate the aspect ratio; If false, you will input the aspect ratio manually. May not work in all situations.")]
            public bool UseDynamicAspectRatio = false;
        }

        [Header("Footer Properties"), Tooltip("Tap to start and Loading screen shared properties")]
        public WGLSetup_LauncherFooter footer;

        [System.Serializable]
        public class WGLSetup_LauncherFooter//12
        {
            [Header("Version, Game Title, and Company")]
            public VersionFooter versionFooter;
            [System.Serializable]
            public class VersionFooter
            {
                [Tooltip("The color of the text that displays the company, version and game name")]
                public Color VersionTextColor = new Color32(230, 230, 230, 255);
                [Tooltip("How large should the font size of the Version Footer be?")]
                public int VersionTextFontSize = 12;
            }
        }
    }

    [System.Serializable]
    public class WGLSetup_LoadingScreen//31
    {
        [Header("General Settings")]
        [ReadOnly]
        public bool usingBGImage = true;

        [HideIfTrue("usingBGImage") ,Tooltip("The loading screens color, if there is no bg image")]
        public Color BackgroundColor = new Color32(25, 25, 25, 255);

        [Header("Loading Bar Logo Properties")]
        public LoadingBarLogo loadingBarLogo;

        [System.Serializable]
        public class LoadingBarLogo
        {
            [Tooltip("Hides the logo image on the loading screen (not the background image)")]
            public bool useLoadingLogoImage = true;

            [Tooltip("The loading bar logo's max width in percent, inside of its container")]
            public int LogoImageMaxWidth = 80;

            [Tooltip("The container that holds the logo and Loading bars y offset in pixels")]
            public int ContainerYOffset = 9;
        }
        
        [Header("Loading Bar Frame Properties"), FormerlySerializedAs("loadingScreenFrameSettings")]
        public LoadingBarFrameSettings FrameSettings;
        [System.Serializable]
        public class LoadingBarFrameSettings
        {
            [Tooltip("The distance between the loading bar and the frame")]
            public int Frame_Padding = 2;
            [Range(0, 90)]
            [Tooltip("The width of the loading bar measured in % where 100% spans across the whole screen."), FormerlySerializedAs("LoadingBarHeight")]
            public int Frame_Width = 50;
            [Tooltip("The height of the loading bar measured in pixels"), FormerlySerializedAs("LoadingBarWidth")]
            public int Frame_Height = 24;
            [Tooltip("The radius of the corners of the loading bar"), FormerlySerializedAs("LoadingBarBorder_Radius")]
            public int Frame_Radius = 5;
            [Tooltip("The thickness of the loading bar border in pixels"), FormerlySerializedAs("LoadingBarBorder_Thickness")]
            public int Frame_Thickness = 1;
            [Tooltip("The style of the loading bar boarder"), FormerlySerializedAs("LoadingBarBorder_Style")]
            public BorderStyles Frame_Style = BorderStyles.Solid;
            [Tooltip("The color of the loading bars border"), FormerlySerializedAs("LoadingBarBorder_Color")]
            public Color Frame_Color = new Color32(191, 191, 191, 255);
            [Tooltip("The color of the inner part / background of the loading bars frame")]
            public Color Frame_BGColor = new Color32(41, 41, 41, 255);
            [Tooltip("How many pixels away is the logo from the loading bar")]
            public int Frame_LogoYOffset = 10;
        }

        [Header("The Loading Bar")]
        public MainLoadingBar loadingBar;
        [System.Serializable]
        public class MainLoadingBar
        {
            [Tooltip("The Color of the inner part of the loading bar"), FormerlySerializedAs("LoadingBarColor")]
            public Color BarColor = new Color32(30, 174, 211, 255);
            [Tooltip("The time (in seconds) it takes for the loading bar to 'animate' to its new width")]
            public float BarTransitionSpeed = 0.3f;

            [Header("Pulse Settings")]
            [Tooltip("Does the loading bar blend between two colors?"), FormerlySerializedAs("LoadingBarPulses")]
            public bool Pulses = true;
            [Tooltip("If the loading bar color pulses, what color does it pulse too?"), FormerlySerializedAs("LoadingBarPulseColor")]
            public Color BarPulseColor = new Color32(30, 62, 211, 255);
        }

        [Header("Loading Bar Text")]
        public LoadingText loadingText;
        [System.Serializable]
        public class LoadingText
        {
            [Header("Finalizing Text")]
            [Tooltip("What does the loading bar text read after 90%?")]
            public string FinalizingPhrase = "Finishing Up...";

            [Header("Text Format")]
            [Tooltip("If displaying the percent value to the player, what font size is the text?")]
            public int FontSize = 12;
            [Range(1, 2), Tooltip("The distance between characters, per line")]
            public float TextLineHeight = 1;    
            [Tooltip("When the game is loading, while its loaded less than 50% what is the color of the loading text?"), FormerlySerializedAs("LoadingBarTextColorLow")]
            public Color TextColorLow = new Color32(191,191,191, 255);
            [Tooltip("When the game is loading, while its loaded more than 50% what is the color of the loading text?"), FormerlySerializedAs("LoadingBarTextColorHigh")]
            public Color TextColorHigh = new Color32(25, 25, 25, 255);
        }
    }
}