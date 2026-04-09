using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace DevonLively.Attributes
{
#if UNITY_EDITOR
    [CustomPropertyDrawer(typeof(HideIfTrueAttribute))]
    public class HideIfTrueDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            HideIfTrueAttribute hideAttribute = (HideIfTrueAttribute)attribute;
            SerializedProperty sourceProperty = AttributeCommons.FindPropertyRecursive(property.serializedObject, hideAttribute.ConditionalSourceField);

            bool shouldShow = true;
            if (sourceProperty != null && sourceProperty.propertyType == SerializedPropertyType.Boolean)
            {
                shouldShow = !sourceProperty.boolValue;
            }

            if (shouldShow)
            {
                EditorGUI.PropertyField(position, property, label, true);
            }
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            HideIfTrueAttribute hideAttribute = (HideIfTrueAttribute)attribute;
            SerializedProperty sourceProperty = AttributeCommons.FindPropertyRecursive(property.serializedObject, hideAttribute.ConditionalSourceField);

            if (sourceProperty != null && sourceProperty.propertyType == SerializedPropertyType.Boolean)
            {
                if (sourceProperty.boolValue)
                    return -EditorGUIUtility.standardVerticalSpacing;
            }

            return EditorGUI.GetPropertyHeight(property, label, true);
        }
    }
#endif
}
