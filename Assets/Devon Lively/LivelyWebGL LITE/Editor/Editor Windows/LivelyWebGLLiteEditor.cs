using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEngine;

namespace DevonLively.LivelyWebGLLite.Editor
{
    public class LivelyWebGLLiteEditor : EditorWindow
    {   
        public static LivelyWebGLLiteEditor Instance;

        public LivelyWebGLLitePreset livelyWebGLPreset, lastlivelyWebGLPreset;
        // stores the button list // stores the WebGL setup preset
        public SerializedObject serializedTargetSetup;

        public int callbackOrder => 0;

        string templateId = "PROJECT:LivelyWebGL LITE";
        bool CorrectTemplateUsed
        {
            get
            {
                return templateId == "PROJECT:LivelyWebGL LITE";
            }
        }

        [MenuItem("Window/Devon Lively/LivelyWebGL LITE/LivelyWebGL LITE Editor")]
        public static void ShowWindow()
        {
            Instance = GetWindow<LivelyWebGLLiteEditor>("LivelyWebGL LITE Editor");

            Instance.livelyWebGLPreset = EditorPrefs.GetString("LivelyWebGLLiteSetupPreset") != "" ?
                AssetDatabase.LoadAssetAtPath<LivelyWebGLLitePreset>(EditorPrefs.GetString("LivelyWebGLLiteSetupPreset", "")) : null;

            Instance.lastlivelyWebGLPreset = Instance.livelyWebGLPreset;
            UndoStateHelper.Instance.changesMade = EditorPrefs.GetBool("LivelyWTLChangesMade", false);
        }

        private void OnDisable()
        {
            WebGLSetup.headerStyle = null;
        }
        
        void OnEnable() {
            EditorHandlers.ChangePage(0);
            titleContent = new GUIContent("Lively WebGL LITE Editor");
        }

        void OnGUI()
        {
            GUILayout.Space(5);

            EditorHandlers.DrawSDLogo();
            Color defaultBg = GUI.backgroundColor;

            templateId = PlayerSettings.WebGL.template;
            if (templateId == "Project:SDResponsive" || templateId == "Project:LivelyWebGL")
                Debug.LogWarning("You have to switch WebGLTemplates, then back to LivelyWebGL LITE; You've updated from an old version");

            if (string.IsNullOrEmpty(templateId))
                templateId = "(Unity Default)";

            GUILayout.Space(5);
            GUILayout.BeginVertical();
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();

            if (GUILayout.Button("WebGL Setup", GUILayout.Width(115)))
                EditorHandlers.ChangePage(0);
            if (GUILayout.Button("Tools", GUILayout.Width(115)))
                EditorHandlers.ChangePage(1);
            if (GUILayout.Button("Player", GUILayout.Width(115)))
                SettingsService.OpenProjectSettings("Project/Player");

            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
            GUILayout.EndVertical();

            if (EditorHandlers.pageNum[0])
            {
                GUILayout.BeginVertical();
                GUILayout.BeginHorizontal();
                GUILayout.Space(15);
                EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
                GUILayout.Space(15);
                GUILayout.EndHorizontal();
                GUILayout.BeginHorizontal();
                GUILayout.FlexibleSpace();
                if (!CorrectTemplateUsed)
                {
                    Color previousColor = GUI.color;
                    GUI.backgroundColor = new Color(0.15f, 0.15f, 0.15f);
                    GUIStyle paddedButton = new GUIStyle(GUI.skin.button);
                    paddedButton.padding = new RectOffset(10, 10, 8, 8); // L, R, T, B
                    GUI.color = Color.red;
                    if (GUILayout.Button("Click Here & Select LWTL from Resolution and Presentation", paddedButton, GUILayout.Width(375)))
                    {
                        SettingsService.OpenProjectSettings("Project/Player");
                    }
                    GUI.color = previousColor;
                    GUI.backgroundColor = defaultBg;
                    GUILayout.FlexibleSpace();
                    GUILayout.EndHorizontal();
                    GUILayout.EndVertical();
                    return;
                }

                if (livelyWebGLPreset != null)
                {
                    GUIStyle bold = new GUIStyle(GUI.skin.button);
                    bold.fontStyle = FontStyle.Bold;
                    bold.normal.textColor = new Color32(235, 235, 235, 255);

                    if (UndoStateHelper.Instance.changesMade)
                    {
                        GUI.backgroundColor = new Color32(220, 40, 40, 175);
                        GUIStyle paddedButton = new GUIStyle(GUI.skin.button);
                        paddedButton.fontStyle = FontStyle.Bold;
                        paddedButton.normal.textColor = Color.white;
                        paddedButton.padding = new RectOffset(10, 10, 8, 8); // L, R, T, B

                        if (GUILayout.Button("CLICK HERE BEFORE PREVIEW / BUILD TO SAVE CHANGES!", paddedButton, GUILayout.Width(375)))
                        {
                            TemplateProcessor.ProcessTemplate(livelyWebGLPreset);
                            EditorHandlers.UpdateChangesMade(false);
                        }
                        GUI.backgroundColor = defaultBg;
                    }
                    else
                    {
                        GUI.backgroundColor = new Color32(220, 53, 69, 255);
                        if (GUILayout.Button("SAVE", bold, GUILayout.Width(92)))
                        {
                            TemplateProcessor.ProcessTemplate(livelyWebGLPreset);
                            EditorHandlers.UpdateChangesMade(false);
                        }
                        GUILayout.Space(10);
                        GUI.backgroundColor = new Color32(0, 200, 0, 255);
                        if (GUILayout.Button(new GUIContent("BUILD", "Builds the project using the WebGL template currently selected in Player Settings."), bold, GUILayout.Width(92)))
                        {
                            BuildAndPreviewManager.Build();
                        }
                        GUI.backgroundColor = defaultBg;
                    }
                }
                GUILayout.FlexibleSpace();
                GUILayout.EndHorizontal();
                GUILayout.EndVertical();
                GUILayout.Space(10);

                WebGLSetup.ShowWebGLSetup(ref serializedTargetSetup, ref livelyWebGLPreset, ref lastlivelyWebGLPreset);
            }
            else if (EditorHandlers.pageNum[1])
                ToolEditor.ShowToolEditor();

            GUILayout.Space(10);

            // Push everything down
            GUILayout.FlexibleSpace();
            EditorHandlers.ShowFooter();
        }

