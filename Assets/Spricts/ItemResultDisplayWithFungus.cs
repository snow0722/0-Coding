using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using Fungus;

public class ItemResultDisplayWithFungus : MonoBehaviour
{
    [Header("UI 元件")]
    public CanvasGroup canvasGroup;       // 整個結算 Canvas，用於淡入淡出
    public Image resultImage;             // 上半部圖片

    [Header("水晶對應圖片")]
    public List<Sprite> itemSprites;      // 水晶數量對應圖片

    [Header("淡入動畫設定")]
    public float fadeDuration = 1f;       // Canvas 淡入時間

    [Header("Fungus Flowchart")]
    public Flowchart flowchart;           // Flowchart 物件

    private void Awake()
    {
        // 遊戲一開始隱藏結算畫面
        HideResult();
    }

    /// <summary>
    /// 顯示結算畫面
    /// </summary>
    public void ShowResult()
    {
        int collectedCount = 0;
        if (CrystalUIManager.Instance != null)
            collectedCount = CrystalUIManager.Instance.GetCurrentCrystalCount();

        collectedCount = Mathf.Clamp(collectedCount, 0, Mathf.Max(itemSprites.Count - 1, 0));

        if (resultImage != null && itemSprites.Count > 0)
            resultImage.sprite = itemSprites[collectedCount];

        // 直接發送訊息給 Fungus，文字由 Flowchart 控制
        if (flowchart != null)
            flowchart.SendFungusMessage("ShowResultMessage");

        if (canvasGroup != null)
        {
            canvasGroup.alpha = 0f;
            canvasGroup.gameObject.SetActive(true);
            StartCoroutine(FadeCanvas(0f, 1f));
        }
    }

    private IEnumerator FadeCanvas(float start, float end)
    {
        float t = 0f;
        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            if (canvasGroup != null)
                canvasGroup.alpha = Mathf.Lerp(start, end, t / fadeDuration);
            yield return null;
        }
        if (canvasGroup != null)
            canvasGroup.alpha = end;
    }

    /// <summary>
    /// 隱藏結算畫面
    /// </summary>
    public void HideResult()
    {
        if (canvasGroup != null)
            canvasGroup.gameObject.SetActive(false);
    }
}
