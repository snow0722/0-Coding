using UnityEngine;
using UnityEngine.UI;
using Fungus;

/// <summary>
/// 水晶數量 UI 管理器
/// 1. 管理目前撿到幾顆水晶
/// 2. 更新畫面 UI
/// 3. 同步數量給 Fungus
/// 4. 觸發對應的對話事件
/// </summary>
public class CrystalUIManager : MonoBehaviour
{
    public static CrystalUIManager Instance;

    [Header("水晶數量 UI")]
    [SerializeField] private Text crystalCountText;

    [Header("總水晶數量")]
    [SerializeField] private int totalCrystal = 3;

    [Header("共用的 Fungus Flowchart")]
    [SerializeField] private Flowchart flowchart;

    // 目前已蒐集的水晶數量
    private int currentCrystal = 0;

    private void Awake()
    {
        // Singleton
        if (Instance == null)
            Instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }

        // 進場先同步一次 UI
        UpdateUI();
    }

    /// <summary>
    /// 撿到一顆水晶時呼叫
    /// </summary>
    public void AddCrystal()
    {
        currentCrystal++;

        // 同步數量給 Fungus
        if (flowchart != null)
        {
            flowchart.SetIntegerVariable("CrystalCount", currentCrystal);
            flowchart.SendFungusMessage("OnCrystalCollected");
        }

        UpdateUI();

        Debug.Log($"[CrystalUIManager] 目前水晶數量：{currentCrystal}");
    }

    /// <summary>
    /// 更新 UI 顯示
    /// </summary>
    private void UpdateUI()
    {
        if (crystalCountText != null)
            crystalCountText.text = $"水晶 x{currentCrystal}";
    }

    /// <summary>
    /// （可選）外部查詢目前水晶數量
    /// </summary>
    public int GetCurrentCrystalCount()
    {
        return currentCrystal;
    }
}