        public static class EditorHandlers
        {
            public static bool[] pageNum = new bool[3];
            static Texture2D myTexture;

            static string versionCode = "1.0.01";

            public static void HelpBox(string title, string helpText, string url = "")
            {
                if (UndoStateHelper.Instance.showHelp)
                {
                    GUILayout.Space(5);
                    if (!string.IsNullOrEmpty(url))
                    {
                        GUIStyle linkStyle = new GUIStyle(EditorStyles.label);
                        linkStyle.normal.textColor = new Color(0.4f, 0.6f, 1f);
                        linkStyle.hover.textColor = Color.cyan;
                        linkStyle.richText = true;

                        EditorGUILayout.HelpBox(helpText, MessageType.Info);
                        GUILayout.BeginHorizontal();
                        GUILayout.Space(15);
                        if (GUILayout.Button($"<color=#4090FF><u>{title}?</u></color>", linkStyle))
                        {
                            Application.OpenURL(url);
                        }
                        GUILayout.EndHorizontal();
                        //EditorGUILayout.HelpBox("You can turn off help boxes in advanced settings.", MessageType.Info);
                    }
                    else
                        EditorGUILayout.HelpBox(title + "\nYou can turn off help boxes in advanced settings.", MessageType.Info);
                }
            }
            
            static public void DrawCopyableField(string label, string value, float width = 100)
            {
                EditorGUILayout.BeginHorizontal();

                GUIStyle boldGoldStyle = new GUIStyle(EditorStyles.label);
                boldGoldStyle.fontStyle = FontStyle.Bold;
                boldGoldStyle.fontSize = 12;
                boldGoldStyle.normal.textColor = new Color(1f, 0.84f, 0f);

                // Layout row
                EditorGUILayout.BeginHorizontal();

                // Draw just the label with custom style
                EditorGUILayout.LabelField(label, boldGoldStyle, GUILayout.Width(width));

                // Draw the text field with default style
                EditorGUILayout.SelectableLabel(value, EditorStyles.textField, GUILayout.Height(EditorGUIUtility.singleLineHeight));

                // Copy button
                if (GUILayout.Button("Copy", GUILayout.Width(50)))
                {
                    EditorGUIUtility.systemCopyBuffer = value;
                }

                EditorGUILayout.EndHorizontal();
                EditorGUILayout.EndHorizontal();
            }

            public static void UpdateChangesMade(bool _changesMade)
            {
                UndoStateHelper.Instance.changesMade = _changesMade;
                EditorPrefs.SetBool("LivelyLiteChangesMade", UndoStateHelper.Instance.changesMade);
            }

            public static void ChangePage(int pageIndex)
            {
                for (int i = 0; i < pageNum.Length; i++)
                    pageNum[i] = false;
                pageNum[pageIndex] = true;
            }

            public static void DrawSDLogo()
            {
                if (myTexture == null)
                {
                    myTexture = FindTextureByName("DLWTLEGraphic");
                }

                float inspectorWidth = EditorGUIUtility.currentViewWidth;

                // Draw the image at the top
                if (myTexture != null)
                {
                    // Get the texture's original size
                    float originalWidth = myTexture.width;
                    float originalHeight = myTexture.height;

                    // Clamp the width to the smaller of the inspector width minus padding or the texture's original width
                    float maxWidth = Mathf.Min(originalWidth, inspectorWidth - 20);
                    float aspectRatio = originalHeight / originalWidth;

                    // Calculate the height while maintaining the aspect ratio
                    float height = maxWidth * aspectRatio;

                    // Calculate the horizontal offset to center the texture
                    float offsetX = (inspectorWidth - maxWidth) / 2;

                    // Add a vertical space and create a centered Rect
                    Rect rect = new Rect(offsetX, GUILayoutUtility.GetRect(maxWidth, height).y, maxWidth, height);

                    EditorGUI.DrawPreviewTexture(rect, myTexture);
                }

                Texture2D FindTextureByName(string textureName)
                {
                    // Find all assets with the specified name
                    string[] guids = AssetDatabase.FindAssets(textureName + " t:Texture2D");

                    // Ensure a texture was found
                    if (guids.Length > 0)
                    {
                        // Load the first match found
                        string path = AssetDatabase.GUIDToAssetPath(guids[0]);
                        return AssetDatabase.LoadAssetAtPath<Texture2D>(path);
                    }
                    else
                    {
                        Debug.LogWarning($"Texture named '{textureName}' not found.");
                        return null;
                    }
                }
            }

            public static void ShowFooter()
            {
                EditorGUILayout.BeginHorizontal();
                GUILayout.FlexibleSpace();

                if (GUILayout.Button("Need Support?", GUILayout.Width(125)))
                    Application.OpenURL("https://bumblingwizard.com");

                if (GUILayout.Button("Wiki", GUILayout.Width(75)))
                    Application.OpenURL("https://bumblingwizard.com/documentation/livelywebgl/");

                string footer = " V"+ versionCode+" Devon Lively " + System.DateTime.Now.Year.ToString();
                if (GUILayout.Button(footer, GUILayout.Width(175)))
                    Application.OpenURL("https://assetstore.unity.com/publishers/15912");
                GUILayout.FlexibleSpace();
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.BeginHorizontal();
                GUILayout.FlexibleSpace();
                if (GUILayout.Button("Get More Features", GUILayout.Width(175)))
                    Application.OpenURL("https://assetstore.unity.com/packages/tools/integration/lively-webgl-template-314596");
                GUILayout.FlexibleSpace();
                EditorGUILayout.EndHorizontal();
            }
        }

        public static class BuildAndPreviewManager
        {
            static bool showBuildSettings = false;

            private static Vector2 scroll;

            public static bool developmentBuild;
            public static bool autoOpenBuild = true;


