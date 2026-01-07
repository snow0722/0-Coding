using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class SceneLoader : MonoBehaviour
{
    public TransitionController transitionController;

    // 普通切換
    public void LoadSceneNormal(string sceneName)
    {
        transitionController.PlayTransition(null, null, () =>
        {
            SceneManager.LoadScene(sceneName);
        });
    }

    // 預載切換
    public void LoadSceneAsync(string sceneName)
    {
        transitionController.PlayTransition(null, null, () =>
        {
            StartCoroutine(PreloadAndActivate(sceneName));
        });
    }

    private IEnumerator PreloadAndActivate(string sceneName)
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);
        asyncLoad.allowSceneActivation = false;

        while (!asyncLoad.isDone)
        {
            // 載入完成
            if (asyncLoad.progress >= 0.9f)
            {
                asyncLoad.allowSceneActivation = true;
            }
            yield return null;
        }

        // 等一幀確保新場景初始化完成
        yield return null;
    }

    public void Quit()
    {
        // 應用程式的離開遊戲
        Debug.Log("[已離開遊戲]");
        Application.Quit();
    }
}