using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    /// <summary>
    /// 場景控制器
    /// </summary>
    public void LoadScene(string sceneName)
    {
        // 場景管理器的載入場景（場景名稱）
        SceneManager.LoadScene(sceneName);
    }

    public void Instructions(string sceneName)
    {
        // 遊戲說明的場景畫面
        SceneManager.LoadScene(sceneName);
    }

    public void Quit()
    {
        // 應用程式的離開遊戲
        Debug.Log("[已離開遊戲]");
        Application.Quit();
    }
}