            public static void BuildSettings()
            {
                WebGLSetup.ShowHeader(null, "", WebGLSetup.gradient[10]);
                showBuildSettings = EditorGUILayout.Foldout(showBuildSettings, new GUIContent(" Build Settings"), true, WebGLSetup.headerStyle);
                if (showBuildSettings)
                {
                    EditorGUI.indentLevel++;
                    using (new EditorGUILayout.VerticalScope("box"))
                    {
                        EditorGUILayout.LabelField("Options", EditorStyles.boldLabel);
                        developmentBuild = EditorGUILayout.Toggle(new GUIContent("Development Build", "Enable Development build"), developmentBuild);
                        autoOpenBuild = EditorGUILayout.Toggle(new GUIContent("Open Build Folder", "Open the output folder when the build completes"), autoOpenBuild);
                    }
                    EditorHandlers.HelpBox("Build Settings",
                        "Hover your mouse over a field for its definition",
                        "https://bumblingwizard.com/documentation/livelywebgl/doku.php?id=docs:definitions:buildsettings");
                    EditorGUI.indentLevel--;
                }
            }

            public static void Build()
            {
                DateTime startTime = DateTime.Now;

                var scenes = EditorBuildSettings.scenes
                    .Where(s => s.enabled)
                    .Select(s => s.path)
                    .ToArray();

                if (scenes.Length == 0)
                {
                    EditorUtility.DisplayDialog("No Scenes Enabled", "Enable at least one scene in File > Build Settings.", "OK");
                    return;
                }

                string defaultPath = System.IO.Path.Combine("Builds", "WebGL");
                string outDir = EditorUtility.SaveFolderPanel("Choose WebGL Build Folder", defaultPath, "");
                if (string.IsNullOrEmpty(outDir))
                    return;

                // Ensure build target is WebGL before building
                if (EditorUserBuildSettings.activeBuildTarget != BuildTarget.WebGL)
                {
                    if (!EditorUserBuildSettings.SwitchActiveBuildTarget(BuildTargetGroup.WebGL, BuildTarget.WebGL))
                    {
                        EditorUtility.DisplayDialog("Switch Target Failed", "Could not switch active build target to WebGL.", "OK");
                        return;
                    }
                }

                var options = new BuildPlayerOptions
                {
                    scenes = scenes,
                    locationPathName = outDir,
                    target = BuildTarget.WebGL,
                    options = BuildAndPreviewManager.developmentBuild ? BuildOptions.Development | BuildOptions.ConnectWithProfiler : BuildOptions.None
                };

                BuildReport report = null;
                try
                {
                    EditorUtility.DisplayProgressBar("Building WebGL", "Building player...", 0.25f);
                    report = BuildPipeline.BuildPlayer(options);
                }
                finally
                {
                    EditorUtility.ClearProgressBar();
                }

                if (report == null || report.summary.result != BuildResult.Succeeded)
                {
                    Debug.LogError($"WebGL build failed: {report?.summary.result}");
                    EditorUtility.DisplayDialog("Build Failed", $"WebGL build failed: {report?.summary.result}", "OK");
                    return;
                }

                // After build is complete, copy StreamingAssets
                string streamingAssetsPath = Application.streamingAssetsPath;
                string buildStreamingAssetsPath = System.IO.Path.Combine(outDir, "StreamingAssets");

                if (System.IO.Directory.Exists(streamingAssetsPath) &&
                    System.IO.Directory.GetFiles(streamingAssetsPath).Length > 0)
                {
                    try
                    {
                        EditorUtility.DisplayProgressBar("Building WebGL", "Copying StreamingAssets...", 0.9f);

                        if (System.IO.Directory.Exists(buildStreamingAssetsPath))
                        {
                            System.IO.Directory.Delete(buildStreamingAssetsPath, true);
                        }

                        CopyDirectory(streamingAssetsPath, buildStreamingAssetsPath, true);
                        Debug.Log($"StreamingAssets copied successfully to: {buildStreamingAssetsPath}");
                    }
                    catch (System.Exception ex)
                    {
                        Debug.LogError($"Failed to copy StreamingAssets: {ex.Message}");
                        EditorUtility.DisplayDialog("Build Warning", $"StreamingAssets copy failed: {ex.Message}", "OK");
                    }
                }

                int secondsTotal = (int)(DateTime.Now - startTime).TotalSeconds;
                Debug.Log($"WebGL build succeeded. Build Time: '{secondsTotal}' seconds. Output: {report.summary.outputPath}");

                if (BuildAndPreviewManager.autoOpenBuild)
                {
                    EditorUtility.RevealInFinder(report.summary.outputPath);
                }

                EditorUtility.DisplayDialog("Build Succeeded", $"WebGL build completed in '{secondsTotal}' seconds.\nOutput: {report.summary.outputPath}", "OK");

                // REVIEW POPUP
                EditorPrefs.SetInt("LivelyWebGLLite_BuildCount", EditorPrefs.GetInt("LivelyWebGLLite_BuildCount", 0) + 1);
                if(EditorPrefs.GetInt("LivelyWebGLLite_BuildCount", 0) % 3 == 0)
                {
                    LivelyWebGLLiteReviewWindowEditor.ShowWindow();
                }
            }

            // Helper method to copy directories recursively
            static void CopyDirectory(string sourceDir, string destinationDir, bool recursive)
            {
                // Get information about the source directory
                var dir = new System.IO.DirectoryInfo(sourceDir);

                // Check if the source directory exists
                if (!dir.Exists)
                    return;

                // Cache directories before we start copying
                System.IO.DirectoryInfo[] dirs = dir.GetDirectories();

                // Create the destination directory
                System.IO.Directory.CreateDirectory(destinationDir);

                // Get the files in the source directory and copy to the destination directory
                foreach (System.IO.FileInfo file in dir.GetFiles())
                {
                    string targetFilePath = System.IO.Path.Combine(destinationDir, file.Name);
                    file.CopyTo(targetFilePath, true);
                }

                // If recursive and copying subdirectories, recursively call this method
                if (recursive)
                {
                    foreach (System.IO.DirectoryInfo subDir in dirs)
                    {
                        string newDestinationDir = System.IO.Path.Combine(destinationDir, subDir.Name);
                        CopyDirectory(subDir.FullName, newDestinationDir, true);
                    }
                }
            }

        }

