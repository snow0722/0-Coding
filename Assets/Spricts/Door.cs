using UnityEngine;
using Fungus;

public class Door : MonoBehaviour, IInteraction
{
    [Header("Fungus Flowchart")]
    [SerializeField] private Flowchart flowchartObject;

    [Header("需要的鑰匙")]
    [SerializeField] private Key key;

    [Header("結算畫面控制器")]
    [SerializeField] private ItemResultDisplayWithFungus resultDisplay;

    [Header("是否結算確認 UI")]
    [SerializeField] private GameObject confirmResultUI;

    [Header("Fungus 訊息名稱")]
    [SerializeField] private string messageNoKey = "沒有鑰匙";
    [SerializeField] private string messageHasKey = "已有鑰匙";

    [Header("玩家移動控制")]
    [SerializeField] private PlayerControl playerControl;


    private void Awake()
    {
        // 遊戲一開始隱藏結算畫面
        HideResultUI();
    }

    // =========================
    // 玩家互動
    // =========================
    public void Interaction()
    {
        bool hasKey = GameData.Instance != null && GameData.Instance.key;
        Debug.Log($"玩家是否有鑰匙: {hasKey}");

        if (!hasKey)
        {
            if (flowchartObject != null)
                flowchartObject.SendFungusMessage(messageNoKey);
        }
        else
        {
            if (flowchartObject != null)
                flowchartObject.SendFungusMessage(messageHasKey);
            // 是否結算 UI 由 Fungus 在對話結束後呼叫
        }
    }

    public void PickUp()
    {
        Interaction();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        Interaction();
    }

    // =========================
    // 給 Fungus 呼叫（對話結束）
    // =========================
    public void ShowConfirmUI()
    {
        if (confirmResultUI != null)
            confirmResultUI.SetActive(true);

        // UI 出現 → 解鎖滑鼠
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    // =========================
    // 給 UI Button 呼叫
    // =========================
    public void OnConfirmYes()
    {
        if (confirmResultUI != null)
            confirmResultUI.SetActive(false);

        // 直接進結算，不鎖回滑鼠
        if (resultDisplay != null)
            resultDisplay.ShowResult();
        else
            Debug.LogWarning("未指定結算畫面控制器！");
    }

    public void OnConfirmNo()
    {
        if (confirmResultUI != null)
            confirmResultUI.SetActive(false);

        // ⬅ 選否 → 回到遊戲 → 鎖回滑鼠
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        // ✅ 恢復玩家移動
        playerControl.SetMove(true);
    }

    /// <summary>
    /// 隱藏結算畫面
    /// </summary>
    public void HideResultUI()
    {
        if (confirmResultUI != null)
            confirmResultUI.gameObject.SetActive(false);
    }
}
