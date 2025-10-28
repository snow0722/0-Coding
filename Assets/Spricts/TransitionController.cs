using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using System;
using System.Collections;
using UnityEngine.SceneManagement;

public class TransitionController : MonoBehaviour
{
    public Image transitionImage;
    public RawImage videoRawImage;
    public VideoPlayer videoPlayer;

    public Sprite defaultSprite;
    public VideoClip defaultClip;

    private CanvasGroup canvasGroup;

    private void Awake()
    {
        canvasGroup = transitionImage.GetComponent<CanvasGroup>();
        if (canvasGroup == null)
            canvasGroup = transitionImage.gameObject.AddComponent<CanvasGroup>();

        ResetTransitionUI();
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // 新場景載入完成後再隱藏過場 UI
        ResetTransitionUI();
    }

    public void ResetTransitionUI()
    {
        if (transitionImage != null)
        {
            transitionImage.enabled = false;
            transitionImage.sprite = defaultSprite;
            canvasGroup.alpha = 1f;
            canvasGroup.blocksRaycasts = false;
        }

        if (videoRawImage != null)
        {
            videoRawImage.enabled = false;
            videoRawImage.texture = null;
        }

        if (videoPlayer != null)
        {
            videoPlayer.Stop();
            videoPlayer.clip = null;
        }
    }

    // 播放過場，完成後呼叫 onComplete
    public void PlayTransition(Sprite customSprite = null, VideoClip customClip = null, Action onComplete = null)
    {
        StartCoroutine(DoTransition(customSprite, customClip, onComplete));
    }

    private IEnumerator DoTransition(Sprite customSprite, VideoClip customClip, Action onComplete)
    {
        // 顯示過場
        if (transitionImage != null)
        {
            transitionImage.sprite = customSprite != null ? customSprite : defaultSprite;
            transitionImage.enabled = true;
            canvasGroup.blocksRaycasts = true;  // 阻擋點擊
        }

        bool useVideo = customClip != null && videoPlayer != null;
        if (useVideo)
        {
            videoPlayer.Stop();
            videoPlayer.clip = customClip;
            videoRawImage.texture = null;
            videoRawImage.enabled = true;

            videoPlayer.Prepare();
            while (!videoPlayer.isPrepared)
                yield return null;

            videoRawImage.texture = videoPlayer.texture;
            videoPlayer.Play();

            // 等影片播放完
            while (videoPlayer.isPlaying)
                yield return null;
        }
        else
        {
            // 沒影片就短暫停留一下
            yield return new WaitForSeconds(2f);
        }

        // 不隱藏 UI，先回調讓 SceneLoader 切換場景
        onComplete?.Invoke();
    }
}