        public static class WebGLSetup
        {
            static Vector2 scrollPos = new Vector2();
            public static GUIStyle headerStyle;

            public static Color32[] gradient = new Color32[]
            {
                new Color32(220, 40, 40, 50),     // 0. Page Setup        → red (important base setup)
                new Color32(80, 120, 255, 50),    // 1. Loading Screen    → soft blue (calm waiting tone)
                new Color32(255, 170, 60, 50),    // 2. MiniGames         → orange-gold (fun & energetic)
                new Color32(255, 230, 90, 50),    // 3. Tap To Start      → bright yellow (inviting)
                new Color32(90, 200, 120, 50),    // 4. Hamburger Menu    → mint green (fresh & organized)
                new Color32(120, 220, 255, 50),   // 5. Installer Menu    → light cyan (setup / installation)
                new Color32(190, 110, 255, 50),   // 6. Supabase Menu     → violet (data / backend)
                new Color32(255, 110, 160, 50),   // 7. Advanced          → pink (expert tools)
                new Color32(255, 80, 80, 50),     // 8. Script Injection  → strong red (dangerous / warning)
                new Color32(130, 170, 255, 50),   // 9. Preview Settings  → cool sky blue (testing)
                new Color32(70, 70, 90, 50),       // 10. Build            → dark slate (final / stable)
                new Color32(120, 180, 140, 60)  // muted green-teal, soft background

            };


            public static void ShowHeader(SerializedProperty property, string title, Color color)
            {
                EnsureHeaderStyle();
                Rect headerRect = EditorGUILayout.BeginHorizontal();
                {
                    headerRect.height = 20;

                    // Draw solid background before content
                    EditorGUI.DrawRect(headerRect, color);

                    // Now draw the foldout on top
                    if (property != null)
                        property.isExpanded = EditorGUI.Foldout(headerRect, property.isExpanded, title, true, headerStyle);
                }
                EditorGUILayout.EndHorizontal();
            }

            private static void EnsureHeaderStyle()
            {
                if (headerStyle != null) return;

                Color32 white = new Color32(235, 235, 235, 255);
                Color32 black = new Color32(177, 177, 177, 255);
                headerStyle = new GUIStyle(EditorStyles.foldout)
                {
                    fontStyle = FontStyle.Bold,
                    normal = { textColor =  white},
                    onNormal = { textColor = white },
                    hover = { textColor = white },
                    onHover = { textColor = white },
                    focused = { textColor = black },
                    onFocused = { textColor = black },
                    active = { textColor = white },
                    onActive = { textColor = white }
                };
            }

            static void PropertyExpanded(SerializedProperty property, float space)
            {
                // Draw all child properties of 'pageSetup', without the built-in header
                SerializedProperty copy = property.Copy();
                SerializedProperty endProp = copy.GetEndProperty();

                copy.NextVisible(true); // move to first child
                GUILayout.Space(space);
                while (!SerializedProperty.EqualContents(copy, endProp))
                {
                    EditorGUILayout.PropertyField(copy, true);
                    copy.NextVisible(false);
                }
            }

