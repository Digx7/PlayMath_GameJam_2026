using UnityEngine;
using System;

public class GlobalStructs : MonoBehaviour
{
    
}

[System.Serializable]
public struct StringAndGameObject
{
    public string name;
    public GameObject obj;
}

public struct PlayerSpawnInfo
{
    public int ID;
    public Vector3 location;
    public Quaternion rotation;
}

[System.Serializable]
public struct SceneContext
{
    public int SpawnPointID;
    public SceneCameraMode sceneCameraMode;
    public Vector3 cameraLocation;

    public void Clear()
    {
        SpawnPointID = 0;
        sceneCameraMode = SceneCameraMode.FollowPlayer;
    }
}

[System.Serializable]
public struct SceneData
{
    public string sceneName;
    public SceneContext context;

    public void Clear()
    {
        sceneName = "";
        context.Clear();
    }
}