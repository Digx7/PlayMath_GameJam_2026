#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

namespace DevonLively.Attributes
{

    public class ConditionalEnumFieldAttribute : PropertyAttribute
    {
        public string ConditionFieldName;
        public string[] ShowForValues;

        public ConditionalEnumFieldAttribute(string conditionFieldName = null, params string[] showForValues)
        {
            ConditionFieldName = conditionFieldName;
            ShowForValues = showForValues;
        }
    }

    public class VectorLabelsAttribute : PropertyAttribute
    {
        public readonly string[] Labels;
        public readonly int Spacing;

        public VectorLabelsAttribute(params string[] labels)
        {
            Labels = labels;
            Spacing = 5;
        }   
        public VectorLabelsAttribute(int spacing, params string[] labels)
        {
            Labels = labels;
            Spacing = spacing;
        }
    }

    public class ReadOnlyAttribute : PropertyAttribute {}

    #if UNITY_EDITOR
    [CustomPropertyDrawer(typeof(ReadOnlyAttribute))]
    public class ReadOnlyDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            // Disable the GUI so that the field is read-only
            GUI.enabled = false;

            // Draw the property field
            EditorGUI.PropertyField(position, property, label, true);

            // Re-enable the GUI so other fields can be edited
            GUI.enabled = true;
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            // Ensure the property height is calculated correctly
            return EditorGUI.GetPropertyHeight(property, label, true);
        }
    }
    #endif
}