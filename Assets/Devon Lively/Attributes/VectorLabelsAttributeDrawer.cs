#if UNITY_EDITOR
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace DevonLively.Attributes
{
    [CustomPropertyDrawer(typeof(VectorLabelsAttribute))]
    public class VectorLabelsAttributeDrawer : PropertyDrawer
    {
        private static readonly string[] defaultLabels = new string[] { "X", "Y", "Z", "W" };

        private const int twoLinesThreshold = 375;

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            int factor = Screen.width < twoLinesThreshold ? 2 : 1;
            return factor * base.GetPropertyHeight(property, label);
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            // Get the tooltip from the field attribute, if it exists
            TooltipAttribute tooltipAttribute = fieldInfo.GetCustomAttribute<TooltipAttribute>();
            if (tooltipAttribute != null)
            {
                // Add the tooltip to the label
                label.tooltip = tooltipAttribute.tooltip;
            }

            VectorLabelsAttribute vectorLabels = (VectorLabelsAttribute)attribute;
            int spacing = vectorLabels.Spacing; // Use spacing from attribute

            if (property.propertyType == SerializedPropertyType.Vector2Int)
            {
                int[] array = new int[] { property.vector2IntValue.x, property.vector2IntValue.y };
                array = DrawFields(position, array, label, EditorGUI.IntField, vectorLabels, spacing);
                property.vector2IntValue = new Vector2Int(array[0], array[1]);
            }
            else if (property.propertyType == SerializedPropertyType.Vector2)
            {
                float[] array = new float[] { property.vector2Value.x, property.vector2Value.y };
                array = DrawFields(position, array, label, EditorGUI.FloatField, vectorLabels, spacing);
                property.vector2Value = new Vector2(array[0], array[1]);
            }
            else if (property.propertyType == SerializedPropertyType.Vector3Int)
            {
                int[] array = new int[] { property.vector3IntValue.x, property.vector3IntValue.y, property.vector3IntValue.z };
                array = DrawFields(position, array, label, EditorGUI.IntField, vectorLabels, spacing);
                property.vector3IntValue = new Vector3Int(array[0], array[1], array[2]);
            }
            else if (property.propertyType == SerializedPropertyType.Vector3)
            {
                float[] array = new float[] { property.vector3Value.x, property.vector3Value.y, property.vector3Value.z };
                array = DrawFields(position, array, label, EditorGUI.FloatField, vectorLabels, spacing);
                property.vector3Value = new Vector3(array[0], array[1], array[2]);
            }
            else if (property.propertyType == SerializedPropertyType.Vector4)
            {
                float[] array = new float[] { property.vector4Value.x, property.vector4Value.y, property.vector4Value.z, property.vector4Value.w };
                array = DrawFields(position, array, label, EditorGUI.FloatField, vectorLabels, spacing);
                property.vector4Value = new Vector4(array[0], array[1], array[2], array[3]);
            }
        }

        private T[] DrawFields<T>(Rect rect, T[] vector, GUIContent mainLabel, System.Func<Rect, T, T> fieldDrawer, VectorLabelsAttribute vectorLabels, int spacing)
        {
            T[] result = vector;

            bool twoLinesLayout = Screen.width < twoLinesThreshold;

            // Get the rect of the main label
            Rect mainLabelRect = rect;
            mainLabelRect.width = EditorGUIUtility.labelWidth;
            if (twoLinesLayout)
                mainLabelRect.height *= 0.5f;

            // Get the size of each field rect
            Rect fieldRect = rect;
            if (twoLinesLayout)
            {
                fieldRect.height *= 0.5f;
                fieldRect.y += fieldRect.height;
                fieldRect.width = rect.width / vector.Length;
            }
            else
            {
                fieldRect.x += mainLabelRect.width;
                fieldRect.width = (rect.width - mainLabelRect.width) / vector.Length;
            }

            // Draw the main label with tooltip
            EditorGUI.LabelField(mainLabelRect, mainLabel);

            for (int i = 0; i < vector.Length; i++)
            {
                string label = vectorLabels.Labels.Length > i ? vectorLabels.Labels[i] : defaultLabels[i];
                Vector2 labelSize = EditorStyles.label.CalcSize(new GUIContent(label));

                Rect labelRect = fieldRect;
                labelRect.width = Mathf.Max(labelSize.x + 5 + spacing, 0.5f * fieldRect.width);
                EditorGUI.LabelField(labelRect, label);

                Rect valueRect = fieldRect;
                valueRect.x += labelRect.width / 2;
                valueRect.width -= labelRect.width;

                result[i] = fieldDrawer(valueRect, vector[i]);

                fieldRect.x += fieldRect.width + spacing;
            }

            return result;
        }
    }
}
#endif