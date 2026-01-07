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

    [Header("開頭動畫的 Fungus Flowchart")]
    [SerializeField] private Flowchart firstflowchart;

    [Header("物件共用的 Fungus Flowchart")]
    [SerializeField] private Flowchart flowchart;

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

    private void Start()
    {
        CheckBasketStateAndNotifyFungus();
    }

    /// <summary>
    /// 撿到一顆水晶時呼叫
    /// </summary>
    public void AddCrystal()
    {
        // 🔹 先累計 GameData
        if (GameData.Instance != null)
            GameData.Instance.AddCrystal();

        // 🔹 從 GameData 讀目前水晶數量
        int currentCrystal = GameData.Instance != null ? GameData.Instance.crystalCount : 0;

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
        int currentCrystal = GameData.Instance != null ? GameData.Instance.crystalCount : 0;

        if (crystalCountText != null)
            crystalCountText.text = $"水晶 x{currentCrystal}";
    }

    /// <summary>
    /// （可選）外部查詢目前水晶數量
    /// </summary>
    public int GetCurrentCrystalCount()
    {
        return GameData.Instance != null ? GameData.Instance.crystalCount : 0;
    }


    public void CheckBasketStateAndNotifyFungus()
    {
        if (GameData.Instance == null || firstflowchart == null)
            return;

        if (GameData.Instance.basket == false)
        {
            firstflowchart.SendFungusMessage("BasketIsClosed");
            Debug.Log("[CrystalUIManager] Basket 關 → 發送 Fungus 訊息");
        }
    }
}