using UnityEngine;


[System.Serializable]
public class ItemGroup
{
    [Header("GameData 名稱（例如 milk、candy、key）")]
    public string itemName;

    [Header("這個物品對應的多個場景物件")]
    public GameObject[] itemObjects;
}

public class ItemHider : MonoBehaviour
{
    [Header("根據 GameData 狀態隱藏的物件群組")]
    public ItemGroup[] items;

    private void Start()
    {
        ApplyHideLogic();
    }

    // 根據 GameData 隱藏已撿取的物件
    public void ApplyHideLogic()
    {
        foreach (var group in items)
        {
            bool alreadyPicked = HasItem(group.itemName.ToLower());

            foreach (var obj in group.itemObjects)
            {
                if (obj == null) continue;
                obj.SetActive(!alreadyPicked); // true = 隱藏
            }

            Debug.Log($"[ItemHider] {group.itemName} → {(alreadyPicked ? "隱藏(已撿取)" : "顯示(未撿取)")}");
        }
    }

    // 一鍵全部顯示
    public void ShowAllObjects()
    {
        foreach (var group in items)
        {
            foreach (var obj in group.itemObjects)
            {
                if (obj == null) continue;
                obj.SetActive(true);
            }
        }

        Debug.Log("[ItemHider] 已全部顯示所有物件");
    }

    private bool HasItem(string itemName)
    {
        switch (itemName)
        {
            case "milk": return GameData.Instance.milk;
            case "candy": return GameData.Instance.candy;
            case "basket": return GameData.Instance.basket;
            case "crystala": return GameData.Instance.crystala;
            case "crystalb": return GameData.Instance.crystalb;
            case "crystalc": return GameData.Instance.crystalc;
            case "key": return GameData.Instance.key;
            case "apple": return GameData.Instance.apple;
            case "bread": return GameData.Instance.bread;
            case "conch": return GameData.Instance.conch;
            default: return false;
        }
    }
}
