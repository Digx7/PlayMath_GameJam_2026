using UnityEngine;
using UnityEditor;
using System;
using System.IO;
using System.Collections.Generic;

[CustomEditor(typeof(SaveSystem))]
public class SaveSystemEditor : Editor
{
    // bool existingSaveToggle = false;
    // bool makeNewSaveToggle = false;
    // string newFileName = "New File";
    
    // public override void OnInspectorGUI()
    // {
    //     DrawDefaultInspector();
    //     SaveSystem saveSystem = (SaveSystem)target;

    //     GUILayout.Space(20);
    //     GUILayout.Label("Editor Buttons ====================");
        
    //     GUILayout.Space(5);
    //     GUILayout.Label($"Active Save File   :   {saveSystem.activeSaveName}");
        
    //     GUILayout.Space(5);
    //     existingSaveToggle = GUILayout.Toggle(existingSaveToggle, "Existing Save Files ====================");

    //     if(existingSaveToggle)
    //     {
    //         List<FileInfo> allSaveFileInfo = new List<FileInfo>(saveSystem.GetAllSaveFileInfo());

    //         foreach (FileInfo fileInfo in allSaveFileInfo)
    //         {
    //             GUILayout.Space(3);
    //             string fileName = Path.GetFileNameWithoutExtension(fileInfo.Name);
    //             GUILayout.Label(fileName);
    //             if (GUILayout.Button("Set Active"))
    //             {
    //                 saveSystem.activeSaveName = fileName;
    //                 PrefabUtility.RecordPrefabInstancePropertyModifications(saveSystem);
    //             }
    //             if (GUILayout.Button("Delete"))
    //             {
    //                 saveSystem.DeleteDataWithName(fileName);
    //                 if(saveSystem.activeSaveName == fileName) saveSystem.activeSaveName = "";
    //                 PrefabUtility.RecordPrefabInstancePropertyModifications(saveSystem);
    //             }
    //         }
    //     }
    
    //     GUILayout.Space(5);
    //     makeNewSaveToggle = GUILayout.Toggle(makeNewSaveToggle, "New Save ==============");

    //     if(makeNewSaveToggle)
    //     {
    //         newFileName = GUILayout.TextField(newFileName);

    //         if (GUILayout.Button("Make New Save"))
    //         {
    //             saveSystem.MakeNewSaveDataWithName(newFileName);
    //             saveSystem.activeSaveName = newFileName;
    //             PrefabUtility.RecordPrefabInstancePropertyModifications(saveSystem);
    //         }
    //     }
    
    // }
}
