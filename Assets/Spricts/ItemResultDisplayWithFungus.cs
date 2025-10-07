using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using Fungus;

public class ItemResultDisplayWithFungus : MonoBehaviour
{
    [Header("UI 元件")]
    public GameObject resultPanel;
    public Image resultImage;
    public Animator panelAnimator;  // Panel 上的 Animator

    [Header("對應照片")]
    public List<Sprite> itemSprites;     // 依收集數量對應照片

    [Header("Fungus 對話")]
    public Flowchart flowchart;           // Flowchart 物件
    public List<string> blockNames;       // 對應收集數量的 Block 名稱

    void Start()
    {
        ShowResult();
    }

    public void ShowResult()
    {
        resultPanel.SetActive(true);

        // 找出場景裡所有水晶
        Crystal[] crystals = FindObjectsOfType<Crystal>();
        int collectedCount = 0;
        foreach (var c in crystals)
        {
            if (c.Collected) collectedCount++;
        }

        // 避免超出陣列範圍
        collectedCount = Mathf.Clamp(collectedCount, 0, itemSprites.Count - 1);

        // 更新圖片
        resultImage.sprite = itemSprites[collectedCount];

        // 觸發動畫
        if (panelAnimator != null)
        {
            panelAnimator.SetTrigger("ShowResult");
        }

        // 觸發 Fungus 對話
        if (flowchart != null && blockNames.Count > collectedCount)
        {
            string blockName = blockNames[collectedCount];
            flowchart.ExecuteBlock(blockName);
        }
    }
}
