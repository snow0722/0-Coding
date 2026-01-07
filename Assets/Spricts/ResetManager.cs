using UnityEngine;

public class ResetManager : MonoBehaviour
{
    public static ResetManager Instance;

    // 初始快照
    private int initialCrystalCount;
    private bool initialConch;
    private bool initialCandy;
    private bool initialBasket;
    private bool initialCrystala;
    private bool initialCrystalb;
    private bool initialCrystalc;
    private bool initialKey;
    private bool initialApple;
    private bool initialMilk;
    private bool initialBread;

    // 🔹 是否已經記錄過初始狀態
    private bool hasRecordedInitial = false;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }

        // 只在第一次進場記錄初始狀態
        RecordInitialStateOnce();
    }

    /// <summary>
    /// 只紀錄第一次進場的初始狀態
    /// </summary>
    public void RecordInitialStateOnce()
    {
        if (hasRecordedInitial) return; // 已經記錄過就跳過
        if (GameData.Instance == null) return;

        GameData gd = GameData.Instance;

        initialCrystalCount = gd.crystalCount;
        initialConch = gd.conch;
        initialCandy = gd.candy;
        initialBasket = gd.basket;
        initialCrystala = gd.crystala;
        initialCrystalb = gd.crystalb;
        initialCrystalc = gd.crystalc;
        initialKey = gd.key;
        initialApple = gd.apple;
        initialMilk = gd.milk;
        initialBread = gd.bread;

        hasRecordedInitial = true; // 標記已經記錄
        Debug.Log("[ResetManager] 初始狀態已記錄（第一次進場）");
    }

    /// <summary>
    /// 回復 GameData 到初始快照
    /// </summary>
    public void RestoreInitialState()
    {
        if (GameData.Instance == null) return;

        GameData gd = GameData.Instance;

        gd.crystalCount = initialCrystalCount;
        gd.conch = initialConch;
        gd.candy = initialCandy;
        gd.basket = initialBasket;
        gd.crystala = initialCrystala;
        gd.crystalb = initialCrystalb;
        gd.crystalc = initialCrystalc;
        gd.key = initialKey;
        gd.apple = initialApple;
        gd.milk = initialMilk;
        gd.bread = initialBread;

        Debug.Log("[ResetManager] GameData 已恢復到第一次進場初始狀態");
    }

    /// <summary>
    /// 只重置水晶（回到第一次進場水晶快照）
    /// </summary>
    public void ResetCrystalsOnly()
    {
        if (GameData.Instance == null) return;

        GameData gd = GameData.Instance;

        gd.crystalCount = initialCrystalCount;
        gd.crystala = initialCrystala;
        gd.crystalb = initialCrystalb;
        gd.crystalc = initialCrystalc;

        Debug.Log("[ResetManager] 只重置水晶（回第一次進場初始快照）");
    }

    /// <summary>
    /// 全重置（回第一次進場初始快照）
    /// </summary>
    public void FullReset()
    {
        RestoreInitialState();
        Debug.Log("[ResetManager] 全部重置完成（回第一次進場初始快照）");
    }
}