using UnityEngine;
using UnityEngine.UI;

public class InventorySlotGray : MonoBehaviour
{
    [Header("圖片 UI")]
    [SerializeField] private Image itemImage;

    [Header("彩色圖 & 灰色剪影圖")]
    [SerializeField] private Sprite colorSprite;
    [SerializeField] private Sprite graySprite;

    [Header("對應 GameData 的 bool")]
    [SerializeField] private ItemType itemType;

    private void Start()
    {
        UpdateState();
    }

    public void UpdateState()
    {
        bool hasItem = false;

        switch (itemType)
        {
            case ItemType.Apple: hasItem = GameData.Instance.apple; break;
            case ItemType.Milk: hasItem = GameData.Instance.milk; break;
            case ItemType.Bread: hasItem = GameData.Instance.bread; break;
            case ItemType.Basket: hasItem = GameData.Instance.basket; break;
        }

        // 根據是否取得物品換圖
        itemImage.sprite = hasItem ? colorSprite : graySprite;
    }
}

public enum ItemType
{
    Apple,
    Milk,
    Bread,
    Basket
}