            public static void ShowWebGLSetup(
                ref SerializedObject serializedTargetSetup,
                ref LivelyWebGLLitePreset livelyWebGLPreset,
                ref LivelyWebGLLitePreset lastlivelyWebGLPreset
            )
            {
                scrollPos = EditorGUILayout.BeginScrollView(scrollPos);

                GUILayout.BeginVertical("HelpBox");
                WebGLSetup.ShowHeader(null, "", WebGLSetup.gradient[10]);
                GUILayout.BeginHorizontal();
                GUILayout.Space(5);
                GUIStyle specialHeader = new GUIStyle();
                specialHeader.fontStyle = FontStyle.Bold;
                specialHeader.normal.textColor = Color.white;
                specialHeader.onNormal.textColor = Color.white;

                EditorGUILayout.LabelField(" Lively WebGL LITE Preset:", specialHeader, GUILayout.Width(145));
                livelyWebGLPreset = (LivelyWebGLLitePreset)EditorGUILayout.ObjectField(
                    livelyWebGLPreset,
                    typeof(LivelyWebGLLitePreset),
                    false
                );
                if (livelyWebGLPreset != lastlivelyWebGLPreset)
                {
                    EditorHandlers.UpdateChangesMade(true);
                    lastlivelyWebGLPreset = livelyWebGLPreset;
                    EditorPrefs.SetString("LivelyWebGLLiteSetupPreset", livelyWebGLPreset != null ? AssetDatabase.GetAssetPath(livelyWebGLPreset) : "");
                }

                if (GUILayout.Button("Create New", GUILayout.Width(92)))
                {
                    string path = EditorUtility.SaveFilePanelInProject("Create New WebGL Preset", Application.companyName + "WebGLPreset", "asset", "Specify where to save the new preset");
                    if (!string.IsNullOrEmpty(path))
                    {
                        var newAsset = ScriptableObject.CreateInstance<LivelyWebGLLitePreset>();
                        AssetDatabase.CreateAsset(newAsset, path);
                        AssetDatabase.SaveAssets();
                        AssetDatabase.Refresh();
                        livelyWebGLPreset = newAsset;
                        serializedTargetSetup = new SerializedObject(livelyWebGLPreset);
                    }
                }
                GUILayout.EndHorizontal();

                if (livelyWebGLPreset != null)
                {
                    if (serializedTargetSetup == null || serializedTargetSetup.targetObject != livelyWebGLPreset)
                        serializedTargetSetup = new SerializedObject(livelyWebGLPreset);

                    string beforeState = serializedTargetSetup.targetObject == null ? "" : EditorJsonUtility.ToJson(livelyWebGLPreset);

                    serializedTargetSetup.Update();

                    SerializedProperty property = serializedTargetSetup.GetIterator();
                    property.NextVisible(true);

                    while (property.NextVisible(false))
                    {
                        using (new EditorGUILayout.VerticalScope("box"))
                        {
                            EditorGUIUtility.labelWidth = 200;

                            switch (property.name)
                            {
                                case "pageSetup":
                                    ShowHeader(property, " Page Setup", gradient[0]);
                                    if (property.isExpanded)
                                    {
                                        EditorGUI.indentLevel++;
                                        PropertyExpanded(property, 10);

                                        SerializedProperty footer = property.FindPropertyRelative("footer");
                                        if (footer != null)
                                        {
                                            if (footer.isExpanded)
                                            {
                                                EditorGUI.indentLevel++;
                                                // Company Name
                                                string companyName = PlayerSettings.companyName;
                                                string newCompany = EditorGUILayout.TextField(new GUIContent("Company Name", "The name of your company. This appears at the bottom of the screen while the game is loading."), companyName);
                                                if (newCompany != companyName)
                                                {
                                                    PlayerSettings.companyName = newCompany;
                                                    EditorHandlers.UpdateChangesMade(true);
                                                }

                                                // Product Name
                                                string productName = PlayerSettings.productName;
                                                string newProduct = EditorGUILayout.TextField(new GUIContent("Product Name", "The name of your Game. This appears at the bottom of the screen while the game is loading."), productName);
                                                if (newProduct != productName)
                                                {
                                                    PlayerSettings.productName = newProduct;
                                                    EditorHandlers.UpdateChangesMade(true);
                                                }

                                                // Version
                                                string version = PlayerSettings.bundleVersion;
                                                string newVersion = EditorGUILayout.TextField(new GUIContent("Version", "your games current version. This appears at the bottom of the screen while the game is loading."), version);
                                                if (newVersion != version)
                                                {
                                                    PlayerSettings.bundleVersion = newVersion;
                                                    EditorHandlers.UpdateChangesMade(true);
                                                }
                                                EditorGUI.indentLevel--;
                                            }
                                        }
                                        GUILayout.Space(16);
                                        GUILayout.BeginHorizontal();

                                        GUILayout.EndHorizontal();
                                        EditorHandlers.HelpBox("Page Setup",
                                                "Hover your mouse over a field for its definition;\n\n" +
                                                "This section covers the webpage details. " +
                                                "The Page Title is what appears in (but not limited to) the browsers tab / title bar; \n\n" +
                                                "The Footer refers to the bottom of the page:\n" +
                                                "The Company, Product Name and Version will update in the player settings and visa versa.\n\n",
                                                "https://bumblingwizard.com/documentation/livelywebgl/doku.php?id=docs:definitions:pagesetup");
                                        EditorGUI.indentLevel--;
                                    }
                                    GUILayout.Space(15);
                                    break;

                                case "loadingScreen":

                                    ShowHeader(property, " Loading Screen", gradient[1]);
                                    if (property.isExpanded)
                                    {
                                        EditorGUI.indentLevel++;
                                        PropertyExpanded(property, 20);

                                        EditorHandlers.HelpBox("Loading Screen",
                                            "Hover your mouse over a field for its definition;\n\n" +
                                            "This section covers how the loading screen appears. The Logo Graphic is the 'Your Logo' image that appears when the loading bar is visible;",
                                            "https://bumblingwizard.com/documentation/livelywebgl/doku.php?id=docs:definitions:loadingscreen");

                                        EditorGUI.indentLevel--;

                                    }
                                    GUILayout.Space(15);
                                    break;

                                default:
                                    EditorGUILayout.PropertyField(property, true);
                                    break;
                            }
                        }

                        EditorGUIUtility.labelWidth = 0;
                        GUILayout.Space(10);
                    }

                    using (new EditorGUILayout.VerticalScope("box"))
                    {
                        BuildAndPreviewManager.BuildSettings();
                    }

                    serializedTargetSetup.ApplyModifiedProperties();
                    string afterState = serializedTargetSetup.targetObject == null ? "" : EditorJsonUtility.ToJson(livelyWebGLPreset);

                    if (beforeState != afterState)
                    {
                        EditorHandlers.UpdateChangesMade(true);
                        EditorUtility.SetDirty(livelyWebGLPreset);
                    }
                }
                else
                {
                    GUILayout.BeginHorizontal();
                    GUILayout.FlexibleSpace();
                    Color previousColor = GUI.contentColor;
                    GUI.contentColor = new Color(0.9f, 0.4f, 0.4f);
                    Color defaultBg = GUI.backgroundColor;
                    GUI.backgroundColor = new Color(0.15f, 0.15f, 0.15f);
                    GUIStyle paddedButton = new GUIStyle(GUI.skin.button);
                    paddedButton.padding = new RectOffset(10, 10, 8, 8); // L, R, T, B
                    if (GUILayout.Button("No WebGL Lite Setup Preset Selected!", paddedButton, GUILayout.Width(250)))
                    {
                    }
                    GUI.backgroundColor = defaultBg;
                    GUI.contentColor = previousColor;
                    GUILayout.FlexibleSpace();
                    GUILayout.EndHorizontal();
                }
                GUILayout.EndVertical();
                GUILayout.Space(6);

                EditorGUILayout.EndScrollView();
            }
        }

        public static class ToolEditor
        {
            static Vector2 scrollPos = new Vector2();

            static readonly string defaultEmbedInput = @"<link rel=""preconnect"" href=""https://fonts.googleapis.com"">
            <link rel=""preconnect"" href=""https://fonts.gstatic.com"" crossorigin>
            <link href=""https://fonts.googleapis.com/css2?family=Roboto:ital,wght@0,100..900;1,100..900&display=swap"" rel=""stylesheet"">";

            static readonly string defaultBlockInput = @".roboto-<uniquifier> {
                font-family: ""Roboto"", sans-serif;
                font-optical-sizing: auto;
                font-weight: <weight>;
                font-style: normal;
                font-variation-settings:
                ""wdth"" 100;
            }";

