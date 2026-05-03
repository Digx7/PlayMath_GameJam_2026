using UnityEngine;
using UnityEditor;

namespace Digx7.Zygote
{    
    [CustomEditor(typeof(ScreenBreakPointData))]
    public class ScreenBreakPointDataCustomInspector : UnityEditor.Editor 
    {
        
        // TODO: Add your serialized properties here        
        SerializedProperty checkScreenHeightProp;
        SerializedProperty minScreenHeightProp;
        SerializedProperty maxScreenHeightProp;

        SerializedProperty checkScreenWidthProp;
        SerializedProperty minScreenWidthProp;
        SerializedProperty maxScreenWidthProp;

        SerializedProperty mobileOnlyProp;
        SerializedProperty portraitOnlyProp;
        SerializedProperty landscapeOnlyProp;

        void OnEnable()
        {
            // TODO: initialize your SerializedProperty's here
            checkScreenHeightProp = serializedObject.FindProperty("checkScreenHeight");
            minScreenHeightProp = serializedObject.FindProperty("minScreenHeight");
            maxScreenHeightProp = serializedObject.FindProperty("maxScreenHeight");

            checkScreenWidthProp = serializedObject.FindProperty("checkScreenWidth");
            minScreenWidthProp = serializedObject.FindProperty("minScreenWidth");
            maxScreenWidthProp = serializedObject.FindProperty("maxScreenWidth");

            mobileOnlyProp = serializedObject.FindProperty("mobileOnly");
            portraitOnlyProp = serializedObject.FindProperty("portraitOnly");
            landscapeOnlyProp = serializedObject.FindProperty("landscapeOnly");
        }

        public override void OnInspectorGUI() 
        {
            // base.OnInspectorGUI();

            ScreenBreakPointData myData = target as ScreenBreakPointData;

            // TODO (Optional): comment out the 'base.OnInspectorGUI()' line to fully create your own inspector
            // Use the following code as a starting point

            EditorGUILayout.PropertyField(checkScreenHeightProp);
            if (checkScreenHeightProp.boolValue)
            {
                EditorGUILayout.PropertyField(minScreenHeightProp);
                EditorGUILayout.PropertyField(maxScreenHeightProp);
            }

            EditorGUILayout.PropertyField(checkScreenWidthProp);
            if (checkScreenWidthProp.boolValue)
            {
                EditorGUILayout.PropertyField(minScreenWidthProp);
                EditorGUILayout.PropertyField(maxScreenWidthProp);
            }

            EditorGUILayout.PropertyField(mobileOnlyProp);
            EditorGUILayout.PropertyField(portraitOnlyProp);
            EditorGUILayout.PropertyField(landscapeOnlyProp);

            // Apply changes to the serializedProperty - always do this at the end of OnInspectorGUI.
            serializedObject.ApplyModifiedProperties();
        }
    }
}

// 