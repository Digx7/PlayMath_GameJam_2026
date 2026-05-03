using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor;
using UnityEditor.DeviceSimulation;
using Digx7.Grids;
using Digx7.Levels;
using System;
using System.IO;
using System.Reflection;
using System.Collections.Generic;
using System.Text.RegularExpressions;

public class SimulatorWindowToggle : EditorWindow
{
    private static FieldInfo _mainField;
    private static PropertyInfo _deviceIndexProperty;
    private static FieldInfo _devicesField;

    
    private static UnityEngine.Object _simulatorWindow;
    

    [MenuItem("Tools/Simulator Window Toggle")]
    public static void ShowWindow()
    {   
        var assembly = Assembly.GetAssembly(typeof(DeviceSimulator));
        var windowType = assembly.GetType("UnityEditor.DeviceSimulation.SimulatorWindow");
        _mainField = windowType.GetField("m_Main", BindingFlags.Instance | BindingFlags.NonPublic);
        var mainType = assembly.GetType("UnityEditor.DeviceSimulation.DeviceSimulatorMain");
        _deviceIndexProperty = mainType.GetProperty("deviceIndex", BindingFlags.Instance | BindingFlags.Public);
        _devicesField = mainType.GetField("m_Devices", BindingFlags.Instance | BindingFlags.NonPublic);

        _simulatorWindow = Resources.FindObjectsOfTypeAll(windowType)[0];
        
        GetWindow<SimulatorWindowToggle>("Simulator Window Toggle");
    }

    private void OnGUI()
    {
        if(GUILayout.Button("Next Device"))
        {
            SelectNextSimulatorDevice();
        }

        if(GUILayout.Button("Previous Device"))
        {
            SelectPreviousSimulatorDevice();
        }
        
    }

    private void SelectPreviousSimulatorDevice() => SelectDeviceWithOffset(-1);

    private void SelectNextSimulatorDevice() => SelectDeviceWithOffset(1);

    private void SelectDeviceWithOffset(int offset)
    {
        var main = _mainField.GetValue(_simulatorWindow);
        var devices = _devicesField.GetValue(main) as Array;

        var index = (int)_deviceIndexProperty.GetValue(main);
        index = (int)Mathf.Repeat(index + offset, devices!.Length);

        _deviceIndexProperty.SetValue(main, index);
    }
}