            public static void ShowToolEditor()
            {

                EditorGUILayout.BeginVertical("HelpBox");

                EditorGUILayout.EndVertical();
                scrollPos = EditorGUILayout.BeginScrollView(scrollPos, false, false);
                EditorGUILayout.BeginVertical(GUILayout.ExpandWidth(true));

                GUILayout.BeginHorizontal();
                GUILayout.FlexibleSpace();
            GUILayout.Space(60);
            EditorGUILayout.LabelField("Aspect Ratio Calculator", EditorStyles.boldLabel);
                GUILayout.FlexibleSpace();
                GUILayout.EndHorizontal(); 

                EditorGUILayout.BeginVertical("HelpBox");

                GUILayout.BeginHorizontal();
                EditorGUI.BeginChangeCheck();
                string resolutionInputX = EditorGUILayout.TextField("Reference Width:", UndoStateHelper.Instance.resolutionInputX);
                string resolutionInputY = EditorGUILayout.TextField("Reference Height:", UndoStateHelper.Instance.resolutionInputY);
                if (EditorGUI.EndChangeCheck())
                {
                    Undo.RecordObject(UndoStateHelper.Instance, "LivelyWebGL: Font Link Changed");
                    if (int.TryParse(resolutionInputX, out int w) && int.TryParse(resolutionInputY, out int h) && h != 0)
                    {
                        int GCD(int a, int b)
                        {
                            while (b != 0)
                            {
                                int temp = b;
                                b = a % b;
                                a = temp;
                            }
                            return a;
                        }

                        int gcd = GCD(w, h);
                        int ratioW = w / gcd;
                        int ratioH = h / gcd;
                        UndoStateHelper.Instance.aspectRatioOutput = $"{ratioW}:{ratioH}";
                    }
                    else
                    {
                        UndoStateHelper.Instance.aspectRatioOutput = "Invalid format";
                    }
                    UndoStateHelper.Instance.resolutionInputX = resolutionInputX;
                    UndoStateHelper.Instance.resolutionInputY = resolutionInputY;
                }
                GUILayout.EndHorizontal();

                EditorHandlers.DrawCopyableField("Aspect Ratio", UndoStateHelper.Instance.aspectRatioOutput);
                EditorGUILayout.EndVertical();

                GUILayout.Space(25);

                GUILayout.BeginHorizontal();
                GUILayout.FlexibleSpace();
                GUILayout.Space(65);
                EditorGUILayout.LabelField("Google Font Extractor", EditorStyles.boldLabel);
                GUILayout.FlexibleSpace();
                GUILayout.EndHorizontal();

                EditorGUILayout.BeginVertical("HelpBox");

                GUIStyle wrappingTextArea = new GUIStyle(EditorStyles.textArea);
                wrappingTextArea.wordWrap = true;

                EditorGUILayout.BeginVertical("HelpBox");
                EditorGUILayout.LabelField("Paste Font Href Here:", EditorStyles.boldLabel);
                EditorGUI.BeginChangeCheck();
                string fontLinkEmbedInput = EditorGUILayout.TextArea(UndoStateHelper.Instance.fontLinkEmbedInput, wrappingTextArea, GUILayout.MinHeight(80), GUILayout.ExpandWidth(true));
                if (EditorGUI.EndChangeCheck())
                {
                    Undo.RecordObject(UndoStateHelper.Instance, "LivelyWebGL LITE: Font Link Changed");
                    if (string.IsNullOrEmpty(fontLinkEmbedInput))
                        UndoStateHelper.Instance.fontLinkEmbedInput = defaultEmbedInput;
                    else
                        UndoStateHelper.Instance.fontLinkEmbedInput = fontLinkEmbedInput;

                }
                UndoStateHelper.Instance.extractedHref = ParseHref(fontLinkEmbedInput);
                if (!string.IsNullOrEmpty(UndoStateHelper.Instance.extractedHref))
                    EditorHandlers.DrawCopyableField("Font Href", UndoStateHelper.Instance.extractedHref);
                GUILayout.EndVertical();
                GUILayout.Space(10);

                EditorGUILayout.BeginVertical("HelpBox");
                EditorGUILayout.LabelField("Paste Font Family Here:", EditorStyles.boldLabel);
                EditorGUI.BeginChangeCheck();
                string fontCssBlockInput = EditorGUILayout.TextArea(UndoStateHelper.Instance.fontCssBlockInput, wrappingTextArea, GUILayout.MinHeight(120), GUILayout.ExpandWidth(true));
                if (EditorGUI.EndChangeCheck())
                {
                    Undo.RecordObject(UndoStateHelper.Instance, "LivelyWebGL LITE: Font Link Changed");
                    if (string.IsNullOrEmpty(fontCssBlockInput))
                        UndoStateHelper.Instance.fontCssBlockInput = defaultBlockInput;
                    else
                        UndoStateHelper.Instance.fontCssBlockInput = fontCssBlockInput;
                }
                UndoStateHelper.Instance.extractedFontFamily = ParseFontFamily(fontCssBlockInput);
                if (!string.IsNullOrEmpty(UndoStateHelper.Instance.extractedFontFamily))
                    EditorHandlers.DrawCopyableField("Font Family", UndoStateHelper.Instance.extractedFontFamily);
                GUILayout.EndVertical();
                EditorGUILayout.EndVertical();

                EditorGUILayout.EndVertical();
                EditorGUILayout.EndScrollView();

                string ParseHref(string linkBlock)
                {
                    string extractedHref = "";

                    // Extract correct href
                    string[] lines = linkBlock.Split('\n');
                    foreach (string line in lines)
                    {
                        if (line.Contains("href=\"https://fonts.googleapis.com/css2"))
                        {
                            int hrefStart = line.IndexOf("href=\"") + 6;
                            int hrefEnd = line.IndexOf("\"", hrefStart);
                            if (hrefStart > 5 && hrefEnd > hrefStart)
                            {
                                extractedHref = line.Substring(hrefStart, hrefEnd - hrefStart);
                                break;
                            }
                        }
                    }
                    return extractedHref;
                }

                string ParseFontFamily(string cssBlock)
                {
                    string extractedFontFamily = "";

                    // Extract font-family from CSS block
                    int fontStart = cssBlock.IndexOf("font-family:");
                    if (fontStart >= 0)
                    {
                        fontStart += "font-family:".Length;
                        int semi = cssBlock.IndexOf(";", fontStart);
                        if (semi > fontStart)
                        {
                            string fontRaw = cssBlock.Substring(fontStart, semi - fontStart).Trim();
                            extractedFontFamily = fontRaw;
                        }
                    }

                    return extractedFontFamily;
                }

            }
        }
        
