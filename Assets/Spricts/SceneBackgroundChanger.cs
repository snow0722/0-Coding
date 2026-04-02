using UnityEngine;
using UnityEngine.UI;

public class SceneBackgroundChanger : MonoBehaviour
{
    [Header("背景 Image")]
    [SerializeField] private Image backgroundImage;

    [Header("隨機背景列表")]
    [SerializeField] private Sprite[] backgrounds;

    void Start()
    {
        if (backgroundImage == null || backgrounds == null || backgrounds.Length == 0)
            return;

        int index = Random.Range(0, backgrounds.Length);
        backgroundImage.sprite = backgrounds[index];
    }
}
