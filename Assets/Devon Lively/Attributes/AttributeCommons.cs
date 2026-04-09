#if UNITY_EDITOR
using UnityEditor;
#endif

namespace DevonLively.Attributes
{
    public static class AttributeCommons
    {
#if UNITY_EDITOR
        public static SerializedProperty FindPropertyRecursive(SerializedObject serializedObject, string propertyPath)
        {
            // Try direct path first
            SerializedProperty property = serializedObject.FindProperty(propertyPath);
            if (property != null)
                return property;

            // If not found, try searching all properties recursively
            SerializedProperty iterator = serializedObject.GetIterator();
            if (iterator.NextVisible(true))
            {
                do
                {
                    if (iterator.name == propertyPath || iterator.propertyPath.EndsWith("." + propertyPath))
                        return serializedObject.FindProperty(iterator.propertyPath);
                }
                while (iterator.NextVisible(true));
            }

            return null;
        }
#endif
    }
}