        public static class TemplateProcessor
        {
            public static string HashString(string input)
            {
                using (SHA256 sha256 = SHA256.Create())
                {
                    // Convert the input string to a byte array
                    byte[] bytes = Encoding.UTF8.GetBytes(input);

                    // Compute the hash
                    byte[] hashBytes = sha256.ComputeHash(bytes);

                    // Convert the hash to a hexadecimal string
                    StringBuilder builder = new StringBuilder();
                    for (int i = 0; i < hashBytes.Length; i++)
                    {
                        builder.Append(hashBytes[i].ToString("x2")); // "x2" formats each byte as two hex digits
                    }
                    return builder.ToString();
                }
            }
            public static string SearchFileByName(string fileName)
            {
                string[] guids = AssetDatabase.FindAssets(fileName);
                foreach (string guid in guids)
                {
                    string path = AssetDatabase.GUIDToAssetPath(guid);
                    return path;
                }
                return "";
            }

            public static void ProcessTemplate(LivelyWebGLLitePreset preset)
            {
                // create the index.html
                ProcessTemplateInternal("DLWTL_indexTemplate", "index.html", preset);

                // create the constants library
                ProcessTemplateInternal("DLWTL_constantsTemplate", "core/constants.js", preset);

                // create the style sheet
                ProcessTemplateInternal("DLWTL_styleVariablesTemplate", "TemplateData/styles/styleVariables.css", preset);
                ProcessTemplateInternal("DLWTL_styleTemplate", "TemplateData/styles/style.css", preset);

                Debug.Log("WebGL Preset Updated");
            }

