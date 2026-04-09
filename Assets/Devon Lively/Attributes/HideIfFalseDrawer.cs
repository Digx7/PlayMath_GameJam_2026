using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace DevonLively.Attributes
{
#if UNITY_EDITOR
    [CustomPropertyDrawer(typeof(HideIfFalseAttribute))]
    public class HideIfFalseDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            HideIfFalseAttribute hideAttribute = (HideIfFalseAttribute)attribute;
            SerializedProperty sourceProperty = AttributeCommons.FindPropertyRecursive(property.serializedObject, hideAttribute.ConditionalSourceField);

            bool shouldShow = false;
            if (sourceProperty != null && sourceProperty.propertyType == SerializedPropertyType.Boolean)
            {
                shouldShow = sourceProperty.boolValue;
            }
            else
            {
                shouldShow = true;
            }

            if (shouldShow)
            {
                EditorGUI.PropertyField(position, property, label, true);
            }
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            HideIfFalseAttribute hideAttribute = (HideIfFalseAttribute)attribute;
            SerializedProperty sourceProperty = AttributeCommons.FindPropertyRecursive(property.serializedObject, hideAttribute.ConditionalSourceField);

            bool shouldShow = false;
            if (sourceProperty != null && sourceProperty.propertyType == SerializedPropertyType.Boolean)
            {
                shouldShow = sourceProperty.boolValue;
            }
            else
            {
                shouldShow = true;
            }

            return shouldShow
                ? EditorGUI.GetPropertyHeight(property, label, true)
                : 0; 
        }
    }
#endif
}
