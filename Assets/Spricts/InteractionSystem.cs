using UnityEngine;

public class InteractionSystem : MonoBehaviour
{
    [Header("可撿取物品圖層")]
    [SerializeField] private string pickableLayerName = "Pickable";

    [Header("互動提示控制器")]
    [SerializeField] private InteractionUI interactionUI; // 負責顯示頭上圖片

    void OnTriggerEnter(Collider other)
    {
        // 先判斷物件圖層是否符合
        if (other.gameObject.layer == LayerMask.NameToLayer(pickableLayerName))
        {
            Debug.Log("圖層符合，觸發互動: " + other.name);

            // 檢查是否有互動接口
            if (other.TryGetComponent<IInteraction>(out IInteraction interaction))
            {
                interaction.PickUp(); // 呼叫物件上的互動邏輯
            }

            // 顯示角色頭上的提示圖示
            if (interactionUI != null)
            {
                interactionUI.ShowIcon();
            }
        }
    }
}