            private static void ProcessTemplateInternal(string templateAssetName, string outputFileName, LivelyWebGLLitePreset preset)
            {
                string templatePath = AssetDatabase.FindAssets($"{templateAssetName} t:TextAsset")
                    .Select(AssetDatabase.GUIDToAssetPath)
                    .FirstOrDefault();

                // look for js files
                if (string.IsNullOrEmpty(templatePath))
                {
                    templatePath = AssetDatabase.FindAssets(templateAssetName)
                        .Select(AssetDatabase.GUIDToAssetPath)
                        .FirstOrDefault(path => path.EndsWith(".js"));
                }

                if (string.IsNullOrEmpty(templatePath))
                {
                    Debug.LogError($"Template file '{templateAssetName}' not found!");
                    return;
                }

                string fullTemplatePath = Path.Combine(Directory.GetCurrentDirectory(), templatePath);
                string content = File.ReadAllText(fullTemplatePath);

                string ReplacePlaceholder(string input, string placeholder, string value) =>
                    Regex.Replace(input, $@"\[\[\[{placeholder}\]\]\]", value);

                int itterationsCount = 5;// we go over each file x amount of times, ensuring that any property gets processed X amount of times
                
                for (int itterations = 0; itterations < itterationsCount; itterations++)
                {
                    /*-------------------------------------------------------------------*/
                    // Page Setup
                    /*-------------------------------------------------------------------*/
                    // Apply replacements from all the setup categories
                    WGLSetup_PageSetup pageSetup = preset.pageSetup;
                    content = ReplacePlaceholder(content, "PageSetup_PageTitle", pageSetup.PageTitle);
                    content = ReplacePlaceholder(content, "PageSetup_MaxDPI", pageSetup.displaySetup.MaxDPR.ToString());
                    content = ReplacePlaceholder(content, "PageSetup_DefaultDPR", pageSetup.displaySetup.DefaultDPR.ToString());
                    content = ReplacePlaceholder(content, "PageSetup_PageBackgroundColor", "#" + ColorUtility.ToHtmlStringRGB(pageSetup.PageBackgroundColor));
                    content = ReplacePlaceholder(content, "PageSetup_DisableSaveMenuPopup", pageSetup.DisableSaveMenuPopup.ToString().ToLower());
                    //content = ReplacePlaceholder(content, "PageSetup_DisableCtrlWClosesTab", pageSetup.DisableCtrlWClosesTab.ToString().ToLower());
                    content = ReplacePlaceholder(content, "fullscreenHidesCornerButton", pageSetup.FullscreenHidesCornerButton.ToString().ToLower());

                    content = ReplacePlaceholder(content, "PageSetup_ImageSizeStyle", pageSetup.BackgroundImageConfig.ImageSizeStyle.ToString().ToLower());
                    string imageRepeatStyle = pageSetup.BackgroundImageConfig.ImageRepeatStyle.ToString().ToLower();
                    if(imageRepeatStyle == "norepeat")
                        imageRepeatStyle = "no-repeat";
                    if (imageRepeatStyle == "repeatx")
                        imageRepeatStyle = "repeat-x";
                    if (imageRepeatStyle == "repeaty")
                        imageRepeatStyle = "repeat-y";
                    content = ReplacePlaceholder(content, "PageSetup_ImageRepeatStyle", imageRepeatStyle);
                    content = ReplacePlaceholder(content, "HamburgerMenu_Disable", pageSetup.DisableFullscreenButton.ToString().ToLower());


                    /*-------------------------------------------------------------------*/
                    // Display Setup
                    /*-------------------------------------------------------------------*/
                    WGLSetup_PageSetup.DisplaySetup displaySetup = pageSetup.displaySetup;
                    content = ReplacePlaceholder(content, "DisplaySetup_DisplayScalingMode", ((int)displaySetup.DisplayScalingMode).ToString());
                    content = ReplacePlaceholder(content, "DisplaySetup_UseDynamicAspectRatio", displaySetup.UseDynamicAspectRatio.ToString().ToLower());
                    content = ReplacePlaceholder(content, "DisplaySetup_DefaultResolution" + "X", displaySetup.DefaultResolution.x.ToString());
                    content = ReplacePlaceholder(content, "DisplaySetup_DefaultResolution" + "Y", displaySetup.DefaultResolution.y.ToString());
                    content = ReplacePlaceholder(content, "DisplaySetup_AspectRatio" + "X", displaySetup.AspectRatio.x.ToString());
                    content = ReplacePlaceholder(content, "DisplaySetup_AspectRatio" + "Y", displaySetup.AspectRatio.y.ToString());


                    /*-------------------------------------------------------------------*/
                    // Footer
                    /*-------------------------------------------------------------------*/
                    WGLSetup_PageSetup.WGLSetup_LauncherFooter footerSetup = pageSetup.footer;
                    content = ReplacePlaceholder(content, "Footer_UseBackgroundImage", pageSetup.UseBackgroundImage.ToString().ToLower());
                    content = ReplacePlaceholder(content, "Footer_VersionTextColor", "#" + ColorUtility.ToHtmlStringRGB(footerSetup.versionFooter.VersionTextColor));
                    content = ReplacePlaceholder(content, "Footer_VersionTextFontSize", footerSetup.versionFooter.VersionTextFontSize.ToString());

                    /*-------------------------------------------------------------------*/
                    // Loading Screen
                    /*-------------------------------------------------------------------*/
                    WGLSetup_LoadingScreen loadingScreen = preset.loadingScreen;
                    content = ReplacePlaceholder(content, "LoadingScreen_UseLoadingLogoImage", loadingScreen.loadingBarLogo.useLoadingLogoImage.ToString().ToLower());
                    content = ReplacePlaceholder(content, "LoadingScreen_LogoImageMaxWidth", loadingScreen.loadingBarLogo.LogoImageMaxWidth.ToString());
                    content = ReplacePlaceholder(content, "LoadingScreen_ContainerYOffset", loadingScreen.loadingBarLogo.ContainerYOffset.ToString());

                    content = ReplacePlaceholder(content, "LoadingScreen_BackgroundColor", "#" + ColorUtility.ToHtmlStringRGB(loadingScreen.BackgroundColor));

                    content = ReplacePlaceholder(content, "LoadingScreen_Frame_Width", loadingScreen.FrameSettings.Frame_Width.ToString());
                    content = ReplacePlaceholder(content, "LoadingScreen_Frame_Height", loadingScreen.FrameSettings.Frame_Height.ToString());

                    content = ReplacePlaceholder(content, "LoadingScreen_Frame_Radius", loadingScreen.FrameSettings.Frame_Radius.ToString());
                    content = ReplacePlaceholder(content, "LoadingScreen_Frame_Thickness", loadingScreen.FrameSettings.Frame_Thickness.ToString());
                    content = ReplacePlaceholder(content, "LoadingScreen_Frame_Padding", loadingScreen.FrameSettings.Frame_Padding.ToString());
                    content = ReplacePlaceholder(content, "LoadingScreen_Frame_Style", loadingScreen.FrameSettings.Frame_Style.ToString());
                    content = ReplacePlaceholder(content, "LoadingScreen_Frame_Color", "#" + ColorUtility.ToHtmlStringRGB(loadingScreen.FrameSettings.Frame_Color));
                    content = ReplacePlaceholder(content, "LoadingScreen_Frame_BGColor", "#" + ColorUtility.ToHtmlStringRGB(loadingScreen.FrameSettings.Frame_BGColor));
                    content = ReplacePlaceholder(content, "LoadingScreen_Frame_LogoYOffset", loadingScreen.FrameSettings.Frame_LogoYOffset.ToString());

                    content = ReplacePlaceholder(content, "LoadingScreen_Pulses", loadingScreen.loadingBar.Pulses.ToString().ToLower());
                    content = ReplacePlaceholder(content, "LoadingScreen_BarColor", "#" + ColorUtility.ToHtmlStringRGB(loadingScreen.loadingBar.BarColor));
                    content = ReplacePlaceholder(content, "LoadingScreen_BarTransitionSpeed", loadingScreen.loadingBar.BarTransitionSpeed.ToString());
                    content = ReplacePlaceholder(content, "LoadingScreen_BarPulseColor", "#" + ColorUtility.ToHtmlStringRGB(loadingScreen.loadingBar.BarPulseColor));

                    content = ReplacePlaceholder(content, "LoadingScreen_FinalizingPhrase", loadingScreen.loadingText.FinalizingPhrase);

                    content = ReplacePlaceholder(content, "LoadingScreen_PercentValueFontSize", loadingScreen.loadingText.FontSize.ToString());
                    content = ReplacePlaceholder(content, "LoadingScreen_TextLineHeight", loadingScreen.loadingText.TextLineHeight.ToString());
                    content = ReplacePlaceholder(content, "LoadingScreen_PercentValueTextColorLow", "#" + ColorUtility.ToHtmlStringRGB(loadingScreen.loadingText.TextColorLow));
                    content = ReplacePlaceholder(content, "LoadingScreen_PercentValueTextColorHigh", "#" + ColorUtility.ToHtmlStringRGB(loadingScreen.loadingText.TextColorHigh));

                    /*-------------------------------------------------------------------*/
                    // Google Fonts
                    /*-------------------------------------------------------------------*/
                    WGLSetup_PageSetup.GoogleFonts googleFonts = pageSetup.googleFonts;
                    content = ReplacePlaceholder(content, "GoogleFonts_FontHref", googleFonts.FontHref.ToString());
                    content = ReplacePlaceholder(content, "GoogleFonts_FontFamily", googleFonts.FontFamily.ToString());

                    /*-------------------------------------------------------------------*/
                    // General Properties
                    /*-------------------------------------------------------------------*/
                    content = ReplacePlaceholder(content, "COMPANY_NAME", Application.companyName);
                    content = ReplacePlaceholder(content, "PRODUCT_NAME", Application.productName);
                    content = ReplacePlaceholder(content, "PRODUCT_VERSION", Application.version);

                }

                string targetFolder = targetFolder = Path.Combine("Assets", "WebGLTemplates", "LivelyWebGL LITE");                

                string savePath = outputFileName;
                if (targetFolder != "")
                {
                    Directory.CreateDirectory(targetFolder);
                    savePath = Path.Combine(targetFolder, outputFileName);
                }
                File.WriteAllText(savePath, content);
                AssetDatabase.Refresh();
            }
        }
    }
}