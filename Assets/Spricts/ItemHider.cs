using UnityEngine;

public class ItemHider : MonoBehaviour
{
    [Header("要檢查的物件 Tag（可在 Inspector 選擇）")]
    [Tooltip("選擇所有可撿取物件所使用的 Tag")]
    [SerializeField] private string targetTag; // 可以從 Inspector 選 tag

    private void Start()
    {
        if (string.IsNullOrEmpty(targetTag))
        {
            Debug.LogWarning("[ItemHider] 尚未設定 Tag，請在 Inspector 選擇要偵測的 Tag。");
            return;
        }

        // 找出所有有指定 Tag 的物件
        GameObject[] pickups = GameObject.FindGameObjectsWithTag(targetTag);

        foreach (GameObject item in pickups)
        {
            string name = item.name.ToLower();

            // 根據 GameData 狀態隱藏已撿取的物件
            if (HasItem(name))
            {
                item.SetActive(false);
                Debug.Log($"[ItemHider] 已撿取過：{item.name}，自動隱藏。");
            }
        }
    }

    private bool HasItem(string itemName)
    {
        switch (itemName)
        {
            case "milk": return GameData.Instance.milk;
            case "candy": return GameData.Instance.candy;
            case "basket": return GameData.Instance.basket;
            case "crystal": return GameData.Instance.crystal;
            case "key": return GameData.Instance.key;
            case "apple": return GameData.Instance.apple;
            case "bread": return GameData.Instance.bread;
            case "conch": return GameData.Instance.conch;
            default: return false;
        }
    }
}
