using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class InteractionUI : MonoBehaviour
{
    [Header("互動提示圖片")]
    public Image interactIcon; // 頭上的圖片
    [SerializeField] private float iconShowTime = 2f;  // 顯示秒數

    void Start()
    {
        if (interactIcon != null)
        {
            interactIcon.gameObject.SetActive(false);
        }
    }

    public void ShowIcon()
    {
        if (interactIcon == null) return;

        interactIcon.gameObject.SetActive(true);
        StopAllCoroutines(); // 避免多次觸發疊加
        StartCoroutine(HideIconAfterSeconds());
    }

    private IEnumerator HideIconAfterSeconds()
    {
        yield return new WaitForSeconds(iconShowTime);
        if (interactIcon != null)
        {
            interactIcon.gameObject.SetActive(false);
        }
    }
}

