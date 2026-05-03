using UnityEngine;
using UnityEditor;

namespace Digx7.Zygote
{    
    [CustomEditor(typeof(GameManager))]
    public class GameManagerCustomInspector : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            GameManager myGameManager = target as GameManager;

            GameManager.IsEditorMobilePreview = EditorGUILayout.Toggle("Is Mobile Preview", GameManager.IsEditorMobilePreview);
        }
    }
}