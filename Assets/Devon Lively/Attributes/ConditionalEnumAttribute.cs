using UnityEngine;
using UnityEditor;
using System;
using System.Reflection;

namespace DevonLively.Attributes
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = true)]
    public class ConditionalEnumAttribute : PropertyAttribute
    {
        public string EnumFieldName { get; private set; }
        public int EnumValue { get; private set; }

        public ConditionalEnumAttribute(string enumFieldName, int enumValue)
        {
            EnumFieldName = enumFieldName;
            EnumValue = enumValue;
        }
    }

#if UNITY_EDITOR

[CustomPropertyDrawer(typeof(ConditionalEnumAttribute))]
    public class ConditionalEnumDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            ConditionalEnumAttribute cond = attribute as ConditionalEnumAttribute;

            object targetObject = GetParentObject(property);
            if (targetObject == null)
                return;

            FieldInfo enumField = targetObject.GetType().GetField(cond.EnumFieldName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            if (enumField == null)
                return;

            object enumValue = enumField.GetValue(targetObject);
            if (enumValue == null)
                return;

            if ((int)enumValue != cond.EnumValue)
                return;

            EditorGUI.PropertyField(position, property, label, true);
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            ConditionalEnumAttribute cond = attribute as ConditionalEnumAttribute;

            object targetObject = GetParentObject(property);
            if (targetObject == null)
                return 0f;

            FieldInfo enumField = targetObject.GetType().GetField(cond.EnumFieldName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            if (enumField == null)
                return 0f;

            object enumValue = enumField.GetValue(targetObject);
            if (enumValue == null)
                return 0f;

            if ((int)enumValue != cond.EnumValue)
                return 0f;

            return EditorGUI.GetPropertyHeight(property, label, true);
        }

        private object GetParentObject(SerializedProperty property)
        {
            // Extracts the actual parent instance even inside nested structs or arrays
            string path = property.propertyPath.Replace(".Array.data[", "[");
            object obj = property.serializedObject.targetObject;
            string[] elements = path.Split('.');

            // Skip the last (the property itself)
            for (int i = 0; i < elements.Length - 1; i++)
            {
                string element = elements[i];
                if (element.Contains("["))
                {
                    string elementName = element.Substring(0, element.IndexOf("["));
                    int index = Convert.ToInt32(element.Substring(element.IndexOf("[")).Replace("[", "").Replace("]", ""));
                    obj = GetValue(obj, elementName, index);
                }
                else
                {
                    obj = GetValue(obj, element);
                }

                if (obj == null)
                    return null;
            }

            return obj;
        }

        private object GetValue(object source, string name)
        {
            if (source == null)
                return null;
            Type type = source.GetType();
            FieldInfo f = type.GetField(name, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            if (f == null)
                return null;
            return f.GetValue(source);
        }

        private object GetValue(object source, string name, int index)
        {
            var enumerable = GetValue(source, name) as System.Collections.IEnumerable;
            if (enumerable == null)
                return null;

            var enm = enumerable.GetEnumerator();
            for (int i = 0; i <= index; i++)
            {
                if (!enm.MoveNext())
                    return null;
            }

            return enm.Current;
        }
    }
#endif
}