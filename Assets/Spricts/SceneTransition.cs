using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Video;
using System.Collections;

public class SceneTransition : MonoBehaviour
{
    public static SceneTransition Instance;

    [Header("過場 UI")]
    public Image transitionImage;       // 過場圖片
    public RawImage videoRawImage;      // 過場影片
    public VideoPlayer videoPlayer;     // 影片播放器

    [Header("預設過場素材")]
    public Sprite defaultSprite;
    public VideoClip defaultClip;

    [Header("過場時間")]
    public float minDuration = 2f;      // 最少過場秒數

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        // 初始關閉
        if (transitionImage != null)
            transitionImage.enabled = false;
        if (videoRawImage != null)
            videoRawImage.enabled = false;
    }

    // -----------------------------
    // 對外 API
    // -----------------------------
    public void LoadGameScene()
    {
        LoadScene("GameScene");
    }

    public void LoadSceneByName(string sceneName)
    {
        LoadScene(sceneName);
    }

    public void LoadScene(string sceneName, Sprite customSprite = null, VideoClip customClip = null)
    {
        StartCoroutine(DoSceneTransition(sceneName, customSprite, customClip));
    }

    // -----------------------------
    // 主流程
    // -----------------------------
    private IEnumerator DoSceneTransition(string sceneName, Sprite customSprite, VideoClip customClip)
    {
        float startTime = Time.time;

        // 顯示過場圖片或影片
        if (transitionImage != null)
        {
            transitionImage.sprite = customSprite != null ? customSprite : defaultSprite;
            transitionImage.enabled = true;
        }

        bool useVideo = customClip != null;
        if (useVideo && videoPlayer != null)
        {
            videoPlayer.clip = customClip;
            videoPlayer.Prepare();
            while (!videoPlayer.isPrepared)
                yield return null;

            videoRawImage.texture = videoPlayer.texture;
            videoRawImage.enabled = true;
            videoPlayer.Play();
        }

        // 預載場景
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);
        asyncLoad.allowSceneActivation = false;

        // 等待場景載入完成 + 過場時間達到
        while (!asyncLoad.isDone)
        {
            float elapsed = Time.time - startTime;
            if (asyncLoad.progress >= 0.9f && elapsed >= minDuration && (!useVideo || !videoPlayer.isPlaying))
            {
                asyncLoad.allowSceneActivation = true;
            }
            yield return null;
        }

        // 清理
        if (transitionImage != null)
            transitionImage.enabled = false;

        if (videoRawImage != null)
            videoRawImage.enabled = false;

        if (videoPlayer != null && videoPlayer.isPlaying)
            videoPlayer.Stop();
    }
}