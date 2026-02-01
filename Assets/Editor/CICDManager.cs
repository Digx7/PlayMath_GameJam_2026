using UnityEngine;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEditor.Build.Profile;
using System;
using System.IO;

public static class CICDManager
{
    static string buildPath = @"C:/Users/Digx7/Desktop/PlayMathGames_GameJam_2026 Builds/";
    
    static string GetDecreasedVersionNumber()
    {
        string[] verRaw = Application.version.Split('.');
        int lastNumber = int.Parse(verRaw[verRaw.Length - 1]);
        lastNumber--;
        verRaw[verRaw.Length - 1] = lastNumber.ToString();
        return string.Join('.',verRaw);
    }

    static string GetIncrementedVersionNumber()
    {
        string[] verRaw = Application.version.Split('.');
        int lastNumber = int.Parse(verRaw[verRaw.Length - 1]);
        lastNumber++;
        verRaw[verRaw.Length - 1] = lastNumber.ToString();
        return string.Join('.',verRaw);
    }
    
    // Helper to get all enabled scenes in Build Settings
    static string[] GetEnabledScenes()
    {
        // Get all enabled scenes from Build Settings
        var scenesInSettings = EditorBuildSettings.scenes;
        var enabledScenes = new System.Collections.Generic.List<string>();

        // Iterate through all scenes and add the enabled ones
        for (int i = 0; i < scenesInSettings.Length; i++)
        {
            if (scenesInSettings[i].enabled)
                enabledScenes.Add(scenesInSettings[i].path);
        }

        return enabledScenes.ToArray();
    }
    
    // General build method
    static void BuildForTarget(BuildTarget target, string outputPath, string versionNumber)
    {
        string[] scenes = GetEnabledScenes();

        PlayerSettings.bundleVersion = versionNumber;

        // Build player options
        BuildPlayerOptions options = new BuildPlayerOptions
        {
            scenes = scenes,
            locationPathName = outputPath,
            target = target,
            options = BuildOptions.None
        };

        // Execute the build
        BuildReport report = BuildPipeline.BuildPlayer(options);
        CheckBuildResult(report, outputPath);
    }

    static void BuildForActiveProfile(string outputPath, string versionNumber)
    {
        Debug.Log("MYLOG: Build for active profile");
        
        BuildProfile profile = BuildProfile.GetActiveBuildProfile();
        if (profile == null)
            throw new BuildFailedException("No active build profile is set." +
                "Use the Build Profiles window or the `-activeBuildProfile` cli argument");

        Debug.Log($"MYLOG: active build profile is {profile.name}");

        PlayerSettings.bundleVersion = versionNumber;

        // Build player options
        BuildPlayerWithProfileOptions optionsWithBuildProfile = new BuildPlayerWithProfileOptions
        {
            buildProfile = profile,
            locationPathName = outputPath,
            options = BuildOptions.None
        };

        // Execute the build
        BuildReport report = BuildPipeline.BuildPlayer(optionsWithBuildProfile);
        CheckBuildResult(report, outputPath);
    }

    // [MenuItem("Build/Windows")]
    public static void BuildWindows()
    {
        string versionNumber = GetIncrementedVersionNumber();
        BuildForTarget(BuildTarget.StandaloneWindows64, $"{buildPath}Win_{versionNumber}/{Application.productName}.exe", versionNumber);
    }

    [MenuItem("Build/Steam/Demo/Windows")]
    public static void BuildSteamDemoWindows()
    {
        string os = "Win";
        string versionNumber = Application.version;
        string sku = "Demo";
        BuildForActiveProfile($"{buildPath}{os}_{versionNumber}_{sku}/{Application.productName}.exe", $"{os}_{versionNumber}_{sku}");
    }

    public static void BuildWindows_CLI()
    {
        try
        {
            string[] arguments = Environment.GetCommandLineArgs();
            string versionNumber = arguments[3];
            BuildForTarget(BuildTarget.StandaloneWindows64, $"{buildPath}Win_{versionNumber}/{Application.productName}.exe", versionNumber);
        }
        catch (System.Exception e)
        {
            throw e;
        }
        
    }

    public static void BuildForProfile_CLI()
    {
        try
        {
            string[] arguments = Environment.GetCommandLineArgs();
            string os = arguments[3];
            string versionNumber = arguments[4];
            string sku = arguments[5];
            BuildForActiveProfile($"{buildPath}{os}_{versionNumber}_{sku}/{Application.productName}.exe", $"{os}_{versionNumber}_{sku}");
        }
        catch (System.Exception e)
        {
            throw e;
        }
    }

    [MenuItem("Build/macOS")]
    public static void BuildMacOS()
    {
        string versionNumber = GetIncrementedVersionNumber();
        BuildForTarget(BuildTarget.StandaloneOSX, $"{buildPath}Mac_{versionNumber}/{Application.productName}.app", versionNumber);
    }

    // [MenuItem("Build/Android (AAB)")]
    // public static void BuildAndroidAAB()
    // {
    //     string versionNumber = GetIncrementedVersionNumber();
    //     BuildForTarget(BuildTarget.Android, $"{buildPath}Android_{versionNumber}/{Application.productName}.aab", versionNumber);
    // }

    // Helper to validate and log the build result
    static void CheckBuildResult(BuildReport report, string outputPath)
    {
        // Log the build summary
        var summary = report.summary;
        if (summary.result == BuildResult.Succeeded)
        {
            UnityEngine.Debug.Log("Build succeeded at: " + outputPath + " (" + summary.totalSize + " bytes)");
        }
        else
        {
            throw new System.Exception("Build failed: " + report.SummarizeErrors());
        }
    }
}
