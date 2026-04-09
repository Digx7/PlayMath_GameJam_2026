using UnityEngine;
using UnityEditor;

namespace DevonLively.LivelyWebGLLite.Editor
{
    public class LivelyWebGLLiteStartupWindowEditor : EditorWindow
    {
        private static LivelyWebGLLiteStartupWindowEditor instance;
        private static string PrefKey => Application.productName + "_LivelyWebGLLite_FirstRun";
        private bool userDisabledEditor;

        [MenuItem("Window/Devon Lively/LivelyWebGL LITE/Startup Window", false, 2)]
        public static void ShowWindow()
        {
            if (instance == null)
                instance = GetWindow<LivelyWebGLLiteStartupWindowEditor>(true, "Lively WebGL Lite Startup", true);

            instance.maxSize = new Vector2(420, 280 + EditorGUIUtility.singleLineHeight * 1.5f);
            instance.minSize = new Vector2(420, 280 + EditorGUIUtility.singleLineHeight * 1.5f);

            instance.Show();
            instance.Focus();
        }

        private void OnEnable()
        {
            instance = this;
            userDisabledEditor = EditorPrefs.GetBool(PrefKey, false);
        }

        private void OnDisable()
        {
            if (instance == this)
                instance = null;
        }

        private void OnGUI()
        {
            GUILayout.Space(10);
            DrawSDLogo("DLWTLEGraphic");
            GUILayout.Space(20);

            GUIStyle descStyle = new GUIStyle(EditorStyles.wordWrappedLabel)
            {
                alignment = TextAnchor.MiddleCenter,
                fontSize = 15,
                fontStyle = FontStyle.BoldAndItalic,
                normal = { textColor = EditorStyles.label.normal.textColor }
            };

            GUILayout.Label(
                "At the top of your editor,\n" +
                "Press Window / Devon Lively / Lively WebGL LITE/ LivelyWebGL LITE Editor",
                descStyle);

            GUILayout.FlexibleSpace();

            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();

            GUIStyle toggleStyle = new GUIStyle(EditorStyles.toggle)
            {
                fontSize = 11,
                wordWrap = true
            };
            EditorGUI.BeginChangeCheck();
            userDisabledEditor = GUILayout.Toggle(userDisabledEditor, "Don't show me this again", toggleStyle);
            if (EditorGUI.EndChangeCheck())
            {
                EditorPrefs.SetBool(PrefKey, userDisabledEditor);
            }
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
            GUILayout.Space(10);
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();

            GUIStyle buttonStyle = new GUIStyle(GUI.skin.button)
            {
                fontSize = 13,
                fontStyle = FontStyle.Bold,
                fixedWidth = 170,
                fixedHeight = 30
            };

            if (GUILayout.Button("Open Lively WebGL LITE", buttonStyle))
            {
                LivelyWebGLLiteEditor.ShowWindow();
                Close();
            }
            if (GUILayout.Button("Need Support?", buttonStyle))
                Application.OpenURL("bumblingwizard.com");

            GUILayout.Space(20);


            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();

            GUILayout.Space(15);
        }


        [InitializeOnLoadMethod]
        private static void AutoShowOnLoad()
        {
            EditorApplication.update += DelayedShow;
        }

        private static void DelayedShow()
        {
            EditorApplication.update -= DelayedShow;

            if (EditorPrefs.GetBool(PrefKey, false)) return;

            if (IsMainWindowOpen()) return;
            ShowWindow();
        }

        private static bool IsMainWindowOpen()
        {
            return Resources.FindObjectsOfTypeAll<LivelyWebGLLiteEditor>().Length > 0;
        }

        Texture2D myTexture;

        void DrawSDLogo(string graphicName)
        {
            if (myTexture == null)
            {
                myTexture = FindTextureByName(graphicName);
            }
            if (myTexture == null) { return; }
            float inspectorWidth = EditorGUIUtility.currentViewWidth;
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
            string[] guids = AssetDatabase.FindAssets(textureName + " t:Texture2D");

            if (guids.Length > 0)
            {
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
}