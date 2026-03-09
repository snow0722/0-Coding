using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using System.Collections;
using Fungus;

public class InventoryManager : MonoBehaviour
{
    [Header("UI")]
    public RectTransform pausePanel;

    [Header("Position")]
    public float showY = 0f;
    private float hiddenY;

    [Header("Animation")]
    public float slideDuration = 0.4f;
    [Range(0.5f, 2f)]
    public float overshoot = 1.2f;

    [Header("Player")]
    public PlayerControl player;

    private Flowchart[] flowcharts; // 自動抓所有 Flowchart

    private bool isOpen = false;
    private Coroutine slideCoroutine;


    void Start()
    {
        // 隱藏 Panel
        hiddenY = -Screen.height;
        pausePanel.anchoredPosition = new Vector2(0, hiddenY);
        pausePanel.gameObject.SetActive(false);

        // 抓取場景中所有 Flowchart
        flowcharts = FindObjectsOfType<Flowchart>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            // 檢查是否有任何 Flowchart 正在執行對話
            if (IsAnyFlowchartRunning())
                return;

            TogglePause();
        }
    }

    bool IsAnyFlowchartRunning()
    {
        foreach (var fc in flowcharts)
        {
            if (fc.HasExecutingBlocks())
                return true;
        }
        return false;
    }

    public void TogglePause()
    {
        if (isOpen)
            Close();
        else
            Open();
    }

    void Open()
    {
        isOpen = true;
        pausePanel.gameObject.SetActive(true);

        if (player != null)
            player.SetMove(false);

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        StartSlide(showY, true);
    }

    void Close()
    {
        isOpen = false;

        if (player != null)
            player.SetMove(true);

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        StartSlide(hiddenY, false, () =>
        {
            pausePanel.gameObject.SetActive(false);
        });
    }

    void StartSlide(float targetY, bool elastic, System.Action onComplete = null)
    {
        if (slideCoroutine != null)
            StopCoroutine(slideCoroutine);

        slideCoroutine = StartCoroutine(Slide(targetY, elastic, onComplete));
    }

    IEnumerator Slide(float targetY, bool elastic, System.Action onComplete)
    {
        Vector2 startPos = pausePanel.anchoredPosition;
        Vector2 endPos = new Vector2(0, targetY);

        float time = 0f;

        while (time < slideDuration)
        {
            time += Time.deltaTime;
            float t = Mathf.Clamp01(time / slideDuration);

            float easedT = elastic ? EaseOutBack(t, overshoot) : t;

            pausePanel.anchoredPosition =
                Vector2.LerpUnclamped(startPos, endPos, easedT);

            yield return null;
        }

        pausePanel.anchoredPosition = endPos;
        onComplete?.Invoke();
    }

    float EaseOutBack(float t, float s)
    {
        t -= 1f;
        return (t * t * ((s + 1f) * t + s) + 1f);
    }

}