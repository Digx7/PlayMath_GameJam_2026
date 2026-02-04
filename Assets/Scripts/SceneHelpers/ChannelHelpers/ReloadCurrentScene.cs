using UnityEngine;
using UnityEngine.SceneManagement;

public class ReloadCurrentScene : MonoBehaviour {
    public void ReloadCurrenSceneInProject()
    {
        int currentSceneIndex = UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex;

        UnityEngine.SceneManagement.SceneManager.LoadScene(currentSceneIndex);
    }
}