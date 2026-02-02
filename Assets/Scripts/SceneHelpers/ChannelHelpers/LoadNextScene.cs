using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadNextScene : MonoBehaviour {
    public void LoadNextSceneInProject()
    {
        // Get the current scene's build index and add 1 to it
    int nextSceneIndex = UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex + 1;

    // Optional: Check if the next scene index is within the range of scenes in the build settings
    if (nextSceneIndex < UnityEngine.SceneManagement.SceneManager.sceneCountInBuildSettings)
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(nextSceneIndex);
    }
    else
    {
        Debug.Log("It's the last scene in the build settings!");
        // Optional: load the first scene (index 0) to loop the game
        // SceneManager.LoadScene(0); 
    }
    }
}