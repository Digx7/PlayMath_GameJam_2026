using UnityEngine;
using UnityEditor;
using System;
using System.IO;
using System.Collections.Generic;
using Digx7.Zygote;

[CustomEditor(typeof(ResponsiveUIHelper))]
[CanEditMultipleObjects]
public class ResponsiveUIHelperEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Breakpoint Tools", EditorStyles.boldLabel);

        ResponsiveUIHelper[] helpers = GetSelectedHelpers();
        if (helpers.Length == 0)
        {
            EditorGUILayout.HelpBox("No valid ResponsiveUIHelper selected.", MessageType.Info);
            return;
        }

        for (int h = 0; h < helpers.Length; h++)
        {
            DrawHelperSection(helpers[h], h, helpers.Length);
        }
    }

    private static ResponsiveUIHelper[] GetSelectedHelpers()
    {
        UnityEngine.Object[] selected = Selection.GetFiltered(typeof(ResponsiveUIHelper), SelectionMode.Editable);
        ResponsiveUIHelper[] helpers = new ResponsiveUIHelper[selected.Length];
        for (int i = 0; i < selected.Length; i++)
        {
            helpers[i] = selected[i] as ResponsiveUIHelper;
        }

        return helpers;
    }

    private static void DrawHelperSection(ResponsiveUIHelper helper, int index, int totalSelected)
    {
        if (helper == null)
        {
            return;
        }

        RectTransform rectTransform = helper.GetComponent<RectTransform>();

        EditorGUILayout.BeginVertical("box");
        if (totalSelected > 1)
        {
            EditorGUILayout.LabelField(helper.name, EditorStyles.boldLabel);
        }

        if (rectTransform == null)
        {
            EditorGUILayout.HelpBox("No RectTransform found on this object.", MessageType.Warning);
            EditorGUILayout.EndVertical();
            return;
        }

        if (helper.breakPoints == null || helper.breakPoints.Count == 0)
        {
            EditorGUILayout.HelpBox("No breakpoints found.", MessageType.Info);
            EditorGUILayout.EndVertical();
            return;
        }

        for (int i = 0; i < helper.breakPoints.Count; i++)
        {
            UIResponsiveBreakPoint bp = helper.breakPoints[i];
            string label = string.IsNullOrWhiteSpace(bp.breakPointName) ? "Breakpoint " + (i + 1) : bp.breakPointName;

            EditorGUILayout.BeginVertical("HelpBox");
            EditorGUILayout.LabelField(label, EditorStyles.boldLabel);

            if (GUILayout.Button("Apply AnchorPoints -> RectTransform"))
            {
                Undo.RecordObject(rectTransform, "Apply Breakpoint Anchors");
                helper.ApplyBreakPoint(bp);
                EditorUtility.SetDirty(rectTransform);
            }

            if (GUILayout.Button("Capture RectTransform -> AnchorPoints"))
            {
                Undo.RecordObject(helper, "Capture RectTransform Anchors");
                bp.AnchorMinPoints = rectTransform.anchorMin;
                bp.AnchorMaxPoints = rectTransform.anchorMax;
                helper.breakPoints[i] = bp;
                EditorUtility.SetDirty(helper);
            }

            EditorGUILayout.EndVertical();
        }

        EditorGUILayout.EndVertical();
    }
}