using Fungus;
using UnityEngine;

/// <summary>
/// 水晶物件
/// </summary>
public class Crystal : MonoBehaviour, IInteraction
{
    [Header("水晶名稱")]
    public string crystalName;  // "A", "B", "C"

    [Header("Fungus_互動物件說明")]
    [SerializeField] private Flowchart flowchartObject;

    [Header("撿取音效")]
    [SerializeField] private AudioClip soundPickUp;

    private AudioSource aud;
    private Rigidbody rig;
    private Collider col;
    private bool isCollected;

    /// <summary>
    /// 玩家是否已撿到水晶
    /// </summary>
    public bool Collected => isCollected;

    private void Awake()
    {
        rig = GetComponent<Rigidbody>();
        col = GetComponent<Collider>();
        aud = GetComponent<AudioSource>();
    }

    /// <summary>
    /// 互動方法 (IInteraction 介面)
    /// </summary>
    public void Interaction()
    {
        Debug.Log($"<color=#3f3>互動：{crystalName}</color>");
    }

    /// <summary>
    /// 撿取水晶
    /// </summary>
    public void PickUp()
    {
        if (!isCollected)
        {
            isCollected = true;
            Debug.Log($"<color=#37f>撿取：{crystalName}</color>");

            // 剛體設為運動學
            if (rig != null) rig.isKinematic = true;

            // 碰撞器可以選擇關閉
            if (col != null) col.enabled = false;

            // 隱藏水晶 (移到遠方)
            transform.position = new Vector3(0, 0, -200);

            // 播放音效
            if (aud != null && soundPickUp != null)
                aud.PlayOneShot(soundPickUp);

            // 觸發 Fungus 對話
            if (flowchartObject != null)
                flowchartObject.SendFungusMessage(crystalName);
        }
    }
}