using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using Fungus;
using System.Collections;

public class Door : MonoBehaviour, IInteraction
{
    [Header("Fungus Flowchart")]
    [SerializeField] private Flowchart flowchartObject;

    [Header("需要的鑰匙")]
    [SerializeField] private Key key;

    [Header("結算畫面控制器 (ItemResultDisplayWithFungus)")]
    [SerializeField] private ItemResultDisplayWithFungus resultDisplay;

    [Header("過場設定")]
    [SerializeField] private bool useTransition = true;       // 是否使用過場
    [SerializeField] private float fadeDuration = 1f;         // 淡入淡出時間
    [SerializeField] private Sprite transitionSprite;         // 自訂過場圖片
    [SerializeField] private VideoClip transitionVideo;       // 自訂過場影片
    [SerializeField] private RawImage videoRawImage;          // 顯示影片的 UI
    [SerializeField] private Image fadeImage;                 // 顯示圖片的 UI
    [SerializeField] private VideoPlayer videoPlayer;         // VideoPlayer 元件

    private string messageNoKey = "沒有鑰匙";
    private string messageHasKey = "已有鑰匙";

    public void PickUp()
    {
        Debug.Log($"<color=#f37>撿取：{name}</color>");
        Interaction();
    }

    public void Interaction()
    {
        bool hasKey = key != null && key.pickedUp;
        Debug.Log($"玩家是否有鑰匙: {hasKey}");

        if (!hasKey)
        {
            flowchartObject.SendFungusMessage(messageNoKey);
        }
        else
        {
            flowchartObject.SendFungusMessage(messageHasKey);

            // 使用過場動畫或直接顯示結算
            if (useTransition)
                StartCoroutine(ShowTransitionThenResult());
            else
                ShowResultDirectly();
        }
    }

    // 直接開啟結算畫面
    private void ShowResultDirectly()
    {
        if (resultDisplay != null)
        {
            resultDisplay.ShowResult();
        }
        else
        {
            Debug.LogWarning("未指定結算畫面控制器 (ItemResultDisplayWithFungus)！");
        }
    }

    // 有過場動畫的流程
    private IEnumerator ShowTransitionThenResult()
    {
        // 顯示過場畫面（圖片或影片）
        yield return StartCoroutine(PlayTransition());

        // 顯示結算畫面
        ShowResultDirectly();

        // 結束過場
        yield return StartCoroutine(FadeOutTransition());
    }

    // 播放過場畫面
    private IEnumerator PlayTransition()
    {
        if (transitionVideo != null && videoPlayer != null && videoRawImage != null)
        {
            fadeImage.gameObject.SetActive(false);
            videoRawImage.gameObject.SetActive(true);
            videoPlayer.clip = transitionVideo;
            videoPlayer.Prepare();

            while (!videoPlayer.isPrepared)
                yield return null;

            videoRawImage.texture = videoPlayer.texture;
            videoPlayer.Play();

            // 播放淡入
            yield return StartCoroutine(Fade(videoRawImage, 0f, 1f));

            // 等影片播放結束
            while (videoPlayer.isPlaying)
                yield return null;
        }
        else if (transitionSprite != null && fadeImage != null)
        {
            videoRawImage.gameObject.SetActive(false);
            fadeImage.gameObject.SetActive(true);
            fadeImage.sprite = transitionSprite;

            // 播放淡入
            yield return StartCoroutine(Fade(fadeImage, 0f, 1f));
            yield return new WaitForSeconds(1f); // 停留時間
        }
    }

    // 淡出過場畫面
    private IEnumerator FadeOutTransition()
    {
        if (transitionVideo != null && videoRawImage != null)
            yield return StartCoroutine(Fade(videoRawImage, 1f, 0f));
        else if (transitionSprite != null && fadeImage != null)
            yield return StartCoroutine(Fade(fadeImage, 1f, 0f));

        if (videoPlayer != null)
            videoPlayer.Stop();

        fadeImage.gameObject.SetActive(false);
        videoRawImage.gameObject.SetActive(false);
    }

    // 通用淡入淡出效果（圖片或影片都可用）
    private IEnumerator Fade(Graphic target, float start, float end)
    {
        if (target == null) yield break;
        float t = 0f;
        Color color = target.color;

        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            float alpha = Mathf.Lerp(start, end, t / fadeDuration);
            color.a = alpha;
            target.color = color;
            yield return null;
        }

        color.a = end;
        target.color = color;
    }
}