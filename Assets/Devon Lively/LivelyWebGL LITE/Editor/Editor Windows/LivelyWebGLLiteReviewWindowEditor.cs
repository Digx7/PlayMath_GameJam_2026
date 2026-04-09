using UnityEngine;
using UnityEditor;

namespace DevonLively.LivelyWebGLLite.Editor
{
    public class LivelyWebGLLiteReviewWindowEditor : EditorWindow
    {
        private static LivelyWebGLLiteReviewWindowEditor instance;
        private static string PrefKey => Application.productName + "_LivelyWebGLLite_ReviewSuggestion";
        private bool userDisabledEditor;

        [MenuItem("Window/Devon Lively/LivelyWebGL LITE/Leave A Review", false, 2)]
        public static void ShowWindow()
        {
            if (instance == null)
                instance = GetWindow<LivelyWebGLLiteReviewWindowEditor>(true, "Lively WebGL LITE Review", true);

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
            DrawLogo("DLWTLEGraphic");
            GUILayout.Space(20);

            GUIStyle richStyle = new GUIStyle(EditorStyles.wordWrappedLabel)
            {
                alignment = TextAnchor.MiddleCenter,
                fontSize = 15,
                fontStyle = FontStyle.Bold,
                richText = true,
                normal = { textColor = EditorStyles.label.normal.textColor }
            };

            EditorGUILayout.LabelField(
                "<color=#FFD700>Are you enjoying <b>Lively WebGL LITE</b>?</color>\n" +
                "Consider leaving a <b>review</b> on the Unity Asset Store —\n" +
                "your support is very appreciated!",
                richStyle
            );
            GUILayout.Space(20);
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();

            GUIStyle buttonStyle = new GUIStyle(GUI.skin.button)
            {
                fontSize = 13,
                fontStyle = FontStyle.Bold,
                fixedWidth = 160,
                richText = true,
                fixedHeight = 30
            };

            if (GUILayout.Button("<color=#FFD700>Leave Review</color>", buttonStyle))
            {
                Application.OpenURL("https://assetstore.unity.com/packages/slug/347758");
                Close();
            }

            GUILayout.Space(20);


            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();

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

            GUILayout.Space(15);
        }

        Texture2D myTexture;

        void DrawLogo(string graphicName)
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