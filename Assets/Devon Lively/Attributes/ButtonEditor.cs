#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Reflection;

namespace DevonLively.Attributes
{
    [CustomEditor(typeof(UnityEngine.Object), true)]
    public class ButtonEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            var targetObject = target;
            var targetType = targetObject.GetType();

                // Dictionary to group methods by label
                var groupedMethods = new Dictionary<string, List<MethodInfo>>();

                // Get all methods in the target class
                var methods = target.GetType().GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
                foreach (var method in methods)
                {
                    // Check if the method has the ButtonAttribute
                    var buttonAttribute = (ButtonAttribute)System.Attribute.GetCustomAttribute(method, typeof(ButtonAttribute));
                    if (buttonAttribute != null)
                    {
                        bool shouldShow = true;

                        // Check HideIf condition
                        if (buttonAttribute.HideIfField != null)
                        {
                            var field = target.GetType().GetField(buttonAttribute.HideIfField, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
                            if (field != null && field.GetValue(target)?.ToString() == buttonAttribute.ConditionalValue)
                            {
                                shouldShow = false;
                            }
                        }

                        // Check ShowIf condition
                        if (buttonAttribute.ShowIfField != null)
                        {
                            var field = target.GetType().GetField(buttonAttribute.ShowIfField, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
                            if (field != null && field.GetValue(target)?.ToString() != buttonAttribute.ConditionalValue)
                            {
                                shouldShow = false;
                            }
                        }

                        if (shouldShow)
                        {
                            string groupLabel = buttonAttribute.GroupLabel ?? "Default Action(s)";
                            if (!groupedMethods.ContainsKey(groupLabel))
                            {
                                groupedMethods[groupLabel] = new List<MethodInfo>();
                            }
                            groupedMethods[groupLabel].Add(method);
                        }
                    }
                }

                foreach (var group in groupedMethods)
                {
                    int buttonCount = group.Value.Count;
                    if (buttonCount == 0) continue;


                    // Draw buttons for each method in the group
                    foreach (var method in group.Value)
                    {
                        var buttonAttribute = (ButtonAttribute)System.Attribute.GetCustomAttribute(method, typeof(ButtonAttribute));
                        string buttonLabel = buttonAttribute.ButtonLabel ?? method.Name;
                        if (!string.IsNullOrEmpty(buttonAttribute.GroupLabel))
                        {
                            EditorGUILayout.LabelField(buttonAttribute.GroupLabel, EditorStyles.boldLabel);                            
                        }
                        buttonLabel = buttonLabel.Replace("_", " ");
                        if (GUILayout.Button(buttonLabel, GUILayout.Height(32)))
                        {
                            method.Invoke(target, null);
                        }
                    }
                }
            
        }
    }
}
#endif