using UnityEngine;
using System.Collections;

public class MainMenu : MonoBehaviour
{
    public RectTransform helpPanel;
    public float duration = 0.5f;

    private bool isOpen = false;
    private Vector2 hiddenPos, shownPos;

    void Start()
    {
        shownPos = helpPanel.anchoredPosition;
        hiddenPos = shownPos + new Vector2(0, -800f);
        helpPanel.anchoredPosition = hiddenPos;
    }

    public void ToggleHelp()
    {
        StopAllCoroutines();
        StartCoroutine(MovePanel(isOpen ? hiddenPos : shownPos));
        isOpen = !isOpen;
    }

    IEnumerator MovePanel(Vector2 target)
    {
        Vector2 start = helpPanel.anchoredPosition;
        float time = 0;

        while (time < duration)
        {
            time += Time.deltaTime;
            float t = time / duration;

            // 彈性曲線（有點回彈感）
            t = Mathf.Sin(t * Mathf.PI * 0.5f);
            float bounce = 1 + Mathf.Sin(t * Mathf.PI) * 0.1f;

            helpPanel.anchoredPosition = Vector2.Lerp(start, target, t * bounce);
            yield return null;
        }

        helpPanel.anchoredPosition = target;
    }
}
